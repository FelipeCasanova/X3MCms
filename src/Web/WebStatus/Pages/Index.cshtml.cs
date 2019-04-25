using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace WebStatus.Pages
{
    public class IndexModel : PageModel
    {
        private IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            var basePath = _configuration["PATH_BASE"];
            Redirect($"{basePath}/hc-ui");
        }
    }
}
