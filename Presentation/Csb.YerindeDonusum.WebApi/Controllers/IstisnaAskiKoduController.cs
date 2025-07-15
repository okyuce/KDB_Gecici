using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models.DataTable;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IstisnaAskiKoduController : ControllerBase
    {
        private readonly IMediator _mediator;
        public IstisnaAskiKoduController(IMediator mediator) => _mediator = mediator;


        [HttpGet("GetAll")]
        public async Task<ActionResult<List<IstisnaAskiKoduDto>>> GetAll()
        {
            var query = new GetListIstisnaAskiKoduQuery();  // Eğer parametreye ihtiyaç yoksa empty query
            var result = await _mediator.Send(query);
            // Response formatını service'e uydur: Eğer DataTableResponseModel dönüyorsa, result.DataList gibi extract et
            return Ok(result);  // Veya doğrudan List dön
        }

        // GET: api/IstisnaAskiKodu/list
        [HttpPost("list")]
        public async Task<ActionResult<DataTableResponseModel<List<IstisnaAskiKoduListItem>>>> GetList([FromBody] GetListIstisnaAskiKoduQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // POST: api/IstisnaAskiKodu
        [HttpPost]
        public async Task<ActionResult> CreateOrUpdate([FromBody] CreateIstisnaAskiKoduCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsError)
                return BadRequest(response.ErrorMessageContent);
            return Ok(response);
        }

        // DELETE: api/IstisnaAskiKodu/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var cmd = new DeleteIstisnaAskiKoduCommand { Id = id };
            var response = await _mediator.Send(cmd);
            if (response.Success)
                return NoContent();
            return BadRequest("Silme işlemi başarısız.");
        }
    }
}