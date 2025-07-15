using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.DestekTurCQRS.Queries;

public class GetirBasvuruDestekTurListeQuery : IRequest<ResultModel<List<GetirBasvuruDestekTurListeResponseModel>>>
{
    public class GetirBasvuruDestekTuruListeQueryHandler : IRequestHandler<GetirBasvuruDestekTurListeQuery, ResultModel<List<GetirBasvuruDestekTurListeResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruDestekTurRepository _repository;
        
        public GetirBasvuruDestekTuruListeQueryHandler(IMapper mapper, IBasvuruDestekTurRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<List<GetirBasvuruDestekTurListeResponseModel>>> Handle(GetirBasvuruDestekTurListeQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<List<GetirBasvuruDestekTurListeResponseModel>>(_repository.GetAll().ToList());
            return Task.FromResult(new ResultModel<List<GetirBasvuruDestekTurListeResponseModel>>(result));
        }
    }
}