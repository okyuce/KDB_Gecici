using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadBasvuruListeByTcNo;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleBasvuruAfadDurum;

public class GuncelleBasvuruAfadDurumCommand : IRequest<ResultModel<string>>
{
    public int? Take { get; set; }

    public class GuncelleBasvuruAfadDurumCommandHandler : IRequestHandler<GuncelleBasvuruAfadDurumCommand, ResultModel<string>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IMediator _mediator;
        private readonly IBasvuruAfadDurumRepository _basvuruAfadDurumRepository;

        public GuncelleBasvuruAfadDurumCommandHandler(IBasvuruAfadDurumRepository basvuruAfadDurumRepository, IMediator mediator, IBasvuruRepository basvuruRepository)
        {
            _basvuruAfadDurumRepository = basvuruAfadDurumRepository;
            _mediator = mediator;
           
            _basvuruRepository = basvuruRepository;
        }

        public async Task<ResultModel<string>> Handle(GuncelleBasvuruAfadDurumCommand request, CancellationToken cancellationToken)
        {
            ResultModel<string> result = new();

            try
            {
                StringBuilder sb = new();
                sb.AppendLine(string.Format("{0} BasvuruAfadDurumUpdateJob çalışmaya başladı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                var afadBasvuruGuncellemeTarihiKontrol = DateTime.Now.AddDays(-1);

                var basvuruList = await _basvuruRepository.GetWhere(x =>
                            x.AktifMi == true
                            &&
                            x.SilindiMi == false
                            &&
                            (x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzAlinmistir || x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzDegerlendirmeAsamasindadir)
                            &&
                            (x.BasvuruAfadDurumGuncellemeTarihi == null || x.BasvuruAfadDurumGuncellemeTarihi < afadBasvuruGuncellemeTarihiKontrol)
                            &&
                            x.TuzelKisiTipId == null,
                            asNoTracking: true
                            , i=> i.BinaDegerlendirme
                    )
                    .ToListAsync();

                var guncellenenBasvuruList = new List<Basvuru>();

                if (!basvuruList.Any())
                {
                    sb.AppendLine(string.Format("{0} BasvuruAfadDurumUpdateJob güncellenecek başvuru kaydı bulunamadığı için işlem yapmadan tamamlandı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    result.Result = sb.ToString();
                    return result;
                }

                var afadBasvurusuAlinamayanTcListe = new List<string>();

                int guncellenenBasvuruSayisi = 0, basvuruYokSayisi = 0;
                var metotCalismaBaslangicTarihi = DateTime.Now;

                var afadDurumListe = _basvuruAfadDurumRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true, true).ToList();

                await Parallel.ForEachAsync(basvuruList, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 100 }, async (basvuru, cancellationToken2) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var datetimeNow = DateTime.Now;

                    var afadBasvuruListeResult = await _mediator.Send(new GetirAfadBasvuruListeByTcNoQuery { TcKimlikNo = basvuru.TcKimlikNo });
                    if (!afadBasvuruListeResult.IsError)
                    {
                        if (afadBasvuruListeResult.Result?.Any() == true)
                        {
                            basvuru.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(basvuru.HasarTespitAskiKodu);
                            var afadBasvuruFiltered = afadBasvuruListeResult.Result.FirstOrDefault(x => HasarTespitAddon.AskiKoduToUpper(x.AskiBaskiDetayAskiKodu) == basvuru.HasarTespitAskiKodu && x.Ada == basvuru.TapuAda && x.Parsel == basvuru.TapuParsel);
                            if (afadBasvuruFiltered != null)
                            {
                                //afad tarafında başvurusu var
                                var afadDurum = afadDurumListe.FirstOrDefault(x => x.Ad?.ToLower() == afadBasvuruFiltered.DegerlendirmeDurumu?.ToLower().Replace("afad", "").Trim());
                                if (afadDurum != null)
                                {
                                    basvuru.BasvuruAfadDurumId = afadDurum.BasvuruAfadDurumId;
                                    basvuru.BasvuruAfadDurumGuncellemeTarihi = datetimeNow;

                                    if(basvuru.BasvuruAfadDurumId == (long)BasvuruAfadDurumEnum.Kabul && basvuru.BinaDegerlendirme != null)
                                    {
                                        basvuru.BinaDegerlendirme.BinaDegerlendirmeDurumId = (long)BinaDegerlendirmeDurumEnum.BasvurunuzDegerlendirmeyeAlinmistir;
                                    }

                                    guncellenenBasvuruSayisi++;
                                }
                                else
                                {
                                    sb.AppendLine(string.Format("{0} AFAD başvuru durumu veri tabanında bulunamadı! AFAD Verisi: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), JsonConvert.SerializeObject(afadBasvuruFiltered)));
                                }
                            }
                            else if (basvuru.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.BasvuruYok)
                            {
                                //afad tarafında başvurusu yok
                                basvuru.BasvuruAfadDurumId = (long)BasvuruAfadDurumEnum.BasvuruYok;
                                basvuru.BasvuruAfadDurumGuncellemeTarihi = datetimeNow;

                                basvuruYokSayisi++;
                            }
                        }
                        else
                        {
                            //afad tarafında başvurusu yok
                            basvuru.BasvuruAfadDurumId = (long)BasvuruAfadDurumEnum.BasvuruYok;
                            basvuru.BasvuruAfadDurumGuncellemeTarihi = datetimeNow;

                            basvuruYokSayisi++;
                        }
                    }
                    else
                    {
                        if (!afadBasvurusuAlinamayanTcListe.Any(x => x == basvuru.TcKimlikNo))
                            afadBasvurusuAlinamayanTcListe.Add(basvuru.TcKimlikNo);
                    }
                });

                if (afadBasvurusuAlinamayanTcListe.Any())
                    sb.AppendLine(string.Format("AFAD başvurusu alınamayan TC kimlik noları: {1}", string.Join(", ", afadBasvurusuAlinamayanTcListe)));

                //yalnızca ilgili kolonlar update edilecek
                var bulkConfig = new BulkConfig
                {
                    TrackingEntities = false,
                    UpdateByProperties = new List<string> {
                        nameof(Basvuru.BasvuruAfadDurumId),
                        nameof(Basvuru.BasvuruAfadDurumGuncellemeTarihi)
                    }
                };

                //afad durumu güncellenen veriler bulk update yapılıyor
                await _basvuruRepository.BulkUpdate(basvuruList.Where(x => x.BasvuruAfadDurumGuncellemeTarihi >= metotCalismaBaslangicTarihi), CancellationToken.None);

                sb.AppendLine(string.Format("{0} Update işlemi tamamlandı. Toplam kayıt sayısı: {1}, güncellenen kayıt sayısı: {2}, başvurusu olmayan kayıt sayısı: {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), basvuruList.Count, guncellenenBasvuruSayisi, basvuruYokSayisi));

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