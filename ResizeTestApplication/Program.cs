
namespace ResizeTestApplication
{


    class Program
    {


        // https://github.com/JimBobSquarePants/ImageSharp
        static void Main(string[] args)
        {
            System.Console.WriteLine($"{System.IntPtr.Size * 8}-Bit");
            System.Console.WriteLine($"Hardware-Accelerated: {System.Numerics.Vector.IsHardwareAccelerated}");
            string sourceFile = "orig.jpg";

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            const int size = 300;
            const int quality = 75;
#if true 
            using (System.IO.FileStream strmInput = System.IO.File.OpenRead(sourceFile))
            {
                using (System.IO.FileStream strmOutput = System.IO.File.OpenWrite("resized.jpg"))
                {
                    using (ImageSharp.Image imgSource = new ImageSharp.Image(strmInput))
                    {

                        using (ImageSharp.Image<ImageSharp.Color> destImage = ImageSharp.ImageExtensions
                            .Resize(imgSource
                                , new ImageSharp.Processing.ResizeOptions
                                {
                                     Size = new ImageSharp.Size(size, size) 
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
                            destImage.Save(strmOutput);
                        } // End Using destImage 

                    } // End Using imgSource 

                } // End Using strmOutput 

            } // End Using strmInput 
#else

            using (ImageSharp.Image image = new ImageSharp.Image(sourceFile))
            {
                /*
                image.Resize(image.Width / 2, image.Height / 2)
                     .Grayscale()
                     .Save("bar.jpg"); // automatic encoder selected based on extension.
                */

                using (ImageSharp.Image<ImageSharp.Color> imgResized = ImageSharp.ImageExtensions
                    .Resize(image, image.Width / 2, image.Height / 2)
                    )
                {

#if false 
                    using (ImageSharp.Image<ImageSharp.Color> imgGray = ImageSharp.ImageExtensions
                        .Grayscale(imgResized))
                    {
                        imgGray.Save(@"grayscale.jpg");
                    } // End Using imgGray 

#else
                    // OMG after grayscale, was disposed...
                    imgResized.Save(@"resized.jpg");
#endif
                } // End Using imgResized

            } // End Using image 
#endif
            sw.Stop();

            System.Console.WriteLine($"Elapsed time: {sw.Elapsed.TotalSeconds}");

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 


    } // End Class Program 


} // End Namespace ResizeTestApplication 
