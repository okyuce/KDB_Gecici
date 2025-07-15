using Csb.YerindeDonusum.Application.CQRS.HasarTespitCQRS.Queries;
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

public class GuncelleTopluBasvuruHuIdCommand : IRequest<ResultModel<string>>
{
    public class GuncelleTopluBasvuruHuIdCommandHandler : IRequestHandler<GuncelleTopluBasvuruHuIdCommand, ResultModel<string>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IMediator _mediator;

        public GuncelleTopluBasvuruHuIdCommandHandler( IBasvuruRepository basvuruRepository, IMediator mediator)
        {
            _basvuruRepository = basvuruRepository;
            _mediator = mediator;
        }

        public async Task<ResultModel<string>> Handle(GuncelleTopluBasvuruHuIdCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            try
            {
                StringBuilder sb = new();

                sb.AppendLine(string.Format("{0} GuncelleTopluBasvuruHuId çalışmaya başladı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                var dtControl = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

                var basvuruListe = _basvuruRepository
                                           .GetWhere(x =>
                                                x.Huid == null
                                                &&
                                                x.HuidKontrolTarihi == null
                                                &&
                                                x.UavtIcKapiNo != null
                                                &&
                                                !x.SilindiMi
                                                &&
                                                x.AktifMi == true
                                                ,
                                                asNoTracking: false
                                            ).ToList();

                if (!basvuruListe.Any())
                {
                    sb.AppendLine(string.Format("{0} Başvuru tablosunda güncellenebilecek uygun başvuru olmadığı için sondanlandırıldı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    result.Result = sb.ToString();
                    return result;
                }


                int guncellenenBasvuruSayisi = 0, huidSorgulanamadiSayisi = 0;
                var kontrolTarihi = DateTime.Now;
                await Parallel.ForEachAsync(basvuruListe, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (basvuru, cancellationToken2) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var huidSorguSonucu = await _mediator.Send(new GetHuIdQuery()
                    {
                        HasarTespitAskiKodu = basvuru.HasarTespitAskiKodu,
                        IcKapiNo = basvuru.UavtIcKapiNo
                    });

                    if (huidSorguSonucu != null)
                    {
                        if (huidSorguSonucu.IsError == false)
                        {
                            if (!string.IsNullOrWhiteSpace(huidSorguSonucu.Result))
                            {
                                basvuru.Huid = huidSorguSonucu.Result?.Replace("\"","").Trim();
                            }

                            basvuru.HuidKontrolTarihi = kontrolTarihi;

                            Interlocked.Increment(ref guncellenenBasvuruSayisi);
                        }
                        else
                        {
                            sb.AppendLine(string.Format("{0} Hasar Tespit hane sorgusu sırasında hata oluştu! Sorgu Sonucu: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), JsonConvert.SerializeObject(huidSorguSonucu)));
                        }

                    }
                    else
                    {
                        basvuru.HuidKontrolTarihi = kontrolTarihi;

                        Interlocked.Increment(ref huidSorgulanamadiSayisi);
                    }
                });

                //_basvuruRepository.UpdateRange(basvuruListe);

                await _basvuruRepository.SaveChanges();

                sb.AppendLine(string.Format("{0} Update işlemi tamamlandı. Toplam kayıt sayısı: {1}, güncellenen kayıt sayısı: {2}, sorgulanamayan kayıt sayısı: {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), basvuruListe.Count, guncellenenBasvuruSayisi, huidSorgulanamadiSayisi));

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