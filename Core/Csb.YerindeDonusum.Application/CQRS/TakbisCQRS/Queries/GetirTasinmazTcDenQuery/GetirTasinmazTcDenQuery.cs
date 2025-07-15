using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazTcDenQuery;

public class GetirTasinmazTcDenQuery : IRequest<ResultModel<List<GetirTasinmazTcDenQueryResponseModel>>>
{
    public long? TcKimlikNo { get; set; }

    public class GetirTasinmazIdDenQueryHandler : IRequestHandler<GetirTasinmazTcDenQuery, ResultModel<List<GetirTasinmazTcDenQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly ITakbisService _takbisService;

        public GetirTasinmazIdDenQueryHandler(IMapper mapper, ITakbisService takbisService)
        {
            _mapper = mapper;
            _takbisService = takbisService;
        }

        public async Task<ResultModel<List<GetirTasinmazTcDenQueryResponseModel>>> Handle(GetirTasinmazTcDenQuery request, CancellationToken cancellationToken)
        {
            ResultModel<List<GetirTasinmazTcDenQueryResponseModel>> result = new();

            try
            {
                var malikList = await _takbisService.GetirGercekKisiAsync(request?.TcKimlikNo?.ToString());
                if (!malikList.Any())
                {
                    result.ErrorMessage("Tapuda malik bulunamadı!");
                    return await Task.FromResult(result);
                }

                try
                {
                    result.Result = new List<GetirTasinmazTcDenQueryResponseModel>();

                    foreach (var malik in malikList)
                    {
                        result.Result.AddRange(_mapper.Map<List<GetirTasinmazTcDenQueryResponseModel>>(await _takbisService.GetirTasinmazHiseliMalikIDDenAsync(malik.Id.Value)));
                    }
                }
                catch (Exception exc2)
                {
                    result.Exception(exc2, "Tapu bilgileri sorgulanırken hata oluştu.");
                }
            }
            catch (Exception exc)
            {
                result.Exception(exc, "Tapuda gerçek kişi sorgulanırken hata oluştu.");
            }

            return await Task.FromResult(result);
        }
    }
}