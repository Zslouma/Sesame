using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.Core.Models
{
    public class MenuNavigation : MenuItem
    {
        public bool IsSelected { get; set; } = false;

        public string IconFontAwesome { get; set; }
    }
}
