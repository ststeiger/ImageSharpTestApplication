using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


using Microsoft.AspNetCore.Hosting;


namespace ResizeTestWebApplication.Controllers
{
    public class HomeController : Controller
    {

        private IHostingEnvironment m_env;
        public HomeController(IHostingEnvironment env)
        {
            m_env = env;
        }


        public IActionResult Index()
        {
            return View();
        }



        public Tools.ThumbnailResult Thumb(string id)
        {
            string ext = System.IO.Path.GetExtension(id);

            if (!StringComparer.OrdinalIgnoreCase.Equals(ext, ".jpg"))
                return null;

            string webRoot = this.m_env.WebRootPath;
            string fileName = System.IO.Path.Combine(webRoot, "images", id);
            fileName = System.IO.Path.GetFullPath(fileName);

            return new Tools.ThumbnailResult(fileName, Tools.ThumbnailResult.MimeType.Jpeg, 300.0f, 300.0f);
        } // End Action Thumb


        public IActionResult Error()
        {
            return View();
        }
    }
}
