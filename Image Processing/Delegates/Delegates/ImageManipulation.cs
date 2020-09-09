using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Delegates
{        
    public class ImageEventArgs : EventArgs
    {
            public Bitmap bmap { get; set; }
    }

    class ImageManipulation
    {
        // Define the EventHandler Delegate


        // Manipulate method (bitmap)


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

    }

}
