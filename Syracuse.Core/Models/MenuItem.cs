namespace Syracuse.Core.Models
{
    public class MenuItem
    {
        public string Text { get; }
        public string Icon { get; }

        public MenuItem(string text, string icon)
        {
            this.Text = text;
            this.Icon = icon;
        }
    }
}