using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Disertatie.Mvc
{
    public class ImageResult : ActionResult
    {
        private readonly byte[] _imageBytes;
        private readonly int? _width;

        public ImageResult(byte[] imageBytes, int? width = null) {
            this._imageBytes = imageBytes;
            this._width = width;
        }

        public override void ExecuteResult(ControllerContext context) {

            if (!_width.HasValue) {
                var result = new FileContentResult(_imageBytes, "image/jpg");
                result.ExecuteResult(context);
            } else {
                using (var stream = new MemoryStream(_imageBytes))
                using (var bitmap = new Bitmap(stream)) {
                    int width = Math.Min(bitmap.Width, _width.Value);
                    int height;
                    if (bitmap.Width > width) {
                        var ratio = (double)width / bitmap.Width;
                        height = Convert.ToInt32(ratio * bitmap.Height);                        
                    } else {
                        height = bitmap.Height;
                    }

                    using (var resized = new Bitmap(width, height)) {
                        var graphics = Graphics.FromImage(resized);
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.FillRectangle(Brushes.White, 0, 0, width, height);
                        graphics.DrawImage(bitmap, 0, 0, width, height);
                        context.HttpContext.Response.ContentType = "image/jpg";
                        resized.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
                    }
                }
            }            
        }
    }
}
