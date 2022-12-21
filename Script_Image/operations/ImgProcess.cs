using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace ScriptImage
{
    public partial class ImgProcess
    {
        #region Variables

        private Mat _mainImage;
        private Mat _subImage;
        private Mat _result;
        private List<(int X, int Y)> _centerListPoint = new List<(int X, int Y)>();
        private List<Rect> _ListRect = new List<Rect>();

        private double _maxVal, _minVal, _threshold;
        private (int X, int Y) _CenterPo;
        private Point _minLoc, _maxLoc;

        #endregion

        #region Methods

        //Image matching
        public static ImgProcessRef MatchTemplate(Mat mainImage, string subImage, double threshold = 0.50, int match_method = 5)
        {

            Mat subImg = new Mat(subImage);
            Mat result = new Mat();
            using (Mat mref = mainImage.CvtColor(ColorConversionCodes.BGR2GRAY))
            using (Mat sref = subImg.CvtColor(ColorConversionCodes.BGR2GRAY))
            {
                Cv2.MatchTemplate(mref, sref, result, (TemplateMatchModes)match_method);
                Cv2.Threshold(result, result, threshold, 1.0, ThresholdTypes.Tozero);
            }

            return new ImgProcessRef(mainImage, subImg, result, threshold);
        }

        //MatchTemplate in range
        public static ImgProcessRef MatchTemplateInRange(Mat mainImage, string subImage, (int x, int y) Start, (int x, int y) End,
            double threshold = 0.50, int match_method = 5)
        {
            //chick if start and end are in range
            if (Start.x < 0 || Start.y < 0 || End.x > mainImage.Width || End.y > mainImage.Height)
            {
                throw new Exception("Start or End are out of range");
            }

            Mat subImg = new Mat(subImage);
            Mat result = new Mat();
            using (Mat mref = mainImage.CvtColor(ColorConversionCodes.BGR2GRAY))
            using (Mat sref = subImg.CvtColor(ColorConversionCodes.BGR2GRAY))
            {
                //Set range
                using (var range = mref[RangeCalculator(Start, End)])
                {
                    Cv2.MatchTemplate(range, sref, result, (TemplateMatchModes)match_method);
                    Cv2.Threshold(result, result, threshold, 1.0, ThresholdTypes.Tozero);
                }
            }
            return new ImgProcessRef(mainImage, subImg, result, threshold, Start);
        }

        //Draw multi rectangle on image
        public static void DrawMultiRec(Mat mainImage, Mat subImage, Mat result, double threshold)
        {

            while (true)
            {
                double minval, maxval;
                Point minloc, maxloc;
                Cv2.MinMaxLoc(result, out minval, out maxval, out minloc, out maxloc);

                if (maxval >= threshold)
                {
                    //Setup the rectangle to draw
                    Rect r = new Rect(new Point(maxloc.X, maxloc.Y), new Size(subImage.Width, subImage.Height));

                    //Draw a rectangle of the matching area
                    Cv2.Rectangle(mainImage, r, Scalar.LimeGreen, 2);

                    //Fill in the res Mat so you don't find the same area again in the MinMaxLoc

                    Cv2.FloodFill(result, maxloc, new Scalar(0), out Rect outRect, new Scalar(0.1), new Scalar(1.0));

                }
                else
                    break;
            }


        }


        //Draw rectangle on image
        public static void DrawRec(Mat mainImage, Mat subImage, Mat result)
        {
            double minval, maxval;
            Point minloc, maxloc;
            Cv2.MinMaxLoc(result, out minval, out maxval, out minloc, out maxloc);
            Rect rec = new Rect(new Point(maxloc.X, maxloc.Y), new Size(subImage.Width, subImage.Height));
            Cv2.Rectangle(mainImage, rec, Scalar.LimeGreen, 2);
        }

        //Dispose this for fix memory leak
        public void Dispose()
        {
            _subImage?.Dispose();
            _result?.Dispose();
        }


        #endregion

        #region Local function

        //get the Start X and Y, End X and Y of the image, and return the lowest two numbers and the height and width to make a range.
        static Rect RangeCalculator((int x, int y) Start, (int x, int y) End)
        {

            //get the lowest two numbers and the highest two numbers.
            //var numbers = new[] { Start.x, Start.y, End.x, End.y };
            //var lowestTwo = numbers.OrderBy(n => n).Take(2);
            //var highestTwo = numbers.OrderByDescending(n => n).Take(2);

            //get height and width.
            var width = End.x - Start.x;
            var height = End.y - Start.y;

            //return the lowest two numbers and the height and width.
            return new Rect(Start.x, Start.y, width, height);
        }

        #endregion
    }
}


