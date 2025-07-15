
using Csb.YerindeDonusum.Application.CQRS.Base;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IstisnaAskiKoduDosyaController : ControllerBase
    {
        private readonly IMediator _mediator;
        public IstisnaAskiKoduDosyaController(IMediator mediator) => _mediator = mediator;

        // GET: api/IstisnaAskiKoduDosya/{koduId}
        [HttpGet("byKodu/{koduId}")]
        public async Task<ActionResult<IEnumerable<IstisnaAskiKoduDosyaDto>>> GetByKodu(long koduId)
        {
            var query = new GetDosyalarByKoduIdQuery { IstisnaAskiKoduId = koduId };
            var list = await _mediator.Send(query);
            return Ok(list);
        }

        // POST: api/IstisnaAskiKoduDosya/upload
        [HttpPost("upload")]
        public async Task<ActionResult<FileUploadResponseModel>> Upload([FromForm] UploadIstisnaAskiKoduDosyaCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsError)
                return BadRequest(response.ErrorMessageContent);
            return Ok(response);
        }

        // DELETE: api/IstisnaAskiKoduDosya/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var cmd = new DeleteIstisnaAskiKoduDosyaCommand { Id = id };
            var response = await _mediator.Send(cmd);
            if (response.IsError)
                return BadRequest(response.ErrorMessageContent);
            return NoContent();
        }
    }
}