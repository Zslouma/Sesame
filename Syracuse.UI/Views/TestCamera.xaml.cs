using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Syracuse.UI.Views
{
    public class TestCamera : UIViewController, IUIImagePickerControllerDelegate, IUINavigationControllerDelegate

    [Export("imagePickerController:didFinishPickingMediaWithInfo:")]
    void FinishedPickingMedia(UIImagePickerController picker, NSDictionary dic)
    {
        UIImage img = dic.ObjectForKey(new NSString("UIImagePickerControllerOriginalImage")) as UIImage;

        picker.DismissViewController(true,()=>
        {
            ImageView.Image = img;

            img.SaveToPhotosAlbum((uiImage, nsError) =>
            {
                if (nsError != null)
                {
                 // do something about the error..
                }
                else
                {
                       // image should be saved
                }
            )
            };

     );}
}

  [Export("imagePickerControllerDidCancel:")]
void Canceled(UIImagePickerController picker)
{
    picker.DismissViewController(true, null);
}


// call this method as you want
void TakePhote()
{

    if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
    {
        UIImagePickerController picker = new UIImagePickerController();

        picker.SourceType = UIImagePickerControllerSourceType.Camera;

        picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;

        picker.VideoQuality = UIImagePickerControllerQualityType.High;

        picker.WeakDelegate = this;

        PresentViewController(picker, true, null);


    }
    else
    {
        Console.WriteLine("couldn't open the camera");
    }

}
}
