using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Models
{
	public class ValidationErrorModel
	{
		[JsonProperty("alan")]
		public string? Alan { get; set; }

		[JsonProperty("mesaj")]
		public string? Mesaj { get; set; }
	}
}
