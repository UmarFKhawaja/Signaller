using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Signaller.Apps.ApiApp.Controllers
{
    [Route("status")]
    public class StatusController : Controller
    {
        [Authorize]
        [Route("")]
        public IActionResult Index()
        {
            var data = new
            {
                Description = "Alive",
                DateTime = DateTime.UtcNow
            };

            return Json(data);
        }
    }
}