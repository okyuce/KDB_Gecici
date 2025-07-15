using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadTopluBasvuruListe;
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

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleTopluBasvuruAfadDurum;

public class GuncelleTopluBasvuruAfadDurumCommand : IRequest<ResultModel<string>>
{
    public class GuncelleTopluBasvuruAfadDurumCommandHandler : IRequestHandler<GuncelleTopluBasvuruAfadDurumCommand, ResultModel<string>>
    {
        
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IMediator _mediator;
        private readonly IBasvuruAfadDurumRepository _basvuruAfadDurumRepository;

        public GuncelleTopluBasvuruAfadDurumCommandHandler(IMediator mediator, IBasvuruRepository basvuruRepository, IBasvuruAfadDurumRepository basvuruAfadDurumRepository)
        {
            _mediator = mediator;
            _basvuruRepository = basvuruRepository;
            _basvuruAfadDurumRepository = basvuruAfadDurumRepository;
        }

        public async Task<ResultModel<string>> Handle(GuncelleTopluBasvuruAfadDurumCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            try
            {
                StringBuilder sb = new();
                sb.AppendLine(string.Format("{0} BasvuruTopluAfadDurumUpdateJob çalışmaya başladı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                int afadOffset = 0;
                int afadAlinanBasvuruSayisi = 0;
                bool afadHataVerdi = false;
                var afadBasvuruAlinacakTarih = new DateTime(2023, 1, 1);

                var afadBasvuruList = new List<GetirAfadTopluBasvuruDto>();

                #region Afad Servislerini Çek
                do
                {
                    var afadBasvuruListeResult = await _mediator.Send(new GetirAfadTopluBasvuruListeQuery { Tarih = afadBasvuruAlinacakTarih, Offset = afadOffset });

                    if (afadBasvuruListeResult.IsError)
                    {
                        sb.AppendLine(string.Format("{0} BasvuruTopluAfadDurumUpdateJob {1} hedef tarihi için AFAD tarafından veri alınamadığı için işlem sonlandırıldı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), afadBasvuruAlinacakTarih.ToString("yyyy-MM-dd")));

                        afadHataVerdi = true;
                    }
                    else if (afadBasvuruListeResult.Result.Data?.Any() != true)
                    {
                        sb.AppendLine(string.Format("{0} BasvuruTopluAfadDurumUpdateJob {1} hedef tarihi için AFAD tarafında veri yok. Afad mesaj: {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), afadBasvuruAlinacakTarih.ToString("yyyy-MM-dd"), string.Join(", ", afadBasvuruListeResult.Result.MessageList)));

                        afadHataVerdi = true;
                    }

                    afadAlinanBasvuruSayisi = afadBasvuruListeResult.Result?.TotalCount ?? 0;

                    afadBasvuruList.AddRange(afadBasvuruListeResult.Result.Data);

                    afadOffset += 1000;
                } while (afadAlinanBasvuruSayisi >= 1000 && !afadHataVerdi);
                #endregion

                if (!afadBasvuruList.Any())
                {
                    sb.AppendLine(string.Format("{0} BasvuruTopluAfadDurumUpdateJob AFAD tarafından hiçbir başvuru gelmediği için sonlandırıldı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    result.Result = sb.ToString();
                    return result;
                }

                var basvuruAfadDurumGuncellemeTarihiKontrol = DateTime.Now.AddDays(-1);

                var basvuruList = await _basvuruRepository
                                            .GetWhere(x =>
                                                afadBasvuruList.Select(s => s.Tckn.ToString()).Distinct().Contains(x.TcKimlikNo)
                                                &&
                                                x.AktifMi == true
                                                &&
                                                x.SilindiMi == false
                                                &&
                                                x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                &&
                                                x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                &&
                                                (x.BasvuruAfadDurumGuncellemeTarihi == null || x.BasvuruAfadDurumGuncellemeTarihi < basvuruAfadDurumGuncellemeTarihiKontrol)
                                                &&
                                                x.TuzelKisiTipId == null, //sadece gerçek kişileri güncelleyecez
                                                asNoTracking: true,
                                                i => i.BinaDegerlendirme
                                            )
                                            .ToListAsync();

                if (!basvuruList.Any())
                {
                    sb.AppendLine(string.Format("{0} BasvuruTopluAfadDurumUpdateJob veri tabanında güncellenebilecek uygun başvuru olmadığı için sondanlandırıldı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    result.Result = sb.ToString();
                    return result;
                }

                var basvuruAfadDurumList = _basvuruAfadDurumRepository.GetWhere(x => x.AktifMi == true && x.SilindiMi == false, true).ToList();

                int guncellenenBasvuruSayisi = 0, basvuruYokSayisi = 0;
                var metotCalismaBaslangicTarihi = DateTime.Now;

                await Parallel.ForEachAsync(basvuruList, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 100 }, async (basvuru, cancellationToken2) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    basvuru.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(basvuru.HasarTespitAskiKodu);
                    //ada parsel verisi olan afad başvuru varsa alınır yoksa ada parsel şartına bakılmadan kontrol edilir
                    var afadBasvuru = afadBasvuruList.FirstOrDefault(x => x.Tckn.ToString() == basvuru.TcKimlikNo
                                                && HasarTespitAddon.AskiKoduToUpper(x.AskiKodu) == basvuru.HasarTespitAskiKodu
                                                && x.IlId == basvuru.UavtIlNo
                                                && x.Ada == basvuru.TapuAda
                                                && x.Parsel == basvuru.TapuParsel
                                        )
                                        ?? afadBasvuruList.FirstOrDefault(x => x.Tckn.ToString() == basvuru.TcKimlikNo
                                                && HasarTespitAddon.AskiKoduToUpper(x.AskiKodu) == basvuru.HasarTespitAskiKodu
                                                && x.IlId == basvuru.UavtIlNo
                                            );

                    if (afadBasvuru != null)
                    {
                        //afad tarafında başvurusu var
                        if (afadBasvuru.DegerlendirmeIptalDurumu?.ToLower().Trim() == "evet")
                        {
                            basvuru.BasvuruAfadDurumId = (long)BasvuruAfadDurumEnum.Iptal;
                            basvuru.BasvuruAfadDurumGuncellemeTarihi = DateTime.Now;

                            guncellenenBasvuruSayisi++;
                        }
                        else
                        {
                            var afadDurum = basvuruAfadDurumList.FirstOrDefault(x => x.Ad.ToLower() == (afadBasvuru.ItirazDegerlendirmeSonucu ?? afadBasvuru.DegerlendirmeDurumu)?.ToLower().Replace("afad", "").Trim());
                            if (afadDurum != null)
                            {
                                basvuru.BasvuruAfadDurumId = afadDurum.BasvuruAfadDurumId;
                                basvuru.BasvuruAfadDurumGuncellemeTarihi = DateTime.Now;

                                guncellenenBasvuruSayisi++;
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

                        basvuruYokSayisi++;
                    }
                });

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