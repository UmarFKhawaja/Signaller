using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Signaller.Apps.ApiApp.Controllers
{
    [Route("token")]
    public class TokenController : Controller
    {
        [Authorize]
        [Route("")]
        public IActionResult Index()
        {
            return Json
            (
                new
                {
                    User
                }
            );
        }
    }
}