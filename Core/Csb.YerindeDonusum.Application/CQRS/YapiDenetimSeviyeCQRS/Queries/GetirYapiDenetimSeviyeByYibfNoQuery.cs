using AutoMapper;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.YapiDenetimSeviye;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using System.Linq;

namespace Csb.YerindeDonusum.Application.CQRS.YapiDenetimSeviyeCQRS.Queries;

public class GetirYapiDenetimSeviyeByYibfNoQuery : IRequest<ResultModel<GetirYapiDenetimSeviyeByYibfNoQueryResponseModel>>
{
    public long? binaDegerlendirmeId { get; set; }
    public long? yibfNo { get; set; }

    public class GetirYapiDenetimSeviyeByYibfNoQueryHandler : IRequestHandler<GetirYapiDenetimSeviyeByYibfNoQuery, ResultModel<GetirYapiDenetimSeviyeByYibfNoQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IYapiDenetimSeviyeMvMuteahhitYibfListRepository _yapiDenetimSeviyeMvMuteahhitYibfListRepository;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;

        public GetirYapiDenetimSeviyeByYibfNoQueryHandler(IMapper mapper, IYapiDenetimSeviyeMvMuteahhitYibfListRepository yapiDenetimSeviyeMvMuteahhitYibfListRepository, IBinaDegerlendirmeRepository binaDegerlendirmeRepository)
        {
            _mapper = mapper;
            _yapiDenetimSeviyeMvMuteahhitYibfListRepository = yapiDenetimSeviyeMvMuteahhitYibfListRepository;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
        }

        public async Task<ResultModel<GetirYapiDenetimSeviyeByYibfNoQueryResponseModel>> Handle(GetirYapiDenetimSeviyeByYibfNoQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirYapiDenetimSeviyeByYibfNoQueryResponseModel>();

            var binaDegerlendirme = _binaDegerlendirmeRepository.GetWhere(x =>
                x.BinaDegerlendirmeId == request.binaDegerlendirmeId
                &&
                x.AktifMi == true
                &&
                !x.SilindiMi,
                true,
                i => i.Basvurus.Where(x => x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir && x.SilindiMi == false && x.AktifMi == true)
            ).FirstOrDefault();

            if (binaDegerlendirme == null)
            {
                result.ErrorMessage("Bina değerlendirmesine ait veri bulunamadı!");
                return await Task.FromResult(result);
            }

            var yapiDenetimSeviye = _yapiDenetimSeviyeMvMuteahhitYibfListRepository.GetWhere(x => x.YibfNo == request.yibfNo, true).FirstOrDefault();

            if (yapiDenetimSeviye == null)
            {
                result.ErrorMessage("Yibf No bilgisine ait veri bulunamadı!");
                return await Task.FromResult(result);
            }
            //else if (binaDegerlendirme.Basvurus.Any(s => s.TapuMahalleId?.ToString() == yapiDenetimSeviye.MahalleKod) /*|| binaDegerlendirme.Ada != yapiDenetimSeviye.Ada || binaDegerlendirme.Parsel != yapiDenetimSeviye.Parsel*/)
            //{
            //    result.ErrorMessage($"Yibf No bilgisiyle bina adres verileri uyuşmamaktadır. Yibf noya ait adres: {yapiDenetimSeviye.Il} {yapiDenetimSeviye.Ilce} {yapiDenetimSeviye.Mahalle}");
            //    return await Task.FromResult(result);
            //}

            result.Result = _mapper.Map<GetirYapiDenetimSeviyeByYibfNoQueryResponseModel>(yapiDenetimSeviye);

            return await Task.FromResult(result);
        }
    }
}