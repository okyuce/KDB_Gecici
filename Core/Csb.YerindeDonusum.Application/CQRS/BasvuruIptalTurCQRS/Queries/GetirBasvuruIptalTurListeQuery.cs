using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruIptalTurCQRS.Queries;

public class GetirBasvuruIptalTurListeQuery : IRequest<ResultModel<List<GetirBasvuruIptalTurListeResponseModel>>>
{
    public class GetirBasvuruIptalTurListeQueryHandler : IRequestHandler<GetirBasvuruIptalTurListeQuery, ResultModel<List<GetirBasvuruIptalTurListeResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruIptalTurRepository _repository;
        
        public GetirBasvuruIptalTurListeQueryHandler(IMapper mapper, IBasvuruIptalTurRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<List<GetirBasvuruIptalTurListeResponseModel>>> Handle(GetirBasvuruIptalTurListeQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<List<GetirBasvuruIptalTurListeResponseModel>>(_repository.GetAll().ToList());
            return Task.FromResult(new ResultModel<List<GetirBasvuruIptalTurListeResponseModel>>(result));
        }
    }
}