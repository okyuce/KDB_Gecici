using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByAskiKodu
{
    public class KdsHasarTespitVeriByAskiKoduQuery : IRequest<ResultModel<KdsHaneModel>>
    {
        public string? AskiKodu { get; set; }

        public class KdsHasarTespitVeriByAskiKoduQueryHandler : IRequestHandler<KdsHasarTespitVeriByAskiKoduQuery, ResultModel<KdsHaneModel>>
        {
            private readonly IMediator _mediator;

            public KdsHasarTespitVeriByAskiKoduQueryHandler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<ResultModel<KdsHaneModel>> Handle(KdsHasarTespitVeriByAskiKoduQuery request, CancellationToken cancellationToken)
            {
                var result = await _mediator.Send(new KdsHasarTespitVeriByAskiKoduHasarTespitVeriQuery { AskiKodu = request.AskiKodu });
                if (result.IsError)
                    result = await _mediator.Send(new KdsHasarTespitVeriByAskiKoduHaneQuery { AskiKodu = request.AskiKodu });

                return await Task.FromResult(result);
            }
        }
    }
}