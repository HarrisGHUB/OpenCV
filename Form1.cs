using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
//using Emgu.CV.UI;
using Emgu.CV.Flann;
using Emgu.CV.Util;

namespace ImageFeature
{

    public partial class Form1 : Form
    {
        SURFDetector surfCPU = new SURFDetector(300, false);
        Matrix<float> matrix, supermatrix;
        VectorOfKeyPoint observedKeyPoints, modelKeyPoints;
        Image<Gray, byte> observedImage, modelImage;
        int k = 2;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Pruebas();
        }

        //
        


        private void Pruebas()
        {

            //Load Image To Compare With ALL Images in Database
            observedImage = new Image<Gray, byte>(@"C:\Users\haris\Desktop\angle.png");
            observedKeyPoints = surfCPU.DetectKeyPointsRaw(observedImage, null);
            Matrix<float> observedDescriptors = surfCPU.ComputeDescriptorsRaw(observedImage, null, observedKeyPoints);

            //Path With ALL Images Database
            //We will to build the big index for use with flann algorithm
            string[] images = Directory.GetFiles(@"C:\Users\haris\Desktop\crop faces", "*.jpg");
            foreach (string image in images)
            {
                DateTime start = DateTime.Now;
                Debug.WriteLine("Current Image  " + image);
                modelImage = new Image<Gray, byte>(image);

                modelKeyPoints = surfCPU.DetectKeyPointsRaw(modelImage, null);
                matrix = surfCPU.ComputeDescriptorsRaw(modelImage, null, modelKeyPoints);

                if (supermatrix == null)
                {
                    supermatrix = new Matrix<float>(matrix.Rows, matrix.Cols);
                    supermatrix = matrix;
                }
                else
                {
                    supermatrix = supermatrix.ConcateVertical(matrix);
                }


                DateTime end = DateTime.Now;

                Debug.WriteLine("Process Time " + (end - start));

            }

            //At this point, we should to build the index of the big  matrix
            Index fln = new Index(supermatrix, 4);

            //Retrieve The Rows supermatrixs

            Matrix<int> indices = new Matrix<int>(supermatrix.Rows, k);
            Matrix<float> dist = new Matrix<float>(supermatrix.Rows, k);


            fln.KnnSearch(observedDescriptors, indices, dist, k, 12);

            Debug.WriteLine("-----------------------");

            for (int i = 0; i < indices.Size.Height; i++)
            {
                if (dist.Data[i, 0] < 0.6 * dist.Data[i, 1])
                {
                    Debug.WriteLine("This Point Is Good");
                    Debug.WriteLine("The image most similar is" + i);
                }

            }


        }
    }
}
