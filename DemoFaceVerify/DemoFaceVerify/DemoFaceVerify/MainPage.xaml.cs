using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace DemoFaceVerify
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

	    private async void TakePicture1(object sender, EventArgs e)
	    {
	        await CrossMedia.Current.Initialize();

	        if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
	        {
	            await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

	            return;
	        }

	        var file = await CrossMedia.Current.TakePhotoAsync(
	            new StoreCameraMediaOptions
	            {
	                SaveToAlbum = false,
	                PhotoSize = PhotoSize.Medium
                });

	        if (file == null)
	            return;


	        MinhaImagem1.Source = ImageSource.FromStream(() =>
	        {
	            var stream = file.GetStream();
	            file.Dispose();


	            return stream;

	        });
	    }

	    private async void TakePicture2(object sender, EventArgs e)
	    {
	        await CrossMedia.Current.Initialize();

	        if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
	        {
	            await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

	            return;
	        }

	        var file = await CrossMedia.Current.TakePhotoAsync(
	            new StoreCameraMediaOptions
	            {
	                SaveToAlbum = false,
	                PhotoSize = PhotoSize.Medium
	            });

	        if (file == null)
	            return;


	        MinhaImagem2.Source = ImageSource.FromStream(() =>
	        {
	            var stream = file.GetStream();
	            file.Dispose();


	            return stream;

	        });
	    }

    }
}
