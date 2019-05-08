using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace Project.Pages.Home
{
    public class IndexModel : PageModel
    {
        public void OnGet([FromServices] IEndpointAddressScheme<RouteValuesAddress> addressingScheme)
        {

        }
    }
}