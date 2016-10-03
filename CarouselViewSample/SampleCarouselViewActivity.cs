using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using CarouselView;
using FFImageLoading;
using FFImageLoading.Views;
using System;

namespace CarouselViewSample
{
	[Activity(Label = "CarouselView Sample", MainLauncher = true, Icon = "@drawable/icon")]
	public class SampleCarouselViewActivity : AppCompatActivity
	{
		public static int[] SampleImages = { Resource.Drawable.image_1, Resource.Drawable.image_2, Resource.Drawable.image_3, Resource.Drawable.image_4 };
		public static string[] SampleTitles = { "Orange", "Grapes", "Strawberry", "Cherry", "Apricot" };
		public static string[] SampleNetworkImageUrLs = {
			"https://placeholdit.imgix.net/~text?txtsize=15&txt=image1&txt=350%C3%97150&w=350&h=150",
			"https://placeholdit.imgix.net/~text?txtsize=15&txt=image2&txt=350%C3%97150&w=350&h=150",
			"https://placeholdit.imgix.net/~text?txtsize=15&txt=image3&txt=350%C3%97150&w=350&h=150",
			"https://placeholdit.imgix.net/~text?txtsize=15&txt=image4&txt=350%C3%97150&w=350&h=150",
			"https://placeholdit.imgix.net/~text?txtsize=15&txt=image5&txt=350%C3%97150&w=350&h=150"
		};

		CarouselView.CarouselView _carouselView;
		CarouselView.CarouselView _customCarouselView;

		TextView _carouselLabel;
		TextView _customCarouselLabel;
		Button _pauseButton;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_sample_carousel_view);
			this._carouselView = (CarouselView.CarouselView)FindViewById(Resource.Id.carouselView);
			this._customCarouselView = (CarouselView.CarouselView)FindViewById(Resource.Id.customCarouselView);
			this._carouselLabel = (TextView)FindViewById(Resource.Id.carouselLabel);
			this._customCarouselLabel = (TextView)FindViewById(Resource.Id.customCarouselLabel);
			this._pauseButton = (Button)FindViewById(Resource.Id.pauseButton);
			this._pauseButton.SetOnClickListener(new SamplePauseClickListener(_carouselView, _customCarouselView));
			this._carouselView.SetViewListener(new SampleFFViewListener(_carouselView));
			//this._carouselView.SetImageListener(new SampleImageListener());
			this._customCarouselView.SetViewListener(new SampleViewListener(_customCarouselView));
			this._carouselView.SetPageCount(SampleImages.Length);
			this._customCarouselView.SetPageCount(SampleImages.Length);
			this._customCarouselView.SetSlideInterval(4000);
		}
	}

	public class SampleImageListener : Java.Lang.Object, IImageListener
	{
		public void SetImageForPosition(int position, ImageView imageView)
		{
			var iv = imageView as ImageViewAsync;

			ImageService.Instance.LoadUrl(SampleCarouselViewActivity.SampleNetworkImageUrLs[position])
						.LoadingPlaceholder("placeholder.jpg", FFImageLoading.Work.ImageSource.ApplicationBundle)
						.ErrorPlaceholder("error.jpg", FFImageLoading.Work.ImageSource.ApplicationBundle)
						.Retry(3, 200)
						.Into(iv);
		}
	}

	public class SampleFFViewListener : Java.Lang.Object, IViewListener
	{
		private CarouselView.CarouselView _carouselView;

		public SampleFFViewListener(CarouselView.CarouselView carouselView)
		{
			_carouselView = carouselView;
		}

		public View SetViewForPosition(int position)
		{
			var iv = new ImageViewAsync(_carouselView.Context);
			iv.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
			ImageService.Instance.LoadUrl(SampleCarouselViewActivity.SampleNetworkImageUrLs[position])
						.LoadingPlaceholder("Images/placeholder.jpg", FFImageLoading.Work.ImageSource.ApplicationBundle)
						.ErrorPlaceholder("Images/error.jpg", FFImageLoading.Work.ImageSource.ApplicationBundle)
						.Retry(3, 200)
						.Into(iv);
			return iv;
		}
	}

	public class SampleViewListener : Java.Lang.Object, IViewListener
	{
		private CarouselView.CarouselView _customCarouselView;

		public SampleViewListener(CarouselView.CarouselView customCarouselView)
		{
			_customCarouselView = customCarouselView;
		}

		public View SetViewForPosition(int position)
		{
			View customView = LayoutInflater.FromContext(_customCarouselView.Context).Inflate(Resource.Layout.view_custom, null);

			TextView labelTextView = (TextView)customView.FindViewById(Resource.Id.labelTextView);
			ImageView fruitImageView = (ImageView)customView.FindViewById(Resource.Id.fruitImageView);

			fruitImageView.SetImageResource(SampleCarouselViewActivity.SampleImages[position]);
			labelTextView.Text = SampleCarouselViewActivity.SampleTitles[position];

			_customCarouselView.SetIndicatorGravity(GravityFlags.CenterHorizontal | GravityFlags.Top);

			return customView;
		}
	}

	public class SamplePauseClickListener : Java.Lang.Object, View.IOnClickListener
	{
		private CarouselView.CarouselView _carouselView;
		private CarouselView.CarouselView _customCarouselView;

		public SamplePauseClickListener(CarouselView.CarouselView carouselView, CarouselView.CarouselView customCarouselView)
		{
			_carouselView = carouselView;
			_customCarouselView = customCarouselView;
		}

		public void OnClick(View v)
		{
			_carouselView.PauseCarousel();
			_customCarouselView.ReSetSlideInterval(0);
		}
	}
}

