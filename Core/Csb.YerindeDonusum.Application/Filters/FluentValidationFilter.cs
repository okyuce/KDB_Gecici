using Csb.YerindeDonusum.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Csb.YerindeDonusum.Application.Filters;

public class FluentValidationFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		if (!context.ModelState.IsValid)
		{
			var errorsInModelState = context.ModelState.Where(x => x.Value?.Errors?.Count > 0)
				.ToDictionary(x => x.Key, x => x.Value?.Errors?.Select(y => y.ErrorMessage)).ToArray();

			ResultModel<List<ValidationErrorModel>> response = new();

			var errorsList = new List<ValidationErrorModel>();
			foreach (var error in errorsInModelState)
			{
				foreach(var subError in error.Value ?? new List<string>())
				{
					var errorModel = new ValidationErrorModel()
					{
						Alan = error.Key,
						Mesaj = subError,
					};
					errorsList.Add(errorModel);
				}
			}

			response.ValidationError("Alanları kontrol ediniz.", errorsList);

			context.Result = new BadRequestObjectResult(response);
			return;
		}

		await next();
	}
}
