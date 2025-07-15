using Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Queries;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Commands
{
    public class SilOfisKonumCommand : IRequest<ResultModel<SilOfisKonumCommandResponseModel>>
    {
		//public SilOfisKonumCommandModel Model { get; set; }

		public long? OfisKonumId { get; set; }

		public class SilOfisKonumCommandHandler : IRequestHandler<SilOfisKonumCommand, ResultModel<SilOfisKonumCommandResponseModel>>
        {
            private readonly IOfisKonumRepository _repository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly ICacheService _cacheService;
            private readonly IKullaniciBilgi _kullaniciBilgi;   

            public SilOfisKonumCommandHandler(IOfisKonumRepository repository,
                ICacheService cacheService,
                IWebHostEnvironment webHostEnvironment,
                IKullaniciBilgi kullaniciBilgi
            ) {
                _webHostEnvironment = webHostEnvironment;
                _cacheService = cacheService;
                _repository = repository;
                _kullaniciBilgi = kullaniciBilgi;
            }

            public async Task<ResultModel<SilOfisKonumCommandResponseModel>> Handle(SilOfisKonumCommand request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<SilOfisKonumCommandResponseModel>();

                var ofisKonum = await _repository
                    .GetWhere(x => x.OfisKonumId == request.OfisKonumId && !x.SilindiMi)
                    .Select(s => new OfisKonum
                    {
                        OfisKonumId = s.OfisKonumId,
                        IlAdi = s.IlAdi,
                        IlceAdi = s.IlceAdi,
                        Adres = s.Adres,
                        HaritaUrl = s.HaritaUrl,
                        Konum = s.Konum.InteriorPoint,
                        AktifMi = s.AktifMi,
                        OlusturmaTarihi = s.OlusturmaTarihi,
                        OlusturanIp = s.GuncelleyenIp,
                        OlusturanKullaniciId = s.OlusturanKullaniciId
                    })
                    .FirstOrDefaultAsync();

                if (ofisKonum == null)
                {
                    result.ErrorMessage("Ofis konum bulunamadı.");
                    return await Task.FromResult(result);
                }

                long.TryParse(_kullaniciBilgi.GetUserInfo()?.KullaniciId, out long kullaniciId);

                ofisKonum.SilindiMi = true;
                ofisKonum.GuncelleyenKullaniciId = kullaniciId;
                ofisKonum.GuncellemeTarihi = DateTime.Now;
                ofisKonum.GuncelleyenIp = _kullaniciBilgi.GetUserInfo()?.IpAdresi;

                _repository.Update(ofisKonum);
                await _repository.SaveChanges();

				var cacheKey1 = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirOfisKonumListeQuery)}";
                var cacheKey2 = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirOfisKonumListeDetayliQuery)}";
                await _cacheService.Clear(cacheKey1);
                await _cacheService.Clear(cacheKey2);

                result.Result = new SilOfisKonumCommandResponseModel
                {
                    Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
                };

                return await Task.FromResult(result);
            }
        }
    }
}