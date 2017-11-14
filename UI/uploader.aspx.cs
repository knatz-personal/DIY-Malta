using System;
using System.IO;

namespace UI
{
    public partial class uploader : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var file = Request.Files[0];
            string imagesFolder = AppDomain.CurrentDomain.BaseDirectory + "\\img\\catalogue\\";
            string fileExtension = Path.GetExtension(file.FileName);


            if (fileExtension.ToLower().EndsWith(".png") || fileExtension.ToLower().EndsWith(".jpg") ||
                       fileExtension.ToLower().EndsWith(".jpeg") || fileExtension.ToLower().EndsWith(".bmp") ||
                       fileExtension.ToLower().EndsWith(".tiff") || fileExtension.ToLower().EndsWith(".gif"))
            {
                string imageFileName = Guid.NewGuid() + fileExtension;
                string productImagePath = imagesFolder + @"\" + imageFileName;
                file.SaveAs(productImagePath);
                Session["CurrentImagePath"] = "/img/catalogue/" + imageFileName;
            }
        }


    }
}
