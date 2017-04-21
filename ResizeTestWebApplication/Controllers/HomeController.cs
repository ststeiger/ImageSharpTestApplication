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

            bool isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.Linux);

            bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.Windows);

            string cpuInfo = "";
            if (isWindows)
            {
                var lm = Microsoft.Win32.Registry.LocalMachine;
                var key = lm.OpenSubKey("HARDWARE").OpenSubKey("DESCRIPTION").OpenSubKey("System").OpenSubKey("CentralProcessor");
                string[] keys = key.GetSubKeyNames();

                var proc1 = key.OpenSubKey(keys[0]);
                cpuInfo += proc1.GetValue("ProcessorNameString");
                cpuInfo += System.Environment.NewLine;
                cpuInfo += proc1.GetValue("Identifier");
            }
            else if (isLinux)
            {
                cpuInfo = System.IO.File.ReadAllText("/proc/cpuinfo", System.Text.Encoding.UTF8);
            }
            else
                cpuInfo = "CpuInfo not supported.";

            ViewData["CpuInfo"] = cpuInfo;
            
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
