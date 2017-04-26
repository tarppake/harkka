using harkkatyo;
using pelihahmoliikkuvuus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace harkkatyo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage1 : Page
    {
        //liikuteltava hahmo
        private Pelihahmo pelihahmo;
       
        //
        

        //viholliset
        private Pahis pahis;
        private Pahis2 pahis2;
        private Pahis3 pahis3;

        // näppäinten arvot ylös/alas
        private bool LeftPressed;
        private bool RightPressed;
        private bool Uppressed;
        private bool Downpressed;

        //lista seinä blockeille
        private List<lattia> walls;

        //karkkílista
        private List<Karkki> karkit;

        //karkkicount
        private int karkkicount;

        //maali
        private Maali maali;

        //pelilloopi
        private DispatcherTimer timer;

        public object SpriteSheetOffset { get; private set; }
        public double LocationY { get; private set; }
        public double LocationX { get; private set; }

        public GamePage1()
        {
            this.InitializeComponent();

            //pelihahmo taustaan
            pelihahmo = new Pelihahmo
            {
                LocationX = 47,
                LocationY = 506
            };
            Tausta.Children.Add(pelihahmo); //lisää pelihahmon taustaan

            //Viholliset
            pahis = new Pahis
            {
                LocationX = 184,
                LocationY = 50
            };
            Tausta.Children.Add(pahis);

            pahis2 = new Pahis2
            {
                LocationX = 230,
                LocationY = 370
            };
            Tausta.Children.Add(pahis2);

            pahis3 = new Pahis3
            {
                LocationX = 601,
                LocationY = 400
            };
            Tausta.Children.Add(pahis3);
            
            karkit = new List<Karkki>();

            
            //KARKIT
            Karkki karkki = new Karkki();
            karkit.Add(new Karkki { LocationX = 498, LocationY = 47 });
            karkit.Add(new Karkki { LocationX = 230, LocationY = 368 });
            karkit.Add(new Karkki { LocationX = 321, LocationY = 270 });
            karkit.Add(new Karkki { LocationX = 690, LocationY = 138 });
            karkit.Add(new Karkki { LocationX = 414, LocationY = 508 });

            foreach (Karkki Karkki in karkit)
            {
                Tausta.Children.Add(Karkki);
                Karkki.SetLocation();
            };

            //maali
            maali = new Maali
            {
                LocationX = 690,
                LocationY = 414
            };
            Tausta.Children.Add(maali);
            maali.SetLocation();

            //näppäimet alas/ylös
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            //pelilooppi
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            timer.Tick += Timer_Tick;
            timer.Start();

            //lataa kentän seinät
            Loadmap();


        }

        //jos painat jotain nuolta liikkuu sinneppäi
        private void Timer_Tick(object sender, object e)
        {
            if (LeftPressed) pelihahmo.MoveLeft();
            if (RightPressed) pelihahmo.MoveRight();
            if (Uppressed) pelihahmo.MoveUp();
            if (Downpressed) pelihahmo.MoveDown();

            pahis.Move();
            pahis2.Move();
            pahis3.Move();

            //päivitä sijainti
            pelihahmo.SetLocation();
            pahis.SetLocation();

            // collisionit
           KarkkiCollision();

           PahisCollision();
           Pahis2Collision();
           Pahis3Collision();

           MaaliCollide();
           WallCollision();

            VainSuoraa();
       
       }        

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    LeftPressed = false;
                    break;
                case VirtualKey.Right:
                    RightPressed = false;
                    break;
                case VirtualKey.Up:
                    Uppressed = false;
                    break;
                case VirtualKey.Down:
                    Downpressed = false;
                    break;
            }
        }
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    LeftPressed = true;
                    break;
                case VirtualKey.Right:
                    RightPressed = true;
                    break;
                case VirtualKey.Up:
                    Uppressed = true;
                    break;
                case VirtualKey.Down:
                    Downpressed = true;
                    break;
            }
        }

        //takaisin näppäin
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) return;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        //KARTAN LUKEMINEN TIEDOSTOSTA
        private async void Loadmap()
        {
            // yhteys assets-kansioon
            StorageFolder folder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file =
                await folder.GetFileAsync("maze1.txt");
            var stream = await file.OpenAsync(FileAccessMode.Read);

            // luetaan kaikki rivit
            string[] lines = System.IO.File.ReadAllLines(@"Assets\maze1.txt");

            int i = 0; // 0<->rivin pituus
            int j = 0; // 0<->rivien märärä

            // kaikki seinät
            walls = new List<lattia>();

            // käydään kaikki rivit läpi
            foreach (string line in lines)
            {
                // yksi rivi
                foreach(char c in line)
                {
                    // jos on 1 niin tehdään uusi seinä
                    if (c == '1') // wall
                    {
                        // uusi seinä
                        lattia lat = new lattia();
                        // sijainti vaakatasossa
                        lat.LocationX = i * 46;
                        // sijainti pystytasossa
                        lat.LocationY = j * 46;
                        // määritetään sijainti objektiin
                        lat.SetLocation();
                        // lisätään seiniin
                        walls.Add(lat);
                        // lisätään näytölle Canvakseen
                        Tausta.Children.Add(lat);
                    }
                    // seuraava sijainti tällä rivillä
                    i++;
                }
                // aloitetaan seuraavan merkkijonon alusta taas
                i = 0;
                // seuraava merkkijono (rivi)
                j++;
            }

        }

        //KARKKIEN KERÄÄMINEN
        public void KarkkiCollision()
        {
            //loop flowers list
            foreach (Karkki karkki in karkit)
            {
                //get rects
                Rect BRect = new Rect(
                    pelihahmo.LocationX, pelihahmo.LocationY,
                    pelihahmo.ActualWidth, pelihahmo.ActualHeight
                    );
                Rect FRect = new Rect(
                    karkki.LocationX, karkki.LocationY,
                    karkki.ActualWidth, karkki.ActualHeight
                    );
                // does objects intersects
                BRect.Intersect(FRect);
                if (!BRect.IsEmpty) //negaatio
                {
                    //remove flower from canvas
                    Tausta.Children.Remove(karkki);

                    //lisää muuttujaan +1
                    karkkicount ++;
                    //re,momve list
                    karkit.Remove(karkki);
                    break;
                }
            }
        }


        
        //SEINÄ COLLISION
        private void WallCollision()
        {
            foreach (lattia lattia in walls)
            {
                Rect WRerct = new Rect(
                    lattia.LocationX, lattia.LocationY,
                    lattia.ActualWidth, lattia.ActualHeight
                    );
                Rect BRect = new Rect(
                    pelihahmo.LocationX, pelihahmo.LocationY,
                    pelihahmo.ActualWidth, pelihahmo.ActualHeight
                    );
                BRect.Intersect(WRerct);
                if (!BRect.IsEmpty)
                {
                    pelihahmo.LocationX = pelihahmo.OldLocationX;
                    pelihahmo.LocationY = pelihahmo.OldLocationY;
                    //pelihahmo.step = -1;
                    break;
                }
               // pelihahmo.step = 1;              
            }
        }
        
        // VIHOLLIS TÖRMÄÄYS
        private void PahisCollision()
        {
            Rect PRect = new Rect(
                pahis.LocationX, pahis.LocationY,
                pahis.ActualWidth, pahis.ActualHeight);
            Rect HRect = new Rect(
                pelihahmo.LocationX, pelihahmo.LocationY,
                pelihahmo.ActualHeight, pelihahmo.ActualWidth);
            Rect PRect2 = new Rect(
                pahis2.LocationX, pahis2.LocationY,
                pahis2.ActualWidth, pahis2.ActualHeight);
            HRect.Intersect(PRect);
            if (!HRect.IsEmpty)
            {
                this.Frame.Navigate(typeof(GameOver));
            }
        }
        private void Pahis2Collision()
        {
            Rect HRect = new Rect(
                pelihahmo.LocationX, pelihahmo.LocationY,
                pelihahmo.ActualHeight, pelihahmo.ActualWidth);
            Rect PRect2 = new Rect(
                pahis2.LocationX, pahis2.LocationY,
                pahis2.ActualWidth, pahis2.ActualHeight);
            HRect.Intersect(PRect2);
            if (!HRect.IsEmpty)
            {
                this.Frame.Navigate(typeof(GameOver));
            }
        }
        private void Pahis3Collision()
        {
            Rect HRect = new Rect(
                pelihahmo.LocationX, pelihahmo.LocationY,
                pelihahmo.ActualHeight, pelihahmo.ActualWidth);
            Rect PRect3 = new Rect(
                pahis3.LocationX, pahis3.LocationY,
                pahis3.ActualWidth, pahis3.ActualHeight);
            HRect.Intersect(PRect3);
            if (!HRect.IsEmpty)
            {
                this.Frame.Navigate(typeof(GameOver));
            }
        }

        //MAALI COLLISION
        private void MaaliCollide()
        {
            Rect HRect = new Rect(
                pelihahmo.LocationX, pelihahmo.LocationY,
                pelihahmo.ActualHeight, pelihahmo.ActualWidth);
            Rect MRect = new Rect(
                maali.LocationX, maali.LocationY,
                maali.ActualHeight, maali.ActualWidth);
            HRect.Intersect(MRect);
            if (!HRect.IsEmpty && karkit.Count == 0)
            {
                this.Frame.Navigate(typeof(Voitto));
            }
                  
        }

        private void VainSuoraa()
        {
            if(LeftPressed == true)
            {
                RightPressed = false;
                Uppressed = false;
                Downpressed = false;
            }
            if (RightPressed == true)
            {
                LeftPressed = false;
                Uppressed = false;
                Downpressed = false;
            }
            if (Uppressed == true)
            {
                RightPressed = false;
                LeftPressed = false;
                Downpressed = false;
            }
            if (Downpressed == true)
            {
                RightPressed = false;
                Uppressed = false;
                LeftPressed = false;
            }
        }

    







    //dunt touch
}
}
    

   

