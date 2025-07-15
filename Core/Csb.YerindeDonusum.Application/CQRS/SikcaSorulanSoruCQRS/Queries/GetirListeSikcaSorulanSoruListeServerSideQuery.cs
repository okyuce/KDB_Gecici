using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS;

public class GetirListeSikcaSorulanSoruListeServerSideQuery : IRequest<ResultModel<DataTableResponseModel<List<SikcaSorulanSoruServerSideDto>>>>
{
    public GetirListeSikcaSorulanSoruListeServerSideQueryModel Model { get; set; }

    public class GetirListeSikcaSorulanSoruListeServerSideQueryHandler : IRequestHandler<GetirListeSikcaSorulanSoruListeServerSideQuery, ResultModel<DataTableResponseModel<List<SikcaSorulanSoruServerSideDto>>>>
    {
        private readonly IMapper _mapper;
        private readonly ISikcaSorulanSoruRepository _repository;

        public GetirListeSikcaSorulanSoruListeServerSideQueryHandler(IMapper mapper, ISikcaSorulanSoruRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<SikcaSorulanSoruServerSideDto>>>> Handle(GetirListeSikcaSorulanSoruListeServerSideQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.GetWhere(x => !x.SilindiMi);

            if (!string.IsNullOrEmpty(request.Model?.search?.value))
                query = query.Where(x => x.Soru.ToLower().Contains(request.Model.search.value.ToLower().Trim()));

            return await query.OrderBy(o => o.SiraNo).Paginate<SikcaSorulanSoruServerSideDto, Domain.Entities.SikcaSorulanSoru>(request.Model, _mapper);
        }
    }
}