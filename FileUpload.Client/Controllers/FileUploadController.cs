using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Client.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FileAsync(IFormFileCollection multipleFileList)
        {
			using (HttpRequestMessage request = new HttpRequestMessage())
			{
				string boundary = $"{Guid.NewGuid().ToString()}";
				MultipartFormDataContent requestContent = new MultipartFormDataContent(boundary);
				requestContent.Headers.Remove("Content-Type");
				// The two dashes in front of the boundary are important as the framework includes them when serializing the request content.
				requestContent.Headers.TryAddWithoutValidation("Content-Type", $"multipart/form-data; boundary=--{boundary}");

				foreach (IFormFile file in multipleFileList)
				{
					StreamContent streamContent = new StreamContent(file.OpenReadStream());
					requestContent.Add(streamContent, file.Name, file.FileName);
					streamContent.Headers.ContentDisposition.FileNameStar = "";
				}

				request.Content = requestContent;
				request.Method = new HttpMethod("POST");
				request.RequestUri = new Uri($"https://localhost:44395/file-upload-form-collention", UriKind.Relative);
				

				 
			}




			return View();
        }
    }
}
