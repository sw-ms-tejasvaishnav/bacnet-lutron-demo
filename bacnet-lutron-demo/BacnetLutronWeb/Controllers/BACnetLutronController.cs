using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BACKnetLutron.Controllers
{
    public class BACnetLutronController : Controller
    {  
        
        public ActionResult BacNetLutronUi()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}