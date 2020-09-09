using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventsDelegatesDemo
{
    public partial class Form1 : Form
    {

        Bitmap newFile;

        ImageManipulation modifyRGB = new ImageManipulation();
        FileOperations getFile = new FileOperations();

        public Form1()
        {
            InitializeComponent();

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

            DisplayImage(newFile, 3);
        }

        public void OnImageFinished(object sender, ImageEventArgs e)
        {
            DisplayImage(e.bmap, 2);
            label1.Text = "Event Handler Recieved";

        }



        private void btnManipulate_Click(object sender, EventArgs e)
        {
            modifyRGB.Manipulate(newFile);
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

  
    }
}
