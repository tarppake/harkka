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
        // animaation timer
        private DispatcherTimer timer;

        // Hahmon näkyminen
        private int currentFrame = 0;
        private int frameHeight = 46; // objektin koko

        //vauhti
        private readonly double Maxspeed = 10; //maksimivauhti

        // hahmo on törmännyt
        public bool hit = false;
        private double step = 3;

        public Pelihahmo()
        {
            this.InitializeComponent();

            //animaatio
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 125);
            // millisekunnit
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        //sijainti x ja y -akselit
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public object SpriteSheetOffset { get; private set; }

        internal void Move(int v)
        {
            throw new NotImplementedException();
        }

        private void Timer_Tick(object sender, object e)
        {
            SpriteSheetOffset = currentFrame * -frameHeight;
        }

        //vauhti
        public void MoveLeft()
        {

            LocationX -= 10;
            UpdateLocation();
            if (hit == false) LocationX += step;

        }

        // OIKEALLE
        public void MoveRight()
        {
            LocationX += 10;
            UpdateLocation();
            if (hit == false) LocationX -= step;
        }
        
        
        private void UpdateLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);

        }

        //YLÖS
        public void MoveUp()
        {
            LocationY -= 10;
            UpdateLocation();
            if (hit == false) {
                LocationY += step;
            };
        }

        //ALAS
        public void MoveDown()
        {
            LocationY += 10;
            UpdateLocation();
            if (hit == false) LocationY -= step;
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
