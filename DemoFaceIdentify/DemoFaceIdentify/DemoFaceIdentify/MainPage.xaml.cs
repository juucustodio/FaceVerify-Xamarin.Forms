using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace DemoFaceIdentify
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

	    private async void TakePicture(object sender, EventArgs e)
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
	                Directory = "Demo"
	            });

	        if (file == null)
	            return;


	        MinhaImagem.Source = ImageSource.FromStream(() =>
	        {
	            var stream = file.GetStream();
	            file.Dispose();


	            return stream;

	        });
	    }
    }
}
