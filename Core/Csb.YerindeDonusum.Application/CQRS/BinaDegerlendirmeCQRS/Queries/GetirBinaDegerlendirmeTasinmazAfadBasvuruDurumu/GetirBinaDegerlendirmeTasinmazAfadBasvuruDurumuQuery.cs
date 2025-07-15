using AutoMapper;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumu;

public class GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQuery : IRequest<ResultModel<GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQueryResponseModel>>
{
    public long BinaDegerlendirmeId { get; set; }

    public class GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQueryHandler : IRequestHandler<GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQuery, ResultModel<GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQueryResponseModel>>
    {
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;
        private readonly IMapper _mapper;


        public GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQueryHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IMapper mapper, IAfadBasvuruTekilRepository afadBasvuruTekilRepository)
        {
            _mapper = mapper;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
        }

        public async Task<ResultModel<GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQueryResponseModel>> Handle(GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQuery request, CancellationToken cancellationToken)
        {
            ResultModel<GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQueryResponseModel> result = new();

            var binaDegerlendirme = await _binaDegerlendirmeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                    && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                                    , asNoTracking: false
                            ).Include(x => x.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true
                                                         && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruReddedilmistir)
                                                    )
                                                .ThenInclude(x => x.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true))
                            .Include(i => i.BinaYapiRuhsatIzinDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                            .Include(i => i.BinaMuteahhits.Where(y => y.SilindiMi == false && y.AktifMi == true && !string.IsNullOrEmpty(y.YetkiBelgeNo)))
                            .Include(i => i.BinaDegerlendirmeDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                            .Include(i => i.BinaYapiDenetimSeviyeTespits.Where(x => x.SilindiMi == false && x.AktifMi == true))
                                .ThenInclude(i => i.BinaYapiDenetimSeviyeTespitDosyas)
                            .FirstOrDefaultAsync();
            if (binaDegerlendirme == null) return await Task.FromResult(result);
            result.Result = new GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQueryResponseModel()
            {
                BinaDegerlendirmeId = request.BinaDegerlendirmeId,
                TasinmazAfadBasvuruDurumu = binaDegerlendirme.Basvurus.Any(x => x.TapuTasinmazId != null
                && x.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Iptal
                && x.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.IptalEdilmistir
                && x.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Red
                && (
                 (x.BasvuruImzaVerens == null || x.BasvuruImzaVerens.Count == 0) ||
                 (x.BasvuruImzaVerens != null && x.BasvuruImzaVerens.Any(y => y.KrediOdemeTutar != 0 && y.HibeOdemeTutar != 0))
                )
                && _afadBasvuruTekilRepository.GetAllQueryable().Any(m => m.TasinmazId == x.TapuTasinmazId
                         && m.CsbSilindiMi == false && m.CsbAktifMi == true
                        && m.DegerlendirmeIptalDurumu != null && m.DegerlendirmeIptalDurumu!.ToLower().Trim() != "evet"
                && (
                    (m.ItirazDegerlendirmeSonucu != null && m.ItirazDegerlendirmeSonucu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.Iptal.GetDisplayName())
                    || (m.ItirazDegerlendirmeSonucu == null && m.DegerlendirmeDurumu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.Iptal.GetDisplayName())
                )
                && (
                    (m.ItirazDegerlendirmeSonucu != null && m.ItirazDegerlendirmeSonucu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.IptalEdilmistir.GetDisplayName())
                    || (m.ItirazDegerlendirmeSonucu == null && m.DegerlendirmeDurumu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.IptalEdilmistir.GetDisplayName())
                )
                && (
                    (m.ItirazDegerlendirmeSonucu != null && m.ItirazDegerlendirmeSonucu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.Red.GetDisplayName())
                    || (m.ItirazDegerlendirmeSonucu == null && m.DegerlendirmeDurumu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.Red.GetDisplayName())
                )
                )
                        )
            };

            return await Task.FromResult(result);
        }
    }
}