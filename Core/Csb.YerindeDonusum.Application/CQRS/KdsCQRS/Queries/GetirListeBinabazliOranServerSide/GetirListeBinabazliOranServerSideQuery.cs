using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using MediatR;
using static Csb.YerindeDonusum.Application.Extensions.FluentValidationExtension;

namespace Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.GetirListeBinabazliOranServerSide;

public class GetirListeBinabazliOranServerSideQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<GetirListeBinabazliOranServerSideQueryResponseModel>>>>
{
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public int? Oran { get; set; }

    public class GetAllAppealByIdentificationNumberHandler : IRequestHandler<GetirListeBinabazliOranServerSideQuery, ResultModel<DataTableResponseModel<List<GetirListeBinabazliOranServerSideQueryResponseModel>>>>
    {
        private readonly IMapper _mapper;
        private readonly IKdsYerindedonusumBinabazliOranRepository _repository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        public GetAllAppealByIdentificationNumberHandler(IKullaniciBilgi kullaniciBilgi, IMapper mapper, IKdsYerindedonusumBinabazliOranRepository repository)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<GetirListeBinabazliOranServerSideQueryResponseModel>>>> Handle(GetirListeBinabazliOranServerSideQuery request, CancellationToken cancellationToken)
        {
            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();

            if (kullaniciBilgi.BirimIlId > 0)
                request.UavtIlNo = kullaniciBilgi.BirimIlId;   

            var query = _repository.GetAllQueryable().AsQueryable();

            if (NotEmpty(request?.UavtIlNo))
            {
                query = query.Where(x => request.UavtIlNo == x.UavtIlNo);
            }
            if (NotEmpty(request?.UavtIlceNo))
            {
                query = query.Where(x => request.UavtIlceNo == x.UavtIlceNo);
            }
            if (NotEmpty(request?.UavtMahalleNo))
            {
                query = query.Where(x => request.UavtMahalleNo == x.UavtMahalleNo);
            }
            if (NotEmpty(request?.HasarTespitAskiKodu))
            {
                request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);
                query = query.Where(x => x.HasarTespitAskiKodu == request.HasarTespitAskiKodu);
            }
                          
            if (NotEmpty(request?.Oran))
            {
                if (request.Oran == 100)
                    query = query.Where(x => request.Oran == x.Oran);
                else
                    query = query.Where(x => request.Oran < x.Oran);
            }

            return await query.OrderBy(o => o.Id).Paginate<GetirListeBinabazliOranServerSideQueryResponseModel, KdsYerindedonusumBinabazliOran>(request, _mapper);
        }
    }
}