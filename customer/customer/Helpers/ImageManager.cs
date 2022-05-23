using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace customer
{
    public class ImageManager
    {
        private static ImageManager _instance;
        private static Cloudinary _cloudinary;

        private ImageManager()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build().GetSection("Cloudinary");
            
            var account = new Account(
                config["CloudName"],
                config["ApiKey"],
                config["ApiSecret"]
            );
            
            _cloudinary = new Cloudinary(account);
        }
        
        public static ImageManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ImageManager();
            }
            return _instance;
        }

        public ImageUploadResult Upload(IFormFile file)
        {
            var uploadParam = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };
            var uploadResult = _cloudinary.Upload(uploadParam);
            return uploadResult;
        }
    }
}