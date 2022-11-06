using Microsoft.AspNetCore.Http;

namespace JwtApp.Support
{
    public class FileUpload
    {
        public int id { get; set; }
        public IFormFile files { get; set; }
        public string Name { get; set; }
    }
}
