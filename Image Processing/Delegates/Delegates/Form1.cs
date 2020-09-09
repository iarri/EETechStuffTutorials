using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Delegates
{
    public partial class Form1 : Form
    {

        Bitmap newFile;

        ImageManipulation modifyRGB = new ImageManipulation();
        FileOperations getFile = new FileOperations();

        public Form1()
        {
            InitializeComponent();

            btnManipulate.Enabled = false;

            //Subscribe the "OnImageFinished" event handler inside this calss
            // to the ImageFinished Event Handler delegate list (defined in the 
            //ImageManipulation Class)
            modifyRGB.ImageFinished += OnImageFinished;
        }

        public void DisplayImage(Bitmap b, int window)
        {
            if(window == 1)
            {
                pictureBox1.Image = b;
            }
            else if (window == 2)
            {
                pictureBox2.Image = b;
            }
            else
            {
                pictureBox1.Image = b;
                pictureBox2.Image = b;
            }

            btnManipulate.Enabled = true;
        }
        private void btnOpen_Click_1(object sender, EventArgs e)
        {
            // Call OpenFile method in FileOperations Class

            newFile = getFile.OpenFile();

            // Call DisplayImage method
            DisplayImage(newFile, 3);

        }
        private void btnManipulate_Click(object sender, EventArgs e)
        {

            #region Call ImageManipulation Class
            // Set up thread(s) to run "Manipulatie" method
            //Use parameterized thread start

            //**********************************************
            // NOTE: method "manipulate" needs to accept type "object", not Bitmap in order
            // for it to work with parameterized thread start
            // **********************************************

            Thread t1 = new Thread(new ParameterizedThreadStart(modifyRGB.Manipulate));
            t1.Start(newFile);
            #endregion

            //modifyRGB.Manipulate(newFile);

        }
        public void OnImageFinished(object sender, ImageEventArgs e)
        {
            DisplayImage(e.bmap, 2);
   
        }
        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
