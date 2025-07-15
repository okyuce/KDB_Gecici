using AutoMapper;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruDetayById;

public class GetirBasvuruDetayByIdQuery : IRequest<ResultModel<GetirBasvuruDetayByIdQueryResponseModel>>
{
    public GetirBasvuruDetayByIdQueryModel? Model { get; set; }

    public class GetAllAppealByIdentificationNumberHandler : IRequestHandler<GetirBasvuruDetayByIdQuery, ResultModel<GetirBasvuruDetayByIdQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _appealRepository;

        public GetAllAppealByIdentificationNumberHandler(IMapper mapper, IBasvuruRepository appealRepository)
        {
            _mapper = mapper;
            _appealRepository = appealRepository;
        }

        public async Task<ResultModel<GetirBasvuruDetayByIdQueryResponseModel>> Handle(GetirBasvuruDetayByIdQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirBasvuruDetayByIdQueryResponseModel>();
            try
            {
                Guid.TryParse(request?.Model?.BasvuruId, out Guid basvuruGuid);

                var basvuru = _appealRepository
                    .GetAllQueryable(x => x.BasvuruGuid == basvuruGuid && x.TcKimlikNo.Trim() == request.Model.TcKimlikNo.Trim() && x.AktifMi == true && x.SilindiMi == false)
                    .Include(x => x.BasvuruDurum)
                    .Include(x => x.BasvuruIptalTur)
                    .Include(x => x.BasvuruTur)
                    .Include(x => x.BasvuruDestekTur)
                    .Include(x => x.BasvuruKanal)
                    .Include(x => x.BasvuruDosyas.Where(p => p.BasvuruDosyaTurId == BasvuruDosyaTurEnum.TapuFotografi 
                                                          || p.BasvuruDosyaTurId == BasvuruDosyaTurEnum.HazineArazisiMuhtarBeyanBelgesi
                                                          || p.BasvuruDosyaTurId == BasvuruDosyaTurEnum.TuzelKisilikYetkiliOldugunuGosterirBelge)).ThenInclude(x => x.BasvuruDosyaTur)
                    .OrderByDescending(o => o.BasvuruId)
                    .AsNoTracking()
                    .FirstOrDefault();

                if (basvuru != null)
                {
                    result.Result = _mapper.Map<GetirBasvuruDetayByIdQueryResponseModel>(basvuru);
                }
                else
                {
                    result.ErrorMessage("Başvuru Detayı Bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Başvuru Detayı Alınırken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}