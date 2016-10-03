using Android.Views;

namespace CarouselView
{
    /**
     * Created by Sayyam on 3/17/16.
     * To set custom View in CarouselView
     */
    public interface IViewListener
    {
        View SetViewForPosition(int position);
    }
}