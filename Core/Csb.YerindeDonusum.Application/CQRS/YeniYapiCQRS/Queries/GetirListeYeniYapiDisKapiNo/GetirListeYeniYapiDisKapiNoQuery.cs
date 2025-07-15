using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiDisKapiNo;

public class GetirListeYeniYapiDisKapiNoQuery : IRequest<ResultModel<List<SelectDto<string>>>>
{
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }

    public class GetirListeYeniYapiDisKapiNoQueryHandler : IRequestHandler<GetirListeYeniYapiDisKapiNoQuery, ResultModel<List<SelectDto<string>>>>
    {
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;

        public GetirListeYeniYapiDisKapiNoQueryHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository)
        {
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
        }

        public async Task<ResultModel<List<SelectDto<string>>>> Handle(GetirListeYeniYapiDisKapiNoQuery request, CancellationToken cancellationToken)
        {
            request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);

            var result = _binaDegerlendirmeRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                && x.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                                && x.UavtMahalleNo == request.UavtMahalleNo
                                                && x.HasarTespitAskiKodu == request.HasarTespitAskiKodu
                                ).GroupBy(x=> x.BinaDisKapiNo).Select(x => new SelectDto<string>() {
                                        Id = string.IsNullOrWhiteSpace(x.FirstOrDefault().BinaDisKapiNo) ? "belirtilmemis" : x.FirstOrDefault().BinaDisKapiNo,
                                        Ad = string.IsNullOrWhiteSpace(x.FirstOrDefault().BinaDisKapiNo) ? "Dış Kapı No Belirtilmemiş" : x.FirstOrDefault().BinaDisKapiNo,
                                }).OrderBy(x=> x.Id).ToList();

            return await Task.FromResult(new ResultModel<List<SelectDto<string>>>(result));
        }
    }
}