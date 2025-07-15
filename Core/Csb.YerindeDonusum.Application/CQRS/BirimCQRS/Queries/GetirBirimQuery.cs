using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BirimCQRS;

public class GetirBirimListeQuery : IRequest<ResultModel<List<BirimDto>>>
{
    public class GetirBirimuListeQueryHandler : IRequestHandler<GetirBirimListeQuery, ResultModel<List<BirimDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IBirimRepository _repository;
        
        public GetirBirimuListeQueryHandler(IMapper mapper, IBirimRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<List<BirimDto>>> Handle(GetirBirimListeQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<List<BirimDto>>(_repository.GetAll().ToList());
            return Task.FromResult(new ResultModel<List<BirimDto>>(result));
        }
    }
}