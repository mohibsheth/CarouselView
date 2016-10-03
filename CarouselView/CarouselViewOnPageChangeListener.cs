using Android.Support.V4.View;

namespace CarouselView
{
    public class CarouselViewOnPageChangeListener : Java.Lang.Object, ViewPager.IOnPageChangeListener
    {
        private CarouselView _carouselView;

        public CarouselViewOnPageChangeListener(CarouselView carouselView)
        {
            this._carouselView = carouselView;
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            //Programmatic scroll
        }

        public void OnPageSelected(int position)
        {

        }

        public void OnPageScrollStateChanged(int state)
        {
            if (_carouselView.PreviousState == ViewPager.ScrollStateDragging && state == ViewPager.ScrollStateSettling)
            {
                if (_carouselView.DisableAutoPlayOnUserInteraction)
                {
                    _carouselView.PauseCarousel();
                }
                else
                {
                    _carouselView.PlayCarousel();
                }

            }
            else if (_carouselView.PreviousState == ViewPager.ScrollStateSettling && state == ViewPager.ScrollStateIdle)
            {
            }

            _carouselView.PreviousState = state;
        }
    }
}