using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace CarouselView
{
    public class CarouselView : FrameLayout
    {
        private static int _defaultSlideInterval = 3500;
        private static int _defaultSlideVelocity = 400;
        private int _mPageCount;
        private int _slideInterval = _defaultSlideInterval;
        private GravityFlags _mIndicatorGravity = GravityFlags.CenterHorizontal | GravityFlags.Bottom;
        private int _indicatorMarginVertical;
        private int _indicatorMarginHorizontal;
        private int _pageTransformInterval = _defaultSlideVelocity;
        private CarouselViewPager _containerViewPager;
        private CirclePageIndicator _mIndicator;
        private IViewListener _mViewListener = null;
        private IImageListener _mImageListener = null;
        private Timer _swipeTimer;
        private SwipeTask _swipeTask;
        private bool _autoPlay;
        private bool _disableAutoPlayOnUserInteraction;
        private bool _animateOnBoundary = true;
        private int _previousState;
        private ViewPager.IPageTransformer _pageTransformer;

        public bool AnimateOnBoundary
        {
            get { return _animateOnBoundary; }
            set { _animateOnBoundary = value; }
        }

        public int PreviousState
        {
            get { return _previousState; }
            set { _previousState = value; }
        }

        public bool DisableAutoPlayOnUserInteraction
        {
            get { return _disableAutoPlayOnUserInteraction; }
            set { _disableAutoPlayOnUserInteraction = value; }
        }

        public CarouselView(Context context) :
            base(context)
        {
        }

        public CarouselView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            this.InitView(context, attrs, 0, 0);
        }

        public CarouselView(Context context, IAttributeSet attrs, int defStyleAttr) :
            base(context, attrs, defStyleAttr)
        {
            this.InitView(context, attrs, defStyleAttr, 0);
        }

        public CarouselView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) :
            base(context, attrs, defStyleAttr, defStyleRes)
        {
            this.InitView(context, attrs, defStyleAttr, defStyleRes);
        }

        private void InitView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
        {
            if (IsInEditMode)
            {
                return;
            }
            else
            {
                View view = LayoutInflater.From(context).Inflate(Resource.Layout.view_carousel, this, true);
                this._containerViewPager = (CarouselViewPager)view.FindViewById(Resource.Id.containerViewPager);
                this._mIndicator = (CirclePageIndicator)view.FindViewById(Resource.Id.indicator);
                this._containerViewPager.AddOnPageChangeListener(new CarouselViewOnPageChangeListener(this));
                // Retrieve styles attributes
                TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.CarouselView, defStyleAttr, 0);
                try
                {
                    SetIndicatorMarginVertical(a.GetInt(Resource.Styleable.CarouselView_indicatorMarginVertical,
                        Resources.GetDimensionPixelSize(Resource.Dimension.default_indicator_margin_vertical)));
                    SetIndicatorMarginHorizontal(a.GetInt(Resource.Styleable.CarouselView_indicatorMarginHorizontal,
                        Resources.GetDimensionPixelSize(Resource.Dimension.default_indicator_margin_horizontal)));
                    this.SetPageTransformInterval(a.GetInt(Resource.Styleable.CarouselView_pageTransformInterval,
                        _defaultSlideVelocity));
                    this.SetSlideInterval(a.GetInt(Resource.Styleable.CarouselView_slideInterval, _defaultSlideInterval));
                    SetOrientation(a.GetInt(Resource.Styleable.CarouselView_indicatorOrientation,
                        (int)Android.Widget.Orientation.Horizontal));
                    SetIndicatorGravity((GravityFlags)a.GetInt(Resource.Styleable.CarouselView_indicatorGravity, (int)(GravityFlags.Bottom | GravityFlags.CenterHorizontal)));
                    SetAutoPlay(a.GetBoolean(Resource.Styleable.CarouselView_autoPlay, true));
                    SetDisableAutoPlayOnUserInteraction(
                        a.GetBoolean(Resource.Styleable.CarouselView_disableAutoPlayOnUserInteraction, false));
                    SetAnimateOnBoundary(a.GetBoolean(Resource.Styleable.CarouselView_animateOnBoundary, true));
                    this.SetPageTransformer(new CarouselViewPagerTransformer((CarouselViewPagerTransformerType)a.GetInt(Resource.Styleable.CarouselView_pageTransformer, (int)CarouselViewPagerTransformerType.Default)));
                    Color fillColor = a.GetColor(Resource.Styleable.CarouselView_fillColor, 0);
                    if (fillColor != 0)
                    {
                        SetFillColor(fillColor);
                    }

                    Color pageColor = a.GetColor(Resource.Styleable.CarouselView_pageColor, 0);
                    if (pageColor != 0)
                    {
                        SetPageColor(pageColor);
                    }

                    float radius = a.GetDimensionPixelSize(Resource.Styleable.CarouselView_radius, 0);
                    if (radius != 0)
                    {
                        SetRadius(radius);
                    }

                    SetSnap(a.GetBoolean(Resource.Styleable.CarouselView_snap,
                        Resources.GetBoolean(Resource.Boolean.default_circle_indicator_snap)));
                    Color strokeColor = a.GetColor(Resource.Styleable.CarouselView_strokeColor, 0);
                    if (strokeColor != 0)
                    {
                        SetStrokeColor(strokeColor);
                    }

                    float strokeWidth = a.GetDimensionPixelSize(Resource.Styleable.CarouselView_strokeWidth, 0);
                    if (strokeWidth != 0)
                    {
                        SetStrokeWidth(strokeWidth);
                    }
                }
                finally
                {
                    a.Recycle();
                }
            }
        }

        public int GetSlideInterval()
        {
            return this._slideInterval;
        }

        public void SetSlideInterval(int slideInterval)
        {
            this._slideInterval = slideInterval;
            if (null != this._containerViewPager)
            {
                PlayCarousel();
            }
        }

        public void ReSetSlideInterval(int slideInterval)
        {
            this.SetSlideInterval(this._slideInterval);
            if (null != this._containerViewPager)
            {
                PlayCarousel();
            }
        }

        public void SetPageTransformInterval(int pageTransformInterval)
        {
            if (this._pageTransformInterval > 0)
            {
                this._pageTransformInterval = pageTransformInterval;
            }
            else
            {
                this._pageTransformInterval = _defaultSlideVelocity;
            }

            this._containerViewPager.SetTransitionVelocity(this._pageTransformInterval);
        }

        public ViewPager.IPageTransformer GetPageTransformer()
        {
            return this._pageTransformer;
        }

        public void SetPageTransformer(ViewPager.IPageTransformer pageTransformer)
        {
            this._pageTransformer = pageTransformer;
            this._containerViewPager.SetPageTransformer(true, this._pageTransformer);
        }

        public void SetPageTransformer()
        {
        }

        public void SetAnimateOnBoundary(bool animateOnBoundary)
        {
            this._animateOnBoundary = animateOnBoundary;
        }

        public bool IsAutoPlay()
        {
            return _autoPlay;
        }

        private void SetAutoPlay(bool autoPlay)
        {
            this._autoPlay = autoPlay;
        }

        public bool IsDisableAutoPlayOnUserInteraction()
        {
            return _disableAutoPlayOnUserInteraction;
        }

        private void SetDisableAutoPlayOnUserInteraction(bool disableAutoPlayOnUserInteraction)
        {
            this._disableAutoPlayOnUserInteraction = disableAutoPlayOnUserInteraction;
        }

        private void SetData()
        {
            CarouselPagerAdapter carouselPagerAdapter = new CarouselPagerAdapter(Context, this, _mImageListener, _mViewListener);
            _containerViewPager.Adapter = carouselPagerAdapter;
            _mIndicator.SetViewPager(_containerViewPager);
            _mIndicator.RequestLayout();
            _mIndicator.Invalidate();
            _containerViewPager.OffscreenPageLimit = GetPageCount();
            PlayCarousel();
        }

        private void StopScrollTimer()
        {
            _swipeTimer?.Cancel();
            _swipeTask?.Cancel();
        }

        private void ResetScrollTimer()
        {
            StopScrollTimer();
            _swipeTask = new SwipeTask(this, _containerViewPager);
            _swipeTimer = new Timer();
        }

        public void PlayCarousel()
        {
            ResetScrollTimer();
            if (_autoPlay && (_slideInterval > 0) && (_containerViewPager.Adapter != null) && (_containerViewPager.Adapter.Count > 1))
            {
                _swipeTimer.Schedule(_swipeTask, _slideInterval, _slideInterval);
            }
        }

        public void PauseCarousel()
        {
            ResetScrollTimer();
        }

        public void StopCarousel()
        {
            this._autoPlay = false;
        }

        public void SetImageListener(IImageListener mImageListener)
        {
            this._mImageListener = mImageListener;
        }

        public void SetViewListener(IViewListener mViewListener)
        {
            this._mViewListener = mViewListener;
        }

        public int GetPageCount()
        {
            return _mPageCount;
        }

        public void SetPageCount(int mPageCount)
        {
            this._mPageCount = mPageCount;
            SetData();
        }

        public void AddOnPageChangeListener(ViewPager.IOnPageChangeListener listener)
        {
            _containerViewPager.AddOnPageChangeListener(listener);
        }

        public void SetCurrentItem(int item)
        {
            _containerViewPager.CurrentItem = item;
        }

        public int GetIndicatorMarginVertical()
        {
            return _indicatorMarginVertical;
        }

        public void SetIndicatorMarginVertical(int indicatorMarginVertical)
        {
            this._indicatorMarginVertical = indicatorMarginVertical;
        }

        public int GetIndicatorMarginHorizontal()
        {
            return _indicatorMarginHorizontal;
        }

        public void SetIndicatorMarginHorizontal(int indicatorMarginHorizontal)
        {
            this._indicatorMarginHorizontal = indicatorMarginHorizontal;
        }

        public GravityFlags GetIndicatorGravity()
        {
            return _mIndicatorGravity;
        }

        public void SetIndicatorGravity(GravityFlags gravity)
        {
            _mIndicatorGravity = gravity;
            FrameLayout.LayoutParams parms = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            parms.Gravity = _mIndicatorGravity;
            parms.SetMargins(_indicatorMarginHorizontal, _indicatorMarginVertical, _indicatorMarginHorizontal, _indicatorMarginVertical);
            _mIndicator.LayoutParameters = parms;
        }

        public int GetOrientation()
        {
            return _mIndicator.GetOrientation();
        }

        public int GetFillColor()
        {
            return _mIndicator.GetFillColor();
        }

        public int GetStrokeColor()
        {
            return _mIndicator.GetStrokeColor();
        }

        public void SetSnap(bool snap)
        {
            _mIndicator.SetSnap(snap);
        }

        public void SetRadius(float radius)
        {
            _mIndicator.SetRadius(radius);
        }

        public float GetStrokeWidth()
        {
            return _mIndicator.GetStrokeWidth();
        }

        public void SetBackground(Drawable background)
        {
            base.Background = background;
        }

        public Drawable GetIndicatorBackground()
        {
            return _mIndicator.Background;
        }

        public void SetFillColor(Color fillColor)
        {
            _mIndicator.SetFillColor(fillColor);
        }

        public int GetPageColor()
        {
            return _mIndicator.GetPageColor();
        }

        public void SetOrientation(int orientation)
        {
            _mIndicator.setOrientation(orientation);
        }

        public bool IsSnap()
        {
            return _mIndicator.IsSnap();
        }

        public void SetStrokeColor(Color strokeColor)
        {
            _mIndicator.SetStrokeColor(strokeColor);
        }

        public float GetRadius()
        {
            return _mIndicator.GetRadius();
        }

        public void SetPageColor(Color pageColor)
        {
            _mIndicator.SetPageColor(pageColor);
        }

        public void SetStrokeWidth(float strokeWidth)
        {
            _mIndicator.SetStrokeWidth(strokeWidth);
            int padding = (int)strokeWidth;
            _mIndicator.SetPadding(padding, padding, padding, padding);
        }
    }
}