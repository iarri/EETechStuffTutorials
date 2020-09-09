using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;


namespace DelegatesLockBits
{   
    // Set up custom class for event args to pass completed image
    public class ImageEventArgs : EventArgs
    {
            public Bitmap bmap { get; set; }
    }

    class ImageManipulation
    {
        
        //EventHandler<ImageEventArgs> is a delegate (ie, list of event handlers to invoke)
        //ImageFinished is an event associated with the EventHandler<ImageEventArgs> delegate
        //ImageEventArgs is a class which holds the event data(in this case bitmap)
        public event EventHandler<ImageEventArgs> ImageFinished;
     
        protected virtual void OnImageFinished(Bitmap bmap)
        {
            ImageFinished?.Invoke(this, new ImageEventArgs() { bmap = bmap });
        }

        public void Manipulate (object bmp)
        {

            Bitmap bmap = (Bitmap)bmp;

            Color theColor = new Color();

            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    // Get the color of the pixel at (i, j)
                    theColor = bmap.GetPixel(i, j);

                    // Change the color at that pixel; DivideByZeroException out the green and blue
                    Color newColor = Color.FromArgb(theColor.R, theColor.G, 0);

                    // Set the new color of the pixel
                    bmap.SetPixel(i, j, newColor);
                }
            }

            // Call the method to publish the event
            OnImageFinished(bmap);
        }

        public void Manipulate2(object bmp)
        {
            Bitmap bmap = (Bitmap)bmp;

            // Use "unsafe" becausae c# doesnt support pointer arithmetic by default
            unsafe
            {
                // Lock the bitmap into system memory
                // "Pixelformat" can be "Fomat24bppRgb", "Format32bppArgb", etc.
                BitmapData bitmapData =
                    bmap.LockBits(new Rectangle(0, 0, bmap.Width,
                    bmap.Height), ImageLockMode.ReadWrite, bmap.PixelFormat);

                //Define variables for bytes per pixel, as well as Image Width & Height
                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;

                //Define a pointer to the first pixel in the locked image
                // Scan0 gets or sets the address of the first pixel data in the bitmap
                // This can also be thought of as the first scan line in the bitmap.
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                // Step through each pixel in the image using pointers
                // Parallel.For executes a 'for' loop in which iterations
                // may run in parralel
                Parallel.For(0, heightInPixels, y =>
                {

                    // USe the 'Stride' (scanline width) proerty to step line by line thru the image
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {

                        // GET: each pixel color (R, G, & B)
                        int oldBlue = currentLine[x];
                        int oldGreen = currentLine[x + 1];
                        int oldRed = currentLine[x + 2];

                        // SET: Zero out the Blue, copy Green and Red unchanged
                        currentLine[x] = 0;
                        currentLine[x + 1] = (byte)oldGreen;
                        currentLine[x + 2] = (byte)oldRed;
                    }

                });

                bmap.UnlockBits(bitmapData);
            }
            OnImageFinished(bmap);
        }

    }

}
