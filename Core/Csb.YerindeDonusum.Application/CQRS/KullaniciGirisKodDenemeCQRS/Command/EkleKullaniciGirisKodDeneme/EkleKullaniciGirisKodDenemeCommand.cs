using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
namespace Csb.YerindeDonusum.Application.CQRS.KullaniciGirisKodDenemeCQRS.Command.EkleKullaniciGirisKodDeneme;

public class EkleKullaniciGirisKodDenemeCommand: IRequest<ResultModel<string>>
{
    public string GirisGuid { get; set; }
    public string Code { get; set; }
    public string IpAdres { get; set; }

    public class EkleKullaniciGirisKodDenemeCommandHandler : IRequestHandler<EkleKullaniciGirisKodDenemeCommand, ResultModel<string>>
    {
        private readonly IKullaniciGirisKodDenemeRepository _kullaniciGirisKodDenemeRepository;

        public EkleKullaniciGirisKodDenemeCommandHandler(IKullaniciGirisKodDenemeRepository kullaniciGirisKodDenemeRepository)
        {
            _kullaniciGirisKodDenemeRepository = kullaniciGirisKodDenemeRepository;
        }

        public async Task<ResultModel<string>> Handle(EkleKullaniciGirisKodDenemeCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            await _kullaniciGirisKodDenemeRepository.AddAsync(new KullaniciGirisKodDeneme
            {
                Code = request.Code,
                IpAdres = request.IpAdres,
                GirisGuid = request.GirisGuid,
                OlusturmaTarihi = DateTime.Now
            });
            await _kullaniciGirisKodDenemeRepository.SaveChanges(cancellationToken);

            return await Task.FromResult(result);
        }
    }
}


