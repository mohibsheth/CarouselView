# CarouselView

<p align="center"><img src="/assets/carousel_baner.jpg"></p>

CarouselView
=======
A simple yet flexible library to add carousel view in your Xamarin Android application. This is a C# port of CarouselView for Android which can be found at 


<img src="/assets/carousel_gif.gif" title="sample" width="500" height="460" />


Download
--------


Usage
--------

####Include following code in your layout:

```xml
    <com.cynapto.carouselview.CarouselView
        android:id="@+id/carouselView"
        android:layout_width="match_parent"
        android:layout_height="200dp"
        app:fillColor="#FFFFFFFF"
        app:pageColor="#00000000"
        app:radius="6dp"
        app:slideInterval="3000"
        app:strokeColor="#FF777777"
        app:strokeWidth="1dp"/>
```
####Include following code in your activity:
```cs
public class SampleCarouselViewActivity : AppCompatActivity
{
	public static int[] SampleImages = { Resource.Drawable.image_1, Resource.Drawable.image_2, Resource.Drawable.image_3, Resource.Drawable.image_4 };

	CarouselView.CarouselView _carouselView;

	protected override void OnCreate(Bundle savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		SetContentView(Resource.Layout.activity_sample_carousel_view);
		this._carouselView = (CarouselView.CarouselView)FindViewById(Resource.Id.carouselView);
		this._carouselView.SetImageListener(new SampleImageListener());
		this._carouselView.SetPageCount(SampleImages.Length);
	}
}

public class SampleImageListener : Java.Lang.Object, IImageListener
{
	public void SetImageForPosition(int position, ImageView imageView)
	{
		imageView.SetImageResource(SampleCarouselViewActivity.SampleImages[position]);
	}
}
```

####If you want to add custom view, implement ```IViewListener```.
```cs
public class SampleCarouselViewActivity : AppCompatActivity
{
	CarouselView.CarouselView _carouselView;

	protected override void OnCreate(Bundle savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		SetContentView(Resource.Layout.activity_sample_carousel_view);
		this._carouselView = (CarouselView.CarouselView)FindViewById(Resource.Id.carouselView);
		this._carouselView.SetViewListener(new SampleImageListener());
		this._carouselView.SetPageCount(5); //set your own image count.. you can even set this later after loading data from a service
	}
}

public class SampleViewListener : Java.Lang.Object, IViewListener
{
	private CarouselView.CarouselView _carouselView;

	public SampleViewListener(CarouselView.CarouselView carouselView)
	{
		_carouselView = carouselView;
	}

	public View SetViewForPosition(int position)
	{
		var iv = new ImageView(_carouselView.Context);
		//set view attributes here
		return iv;
	}
}
```

####Supported xml Attributes

| Attribute          	                    | Description          							   			  		 | Values 				        |
| ------------------------------------------|--------------------------------------------------------------------|------------------------------|
| app:slideInterval 	                    | Interval per page in ms.           			   		      		 | integer				        |
| app:indicatorGravity                      | Gravity of the indicator.  (Just like layout_gravity) 			 | gravity                      |
| app:indicatorOrientation                  | Orientation of the indicator. 					   			  	 | [horizontal, vertical]       |
| app:fillColor	  		                    | Color of the filled circle that represents the current page. 		 | color 				        |
| app:pageColor   		                    | Color of the filled circles that represents pages. 		  		 | color 				        |
| app:radius 			                    | Radius of the circles. This is also the spacing between circles.   | dimension 			        |
| app:snap 				                    | Whether or not the selected indicator snaps to the circles. 		 | boolean 				        |
| app:strokeColor 		                    | Width of the stroke used to draw the circles. 					 | color 				        |
| app:autoPlay                              | Whether or not to auto play. Default: true                         | boolean                      |
| app:disableAutoPlayOnUserInteraction      | Disables autoPlay when user interacts. Default: false              | boolean                      |
| app:indicatorMarginHorizontal 			| Sets horizontal margin for Indicator in Carousel View              | dimension 			        |
| app:indicatorMarginVertical 			    | Sets vertical margin for Indicator in Carousel View                | dimension 			        |
| app:pageTransformInterval                 | Sets speed at which page will slide from one to another in ms.     | integer                      |
| app:pageTransformer                       | Sets page transition animation.                                    | [zoom,flow,depth,slide_over] |
| app:animateOnBoundary                     | Sets whether to animate from last page. Default: true              | boolean                      |

_Note:_ Add ```xmlns:app="http://schemas.android.com/apk/res-auto"``` in your layout's root view.


Ported By
--------
- Mohib Sheth

License
--------

    Copyright 2016 Mohib.

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and 
    limitations under the License.
