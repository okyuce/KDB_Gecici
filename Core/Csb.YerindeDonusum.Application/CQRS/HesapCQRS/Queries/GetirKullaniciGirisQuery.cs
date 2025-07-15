using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Queries;

public class GetirKullaniciGirisQuery : IRequest<ResultModel<TokenDto>>
{
    public string KullaniciAdi { get; set; } = string.Empty;
    public string Sifre { get; set; } = string.Empty;
    
    
    public class GetirKullaniciGirisQueryHandler : IRequestHandler<GetirKullaniciGirisQuery, ResultModel<TokenDto>>
    {
        public Task<ResultModel<TokenDto>> Handle(GetirKullaniciGirisQuery request, CancellationToken cancellationToken)
        {
            
            throw new NotImplementedException();
        }
    }
}