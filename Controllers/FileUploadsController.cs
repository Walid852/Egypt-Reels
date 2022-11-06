using JwtApp.Support;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JwtApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileUploadsController : ControllerBase
    {


      // public static IWebHostEnvironment _webHostEnvironment;
        public FileUploadsController() { }
        /*public FileUploadsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;   
        }*/
        [HttpPost]
        public string Post([FromForm] FileUpload fileUpload)
        {
            if (true)
            {
                try
                {


                    string path = "C:\\Users\\WinDows\\source\\repos\\jwt-dotnetcore-web-main\\wwwroot" + "\\Uploads\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + fileUpload.files.FileName))
                    {
                        fileUpload.files.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                        ExpressEncription.AESEncription.AES_Encrypt(path + fileUpload.files.FileName, "WalidMohamed");
                        System.IO.File.Delete(path + fileUpload.files.FileName);
                        return path + fileUpload.files.FileName;

                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
                else
                {
                    return "Failed";
                }
            
            
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(string filename)
        
        {
            string path = "C:\\Users\\WinDows\\source\\repos\\jwt-dotnetcore-web-main\\wwwroot" + "\\Uploads\\";
            var filePath=path+filename + ".aes";
            if (System.IO.File.Exists(filePath)) {
                ExpressEncription.AESEncription.AES_Decrypt(filePath , "WalidMohamed");
                byte[] bytes = System.IO.File.ReadAllBytes(filePath + ".decrypted");
                System.IO.File.Delete(filePath + ".decrypted");
                return File(bytes, "images/png");
            }
            return null;

        }
    }
}
