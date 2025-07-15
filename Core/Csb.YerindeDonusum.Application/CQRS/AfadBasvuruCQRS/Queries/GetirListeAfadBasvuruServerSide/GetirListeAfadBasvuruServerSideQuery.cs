using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Domain.Entities;
using CSB.Core.Extensions;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruServerSide;

public class GetirListeAfadBasvuruServerSideQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<GetirListeAfadBasvuruServerSideQueryResponseModel>>>>
{
    public long? Tckn { get; set; }

    public string? AskiKodu { get; set; }

    public int? IlId { get; set; }

    public int? IlceId { get; set; }

    public int? MahalleId { get; set; }

    public string? Ada { get; set; }

    public string? Parsel { get; set; }

    public string? Huid { get; set; }

    public class GetAllAppealByIdentificationNumberHandler : IRequestHandler<GetirListeAfadBasvuruServerSideQuery, ResultModel<DataTableResponseModel<List<GetirListeAfadBasvuruServerSideQueryResponseModel>>>>
    {
        private readonly IMapper _mapper;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;
        public GetAllAppealByIdentificationNumberHandler(IMapper mapper, IKullaniciBilgi kullaniciBilgi, IAfadBasvuruTekilRepository afadBasvuruTekilRepository)
        {
            _mapper = mapper;
            _kullaniciBilgi = kullaniciBilgi;
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<GetirListeAfadBasvuruServerSideQueryResponseModel>>>> Handle(GetirListeAfadBasvuruServerSideQuery request, CancellationToken cancellationToken)
        {
            //il müdürlükleri kontrolleri
            if (_kullaniciBilgi.GetUserInfo().BirimIlId != 0)
            {
                var result = new ResultModel<DataTableResponseModel<List<GetirListeAfadBasvuruServerSideQueryResponseModel>>>();

                if (_kullaniciBilgi.IsInRole(RoleEnum.AfadBasvuruYoneticisi))
                {
                    if (request.MahalleId == null && (string.IsNullOrWhiteSpace(request.AskiKodu) || string.IsNullOrWhiteSpace(request.Huid)) && request.Tckn == null)
                    {
                        result.ErrorMessage("İl, ilçe ve mahalle seçmeniz gerekmektedir veya askı kodu ya da Huid girmeniz gerekmektedir veya tc kimlik no girmeniz gerekmektedir!");
                        return await Task.FromResult(result);
                    }
                }
                else
                {
                    //afad başvuru yöneticisi olmayanlar hem il ilçe mahalle seçmeli hem de askı kodu ya da Huid girmelidir
                    if (request.MahalleId == null)
                    {
                        result.ErrorMessage("İl, ilçe ve mahalle seçmeniz gerekmektedir!");
                        return await Task.FromResult(result);
                    }
                    else if (string.IsNullOrWhiteSpace(request.AskiKodu))
                    {
                        result.ErrorMessage("Askı kodu girmeniz gerekmektedir!");
                        return await Task.FromResult(result);
                    }
                    else if (string.IsNullOrWhiteSpace(request.Huid))
                    {
                        result.ErrorMessage("Askı kodu girmeniz gerekmektedir!");
                        return await Task.FromResult(result);
                    }
                }
            }

            var query = _afadBasvuruTekilRepository.GetWhere(x => x.CsbAktifMi == true && !x.CsbSilindiMi, true);

            if (request.Tckn != null)
                query = query.Where(x => x.Tckn == request.Tckn);

            if (request.IlId != null)
                query = query.Where(x => x.IlId == request.IlId);

            if (request.IlceId != null)
                query = query.Where(x => x.IlceId == request.IlceId);

            if (request.MahalleId != null)
                query = query.Where(x => x.MahalleId == request.MahalleId);

            if (!string.IsNullOrWhiteSpace(request.AskiKodu))
                query = query.Where(x => x.AskiKodu == HasarTespitAddon.AskiKoduToUpper(request.AskiKodu));

            if (!string.IsNullOrWhiteSpace(request.Ada))
                query = query.Where(x => x.Ada == request.Ada);

            if (!string.IsNullOrWhiteSpace(request.Parsel))
                query = query.Where(x => x.Parsel == request.Parsel);

            if (!string.IsNullOrWhiteSpace(request.Huid))
                query = query.Where(x => x.Huid == request.Huid);

            return await query.OrderByDescending(o => o.HedefTarih).ThenByDescending(o => o.CsbOlusturmaTarihi).Paginate<GetirListeAfadBasvuruServerSideQueryResponseModel, AfadBasvuruTekil>(request, _mapper);
        }
    }
}