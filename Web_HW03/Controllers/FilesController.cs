using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Web_HW03.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public FilesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // GET api/files/sample.png
        [HttpGet("{fileName}")]
        public string Get(string fileName)
        {
            string path = _hostingEnvironment.WebRootPath + "/images/" + fileName;
            byte[] b = System.IO.File.ReadAllBytes(path);
            string base64Encoded = Convert.ToBase64String(b);
            return "data:image/png;base64," + base64Encoded;
        }
    }
}
