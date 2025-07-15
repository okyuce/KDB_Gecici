using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciGirisBasariliCQRS.Commands.EkleKullaniciGirisBasarili;

public class EkleKullaniciGirisBasariliCommand : IRequest<ResultModel<EkleKullaniciGirisBasariliCommandResponseModel>>
{
    public string KullaniciAdi { get; set; }
    public string Sifre { get; set; }

    public class EkleKullaniciGirisBasariliCommandHandler : IRequestHandler<EkleKullaniciGirisBasariliCommand, ResultModel<EkleKullaniciGirisBasariliCommandResponseModel>>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IKullaniciGirisBasariliRepository _kullaniciGirisBasariliRepository;

        public EkleKullaniciGirisBasariliCommandHandler(IHttpContextAccessor contextAccessor, IKullaniciGirisBasariliRepository kullaniciGirisBasariliRepository)
        {
            _contextAccessor = contextAccessor;
            _kullaniciGirisBasariliRepository = kullaniciGirisBasariliRepository;
        }

        public async Task<ResultModel<EkleKullaniciGirisBasariliCommandResponseModel>> Handle(EkleKullaniciGirisBasariliCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<EkleKullaniciGirisBasariliCommandResponseModel>();

            var headerKeyValues = new Dictionary<string, string>();

            foreach (var headerKey in _contextAccessor.HttpContext.Request.Headers.Keys)
                headerKeyValues.TryAdd(headerKey, _contextAccessor.HttpContext.Request.Headers[headerKey]);

            await _kullaniciGirisBasariliRepository.AddAsync(new KullaniciGirisBasarili
            {
                KullaniciAdi = request.KullaniciAdi,
                Sifre = request.Sifre,
                IpAdres = _contextAccessor?.HttpContext?.GetIpAddress(),
                RequestHeader = headerKeyValues.Any() ? JsonConvert.SerializeObject(headerKeyValues) : null,
                OlusturmaTarihi = DateTime.Now
            });
            await _kullaniciGirisBasariliRepository.SaveChanges(cancellationToken);
            
            result.Result = new EkleKullaniciGirisBasariliCommandResponseModel { Basari = true };
            return await Task.FromResult(result);
        }
    }
}