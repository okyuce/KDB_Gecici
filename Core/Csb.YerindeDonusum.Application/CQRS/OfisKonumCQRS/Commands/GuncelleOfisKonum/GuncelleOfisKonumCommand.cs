using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Queries;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Commands
{
    public class GuncelleOfisKonumCommand : IRequest<ResultModel<GuncelleOfisKonumCommandResponseModel>>
    {
		public long? OfisKonumId { get; set; }
        public string? IlAdi { get; set; }
        public string? IlceAdi { get; set; }
        public string? Adres { get; set; }
        public string? HaritaUrl { get; set; }
        public string? Konum { get; set; }
        public bool? AktifMi { get; set; }
        public string? Koordinat { get; set; }
        public double? Enlem { get; set; }
        public double? Boylam { get; set; }

        public class GuncelleOfisKonumCommandHandler : IRequestHandler<GuncelleOfisKonumCommand, ResultModel<GuncelleOfisKonumCommandResponseModel>>
        {
            private readonly IMapper _mapper;
            private readonly IOfisKonumRepository _repository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly ICacheService _cacheService;
            private readonly IKullaniciBilgi _kullaniciBilgi;

            public GuncelleOfisKonumCommandHandler(IMapper mapper
                , IOfisKonumRepository repository,
                IWebHostEnvironment webHostEnvironment,
                ICacheService cacheService
                , IKullaniciBilgi kullaniciBilgi
            ) {
                _cacheService = cacheService;
                _webHostEnvironment = webHostEnvironment;
                _mapper = mapper;
				_repository = repository;
                _kullaniciBilgi = kullaniciBilgi;
            }

            public async Task<ResultModel<GuncelleOfisKonumCommandResponseModel>> Handle(GuncelleOfisKonumCommand request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<GuncelleOfisKonumCommandResponseModel>();

                var ofisKonum = await _repository
                    .GetAllQueryable(x => x.OfisKonumId == request.OfisKonumId && !x.SilindiMi)
                    .Select(s => new OfisKonum
                    {
                        OfisKonumId = s.OfisKonumId,
                        SilindiMi = s.SilindiMi,
                        OlusturmaTarihi = s.OlusturmaTarihi,
                        OlusturanIp = s.GuncelleyenIp,
                        OlusturanKullaniciId = s.OlusturanKullaniciId
                    })
                    .FirstOrDefaultAsync();

                if (ofisKonum == null)
                {
                    result.ErrorMessage("Ofis konumu bulunamadı.");
                    return await Task.FromResult(result);
                }

                long.TryParse(_kullaniciBilgi.GetUserInfo()?.KullaniciId, out long kullaniciId);

                ofisKonum.IlAdi = request.IlAdi?.Trim();
                ofisKonum.IlceAdi = request.IlceAdi?.Trim();
                ofisKonum.Adres = request.Adres?.Trim();
                ofisKonum.Konum = new NetTopologySuite.IO.WKTReader().Read(request.Konum);
                ofisKonum.HaritaUrl = request?.HaritaUrl ?? "http://maps.google.com/?q=" + ofisKonum.Konum.InteriorPoint.X.ToString().Replace(",", ".") + ", " + ofisKonum.Konum.InteriorPoint.Y.ToString().Replace(",", ".");
                ofisKonum.AktifMi = request.AktifMi;
                ofisKonum.GuncelleyenKullaniciId = kullaniciId;
                ofisKonum.GuncellemeTarihi = DateTime.Now;
                ofisKonum.GuncelleyenIp = _kullaniciBilgi.GetUserInfo()?.IpAdresi;

                _repository.Update(ofisKonum);
				await _repository.SaveChanges();

                var cacheKey1 = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirOfisKonumListeQuery)}";
                var cacheKey2 = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirOfisKonumListeDetayliQuery)}";
                await _cacheService.Clear(cacheKey1);
                await _cacheService.Clear(cacheKey2);

                result.Result = new GuncelleOfisKonumCommandResponseModel
                {
                    Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
                };

                return await Task.FromResult(result);
            }
        }
    }
}