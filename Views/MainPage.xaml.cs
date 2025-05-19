using FaviconMaker.ViewModels;
using SkiaSharp;
using Svg.Skia;

namespace FaviconMaker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        
        // TODO OnPaintIcon OnPaintSelectedIcon





    }
}
