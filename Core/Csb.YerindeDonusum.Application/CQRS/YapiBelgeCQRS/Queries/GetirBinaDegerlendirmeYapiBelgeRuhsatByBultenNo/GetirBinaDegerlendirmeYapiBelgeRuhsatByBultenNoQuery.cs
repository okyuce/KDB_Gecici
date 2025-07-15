using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirYapiBelgeByYapiKimlikNo;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo;

public class GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQuery : IRequest<ResultModel<GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQueryResponseModel>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public long? BultenNo { get; set; }

    public class GetirYapiBelgeByYapiKimlikNoQueryHandler : IRequestHandler<GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQuery, ResultModel<GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQueryResponseModel>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IBasvuruRepository _basvuruRepository;

        public GetirYapiBelgeByYapiKimlikNoQueryHandler(IMediator mediator, IMapper mapper, IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IBasvuruRepository basvuruRepository)
        {
            _mediator = mediator;
            _mapper = mapper;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _basvuruRepository = basvuruRepository;
        }

        public async Task<ResultModel<GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQueryResponseModel>> Handle(GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQueryResponseModel>();

            if (request?.BultenNo == null)
            {
                result.ErrorMessage("Bülten no girmeniz gerekmektedir!");
                return result;
            }

            var binaDegerlendirme = _binaDegerlendirmeRepository.GetWhere(x => x.AktifMi == true && !x.SilindiMi
                                                && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                        , true
                                        ).FirstOrDefault();

            if (binaDegerlendirme == null)
            {
                result.ErrorMessage("Bina Değerlendirme bilgisi bulunamadı.");
                return result;
            }

            var yapiBelgeRuhsat = await _mediator.Send(new GetirYapiBelgeRuhsatByBultenNoQuery
            {
                BultenNo = request.BultenNo
            });

            if (yapiBelgeRuhsat.IsError)
            {
                result.ErrorMessage(yapiBelgeRuhsat.ErrorMessageContent);
                return result;
            }

            var binaDegerlendirmeBasvuru = _basvuruRepository.GetWhere(x => x.AktifMi == true && x.SilindiMi == false
                                               && x.HasarTespitAskiKodu == binaDegerlendirme.HasarTespitAskiKodu
                                               && x.UavtMahalleNo == binaDegerlendirme.UavtMahalleNo
                                               , true
                                           ).FirstOrDefault();


            if (binaDegerlendirmeBasvuru?.UavtIlNo != yapiBelgeRuhsat.Result.IlKimlikNo
                    || binaDegerlendirmeBasvuru?.UavtIlceNo != yapiBelgeRuhsat.Result.IlceKimlikNo
                    //|| binaDegerlendirmeBasvuru?.UavtMahalleNo != yapiBelgeRuhsat.Result.MahalleKimlikNo// tapu ve maks mahalleleri uyuşmadığı için kaldıırlmıştır.
                    )
            {
                result.ErrorMessage($"Binaya ait il, ilçe verileri eşleşmediği için veri alınamadı. Bülten numarasına ait adres: {yapiBelgeRuhsat.Result.IlAdi} {yapiBelgeRuhsat.Result.IlceAdi} {yapiBelgeRuhsat.Result.MahalleAdi}");
                return result;
            }

            if (yapiBelgeRuhsat.Result.AdaNo != binaDegerlendirme.Ada || yapiBelgeRuhsat.Result.ParselNo != binaDegerlendirme.Parsel)
            {
                //ada verisi null gelen kayıtlar vardı, null gelenler için ada sıfır olmalı, yapı ruhsatında 0 olarak gözüküyor
                if (string.IsNullOrWhiteSpace(binaDegerlendirme.Ada))
                    binaDegerlendirme.Ada = "0";

                if (string.IsNullOrWhiteSpace(binaDegerlendirme.Parsel))
                    binaDegerlendirme.Parsel = "0";
                
                if (string.IsNullOrWhiteSpace(yapiBelgeRuhsat.Result.AdaNo) ||yapiBelgeRuhsat.Result.AdaNo.Contains("-"))
                    yapiBelgeRuhsat.Result.AdaNo = "0";

                if (string.IsNullOrWhiteSpace(yapiBelgeRuhsat.Result.ParselNo))
                    yapiBelgeRuhsat.Result.ParselNo = "0";

                if (yapiBelgeRuhsat.Result.AdaNo != binaDegerlendirme.Ada || yapiBelgeRuhsat.Result.ParselNo != binaDegerlendirme.Parsel)
                {
                    result.ErrorMessage($"Binaya ait ada parsel verileri eşleşmediği için veri alınamadı. Bülten numarasına ait ada parsel: {yapiBelgeRuhsat.Result.AdaNo ?? "0"}/{yapiBelgeRuhsat.Result.ParselNo ?? "0"}");
                    return result;
                }
               
            }

            result.Result = _mapper.Map<GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQueryResponseModel>(yapiBelgeRuhsat.Result);

            return result;
        }
    }
}