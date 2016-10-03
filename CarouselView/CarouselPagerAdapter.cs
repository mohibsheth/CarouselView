using System;
using Android.Content;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace CarouselView
{
    public class CarouselPagerAdapter : PagerAdapter
    {
        private Context mContext;
        private CarouselView _carouselView;
        private IImageListener _mImageListener;
        private IViewListener _mViewListener = null;

        public CarouselPagerAdapter(Context context, CarouselView carouselView, IImageListener mImageListener, IViewListener mViewListener)
        {
            this.mContext = context;
            this._carouselView = carouselView;
            this._mImageListener = mImageListener;
            this._mViewListener = mViewListener;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup collection, int position)
        {
            Java.Lang.Object objectToReturn = null;
            // Either let user set image to ImageView
            if (_mImageListener != null)
            {
                ImageView imageView = new ImageView(this.mContext);
                imageView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                // setting image position
                imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                objectToReturn = imageView;
                _mImageListener.SetImageForPosition(position, imageView);
                collection.AddView(imageView);
                // Or let user add his own ViewGroup
            }
            else if (_mViewListener != null)
            {
                View view = _mViewListener.SetViewForPosition(position);
                if (null != view)
                {
                    objectToReturn = view;
                    collection.AddView(view);
                }
                else
                {
                    throw new Exception("View can not be null for position " + position);
                }
            }
            else
            {
                throw new Exception("View must set ImageListener or ViewListener");
                //throw new RuntimeException(("View must set " + (ImageListener.class.getSimpleName() + (" or " + (ViewListener.class.getSimpleName() + ".")))));
            }

            return objectToReturn;
        }

        public override void DestroyItem(ViewGroup collection, int position, Java.Lang.Object view)
        {
            collection.RemoveView((View)view);
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object obj)
        {
            return view == obj;
        }

        public override int Count
        {
            get
            {
                return this._carouselView.GetPageCount();
            }
        }
    }
}