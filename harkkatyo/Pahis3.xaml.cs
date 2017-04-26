using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace pelihahmoliikkuvuus
{
    public sealed partial class Pahis3 : UserControl
    {
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        private int direction = 1;
        private int MinY = 140;
        private int maxY = 500;
        private int step = 5;


        public Pahis3()
        {
            this.InitializeComponent();

        }
        public void Move()
        {
            if (LocationY < MinY || LocationY > maxY) direction = direction * (-1);
            LocationY += direction * step;
            UpdateLocation();
        }

        private void UpdateLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);

        }
        public void SetLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX); // vaakalinja
            SetValue(Canvas.TopProperty, LocationY); // pystylinja
        }
    }
}
