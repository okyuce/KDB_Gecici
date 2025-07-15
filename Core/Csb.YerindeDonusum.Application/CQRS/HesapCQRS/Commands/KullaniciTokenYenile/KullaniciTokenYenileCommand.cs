using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Claims;

namespace Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciTokenYenile;

public class KullaniciTokenYenileCommand : IRequest<ResultModel<TokenDto>>
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }

    public class KullaniciTokenYenileCommandHandler : IRequestHandler<KullaniciTokenYenileCommand, ResultModel<TokenDto>>
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IKullaniciRepository _kullaniciRepository;
        private JwtOptionModel JwtOptions { get; set; }

        public KullaniciTokenYenileCommandHandler(IServiceProvider serviceProvider, IMapper mapper, IHttpContextAccessor contextAccessor, IKullaniciRepository kullaniciRepository)
        {
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _kullaniciRepository = kullaniciRepository;
            JwtOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtOptionModel>>().CurrentValue;
        }

        public async Task<ResultModel<TokenDto>> Handle(KullaniciTokenYenileCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<TokenDto>();

            var jwtAddon = new JwtAddon(_contextAccessor, JwtOptions);

            var principal = jwtAddon.GetPrincipalFromExpiredToken(request.AccessToken);

            long.TryParse((principal.Identity as ClaimsIdentity)?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "0", out long kullaniciId);

            var kullanici = _kullaniciRepository.GetAllQueryable(x =>
                x.KullaniciId == kullaniciId
                &&
                x.AktifMi == true
                &&
                x.SilindiMi == false
            )
            .Include(x => x.KullaniciRols.Where(y => !y.SilindiMi && y.AktifMi == true))
            .ThenInclude(x => x.Rol)
            .Include(x => x.Birim)
            .FirstOrDefault();

            if (kullanici == null)
            {
                result.Exception(new NullReferenceException("Geçersiz veya Hatalı Kullanıcı Bilgisi! ERR-KTYCH-200"));
                return await Task.FromResult(result);
            }
            else if (kullanici.RefreshToken != request.RefreshToken)
            {
                result.Exception(new NullReferenceException("Refresh token hatalı! ERR-KTYCH-300"));
                return await Task.FromResult(result);
            }

            kullanici.RefreshToken = jwtAddon.GenerateRefreshToken();

            await _kullaniciRepository.SaveChanges(cancellationToken);

            result.Result = new TokenDto()
            {
                AccessToken = jwtAddon.GenerateJwt(kullanici),
                RefreshToken = kullanici.RefreshToken
            };

            return await Task.FromResult(result);
        }
    }
}