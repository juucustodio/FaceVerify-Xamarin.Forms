using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace DemoFaceVerify
{
	public partial class MainPage : ContentPage
	{
	    private readonly IFaceServiceClient _faceServiceClient;
	    private readonly Guid personId;
        private Guid faceId1;
	    private Guid faceId2;
	    private string filePath1;

        public MainPage()
		{
			InitializeComponent();

		    _faceServiceClient = new FaceServiceClient("KEY","URL");

		    faceId1 = new Guid();
		    faceId2 = new Guid();
		    personId = new Guid("PERSONID");
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

	        filePath1 = file.Path;
	        await FaceAddInPerson(file.Path);

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

	        await FaceDetect(file.Path);

	        MinhaImagem2.Source = ImageSource.FromStream(() =>
	        {
	            var stream = file.GetStream();
	            file.Dispose();


	            return stream;

	        });
	    }

	    private async Task FaceAddInPerson(string image)
	    {
            
	        // Call the Face API.
	        try
	        {
	            using (Stream imageFileStream = File.OpenRead(image))
	            {
	                var result = await _faceServiceClient.AddPersonFaceAsync("grupo",personId, imageFileStream);

	                 faceId1 =  result.PersistedFaceId;

	            }

	            await _faceServiceClient.TrainPersonGroupAsync("grupo");
            }
	        // Catch and display Face API errors.
	        catch (FaceAPIException f)
	        {
	            await DisplayAlert("Error", f.ErrorMessage, "ok");
	        }
	        // Catch and display all other errors.
	        catch (Exception e)
	        {
	            await DisplayAlert("Error", e.Message, "ok");
	        }

        }

        public async Task FaceDetect(string image)
	    {

	        // Call the Face API.
	        try
	        {
	            using (Stream imageFileStream = File.OpenRead(image))
	            {
	                var faces = await _faceServiceClient.DetectAsync(imageFileStream,
	                                                                 returnFaceId: true,
	                                                                 returnFaceLandmarks: false,
	                                                                 returnFaceAttributes: null);


	                //Get First Face in List
	                if (faces.Length > 0)
	                    faceId2 = faces[0].FaceId;

	            }
	        }
	        // Catch and display Face API errors.
	        catch (FaceAPIException f)
	        {
	            await DisplayAlert("Error", f.ErrorMessage, "ok");
	        }
	        // Catch and display all other errors.
	        catch (Exception e)
	        {
	            await DisplayAlert("Error", e.Message, "ok");
	        }
	    }

	    public async void FaceVerify(object sender, EventArgs e)
	    {
	        if (faceId1 == new Guid())
	            await FaceAddInPerson(filePath1);

            // Call the Face API.
            try
	        {


	            var result = await _faceServiceClient.VerifyAsync(faceId2,"grupo",personId);


	            await DisplayAlert("Result", 
                                   "IsIdentical: " + result.IsIdentical +
                                   " Confidence: " + result.Confidence, 
                                   "OK");

	            await _faceServiceClient.DeletePersonFaceAsync("grupo", personId, faceId1);
                faceId1 = new Guid();

            }
	        // Catch and display Face API errors.
	        catch (FaceAPIException f)
	        {
	            await DisplayAlert("Error", f.ErrorMessage, "ok");
	        }
	        // Catch and display all other errors.
	        catch (Exception ex)
	        {
	            await DisplayAlert("Error", ex.Message, "ok");
	        }
	    }

	    

	}
}
