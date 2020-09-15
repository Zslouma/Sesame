using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Syracuse.UI.Views
{
    [assembly:ExportRenderer(typeof(ContentPage), typeof(CustomContentPageRenderer))]
    public partial class CustompageRenderer : PageRenderer
    {
        public pageRenderer()
        {
            InitializeComponent();
        }

        public override void ViewDidAppear()
        {
            var imagePicker = new UIImagePickerController { SourceType = UIImagePickerControllerSourceType.Camera };
        }
    }
}
