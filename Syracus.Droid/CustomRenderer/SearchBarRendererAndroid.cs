using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mobitheque.Droid.CustomRenderer;
using Syracuse.Mobitheque.UI.Views.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.SearchBar), typeof(Mobitheque.Droid.CustomRenderer.SearchBarRendererAndroid))]

namespace Mobitheque.Droid.CustomRenderer
{
    [Obsolete]
    class SearchBarRendererAndroid : Xamarin.Forms.Platform.Android.SearchBarRenderer
    {

            protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
            {
                base.OnElementChanged(e);

                if (Control != null)
                {
                    SearchView searchView = Control;
                    var searchIconId = searchView.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                    if (searchIconId > 0)
                    {
                        var searchPlateIcon = searchView.FindViewById(searchIconId);
                        (searchPlateIcon as ImageView).SetColorFilter(Color.Black.ToAndroid());
                    }
                    LinearLayout linearLayout = this.Control.GetChildAt(0) as LinearLayout;
                    linearLayout = linearLayout.GetChildAt(2) as LinearLayout;
                    linearLayout = linearLayout.GetChildAt(1) as LinearLayout;
                    if (linearLayout != null)
                    {
                        linearLayout.Background.ClearColorFilter();
                        linearLayout.Background.SetColorFilter(Color.Black.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcIn);
                    }
                }
            }
        }
    }
