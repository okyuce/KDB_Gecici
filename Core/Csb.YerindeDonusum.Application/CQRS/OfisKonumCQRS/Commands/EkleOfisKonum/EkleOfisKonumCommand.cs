using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Queries;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Commands.EkleOfisKonum;

public class EkleOfisKonumCommand : IRequest<ResultModel<EkleOfisKonumCommandResponseModel>>
{
    public string? IlAdi { get; set; }
    public string? IlceAdi { get; set; }
    public string? Adres { get; set; }
    public string? HaritaUrl { get; set; }
    public string? Konum { get; set; }
    public bool? AktifMi { get; set; }
    public string? Koordinat { get; set; }
    public double? Enlem { get; set; }
    public double? Boylam { get; set; }  
}

public class EkleOfisKonumCommandHandler : IRequestHandler<EkleOfisKonumCommand, ResultModel<EkleOfisKonumCommandResponseModel>>
{
    private readonly IMapper _mapper;
    private readonly IOfisKonumRepository _repository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICacheService _cacheService;
    private readonly IKullaniciBilgi _kullaniciBilgi;

    public EkleOfisKonumCommandHandler(IMapper mapper
        , IOfisKonumRepository repository
        , ICacheService cacheService
        , IWebHostEnvironment webHostEnvironment
        , IKullaniciBilgi kullaniciBilgi
    )
    {
        _cacheService = cacheService;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
        _repository = repository;
        _kullaniciBilgi = kullaniciBilgi;
    }

    public async Task<ResultModel<EkleOfisKonumCommandResponseModel>> Handle(EkleOfisKonumCommand request, CancellationToken cancellationToken)
    {
        var result = new ResultModel<EkleOfisKonumCommandResponseModel>();

        long.TryParse(_kullaniciBilgi.GetUserInfo()?.KullaniciId, out long kullaniciId);

            var ofisKonum = new OfisKonum()
            {
                IlAdi = request.IlAdi?.Trim(),
                IlceAdi = request.IlceAdi?.Trim(),
                Adres = request.Adres?.Trim(),
                Konum = new NetTopologySuite.IO.WKTReader().Read(request.Konum),
                AktifMi = request.AktifMi,
                SilindiMi = false,
                OlusturanKullaniciId = kullaniciId,
                OlusturmaTarihi = DateTime.Now,
                OlusturanIp = _kullaniciBilgi.GetUserInfo()?.IpAdresi,
            };

            ofisKonum.HaritaUrl = request?.HaritaUrl ?? "http://maps.google.com/?q=" + ofisKonum.Konum.InteriorPoint.X.ToString().Replace(",", ".") + ", " + ofisKonum.Konum.InteriorPoint.Y.ToString().Replace(",", ".");

            await _repository.AddAsync(ofisKonum);
            await _repository.SaveChanges(cancellationToken);

        var cacheKey1 = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirOfisKonumListeQuery)}";
        var cacheKey2 = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirOfisKonumListeDetayliQuery)}";
        await _cacheService.Clear(cacheKey1);
        await _cacheService.Clear(cacheKey2);

        result.Result = new EkleOfisKonumCommandResponseModel
        {
            Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
        };

        return await Task.FromResult(result);
    }
}