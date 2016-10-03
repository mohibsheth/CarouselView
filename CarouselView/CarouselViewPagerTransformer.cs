using Android.Support.V4.View;
using Android.Views;
using Math = System.Math;

namespace CarouselView
{
    public class CarouselViewPagerTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        private const float MinScaleDepth = 0.75f;
        private const float MinScaleZoom = 0.85f;
        private const float MinAlphaZoom = 0.5f;
        private const float ScaleFactorSlide = 0.85f;
        private const float MinAlphaSlide = 0.35f;

        private readonly CarouselViewPagerTransformerType _mTransformType;

        public CarouselViewPagerTransformer(CarouselViewPagerTransformerType transformType)
        {
            this._mTransformType = transformType;
        }

        public void TransformPage(View page, float position)
        {
            float alpha;
            float scale;
            float translationX;
            switch (this._mTransformType)
            {
                case CarouselViewPagerTransformerType.Flow:
                    page.RotationY = position * -30;
                    return;
                case CarouselViewPagerTransformerType.SlideOver:
                    if (position < 0 && position > -1)
                    {
                        //  this is the page to the left
                        scale = Math.Abs(Math.Abs(position) - 1) * (1 - ScaleFactorSlide) + ScaleFactorSlide;
                        alpha = Math.Max(MinAlphaSlide, 1 - Math.Abs(position));
                        int pageWidth = page.Width;
                        float translateValue = position * (pageWidth * -1);
                        if (translateValue > pageWidth * -1)
                        {
                            translationX = translateValue;
                        }
                        else
                        {
                            translationX = 0;
                        }
                    }
                    else
                    {
                        alpha = 1;
                        scale = 1;
                        translationX = 0;
                    }
                    break;
                case CarouselViewPagerTransformerType.Depth:
                    if (position > 0 && position < 1)
                    {
                        //  moving to the right
                        alpha = 1 - position;
                        scale = MinScaleDepth + (1 - MinScaleDepth) * (1 - Math.Abs(position));
                        translationX = page.Width * (position * -1);
                    }
                    else
                    {
                        //  use default for all other cases
                        alpha = 1;
                        scale = 1;
                        translationX = 0;
                    }
                    break;
                case CarouselViewPagerTransformerType.Zoom:
                    if (position >= -1 && position <= 1)
                    {
                        scale = Math.Max(MinScaleZoom, 1 - Math.Abs(position));
                        alpha = MinAlphaZoom + (scale - MinScaleZoom) / ((1 - MinScaleZoom) * (1 - MinAlphaZoom));
                        float vMargin = page.Height * ((1 - scale) / 2);
                        float hMargin = page.Width * ((1 - scale) / 2);
                        if (position < 0)
                        {
                            translationX = hMargin - vMargin / 2;
                        }
                        else
                        {
                            translationX = hMargin * -1 + vMargin / 2;
                        }
                    }
                    else
                    {
                        alpha = 1;
                        scale = 1;
                        translationX = 0;
                    }
                    break;
                default:
                    return;
            }
            page.Alpha = alpha;
            page.TranslationX = translationX;
            page.ScaleX = scale;
            page.ScaleY = scale;
        }
    }

    public enum CarouselViewPagerTransformerType
    {
        Default,
        Flow,
        SlideOver,
        Depth,
        Zoom
    }
}