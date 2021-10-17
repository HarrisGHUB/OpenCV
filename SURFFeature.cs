//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.Runtime.InteropServices;
//using System.Windows.Forms;
//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Features2D;
//using Emgu.CV.Structure;
//using Emgu.CV.UI;

//namespace ImageFeature
//{
//   static class Program
//   {
//      /// <summary>
//      /// The main entry point for the application.
//      /// </summary>
//      [STAThread]
//      //static void Main()
//      //{
//      //   if (!IsPlaformCompatable()) return;
//      //   Application.EnableVisualStyles();
//      //   Application.SetCompatibleTextRenderingDefault(false);
//      //   Run();
//      //}






//      static void Run()
//      {
//         SURFDetector surfParam = new SURFDetector(500, false);

//         //Image<Gray, Byte> modelImage = new Image<Gray, byte>("box.png");


//         Image<Gray, Byte> modelImage = new Image<Gray, byte>(@"C:\Emgu\emgucv-windows-x86 2.2.1.1150\bin\images\small5.jpg");
//         //extract features from the object image
//         ImageFeature[] modelFeatures = surfParam.DetectFeatures(modelImage, null);

//         //Create a Feature Tracker
//         Features2DTracker tracker = new Features2DTracker(modelFeatures);

//         //Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");

//         Image<Gray, Byte> observedImage = new Image<Gray, byte>(@"C:\Emgu\emgucv-windows-x86 2.2.1.1150\bin\images\octar.png");

//         //Image<Bgr, Byte> observedImage1 = new Image<Bgr, byte>(@"C:\Emgu\emgucv-windows-x86 2.2.1.1150\bin\images\news.gif");

//         Stopwatch watch = Stopwatch.StartNew();
//         // extract features from the observed image
//         ImageFeature[] imageFeatures = surfParam.DetectFeatures(observedImage, null);

//         Features2DTracker.MatchedImageFeature[] matchedFeatures = tracker.MatchFeature(imageFeatures, 2, 20);
//         matchedFeatures = Features2DTracker.VoteForUniqueness(matchedFeatures, 0.8);
//         matchedFeatures = Features2DTracker.VoteForSizeAndOrientation(matchedFeatures, 1.5, 20);
//         HomographyMatrix homography = Features2DTracker.GetHomographyMatrixFromMatchedFeatures(matchedFeatures);
//         watch.Stop();
          
          
//         //Merge the object image and the observed image into one image for display
//         Image<Gray, Byte> res = modelImage.ConcateVertical(observedImage);

         
//         //Bitmap bm = new Bitmap(res.ToBitmap());

//         //Image<Bgr, Byte> res1 = modelImage.ConcateVertical(bm);
//         #region draw lines between the matched features
//         //foreach (Features2DTracker.MatchedImageFeature matchedFeature in matchedFeatures)
//         //{
//         //    PointF p = matchedFeature.ObservedFeature.KeyPoint.Point;
//         //    p.Y += modelImage.Height;
//         //    res.Draw(new LineSegment2DF(matchedFeature.SimilarFeatures[0].Feature.KeyPoint.Point, p), new Gray(0), 1);
//         //}
//         #endregion

//         #region draw the project region on the image
//         if (homography != null)
//         {  //draw a rectangle along the projected model
//             Rectangle rect = modelImage.ROI;
//             //MessageBox.Show(rect.Width.ToString());
//             //MessageBox.Show(rect.Height.ToString());
             
//             PointF[] pts = new PointF[] { 
//               new PointF(rect.Left, rect.Bottom),
//               new PointF(rect.Right, rect.Bottom),
//               new PointF(rect.Right, rect.Top),
//               new PointF(rect.Left, rect.Top)};
//             homography.ProjectPoints(pts);

//             for (int i = 0; i < pts.Length; i++)
//                 pts[i].Y += modelImage.Height;

//                                                            //new Gray(255.0),5
//             res.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Gray(50.0), 30);
//             //observedImage.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Gray(100.0), 50);
//             //observedImage1.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true,new Bgr(Color.Red), 20);

//             //CvInvoke.cvWatershed(
//         }
//         #endregion

//         ImageViewer.Show(res, String.Format("Matched in {0} milliseconds", watch.ElapsedMilliseconds));
//         //ImageViewer.Show(observedImage1, String.Format("Matched in {0} milliseconds", watch.ElapsedMilliseconds));
         
//         //ImageViewer.Show(pb);
          
//      }
       

//      /// <summary>
//      /// Check if both the managed and unmanaged code are compiled for the same architecture
//      /// </summary>
//      /// <returns>Returns true if both the managed and unmanaged code are compiled for the same architecture</returns>
//      static bool IsPlaformCompatable()
//      {
//         int clrBitness = Marshal.SizeOf(typeof(IntPtr)) * 8;
//         if (clrBitness != CvInvoke.UnmanagedCodeBitness)
//         {
//            MessageBox.Show(String.Format("Platform mismatched: CLR is {0} bit, C++ code is {1} bit." 
//               + " Please consider recompiling the executable with the same platform target as C++ code.", 
//               clrBitness, CvInvoke.UnmanagedCodeBitness));
//            return false;
//         }
//         return true;
//      }
//   }
//}
