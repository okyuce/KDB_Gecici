using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Queries
{
    public class GetirDosyaListeByBasvuruIdQuery : IRequest<ResultModel<List<GetirDosyaListeByBasvuruIdQueryResponseModel>>>
    {
        public GetirDosyaListeByBasvuruIdQueryModel Model { get; set; }

        public class GetAllFilesByAppealIdQueryHandler : IRequestHandler<GetirDosyaListeByBasvuruIdQuery, ResultModel<List<GetirDosyaListeByBasvuruIdQueryResponseModel>>>
        {
            private readonly IMapper _mapper;
            private readonly IBasvuruRepository _basvuruRepository;
            private readonly IBasvuruDosyaRepository _appealFilesRepository;

            public GetAllFilesByAppealIdQueryHandler(IMapper mapper, IBasvuruRepository basvuruRepository, IBasvuruDosyaRepository appealFilesRepository)
            {
                _mapper = mapper;
                _basvuruRepository = basvuruRepository;
                _appealFilesRepository = appealFilesRepository;
            }

            public async Task<ResultModel<List<GetirDosyaListeByBasvuruIdQueryResponseModel>>> Handle(GetirDosyaListeByBasvuruIdQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<List<GetirDosyaListeByBasvuruIdQueryResponseModel>>();

                if (request.Model.BasvuruGuid.IsNullOrEmpty())
                {
                    result.Exception(new ArgumentNullException("Hatalı veya geçersiz (null) parametre gönderimi. Lütfen bilgilerinizi kontrol ederek yeniden deneyiniz."), "Hatalı veya Geçersiz Bilgi Gönderildi. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");

                    return await Task.FromResult(result);
                }

                var basvuruGuid = Guid.Parse(request.Model.BasvuruGuid);

                var basvuru = _basvuruRepository.GetWhere(x => x.BasvuruGuid == basvuruGuid && x.AktifMi == true && x.SilindiMi == false).FirstOrDefault();

                if (basvuru == null)
                {
                    result.Exception(new ArgumentNullException("Hatalı veya geçersiz parametre gönderimi. Lütfen bilgilerinizi kontrol ederek yeniden deneyiniz."), "Hatalı veya Geçersiz Bilgi Gönderildi. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");

                    return await Task.FromResult(result);
                }

                var dosyalar = _appealFilesRepository.GetAllQueryable(x => x.BasvuruId == basvuru.BasvuruId && x.AktifMi == true && x.SilindiMi == false);

                result.Result = _mapper.Map<List<GetirDosyaListeByBasvuruIdQueryResponseModel>>(dosyalar);

                return await Task.FromResult(result);
            }
        }
    }
}