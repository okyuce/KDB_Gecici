using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuranKisiSayisi;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Hubs;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.IptalBasvuruById;

public class IptalBasvuruByIdCommand : IRequest<ResultModel<string>>
{
    public string? BasvuruId { get; set; }

    public string? TcKimlikNo { get; set; }

    public class CancelledAppedByIdCommandHandler : IRequestHandler<IptalBasvuruByIdCommand, ResultModel<string>>
    {
        private readonly IBasvuruRepository _appealRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IMediator _mediator;
        private readonly IHubContext<KdsHub> _hubContext;

        public CancelledAppedByIdCommandHandler(IBasvuruRepository appealRepository, IKullaniciBilgi kullaniciBilgi, IMediator mediator, IHubContext<KdsHub> hubContext)
        {
            _appealRepository = appealRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _mediator = mediator;
            _hubContext = hubContext;
        }

        public async Task<ResultModel<string>> Handle(IptalBasvuruByIdCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            if (request == null)
            {
                result.Exception(new ArgumentNullException("Başvuru İptal İşlemi Gerçekleştirilemedi."), "Başvuru İptal İşlemi Gerçekleştirilemedi.");

                return await Task.FromResult(result);
            }

            try
            {
                Guid.TryParse(request?.BasvuruId, out Guid basvuruGuid);

                if (basvuruGuid == Guid.Empty)
                {
                    result.Exception(new ArgumentNullException("Geçersiz veya Hatalı Başvuru Numarası."), "Geçersiz veya Hatalı Başvuru Numarası.");

                    return await Task.FromResult(result);
                }
                else if (request?.TcKimlikNo?.IsNullOrEmpty() == true)
                {
                    result.Exception(new ArgumentNullException("T.C. Kimlik Numarası Boş Olamaz."), "T.C. Kimlik Numarası Boş Olamaz.");

                    return await Task.FromResult(result);
                }

                var appealResult = _appealRepository.GetWhere(x => x.BasvuruGuid == basvuruGuid && x.TcKimlikNo == request.TcKimlikNo.Trim() && x.AktifMi == true && x.SilindiMi == false, false, i => i.BasvuruDurum, i=>i.BinaDegerlendirme).FirstOrDefault();

                if (appealResult == null)
                {
                    result.Exception(new NullReferenceException($"{basvuruGuid} - Başvuru Bilgisine Ait Kayıt Bulunmamaktadır."), "Başvuru Bilgisine Ait Kayıt Bulunmamaktadır.");

                    return await Task.FromResult(result);
                }
                else if (appealResult.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzAlinmistir && appealResult.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzDegerlendirmeAsamasindadir)
                {
                    result.Exception(new InvalidFilterCriteriaException($"{basvuruGuid} - Başvuru İşleminizin Durumu: '{appealResult.BasvuruDurum.Ad}' Olduğu İçin İptal Edilemez."), $"Başvuru İşleminizin Durumu: '{appealResult.BasvuruDurum.Ad}' Olduğu İçin İptal Edilemez.");

                    return await Task.FromResult(result);
                }
                if (appealResult.BinaDegerlendirme != null && appealResult.AktifMi == true && appealResult.SilindiMi == false && appealResult.BinaDegerlendirme.AktifMi == true && appealResult.BinaDegerlendirme.SilindiMi == false && (appealResult.BinaDegerlendirme.BultenNo != null || appealResult.BinaDegerlendirme.IzinBelgesiTarih != null))
                {
                    result.Exception(new InvalidFilterCriteriaException($"{basvuruGuid} - Başvurunun Bina Değerlendirme Durumu: '{BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir.GetDisplayName()}' Olduğu İçin İptal Edemezsiniz."), $"Başvurunun Bina Değerlendirme Durumu: '{BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir.GetDisplayName()}' Olduğu İçin İptal Edemezsiniz.");

                    return await Task.FromResult(result);
                }
                var userInfo = _kullaniciBilgi.GetUserInfo();

                long.TryParse(userInfo.KullaniciId, out long kullaniciId);

                appealResult.BasvuruDurumId = (long)BasvuruDurumEnum.BasvuruIptalEdildi;
                appealResult.GuncellemeTarihi = DateTime.Now;
                appealResult.GuncelleyenIp = userInfo.IpAdresi;

                if (kullaniciId > 0)
                    appealResult.GuncelleyenKullaniciId = kullaniciId;

                // degisiligi db'ye yansitiyoruz
                await _appealRepository.SaveChanges(cancellationToken);

                #region ...: SignalR Hub :...

                await _hubContext.Clients.All.SendAsync("basvuruSayisi", new
                {
                    basvuruSayisi = _appealRepository.Count(p => p.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzAlinmistir && p.AktifMi == true && !p.SilindiMi)
                });

                var basvuranKisiSayisi = await _mediator.Send(new GetirBasvuranKisiSayisiQuery());
                if (!basvuranKisiSayisi.IsError)
                {
                    await _hubContext.Clients.All.SendAsync("basvuranSayisi", new
                    {
                        basvuranKisiSayisi.Result.BasvuranGercekKisiSayisi,
                        basvuranKisiSayisi.Result.BasvuranTuzelKisiSayisi
                    });
                }

                #endregion

                result.Result = "Başvurunuz Başarılı Bir Şekilde İptal Edilmiştir.";
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Başvuru İptal İşlemi Sırasında Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}
