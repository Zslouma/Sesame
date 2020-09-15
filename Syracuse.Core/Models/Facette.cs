using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.Core.Models
{
    public class Facette
    {
        public string Title { get; set; }


        public FacetteValue[] FacetteItem { get; set; }
    }


    public class FacetteValue 
    {
        public long id { get; set; }
        public int groupIndex { get; set; }
        public string value { get; set; }
        public FontAttributes font { get; set; } = FontAttributes.None;
        public bool noTitle { get; set; }
        private bool _isSelected { get; set; } = false;
        public bool IsSelected
        {
            get { return this._isSelected; }
            set {
                if (_isSelected != value)
                {
                    this._isSelected = value;
                }
            }
        }


    }

    public class FacetteGroup : ObservableCollection<FacetteValue>
    {
        private bool _expanded;
        public string Name { get; set; }
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                }
            }
        }
        public bool noTitle { get; set; }
        public int FoodCount { get; set; }

        public string StateIcon
        {
            get { return Expanded ? "expanded_icon.png" : "collapsed_icon.png"; }
        }

        public FacetteGroup(string name, bool expanded = false)
        {

            this.Name = name;
            this.Expanded = expanded;
            this.noTitle = false;
        }
    }

}
