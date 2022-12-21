using OpenCvSharp;
using System.Collections.Generic;


namespace ScriptImage
{

    public class ImgProcessRef
    {

        #region Variables

        private Mat _mainImage;
        private Mat _subImage;
        private Mat _result;

        private double _maxVal, _minVal, _threshold;
        private Point _minLoc, _maxLoc;


        private List<(int X, int Y)> _centerListPoint = new List<(int X, int Y)>();
        private List<Rect> _ListRect = new List<Rect>();
        private (int X, int Y) _CenterPo;
        #endregion

        public ImgProcessRef(Mat mainImage, Mat subImage, Mat result, double threshold)
        {
            this._mainImage = mainImage;
            this._subImage = subImage;
            this._result = result;
            this._threshold = threshold;
            Cv2.MinMaxLoc(_result, out _minVal, out _maxVal, out _minLoc, out _maxLoc);

        }

        #region Methods

        //Dispose this for fix memory leak
        public void Dispose()
        {
            _subImage?.Dispose();
            _result?.Dispose();
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

        //Draw rectangle on image [Overloading]
        public void DrawRec()
        {
            double minval, maxval;
            Point minloc, maxloc;
            Cv2.MinMaxLoc(_result, out minval, out maxval, out minloc, out maxloc);

            Rect rec = new Rect(new Point(maxloc.X, maxloc.Y), new Size(_subImage.Width, _subImage.Height));
            Cv2.Rectangle(_mainImage, rec, Scalar.LimeGreen, 2);
        }
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
            get { return _maxLoc; }
        }
        public Point GetMinLoc
        {
            get { return _minLoc; }
        }
        public double GetMaxVal
        {
            get { return _maxVal; }
        }
        public double GetMinVal
        {
            get { return _minVal; }
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
        {
            get { return getListRect(); }
        }

        #endregion

        #region Private Methods
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

        //return rectangle of single image
        private Rect getRect()
        {
            return new Rect(new Point(_maxLoc.X, _maxLoc.Y), new Size(_subImage.Width, _subImage.Height));
        }

        //return image size
        private (int X, int Y) getImageSize()
        {
            return (_subImage.Width, _subImage.Height);
        }

        //return Center Point of image
        private (int X, int Y) getCenterPoint()
        {
            return (_CenterPo.X = _maxLoc.X + _subImage.Width / 2, _CenterPo.Y = _maxLoc.Y + _subImage.Height / 2);
        }
        #endregion
    }


}