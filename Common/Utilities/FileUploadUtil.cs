using System;
using System.IO;
using System.Web;

namespace Common.Utilities
{
    public class FileUploadUtil
    {
        public FileUploadUtil()
        {
        }

        public string ImageUpload(HttpPostedFile pfb, string imagesFolder)
        {
            string productImagePath = "";
            string imageFileName = "";
            if (pfb != null)
            {
                string fileExtension = Path.GetExtension(pfb.FileName);
                if (!string.IsNullOrEmpty(fileExtension) && (
                    fileExtension.ToLower().EndsWith(".png") ||
                    fileExtension.ToLower().EndsWith(".jpg") ||
                    fileExtension.ToLower().EndsWith(".jpeg") ||
                    fileExtension.ToLower().EndsWith(".bmp") ||
                    fileExtension.ToLower().EndsWith(".tiff") ||
                    fileExtension.ToLower().EndsWith(".gif")))
                {
                    imageFileName = Guid.NewGuid() + fileExtension;
                    productImagePath = Path.Combine(imagesFolder, imageFileName);
                    pfb.SaveAs(productImagePath);
                    productImagePath = Path.Combine(imagesFolder, imageFileName);
                }
            }
            return imagesFolder + imageFileName;
        }
    }
}