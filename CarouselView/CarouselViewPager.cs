using Android.Content;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views.Animations;
using Java.Lang;
using Java.Lang.Reflect;
using Exception = System.Exception;

namespace CarouselView
{
    public class CarouselViewPager : ViewPager
    {
        public CarouselViewPager(Context context) :
            base(context)
        {
            this.PostInitViewPager();
        }

        public CarouselViewPager(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            this.PostInitViewPager();
        }

        private CarouselViewPagerScroller _mScroller = null;

        private void PostInitViewPager()
        {
            try
            {
                Class viewpager = Class.FromType(typeof(ViewPager));
                Field scroller = viewpager.GetDeclaredField("mScroller");
                scroller.Accessible = true;
                Field interpolator = viewpager.GetDeclaredField("sInterpolator");
                interpolator.Accessible = true;
                //var inte = interpolator.Get(this);
                this._mScroller = new CarouselViewPagerScroller(Context);//, (IInterpolator)inte);
                scroller.Set(this, this._mScroller);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public void SetTransitionVelocity(int scrollFactor)
        {
            this._mScroller.MScrollDuration = scrollFactor;
        }
    }
}