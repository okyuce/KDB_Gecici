using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruTurCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruTurCQRS;

public class GetirBasvuruTurListeQuery : IRequest<ResultModel<List<GetirBasvuruTurListeResponseModel>>>
{
    public class GetirBasvuruTuruListeQueryHandler : IRequestHandler<GetirBasvuruTurListeQuery, ResultModel<List<GetirBasvuruTurListeResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruTurRepository _repository;
        
        public GetirBasvuruTuruListeQueryHandler(IMapper mapper, IBasvuruTurRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<List<GetirBasvuruTurListeResponseModel>>> Handle(GetirBasvuruTurListeQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<List<GetirBasvuruTurListeResponseModel>>(_repository.GetAll().ToList());
            return Task.FromResult(new ResultModel<List<GetirBasvuruTurListeResponseModel>>(result));
        }
    }
}