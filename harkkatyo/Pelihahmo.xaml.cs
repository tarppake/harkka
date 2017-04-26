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

namespace harkkatyo
{
    public sealed partial class Pelihahmo : UserControl
    {
        public double step = 5;

        // hahmo on törmännyt
        public bool hit = false;

        public Pelihahmo()
        {
            this.InitializeComponent();
        }

        //sijainti x ja y -akselit
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public object SpriteSheetOffset { get; private set; }
        public double OldLocationX;
        public double OldLocationY;

        //vauhti
        public void MoveLeft()
        {
            OldLocationX = LocationX;
            OldLocationY = LocationY;
            //LocationX -= 10;
            UpdateLocation();
            if (hit == false)
                LocationX -= step;
            else step = 0;
        }

        // OIKEALLE
        public void MoveRight()
        {
            OldLocationX = LocationX;
            OldLocationY = LocationY;
            //LocationX += 10;
            UpdateLocation();
            if (hit == false)
                LocationX += step;
            else step = 0;           
        }
     
        private void UpdateLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
        }

        //YLÖS
        public void MoveUp()
        {
            OldLocationX = LocationX;
            OldLocationY = LocationY;
            //LocationY -= 10;
            UpdateLocation();
            if (hit == false)
                LocationY -= step;
            else step = 0;
        }

        //ALAS
        public void MoveDown()
        {
            OldLocationX = LocationX;
            OldLocationY = LocationY;
            //LocationY += 10;
            UpdateLocation();
            if (hit == false)
                LocationY += step;
            else step = 0;
        }

        // sijainnin päivitys
        public void SetLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX); // vaakalinja
            SetValue(Canvas.TopProperty, LocationY); // pystylinja
        }

        internal void Location()
        {
            throw new NotImplementedException();
        }



    }
}
