using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Queries;

public class GetirOfisKonumDetayQuery : IRequest<ResultModel<GetirOfisKonumDetayResponseModel>>
{
    public long? OfisKonumId { get; set; }

    public class GetirOfisKonumDetayQueryHandler : IRequestHandler<GetirOfisKonumDetayQuery, ResultModel<GetirOfisKonumDetayResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IOfisKonumRepository _repository;

        public GetirOfisKonumDetayQueryHandler(IMapper mapper, IOfisKonumRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ResultModel<GetirOfisKonumDetayResponseModel>> Handle(GetirOfisKonumDetayQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirOfisKonumDetayResponseModel>();

            var ofisKonum = await _repository.GetWhere(x =>
                    x.OfisKonumId == request.OfisKonumId
                    &&
                    !x.SilindiMi
                    &&
                    x.Konum.GeometryType.Equals("POINT")
                )
                .Select(s => new GetirOfisKonumDetayResponseModel
                {
                    OfisKonumId = s.OfisKonumId,
                    IlAdi = s.IlAdi,
                    IlceAdi = s.IlceAdi,
                    Adres = s.Adres,
                    HaritaUrl = s.HaritaUrl,
                    Enlem = s.Konum.InteriorPoint.X,
                    Boylam = s.Konum.InteriorPoint.Y,
                    AktifMi = s.AktifMi
                })
                .FirstOrDefaultAsync();

            if (ofisKonum != null)
                result.Result = ofisKonum;
            else
                result.ErrorMessage("Ofis konum bulunamadý!");

            return await Task.FromResult(result);
        }
    }
}