using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS;

public class GetirListeKullaniciQuery : IRequest<ResultModel<List<KullaniciListeDto>>>
{
    public GetirListeKullaniciQueryModel Model { get; set; }

    public class GetirListeKullaniciListeServerSideQueryHandler : IRequestHandler<GetirListeKullaniciQuery, ResultModel<List<KullaniciListeDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKullaniciRepository _repository;

        public GetirListeKullaniciListeServerSideQueryHandler(IMapper mapper, IKullaniciRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ResultModel<List<KullaniciListeDto>>> Handle(GetirListeKullaniciQuery request, CancellationToken cancellationToken)
        {
            var kullaniciListe = _repository
                                    .GetWhere(x =>
                                        !x.SilindiMi
                                        &&
                                        x.SistemKullanicisiMi != true, //sistem kullanıcıları dışındakileri listeye getir,
                                        true,
                                        i => i.Birim,
                                        i => i.KullaniciHesapTip
                                    )
                                    .Include(x => x.KullaniciRols.Where(y => !y.SilindiMi && y.AktifMi == true))
                                        .ThenInclude(x => x.Rol)
                                    .ToList();

            return new ResultModel<List<KullaniciListeDto>>(_mapper.Map<List<KullaniciListeDto>>(kullaniciListe));
        }
    }
}