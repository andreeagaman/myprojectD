using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DauneService.Mvc
{
    public class ThumbnailResult : ActionResult
    {
        public const int DefaultWidth = 64;
        public const int DefaultHeight = 64;
        private static readonly string ThumbnailDirectory = "~/Dosare/Thumbnails";

        public int Width { get; set; }
        public int Height { get; set; }
        public string Filename { get; set; }
        public DateTime? Expires { get; set; }

        public ThumbnailResult(int width, int height, string filename) {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");
            this.Width = width;
            this.Height = height;
            this.Filename = filename;
        }

        public ThumbnailResult(string filename) : this(DefaultWidth, DefaultHeight, filename) { }

        public override void ExecuteResult(ControllerContext context) {
            using (var bitmap = new Bitmap(this.Filename)) {
                int _newWidth, _newHeight;
                double Ratio;

                if (bitmap.Width > bitmap.Height) {
                    Ratio = (double)Width / bitmap.Height;
                    _newWidth = Width;
                    var temp = bitmap.Height * Ratio;
                    _newHeight = (int)temp;
                } else {
                    Ratio = (double)Height / bitmap.Height;
                    _newHeight = Height;
                    var temp = bitmap.Width * Ratio;
                    _newWidth = (int)temp;
                }

                if (this.Expires.HasValue) {
                    context.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.ServerAndPrivate);
                    context.HttpContext.Response.Cache.SetLastModified(DateTime.Now);
                    context.HttpContext.Response.Cache.SetExpires(this.Expires.Value);                    
                }

                using (var FinalBitmap = new Bitmap(_newWidth, _newHeight)) {
                    var graphics = Graphics.FromImage(FinalBitmap);
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.FillRectangle(Brushes.White, 0, 0, _newWidth, _newHeight);
                    graphics.DrawImage(bitmap, 0, 0, _newWidth, _newHeight);
                    context.HttpContext.Response.ContentType = "image/png";
                    FinalBitmap.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
                }
            }
        }
    }
}
