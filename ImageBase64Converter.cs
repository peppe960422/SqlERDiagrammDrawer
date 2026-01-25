using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlERDiagrammDrawer
{
    public static class ImageBase64Converter
    {
     


      
        private static string CleanBase64String(string base64String)
        {
            if (base64String.Contains("base64,"))
            {
             
                int base64Index = base64String.IndexOf("base64,", StringComparison.Ordinal) + 7;
                return base64String[base64Index..];
            }

            return base64String;
        }

     
        public static Image Base64ToImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                throw new ArgumentNullException(nameof(base64String));

            string cleanBase64 = CleanBase64String(base64String);


            byte[] imageBytes = Convert.FromBase64String(cleanBase64);


            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);


                Image image = Image.FromStream(ms, true);
                return new Bitmap(image);
            }
        }

    }
}
