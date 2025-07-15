using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBelediyeBasvuruListeServerSide;

public class GetirBelediyeBasvuruListeServerSideQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<GetirBelediyeBasvuruListeServerSideResponseModel>>>>
{
    public string? HasarTespitAskiKodu { get; set; }

    public class GetirBelediyeBasvuruListeServerSideQueryHandler : IRequestHandler<GetirBelediyeBasvuruListeServerSideQuery, ResultModel<DataTableResponseModel<List<GetirBelediyeBasvuruListeServerSideResponseModel>>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _repository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirBelediyeBasvuruListeServerSideQueryHandler(IKullaniciBilgi kullaniciBilgi, IMapper mapper, IBasvuruRepository appealRepository)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _mapper = mapper;
            _repository = appealRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<GetirBelediyeBasvuruListeServerSideResponseModel>>>> Handle(GetirBelediyeBasvuruListeServerSideQuery request, CancellationToken cancellationToken)
        {
            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();

            var query = _repository
                            .GetWhere(x =>
                                !x.SilindiMi
                                &&
                                x.AktifMi == true
                                &&
                                x.HasarTespitAskiKodu == HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu)
                                &&
                                x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                &&
                                x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir,
                                true
                            )
                            .OrderByDescending(o => o.OlusturmaTarihi)
                            .AsQueryable();

            if (kullaniciBilgi.BirimIlId > 0)
                query = query.Where(x => x.UavtIlNo == kullaniciBilgi.BirimIlId);

            if (request?.length > 5000)
            {
                //sadece dışarı aktarmada count alalım, dışarı aktarılmıyorsa boşuna yormayalım diye içerideki if şartına taşındı
                if (query.Count() > 5000)
                {
                    var result = new ResultModel<DataTableResponseModel<List<GetirBelediyeBasvuruListeServerSideResponseModel>>>();
                    result.ErrorMessage("Tek seferde en fazla 5.000 kayıt dışarı aktarılabilir.");
                    return await Task.FromResult(result);
                }
            }

            return await query.Paginate<GetirBelediyeBasvuruListeServerSideResponseModel, Basvuru>(request, _mapper);
        }
    }
}