using Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles.Interfaces;
using MediatR;
namespace Csb.YerindeDonusum.WebApi.BasicAuthFiles.Services;
public class AuthenticationService : IBasicAuthenticationService
{
    private readonly IMediator _mediator;

    public AuthenticationService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<KullaniciSonucDto> IsValidUserAsync(string kullaniciAdi, string sifre)
    {
        var userResult = await _mediator.Send(new KullaniciGirisBasicAuthQuery { KullaniciAdi = kullaniciAdi, Sifre = sifre });

        if (!userResult.IsError)
            return userResult.Result;

        return null;
    }
}