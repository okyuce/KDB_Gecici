using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NatamamYapiIslemleriController : Controller
    {
        private readonly INatamamYapiAppService _appService;

        public NatamamYapiIslemleriController(INatamamYapiAppService appService)
        {
            _appService = appService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _appService.GetAllAsync();
            return View(model);
        }
    }
}
