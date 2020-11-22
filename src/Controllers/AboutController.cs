using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace Miniblog.Core.Controllers
{
    public class AboutController : Controller
    {
        [OutputCache(Profile = "default")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
