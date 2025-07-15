using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleTopluDegisenBasvuruAfadDurum;

public class GuncelleTopluDegisenBasvuruAfadDurumCommand : IRequest<ResultModel<string>>
{
    public class GuncelleTopluDegisenBasvuruAfadDurumCommandHandler : IRequestHandler<GuncelleTopluDegisenBasvuruAfadDurumCommand, ResultModel<string>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IAfadBasvuruRepository _afadBasvuruRepository;
        private readonly IBasvuruAfadDurumRepository _basvuruAfadDurumRepository;

        public GuncelleTopluDegisenBasvuruAfadDurumCommandHandler(IBasvuruRepository basvuruRepository, IAfadBasvuruRepository afadBasvuruRepository, IBasvuruAfadDurumRepository basvuruAfadDurumRepository)
        {
            _basvuruRepository = basvuruRepository;
            _afadBasvuruRepository = afadBasvuruRepository;
            _basvuruAfadDurumRepository = basvuruAfadDurumRepository;
        }

        public async Task<ResultModel<string>> Handle(GuncelleTopluDegisenBasvuruAfadDurumCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            try
            {
                StringBuilder sb = new();

                sb.AppendLine(string.Format("{0} GuncelleTopluDegisenBasvuruAfadDurum çalışmaya başladı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                var dtControl = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

                var afadBasvuruListe = _afadBasvuruRepository
                                            .GetWhere(x =>
                                                x.HedefTarih >= dtControl
                                                &&
                                                !x.CsbSilindiMi
                                                &&
                                                x.CsbAktifMi == true
                                                ,
                                                asNoTracking: true
                                            ).ToList();

                if (!afadBasvuruListe.Any())
                {
                    sb.AppendLine(string.Format("{0} Afad başvuru tablosunda uygun kayıt olmadığı için hiçbir işlem yapılmadı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    result.Result = sb.ToString();

                    return result;
                }

                var basvuruListe = await _basvuruRepository
                                            .GetWhere(x =>
                                                afadBasvuruListe.Select(s => s.Tckn.ToString()).Distinct().Contains(x.TcKimlikNo)
                                                &&
                                                x.AktifMi == true
                                                &&
                                                !x.SilindiMi
                                                &&
                                                x.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.IptalEdilmistir
                                                &&
                                                x.TuzelKisiTipId == null, //sadece gerçek kişileri güncelleyecez
                                                asNoTracking: true
                                            )
                                            .ToListAsync();

                if (!basvuruListe.Any())
                {
                    sb.AppendLine(string.Format("{0} Başvuru tablosunda güncellenebilecek uygun başvuru olmadığı için sondanlandırıldı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    result.Result = sb.ToString();
                    return result;
                }

                var basvuruAfadDurumList = _basvuruAfadDurumRepository.GetWhere(x => x.AktifMi == true && x.SilindiMi == false, true).ToList();

                int guncellenenBasvuruSayisi = 0, basvuruYokSayisi = 0;

                await Parallel.ForEachAsync(basvuruListe, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 50 }, async (basvuru, cancellationToken2) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var afadBasvuru = afadBasvuruListe
                                        .Where(x =>
                                            x.Tckn.ToString() == basvuru.TcKimlikNo
                                            &&
                                            HasarTespitAddon.AskiKoduToUpper(x.AskiKodu) == HasarTespitAddon.AskiKoduToUpper(basvuru.HasarTespitAskiKodu)
                                            &&
                                            x.IlId == basvuru.UavtIlNo
                                            &&
                                            x.Ada == basvuru.TapuAda
                                            &&
                                            x.Parsel == basvuru.TapuParsel
                                        )
                                        .OrderByDescending(o => o.HedefTarih)
                                        .ThenByDescending(o => o.OlusturulmaTarihi)
                                        .FirstOrDefault();

                    if (afadBasvuru != null)
                    {
                        //afad tarafında başvurusu var
                        if (afadBasvuru.DegerlendirmeIptalDurumu?.ToLower().Trim() == "evet")
                        {
                            basvuru.BasvuruAfadDurumId = (long)BasvuruAfadDurumEnum.Iptal;
                            basvuru.BasvuruAfadDurumGuncellemeTarihi = DateTime.Now;

                            Interlocked.Increment(ref guncellenenBasvuruSayisi);
                        }
                        else
                        {
                            var afadDurum = basvuruAfadDurumList.FirstOrDefault(x => x.Ad.ToLower() == (afadBasvuru.ItirazDegerlendirmeSonucu ?? afadBasvuru.DegerlendirmeDurumu)?.ToLower().Replace("afad", "").Trim());
                            if (afadDurum != null)
                            {
                                basvuru.BasvuruAfadDurumId = afadDurum.BasvuruAfadDurumId;
                                basvuru.BasvuruAfadDurumGuncellemeTarihi = DateTime.Now;

                                Interlocked.Increment(ref guncellenenBasvuruSayisi);
                            }
                            else
                            {
                                sb.AppendLine(string.Format("{0} AFAD başvuru durumu veri tabanında bulunamadı! AFAD Verisi: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), JsonConvert.SerializeObject(afadBasvuru)));
                            }
                        }
                    }
                    else
                    {
                        //afad tarafında başvurusu yok
                        basvuru.BasvuruAfadDurumId = (long)BasvuruAfadDurumEnum.BasvuruYok;
                        basvuru.BasvuruAfadDurumGuncellemeTarihi = DateTime.Now;

                        Interlocked.Increment(ref basvuruYokSayisi);
                    }
                });

                _basvuruRepository.UpdateRange(basvuruListe);

                await _basvuruRepository.SaveChanges();

                sb.AppendLine(string.Format("{0} Update işlemi tamamlandı. Toplam kayıt sayısı: {1}, güncellenen kayıt sayısı: {2}, başvurusu olmayan kayıt sayısı: {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), basvuruListe.Count, guncellenenBasvuruSayisi, basvuruYokSayisi));

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