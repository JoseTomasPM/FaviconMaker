using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FaviconMaker.Models;
using System.Reflection;
using SkiaSharp;

namespace FaviconMaker.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IconItem> IconItems { get; set; } = new();

        private IconItem _selectedIcon;
        public IconItem SelectedIcon
        {
            get => _selectedIcon;
            set
            {
                if (_selectedIcon != value)
                {
                    _selectedIcon = value;
                    OnPropertyChanged();
                    IconSelected?.Invoke();
                }
            }
        }
        public event Action IconSelected;

        private SKColor _backgroundColor = SKColors.White;

        public SKColor BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }



        private SKColor _iconColor = SKColors.White;

        public SKColor IconColor
        {
            get => _iconColor;
            set
            {
                _iconColor = value;
                OnPropertyChanged();
            }
        }




        public MainViewModel()
        {
            LoadIcons();
        }


        private void LoadIcons()
        {
            // Simula la carga desde Resources. En una app real,
            // deberías copiar los íconos al FileSystem o usar EmbeddedResources.

            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();

            // Filtra íconos embebidos en una carpeta Resources.Icons
            foreach (var res in resourceNames)
            {
                if (res.EndsWith(".svg"))
                {
                    var name = res.Split('.').Reverse().Skip(1).First(); // ejemplo: "icon1"
                    IconItems.Add(new IconItem
                    {
                        Name = name,
                        FilePath = res // Ruta embedded, útil para stream
                    });
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



    }
}
