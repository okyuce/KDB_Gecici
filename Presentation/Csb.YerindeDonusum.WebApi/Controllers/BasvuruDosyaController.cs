using Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Commands;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
    public class BasvuruDosyaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasvuruDosyaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("BasvuruDosyaIndir")]
        [AddInfoLog(PassResponse = true)]
        public async Task<ResultModel<BasvuruDosyaIndirCommandResponseModel>> BasvuruDosyaIndir(BasvuruDosyaIndirCommand request)
        {
            //var commandModel = new BasvuruDosyaIndirCommand() { Model = model };

            return await _mediator.Send(request);
        }
    }
}