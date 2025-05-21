using FaviconMaker.ViewModels;
using SkiaSharp;
using SkiaSharp.Views;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Svg.Skia;
using System.Reflection;
using FaviconMaker.Models;


namespace FaviconMaker
{
    public partial class MainPage : ContentPage
    {

        private MainViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            BindingContext = _viewModel;

            _viewModel.IconSelected += () =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SelectedIconCanvas?.InvalidateSurface();
                });
            };
        }


        // TODO OnPaintIcon OnPaintSelectedIcon

        // Renderiza cada ícono en la lista
        void OnPaintIcon(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            if (sender is SKCanvasView canvasView && canvasView.BindingContext is IconItem icon)
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(icon.FilePath);
                if(stream != null)
                {
                    var svg = new SKSvg();
                    svg.Load(stream);

                    var canvasSize = Math.Min(e.Info.Width, e.Info.Height);
                    var scale = Math.Min(canvasSize / svg.Picture.CullRect.Width, canvasSize / svg.Picture.CullRect.Height);

                    var matrix = SKMatrix.CreateScale(scale, scale);

                    canvas.DrawPicture(svg.Picture, ref matrix);

                }
                DrawSvgFromResource(icon.FilePath, e.Surface.Canvas, 30, 30);
            }
        }

        // Renderiza el ícono seleccionado en el centro
        private void OnPaintSelectedIcon(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(_viewModel.BackgroundColor);

            if (_viewModel.SelectedIcon != null)
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_viewModel.SelectedIcon.FilePath);
                if (stream != null)
                {
                    var svg = new SKSvg();
                    svg.Load(stream);

                    var canvasSize = Math.Min(e.Info.Width, e.Info.Height);
                    var scale = Math.Min(canvasSize / svg.Picture.CullRect.Width,
                                         canvasSize / svg.Picture.CullRect.Height);
                    var matrix = SKMatrix.CreateScale(scale, scale);

                    // Recolorea todo a iconColor
                    using var paint = new SKPaint { ColorFilter = SKColorFilter.CreateBlendMode(_viewModel.IconColor, SKBlendMode.SrcIn) };
                    canvas.DrawPicture(svg.Picture, ref matrix, paint);
                }
            }
        }


        // Utilidad para dibujar SVG embebido
        void DrawSvgFromResource(string resourcePath, SKCanvas canvas, int width, int height)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null) return;

            var svg = new SKSvg();
            svg.Load(stream);

            // Escala el SVG al tamaño del canvas
            var scale = Math.Min(width / svg.Picture.CullRect.Width, height / svg.Picture.CullRect.Height);
            var matrix = SKMatrix.CreateScale(scale, scale);

            canvas.Clear(SKColors.Transparent);
            canvas.DrawPicture(svg.Picture, ref matrix);
        }


        private void OnColorChanged(object sender, ValueChangedEventArgs e)
        {
            _viewModel.BackgroundColor = new SKColor(
                (byte)(BackgroundRed.Value * 255),
                (byte)(BackgroundGreen.Value * 255),
                (byte)(BackgroundBlue.Value * 255));

            _viewModel.IconColor = new SKColor(
                (byte)(IconRed.Value * 255),
                (byte)(IconGreen.Value * 255),
                (byte)(IconBlue.Value * 255));

            SelectedIconCanvas?.InvalidateSurface();
        }

        private void OnColorChangedHex(object sender, TextChangedEventArgs e)
        {
            var hex = e.NewTextValue?.Trim();

            if (!string.IsNullOrWhiteSpace(hex))
            {
                if (!hex.StartsWith("#"))
                    hex = "#" + hex;

                try
                {
                    var color = Color.FromArgb(hex);
                    var skColor = new SKColor(
                        (byte)(color.Red * 255),
                        (byte)(color.Green * 255),
                        (byte)(color.Blue * 255));

                    _viewModel.BackgroundColor = skColor;

                    BackgroundRed.Value = color.Red;
                    BackgroundGreen.Value = color.Green;
                    BackgroundBlue.Value = color.Blue;

                    SelectedIconCanvas?.InvalidateSurface();
                }
                catch { /* HEX inválido */ }
            }
        }


        private void OnIconColorChangedHex(object sender, TextChangedEventArgs e)
        {
            var hex = e.NewTextValue?.Trim();

            if (!string.IsNullOrWhiteSpace(hex))
            {
                if (!hex.StartsWith("#"))
                    hex = "#" + hex;

                try
                {
                    var color = Color.FromArgb(hex);
                    var skColor = new SKColor(
                        (byte)(color.Red * 255),
                        (byte)(color.Green * 255),
                        (byte)(color.Blue * 255));

                    _viewModel.IconColor = skColor;

                    IconRed.Value = color.Red;
                    IconGreen.Value = color.Green;
                    IconBlue.Value = color.Blue;

                    SelectedIconCanvas?.InvalidateSurface();
                }
                catch { /* HEX inválido */ }
            }
        }




    }
}
