using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheOnePercents.TestWeb.Controllers
{
    public class FileController : Controller
    {
        //
        // GET: /File/
        [Authorize]
        public ActionResult GetVideo(string id)
        {
            return File(Server.MapPath("~/Content/videos/" + id + ".mp4"), "video/mp4");
        }

        [Authorize]
        public ActionResult GetImage(string id)
        {
            return File(Server.MapPath("~/Content/videos/" + id + ".png"), "image/png");
        }
	}
}