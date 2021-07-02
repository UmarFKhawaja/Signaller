using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Signaller.Apps.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IConfiguration configuration, ILogger<IndexModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["Title"] = "Home page";
            ViewData["ApiUrl"] = _configuration["Communications:ChatHub"];
        }
    }
}
