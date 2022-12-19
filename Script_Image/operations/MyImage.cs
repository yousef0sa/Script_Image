using Microsoft.VisualBasic;
using OpenCvSharp;
using System;
using System.Collections.Generic;


namespace ScriptImage
{
    public partial class MyImage : IDisposable
    {
        #region Variables

        private Mat _mainImage;
        private Mat _subImage;
        private Mat _result;
        private List<(int X, int Y)> _centerListPoint = new List<(int X, int Y)>();
        private List<Rect> _ListRect = new List<Rect>();

        private double maxVal, minVal, _threshold;
        private (int X, int Y) _CenterPo;
        private Point minLoc, maxLoc;

        #endregion

        #region Get_Set

        public double GetThreshold
        {
            get { return _threshold; }
            set { _threshold = value; }
        }
        public (int X, int Y) GetCenterPoint
        {
            get { return getCenterPoint(); }
            set { _CenterPo = value; }
        }
        public List<(int X, int Y)> GetCenterListPoint
        {
            get { return getCenterListPoint(); }
        }
        public (int X, int Y) GetImageSize
        {
            get { return getImageSize(); }
        }
        public Point GetMaxLoc
        {
            get { return maxLoc; }
        }
        public Point GetMinLoc
        {
            get { return minLoc; }
        }
        public double GetMaxVal
        {
            get { return maxVal; }
        }
        public double GetMinVal
        {
            get { return minVal; }
        }
        public Rect GetRect
        {
            get { return getRect(); }
        }
        public Mat GetResult
        {
            get { return _result; }
        }
        public Mat GetSubImage
        {
            get { return _subImage; }
        }
        public List<Rect> GetListRects
        { get { return getListRect(); } }
        #endregion

        #region Methods

        //Image matching
        public void MatchTemplate(Mat mainImage, string subImage, double threshold = 0.50, int match_method = 5)
        {

            Mat subImg = new Mat(subImage);
            Mat result = new Mat();
            using (Mat mref = mainImage.CvtColor(ColorConversionCodes.BGR2GRAY))
            using (Mat sref = subImg.CvtColor(ColorConversionCodes.BGR2GRAY))
            {
                Cv2.MatchTemplate(mref, sref, result, (TemplateMatchModes)match_method);
                Cv2.Threshold(result, result, threshold, 1.0, ThresholdTypes.Tozero);
            }
            updateData(mainImage, subImg, result, threshold);
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

        //Draw multi rectangle on image [Overloading]
        public void DrawMultiRec()
        {
            {
                while (true)
                {
                    double minval, maxval;
                    Point minloc, maxloc;
                    Cv2.MinMaxLoc(_result, out minval, out maxval, out minloc, out maxloc);

                    if (maxval >= _threshold)
                    {
                        //Setup the rectangle to draw
                        Rect r = new Rect(new Point(maxloc.X, maxloc.Y), new Size(_subImage.Width, _subImage.Height));

                        //Draw a rectangle of the matching area
                        Cv2.Rectangle(_mainImage, r, Scalar.LimeGreen, 2);

                        //Fill in the res Mat so you don't find the same area again in the MinMaxLoc
                        Cv2.FloodFill(_result, maxloc, new Scalar(0), out Rect outRect, new Scalar(0.1), new Scalar(1.0));
                    }
                    else
                        break;
                }
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

        //Draw rectangle on image [Overloading]
        public void DrawRec()
        {
            double minval, maxval;
            Point minloc, maxloc;
            Cv2.MinMaxLoc(_result, out minval, out maxval, out minloc, out maxloc);

            Rect rec = new Rect(new Point(maxloc.X, maxloc.Y), new Size(_subImage.Width, _subImage.Height));
            Cv2.Rectangle(_mainImage, rec, Scalar.LimeGreen, 2);
        }

        //Dispose this for fix memory leak
        public void Dispose()
        {
            _subImage?.Dispose();
            _result?.Dispose();
        }
        #endregion

        #region Local function

        //Update variable data
        private void updateData (Mat mainImage, Mat subImage, Mat result, double threshold)
        {
            this._mainImage = mainImage;
            this._subImage = subImage;
            this._result = result;
            this._threshold = threshold;
            Cv2.MinMaxLoc(_result, out minVal, out maxVal, out minLoc, out maxLoc);
        }

        //return Center Point of image
        private (int X, int Y) getCenterPoint()
        {
            return (_CenterPo.X = maxLoc.X + _subImage.Width / 2, _CenterPo.Y = maxLoc.Y + _subImage.Height / 2);
        }

        //return list Center Point
        private List<(int X, int Y)> getCenterListPoint()
        {
            while (true)
            {

                double minval, maxval;
                Point minloc, maxloc;
                Cv2.MinMaxLoc(_result, out minval, out maxval, out minloc, out maxloc);

                if (maxval >= _threshold)
                {

                    //Fill in the res Mat so you don't find the same area again in the MinMaxLoc
                    Cv2.FloodFill(_result, maxloc, new Scalar(0), out Rect outRect, new Scalar(0.1), new Scalar(1.0));
                    _centerListPoint.Add((maxloc.X + _subImage.Width / 2, maxloc.Y = maxloc.Y + _subImage.Height / 2));

                }
                else
                    break;
            }
            return _centerListPoint;
        }

        //return image size
        private (int X, int Y) getImageSize()
        {
            return (_subImage.Width, _subImage.Height);
        }

        //return rectangle of single image
        private Rect getRect()
        {
            return new Rect(new Point(maxLoc.X, maxLoc.Y), new Size(_subImage.Width, _subImage.Height));
        }

        //return list of rectangle
        private List<Rect> getListRect()
        {
            while (true)
            {

                double minval, maxval;
                Point minloc, maxloc;
                Cv2.MinMaxLoc(_result, out minval, out maxval, out minloc, out maxloc);

                if (maxval >= _threshold)
                {
                    //Setup the rectangle to draw
                    Rect r = new Rect(new Point(maxloc.X, maxloc.Y), new Size(_subImage.Width, _subImage.Height));

                    //Fill in the res Mat so you don't find the same area again in the MinMaxLoc
                    Cv2.FloodFill(_result, maxloc, new Scalar(0), out Rect outRect, new Scalar(0.1), new Scalar(1.0));
                    _ListRect.Add(r);

                }
                else
                    break;
            }
            return _ListRect;

        }


        #endregion
    }
}


