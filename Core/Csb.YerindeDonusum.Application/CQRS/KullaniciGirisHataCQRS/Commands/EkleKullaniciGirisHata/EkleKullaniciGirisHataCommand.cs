using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Cryptography;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciGirisHataCQRS.Commands.EkleKullaniciGirisHata;

public class EkleKullaniciGirisHataCommand : IRequest<ResultModel<string>>
{
    public string KullaniciAdi { get; set; }
    public string Sifre { get; set; }
    public string Aciklama { get; set; }
    public string Code { get; set; }
    public string GirisGuid { get; set; }

    public class EkleKullaniciGirisHataCommandHandler : IRequestHandler<EkleKullaniciGirisHataCommand, ResultModel<string>>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IKullaniciGirisHataRepository _kullaniciGirisHataRepository;

        public EkleKullaniciGirisHataCommandHandler(IHttpContextAccessor contextAccessor, IKullaniciGirisHataRepository kullaniciGirisHataRepository)
        {
            _contextAccessor = contextAccessor;
            _kullaniciGirisHataRepository = kullaniciGirisHataRepository;
        }

        public async Task<ResultModel<string>> Handle(EkleKullaniciGirisHataCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            var headerKeyValues = new Dictionary<string, string>();

            foreach (var headerKey in _contextAccessor.HttpContext.Request.Headers.Keys)
                headerKeyValues.TryAdd(headerKey, _contextAccessor.HttpContext.Request.Headers[headerKey]);

            await _kullaniciGirisHataRepository.AddAsync(new KullaniciGirisHatum
            {
                KullaniciAdi = request.KullaniciAdi,
                Sifre = !string.IsNullOrWhiteSpace(request.Sifre) ? CsbCryptography.Sha256(CsbCryptography.Sha256(CsbCryptography.MD5(request.Sifre))) : request.Sifre,
                IpAdres = _contextAccessor?.HttpContext?.GetIpAddress(),
                Aciklama = request.Aciklama,
                Code = request.Code,
                GirisGuid = request.GirisGuid,
                RequestHeader = headerKeyValues.Any() ? JsonConvert.SerializeObject(headerKeyValues) : null,
                OlusturmaTarihi = DateTime.Now
            });
            await _kullaniciGirisHataRepository.SaveChanges(cancellationToken);
            
            return await Task.FromResult(result);
        }
    }
}
