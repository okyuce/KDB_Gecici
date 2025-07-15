using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.DosyaTurCQRS;

public class GetirBasvuruDosyaTurListeQuery : IRequest<ResultModel<List<BasvuruDosyaTurDto>>>
{
    public class GetirBasvuruDosyaTurListeQueryHandler : IRequestHandler<GetirBasvuruDosyaTurListeQuery, ResultModel<List<BasvuruDosyaTurDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruDosyaTurRepository _repository;
        
        public GetirBasvuruDosyaTurListeQueryHandler(IMapper mapper, IBasvuruDosyaTurRepository AppealFileTypesRepository)
        {
            _mapper = mapper;
            _repository = AppealFileTypesRepository;
        }
        
        public Task<ResultModel<List<BasvuruDosyaTurDto>>> Handle(GetirBasvuruDosyaTurListeQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<List<BasvuruDosyaTurDto>>(_repository.GetAll().ToList());
            return Task.FromResult(new ResultModel<List<BasvuruDosyaTurDto>>(result));
        }
    }
}