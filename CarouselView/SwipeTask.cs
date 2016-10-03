using Java.Util;

namespace CarouselView
{
    public class SwipeTask : TimerTask
    {
        private CarouselView _carouselView;
        private CarouselViewPager _containerViewPager;

        public SwipeTask(CarouselView carouselView, CarouselViewPager containerViewPager)
        {
            this._carouselView = carouselView;
            this._containerViewPager = containerViewPager;
        }

        public override void Run()
        {
            _containerViewPager.Post(() =>
            {
                int nextPage = (_containerViewPager.CurrentItem + 1) % this._carouselView.GetPageCount();
                _containerViewPager.SetCurrentItem(nextPage, 0 != nextPage || this._carouselView.AnimateOnBoundary);
            });
        }
    }
}