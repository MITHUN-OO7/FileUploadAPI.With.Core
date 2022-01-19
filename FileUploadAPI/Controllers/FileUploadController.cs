using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadAPI.Controllers
{
    public class FileUploadController : Controller
    {
        private IWebHostEnvironment _env;

        public FileUploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            try
            {
                var result = new List<FileUploadResult>();
                foreach (var file in files)
                {
                    var path = Path.Combine(_env.ContentRootPath, "content\\images", file.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);
                    result.Add(new FileUploadResult() { Name = file.FileName, Length = file.Length });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("file-upload-form-collention")]
        public IActionResult FileUploadFormCollention(IFormCollection formdata)
        {
            try
            {
                string host = Request.Scheme + "://" + Request.Host.ToString().ToLower() + "/";
                var files = HttpContext.Request.Form.Files;
                string path = "";
                path = Path.Combine(_env.ContentRootPath, "contents\\images");
                bool exists = System.IO.Directory.Exists(path);
                if (!exists)
                    System.IO.Directory.CreateDirectory(path);

                foreach (var file in files)
                {
                    var uploads = Path.Combine(_env.ContentRootPath, "contents\\images");
                    if (file.Length > 0)
                    {
                        string FileName = Guid.NewGuid().ToString() + "." + file.ContentType.Split('/')[1]; // Give file name
                        using (var fileStream = new FileStream(Path.Combine(uploads, FileName), FileMode.Create))
                        {
                            file.CopyToAsync(fileStream);
                        }
                    }
                }
                return Ok("file uploaded Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
