using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.BasvuruOnaylaReddet;

public class BasvuruOnaylaReddetCommand : IRequest<ResultModel<string>>
{
    public List<long>? BasvuruIds { get; set; }
    public bool? Onayla { get; set; }

    public class BasvuruOnaylaReddetCommandHandler : IRequestHandler<BasvuruOnaylaReddetCommand, ResultModel<string>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IMediator _mediator;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public BasvuruOnaylaReddetCommandHandler(IBasvuruRepository basvuruRepository, IMediator mediator, IKullaniciBilgi kullaniciBilgi)
        {
            _basvuruRepository = basvuruRepository;
            _mediator = mediator;
            _kullaniciBilgi = kullaniciBilgi;
        }

        public async Task<ResultModel<string>> Handle(BasvuruOnaylaReddetCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            var basvuruList = _basvuruRepository.GetWhere(x => request.BasvuruIds.Any(y=> y == x.BasvuruId)).ToList();

            if (basvuruList?.Any() != true)
            {
                result.ErrorMessage("Başvuru bulunamadı.");
                return await Task.FromResult(result);
            }

            // TODO: Onayla ve reddet işlemleri iptal mi oldu, iptal olmamışsa iş kuralı öğrenilip devam edilecek

            result.ErrorMessage("Geliştirme süreci devam ettiği için işlem iptal edildi.");

            return await Task.FromResult(result);
        }
    }
}