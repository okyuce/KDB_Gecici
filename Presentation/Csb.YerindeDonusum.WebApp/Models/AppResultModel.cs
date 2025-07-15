using Csb.YerindeDonusum.Application.Models;

namespace Csb.YerindeDonusum.WebApp.Models
{
    public class AppResultModel<T> where T : class
    {
        public int StatusCode { get; set; } = 400;

        public ResultModel<T> ResultModel { get; set; } = new ResultModel<T>();

    }
}
