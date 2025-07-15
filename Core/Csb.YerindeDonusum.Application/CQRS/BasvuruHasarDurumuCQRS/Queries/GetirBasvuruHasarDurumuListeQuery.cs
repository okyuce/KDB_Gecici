using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using System;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruHasarDurumuCQRS;

public class GetirBasvuruHasarDurumuListeQuery : IRequest<ResultModel<List<BasvuruHasarDurumuDto>>>
{
    public class GetirBasvuruHasarDurumuListeQueryHandler : IRequestHandler<GetirBasvuruHasarDurumuListeQuery, ResultModel<List<BasvuruHasarDurumuDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _repository;
        
        public GetirBasvuruHasarDurumuListeQueryHandler(IMapper mapper, IBasvuruRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<List<BasvuruHasarDurumuDto>>> Handle(GetirBasvuruHasarDurumuListeQuery request, CancellationToken cancellationToken)
        {
            var result = _repository.GetAllQueryable(x=> !string.IsNullOrWhiteSpace(x.HasarTespitHasarDurumu)).OrderBy(x=> x.HasarTespitHasarDurumu)
                              .GroupBy(p => p.HasarTespitHasarDurumu)
                              .Select(g => new BasvuruHasarDurumuDto()
                              {
                                  Ad = g.First().HasarTespitHasarDurumu,
                              }).ToList();

            //var result = _mapper.Map<List<BasvuruHasarDurumuDto>>(_repository.GetAll().ToList());

            return Task.FromResult(new ResultModel<List<BasvuruHasarDurumuDto>>(result));
        }
    }
}