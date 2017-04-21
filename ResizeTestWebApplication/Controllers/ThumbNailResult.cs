
namespace ResizeTestWebApplication.Tools
{


    //
    // Summary:
    //     Channel selector type for the SkiaSharp.SKBitmap.Resize method or the SkiaSharp.SKPixmap.Resize
    //     method.
    public enum ResizeAlgorithm
    {
        //
        // Summary:
        //     Use the box interpolated filter (Shrink: average color; Grow: pixel replication).
        BoxInterpolation = 0, // Box
        //
        // Summary:
        //     Use the box triangle or bilinear filter.
        Bilinear = 1, // Triangle
        //
        // Summary:
        //     Use the Lanczos windowed Sinc filter.
        Bicubic = 2, // Lanczos
        // Bicubic can be regardes as computationally efficient approximation to Lanczos sampling
        // Best possible reconstruction for perfectly bandlimited signal 

        //
        // Summary:
        //     Use the Hamming windowed Sinc filter (cosine bell variant).
        CosineBell = 3,
        //
        // Summary:
        //     Use the Mitchell cubic filter.
        CubicInterpolation = 4 // ImageMagick-Default - tends to blur image in order to smooth edges
    }


    // https://weblogs.asp.net/imranbaloch/custom-actionresult-aspnet5-mvc6
    // https://stackoverflow.com/questions/34131326/using-mimemapping-in-asp-net-5-vnext/34131458
    // https://github.com/jstedfast/MimeKit
    // http://pwet.fr/man/linux/formats/magic
    // https://stackoverflow.com/questions/55869/determine-file-type-of-an-image
    public class ThumbnailResult : Microsoft.AspNetCore.Mvc.ActionResult
    {


        public static void ResizeImage(System.IO.Stream sourceStream
        , System.IO.Stream stream
        , float maxWidth, float maxHeight
        , MimeType thumbnailMime
        )
        {
            using (ImageSharp.Image imgSource = new ImageSharp.Image(sourceStream))
            {
                //float ratioX = maxWidth / imgSource.Width;
                //float ratioY = maxHeight / imgSource.Height;
                //float resizeFactor = System.Math.Min(ratioX, ratioY);

                //int newWidth = (int)(imgSource.Width * resizeFactor);
                //int newHeight = (int)(imgSource.Height * resizeFactor);


                using (ImageSharp.Image<ImageSharp.Color> destImage = ImageSharp.ImageExtensions
                    .Resize(imgSource
                        , new ImageSharp.Processing.ResizeOptions
                        {
                             Size = new ImageSharp.Size((int)maxWidth, (int)maxHeight)
                            ,Mode = ImageSharp.Processing.ResizeMode.Max

                            // Fastest, but horrible quality
                            // ,Sampler = new ImageSharp.Processing.NearestNeighborResampler()

                            // Slowest, but best quality 
                            ,Sampler = new ImageSharp.Processing.Lanczos3Resampler()

                            // No recognizable advantage  
                            // ,Sampler = new ImageSharp.Processing.MitchellNetravaliResampler()
                            // ,Sampler = new ImageSharp.Processing.SplineResampler()
                            // ,Sampler = new ImageSharp.Processing.RobidouxResampler()
                            // ,Sampler = new ImageSharp.Processing.TriangleResampler()

                            // Fastest with acceptable quality 
                            //,Sampler = new ImageSharp.Processing.BicubicResampler()
                        }
                    )
                )
                {
                    // destImage.ExifProfile = null;
                    // destImage.Quality = quality;

                    // var codec = new ImageSharp.Formats.PngEncoder();
                    // var format = new ImageSharp.Formats.PngFormat();
                    ImageSharp.Formats.IImageFormat format = null;

                    if (thumbnailMime == MimeType.Png)
                        format = new ImageSharp.Formats.PngFormat();
                    else if (thumbnailMime == MimeType.Jpeg)
                        format = new ImageSharp.Formats.JpegFormat();
                    else if (thumbnailMime == MimeType.Gif)
                        format = new ImageSharp.Formats.GifFormat();
                    else if (thumbnailMime == MimeType.Webp)
                        throw new System.NotSupportedException("MimeType.Webp");
                    else if (thumbnailMime == MimeType.Bmp)
                        format = new ImageSharp.Formats.BmpFormat();
                    else if (thumbnailMime == MimeType.Ico)
                        throw new System.NotSupportedException("MimeType.Ico");
                    else if (thumbnailMime == MimeType.Ktx)
                        throw new System.NotSupportedException("MimeType.Ico");
                    else if (thumbnailMime == MimeType.Wbmp)
                        throw new System.NotSupportedException("MimeType.Wbmp");
                    else
                        throw new System.NotSupportedException("MimeType");

                    destImage.Save(stream, format);
                } // End Using destImage 

            } // End Using imgSource 

        }


        public static void ResizeImage(string sourceFile, System.IO.Stream stream)
        {
            float maxSize = 200;

            using (System.IO.Stream strm = System.IO.File.OpenRead(sourceFile))
            {
                ResizeImage(strm, stream, maxSize, maxSize, MimeType.Jpeg);
            } // End Using strm 

        } // End Sub ResizeImage 


        public static void TestResizeImage()
        {
            string sourceFile = System.IO.Path.GetDirectoryName(
                System.Reflection.IntrospectionExtensions.GetTypeInfo(
                    typeof(ThumbnailResult)
                    ).Assembly.Location
            );

            sourceFile = System.IO.Path.Combine(sourceFile, "..", "..", "..", "wwwroot", "images", "orig.jpg");
            sourceFile = System.IO.Path.GetFullPath(sourceFile);

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            using (System.IO.Stream stream = new System.IO.FileStream("output.jpg"
                                , System.IO.FileMode.Create
                                , System.IO.FileAccess.Write
                                , System.IO.FileShare.None))
            {
                ResizeImage(sourceFile, stream);
            }

            sw.Stop();
            // 573 MS
            System.Console.WriteLine("Elapsed: " + sw.Elapsed.TotalMilliseconds.ToString());
        } // End Sub ResizeImage 



        public MimeType ThumbnailMime { get; private set; }
        public string FileDownloadName { get; set; }
        public System.IO.Stream SourceStream { get; set; }


        public float MaxWidth { get; set; }
        public float MaxHeight { get; set; }

        // Must be same as SkiaSharp-Type
        public enum MimeType : int
        {
            //
            // Summary:
            //     The image format is unknown.
            Unknown = 0,
            //
            // Summary:
            //     The BMP image format - Bitmap 
            Bmp = 1,
            //
            // Summary:
            //     The GIF image format - Graphics Interchange Format
            Gif = 2,
            //
            // Summary:
            //     The ICO image format - Windows Icon
            Ico = 3,
            //
            // Summary:
            //     The JPEG image format - Joint Photographic Experts Group
            Jpeg = 4,
            //
            // Summary:
            //     The PNG image format - Portable Network Graphics (PNG)
            Png = 5,
            //
            // Summary:
            //     The WBMP image format - Wireless Bitmap
            Wbmp = 6,
            //
            // Summary:
            //     The WEBP image format - Web Picture
            Webp = 7,
            //
            // Summary:
            //     The KTX image format - OpenGL Textures (KTX)
            Ktx = 8
        }


        private bool m_Dispose;


        public ThumbnailResult(string sourceImageFileName, MimeType thumbnailMimeFormat
            , float maxWidth, float maxHeight) : this(sourceImageFileName, thumbnailMimeFormat, maxWidth, maxHeight, null)
        { }


        public ThumbnailResult(string sourceImageFileName, MimeType thumbnailMimeFormat
            , float maxWidth, float maxHeight, string fileDownloadName) : this(
                    System.IO.File.OpenRead(sourceImageFileName)
                    , thumbnailMimeFormat, maxWidth, maxHeight, fileDownloadName)
        {
            this.m_Dispose = true;
        }


        public ThumbnailResult(System.IO.Stream sourceStream, MimeType thumbnailMimeFormat
            , float maxWidth, float maxHeight) : this(
                    sourceStream, thumbnailMimeFormat
                , maxWidth, maxHeight, null)
        { }


        public ThumbnailResult(System.IO.Stream sourceStream, MimeType thumbnailMimeFormat
            , float maxWidth, float maxHeight, string fileDownloadName) : base()
        {
            this.MaxWidth = maxWidth;
            this.MaxHeight = maxHeight;
            this.SourceStream = sourceStream;
            this.ThumbnailMime = thumbnailMimeFormat;
            this.FileDownloadName = fileDownloadName;
            this.m_Dispose = false;
        }


        public string MapMime(MimeType mt)
        {
            System.Collections.Generic.Dictionary<MimeType, string> dict =
                new System.Collections.Generic.Dictionary<MimeType, string>();

            dict.Add(MimeType.Unknown, "image/unknown");
            dict.Add(MimeType.Bmp, "image/bmp");
            dict.Add(MimeType.Gif, "image/gif");
            dict.Add(MimeType.Ico, "image/x-icon");
            dict.Add(MimeType.Jpeg, "image/jpeg");
            dict.Add(MimeType.Png, "image/png");
            dict.Add(MimeType.Wbmp, "image/vnd.wap.wbmp");
            dict.Add(MimeType.Webp, "image/webp");
            dict.Add(MimeType.Ktx, "image/ktx");

            return dict[mt];
        }


        //
        // Summary:
        //     Executes the result operation of the action method synchronously. This method
        //     is called by MVC to process the result of an action method.
        //
        // Parameters:
        //   context:
        //     The context in which the result is executed. The context information includes
        //     information about the action that was executed and request information.
        public override void ExecuteResult(Microsoft.AspNetCore.Mvc.ActionContext context)
        {
            context.HttpContext.Response.ContentType = MapMime(this.ThumbnailMime);
            // context.HttpContext.Response.ContentLength = 1024;
            ResizeImage(this.SourceStream, context.HttpContext.Response.Body
                , this.MaxWidth, this.MaxHeight, this.ThumbnailMime
                );

            if (this.m_Dispose && this.SourceStream != null)
                this.SourceStream.Dispose();
        }


        //
        // Summary:
        //     Executes the result operation of the action method asynchronously. This method
        //     is called by MVC to process the result of an action method. The default implementation
        //     of this method calls the Microsoft.AspNetCore.Mvc.ActionResult.ExecuteResult(Microsoft.AspNetCore.Mvc.ActionContext)
        //     method and returns a completed task.
        //
        // Parameters:
        //   context:
        //     The context in which the result is executed. The context information includes
        //     information about the action that was executed and request information.
        //
        // Returns:
        //     A task that represents the asynchronous execute operation.
        public /* async */ override System.Threading.Tasks.Task ExecuteResultAsync(
            Microsoft.AspNetCore.Mvc.ActionContext context)
        {

            if (this.FileDownloadName != null)
            {
                context.HttpContext.Response.Headers.Add("Content-Disposition"
                    , new[] { "attachment; filename=" + this.FileDownloadName }
                );
            }

            context.HttpContext.Response.ContentType = MapMime(this.ThumbnailMime);
            // context.HttpContext.Response.ContentLength = 1024;

            ResizeImage(this.SourceStream, context.HttpContext.Response.Body,
                this.MaxWidth, this.MaxHeight, this.ThumbnailMime
            );

            if (this.m_Dispose && this.SourceStream != null)
                this.SourceStream.Dispose();

            return System.Threading.Tasks.Task.FromResult(0);

            /*
            Microsoft.AspNetCore.Http.HttpResponse response = context.HttpContext.Response;
            response.ContentType = MapMime(this.ThumbnailMime);
            context.HttpContext.Response.Headers.Add("Content-Disposition"
                , new[] { "attachment; filename=" + "FileDownloadName" });

            using (System.IO.FileStream fs = new System.IO.FileStream("filePath", System.IO.FileMode.Open))
            {
                await fs.CopyToAsync(context.HttpContext.Response.Body);           
            }
            */
        }


    } // End Class ThumbNailResult : Microsoft.AspNetCore.Mvc.ActionResult 


} // End Namespace CoreCms.Tools 
