using Android.Widget;

namespace CarouselView
{
    /**
    * Created by Sayyam on 3/15/16.
    * To set simple ImageView drawable in CarouselView
    */
    public interface IImageListener
    {
        void SetImageForPosition(int position, ImageView imageView);
    }
}