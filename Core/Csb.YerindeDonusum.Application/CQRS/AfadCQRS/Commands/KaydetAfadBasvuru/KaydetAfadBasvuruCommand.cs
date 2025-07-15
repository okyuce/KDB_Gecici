using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadTopluDegisenBasvuruListe;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.KaydetAfadBasvuru;

public class KaydetAfadBasvuruCommand : IRequest<ResultModel<string>>
{
    public class KaydetAfadBasvuruCommandHandler : IRequestHandler<KaydetAfadBasvuruCommand, ResultModel<string>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IAfadBasvuruRepository _afadBasvuruRepository;
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;

        public KaydetAfadBasvuruCommandHandler(IMapper mapper, IMediator mediator, IAfadBasvuruRepository afadBasvuruRepository, IAfadBasvuruTekilRepository afadBasvuruTekilRepository)
        {
            _mapper = mapper;
            _mediator = mediator;
            _afadBasvuruRepository = afadBasvuruRepository;
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
        }

        public async Task<ResultModel<string>> Handle(KaydetAfadBasvuruCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            try
            {
                StringBuilder sb = new();

                sb.AppendLine(string.Format("{0} KaydetAfadBasvuru çalışmaya başladı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                int afadDegisenIcinKacGunOncesindenBakmayaBaslasin = 3;

                var dtNow = DateTime.Now;
                var dtToday = DateTime.Today;

                var sonHedefTarih = _afadBasvuruRepository.GetAllQueryable().OrderByDescending(x => x.HedefTarih).FirstOrDefault()?.HedefTarih;

                if (sonHedefTarih != null)
                {
                    var gunFarki = (dtToday.Date - sonHedefTarih.Value.ToDateTime(new TimeOnly(0, 0)).Date).Days - 1;

                    if (gunFarki > 0)
                        afadDegisenIcinKacGunOncesindenBakmayaBaslasin = gunFarki;
                }

                int eklenenAfadBasvuruSayisi = 0;

                for (int i = afadDegisenIcinKacGunOncesindenBakmayaBaslasin; i >= 0; i--)
                {
                    var afadBasvuruAlinacakTarih = dtToday.AddDays(-i);

                    //afad tarafı değişen servisi için offset kullanmadığını iletti. o yüzden while döngüsüne gerek kalmadı. her istekte o güne ait tüm data tek seferde geliyormuş.
                    var afadBasvuruListeResult = await _mediator.Send(new GetirAfadTopluDegisenBasvuruListeQuery { Tarih = afadBasvuruAlinacakTarih });

                    if (afadBasvuruListeResult.IsError)
                    {
                        sb.AppendLine(string.Format("{0} KaydetAfadBasvuru {1} hedef tarihi için AFAD tarafından veri alınamadığı için işlem sonlandırıldı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), afadBasvuruAlinacakTarih.ToString("yyyy-MM-dd")));
                    }
                    else if (afadBasvuruListeResult.Result.Data?.Any() != true)
                    {
                        sb.AppendLine(string.Format("{0} KaydetAfadBasvuru {1} hedef tarihi için AFAD tarafında veri yok. Afad mesaj: {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), afadBasvuruAlinacakTarih.ToString("yyyy-MM-dd"), string.Join(", ", afadBasvuruListeResult.Result.MessageList)));
                    }
                    else
                    {
                        var afadBasvuruAlinacakTarihDateOnly = DateOnly.FromDateTime(afadBasvuruAlinacakTarih);

                        var list = _mapper.Map<List<AfadBasvuru>>(afadBasvuruListeResult!.Result!.Data)
                                        .Select(s =>
                                        {
                                            s.HedefTarih = afadBasvuruAlinacakTarihDateOnly;
                                            return s;
                                        })
                                        .ToList();

                        if (afadBasvuruAlinacakTarihDateOnly >= DateOnly.FromDateTime(dtToday.AddDays(-1)))
                        {
                            //bugünün başvuruları tekrar alındı, daha önceden alınanlar alınmayacak
                            //gece çalışırken gün değişirse diye büyük eşit kuralı yapıldı

                            var dbKayitListe = _afadBasvuruRepository
                                                    .GetWhere(x =>
                                                        x.HedefTarih == afadBasvuruAlinacakTarihDateOnly
                                                        &&
                                                        !x.CsbSilindiMi
                                                        &&
                                                        x.CsbAktifMi == true
                                                        ,
                                                        asNoTracking: true
                                                    ).ToList();

                            var yeniListe = new List<AfadBasvuru>();

                            foreach (var item in list)
                            {
                                if (!dbKayitListe.Any(x => x.Tckn == item.Tckn && x.BasvuruNo == item.BasvuruNo && x.KomisyonKararNo == item.KomisyonKararNo))
                                    yeniListe.Add(item);
                            }

                            if (yeniListe.Any())
                            {
                                eklenenAfadBasvuruSayisi += yeniListe.Count;

                                await _afadBasvuruRepository.AddRangeAsync(yeniListe);
                                await _afadBasvuruRepository.SaveChanges();
                            }
                        }
                        else
                        {
                            eklenenAfadBasvuruSayisi += list.Count;

                            await _afadBasvuruRepository.AddRangeAsync(list);
                            await _afadBasvuruRepository.SaveChanges();
                        }
                    }
                }

                sb.AppendLine($"{eklenenAfadBasvuruSayisi} adet afad başvuru eklendi!");

                if (eklenenAfadBasvuruSayisi > 0)
                {
                    int eklenenTekilBasvuru = 0,
                        guncellenenTekilBasvuru = 0;

                    var yeniAfadBasvuruListe = _mapper.Map<List<AfadBasvuruTekil>>(
                                                    _afadBasvuruRepository
                                                        .GetWhere(x =>
                                                            !x.CsbSilindiMi
                                                            &&
                                                            x.CsbAktifMi == true
                                                            &&
                                                            x.BasvuruNo != null
                                                            &&
                                                            x.CsbOlusturmaTarihi >= dtNow,
                                                            asNoTracking: true
                                                        ).ToList()
                                                    );

                    var yeniAfadBasvuruNoListe = yeniAfadBasvuruListe.Where(x => x.BasvuruNo != null).GroupBy(g => g.BasvuruNo).Select(s => s.Key.Value).OrderBy(o => o).ToList();

                    var tekilTabloReflectionUygulanmayacakPropertyListe = new List<string>() { "CsbId", "CsbOlusturmaTarihi", "CsbGuncellemeTarihi" };


                    var chunkSayisi = Math.Ceiling((decimal)yeniAfadBasvuruNoListe.Count / 1000);

                    //tekil tablosundan biner biner like atmak için döngü kuruldu
                    for (int i = 0; i < chunkSayisi; i++)
                    {
                        var yeniAfadBasvuruNoChunkListe = yeniAfadBasvuruNoListe.Skip(i * 1000).Take(1000).ToList();

                        var afadBasvuruTekilListe = _afadBasvuruTekilRepository
                                                    .GetWhere(x =>
                                                        !x.CsbSilindiMi
                                                        &&
                                                        x.CsbAktifMi == true
                                                        &&
                                                        x.BasvuruNo != null
                                                        &&
                                                        yeniAfadBasvuruNoChunkListe.Contains(x.BasvuruNo.Value),
                                                        asNoTracking: true
                                                    )
                                                    .ToList();

                        foreach (var yeniAfadBasvuru in yeniAfadBasvuruListe.Where(x => yeniAfadBasvuruNoChunkListe.Contains(x.BasvuruNo.Value)))
                        {
                            var afadBasvuruTekil = afadBasvuruTekilListe.FirstOrDefault(x => x.BasvuruNo == yeniAfadBasvuru.BasvuruNo && x.Tckn == yeniAfadBasvuru.Tckn);
                            if (afadBasvuruTekil == null)
                            {
                                eklenenTekilBasvuru++;

                                yeniAfadBasvuru.CsbGuncellemeTarihi = dtNow;

                                await _afadBasvuruTekilRepository.AddAsync(yeniAfadBasvuru);
                            }
                            else
                            {
                                guncellenenTekilBasvuru++;

                                foreach (var afadBasvuruTekilProperty in afadBasvuruTekil.GetType().GetProperties())
                                {
                                    if (!tekilTabloReflectionUygulanmayacakPropertyListe.Contains(afadBasvuruTekilProperty.Name))
                                        afadBasvuruTekilProperty.SetValue(afadBasvuruTekil, yeniAfadBasvuru.GetType().GetProperty(afadBasvuruTekilProperty.Name)!.GetValue(yeniAfadBasvuru, null));
                                }

                                afadBasvuruTekil.CsbGuncellemeTarihi = dtNow;

                                _afadBasvuruTekilRepository.Update(afadBasvuruTekil);
                            }
                        }
                    }

                    await _afadBasvuruTekilRepository.SaveChanges();

                    sb.AppendLine($"{eklenenTekilBasvuru} adet tekil afad başvuru eklendi, {guncellenenTekilBasvuru} adet tekil afad başvuru güncellendi.");
                }

                result.Result = sb.ToString();
            }
            catch (Exception ex)
            {
                result.Result = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} İşlem sırasında bir hata meydana geldi: {ex}";
            }

            return result;
        }
    }
}