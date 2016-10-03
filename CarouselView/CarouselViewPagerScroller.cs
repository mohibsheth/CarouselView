using Android.Content;
using Android.Views.Animations;
using Android.Widget;

namespace CarouselView
{
    public class CarouselViewPagerScroller : Scroller
    {
        private int _mScrollDuration = 600;

        public CarouselViewPagerScroller(Context context) : base(context)
        {
        }

        public CarouselViewPagerScroller(Context context, IInterpolator interpolator)
                : base(context, interpolator)
        {
        }

        public int MScrollDuration
        {
            get { return _mScrollDuration; }
            set { _mScrollDuration = value; }
        }

        public override void StartScroll(int startX, int startY, int dx, int dy, int duration)
        {
            base.StartScroll(startX, startY, dx, dy, _mScrollDuration);
        }

        public override void StartScroll(int startX, int startY, int dx, int dy)
        {
            base.StartScroll(startX, startY, dx, dy, _mScrollDuration);
        }
    }
}