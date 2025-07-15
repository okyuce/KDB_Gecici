using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands
{
	public class SilKullaniciCommand : IRequest<ResultModel<SilKullaniciCommandResponseModel>>
    {
		//public SilKullaniciCommandModel Model { get; set; }

		public long? KullaniciId { get; set; }

		public class SilKullaniciCommandHandler : IRequestHandler<SilKullaniciCommand, ResultModel<SilKullaniciCommandResponseModel>>
        {
            private readonly IKullaniciRepository _kullaniciRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly IKullaniciBilgi _kullaniciBilgi;
            private readonly ICacheService _cacheService;

            public SilKullaniciCommandHandler(IKullaniciRepository kullaniciRepository,
                ICacheService cacheService,
                IWebHostEnvironment webHostEnvironment,
                IKullaniciBilgi kullaniciBilgi
            )
            {
                _webHostEnvironment = webHostEnvironment;
                _cacheService = cacheService;
                _kullaniciRepository = kullaniciRepository;
                _kullaniciBilgi = kullaniciBilgi;
            }

            public async Task<ResultModel<SilKullaniciCommandResponseModel>> Handle(SilKullaniciCommand request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<SilKullaniciCommandResponseModel>();

				var kullanici = await _kullaniciRepository.GetWhere(x =>
					x.SistemKullanicisiMi != true //sistem kullanıcıları dışındakiler güncellenebilir
                    &&
					x.KullaniciId == request.KullaniciId
					&&
					!x.SilindiMi
				).FirstOrDefaultAsync();

				if (kullanici == null)
				{
					result.ErrorMessage("Kullanıcı bulunamadı.");
					return await Task.FromResult(result);
				}

				long.TryParse(_kullaniciBilgi.GetUserInfo().KullaniciId, out long kullaniciId);
				var ipAdresi = _kullaniciBilgi.GetUserInfo().IpAdresi;

				kullanici.GuncelleyenKullaniciId = kullaniciId;
				kullanici.GuncelleyenIp = ipAdresi;
				kullanici.GuncellemeTarihi = DateTime.Now;
				kullanici.SilindiMi = true;

				await _kullaniciRepository.SaveChanges(cancellationToken);

				var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirKullaniciListeQuery)}";
				await _cacheService.Clear(cacheKey);

				result.Result = new SilKullaniciCommandResponseModel
				{
					Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
				};

				return await Task.FromResult(result);
            }
        }
    }
}