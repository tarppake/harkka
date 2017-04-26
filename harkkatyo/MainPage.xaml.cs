using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace harkkatyo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //musiikki
        public MediaElement mediaElement;

        public MainPage()
        {
            this.InitializeComponent();
            //load audio
            LoadAudio();
         
        }

        //musiikki
        private async void LoadAudio()
        {
            StorageFolder folder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file =
                await folder.GetFileAsync("Rainbows.mp3");
            var stream = await file.OpenAsync(FileAccessMode.Read);

            mediaElement = new MediaElement();
           // mediaElement.AutoPlay = false;
            mediaElement.SetSource(stream, file.ContentType);
           //play media element
            mediaElement.Play();

        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage1));
        }

        private void CreditsButton_Click(object sender, RoutedEventArgs e)
        {
            //siirry tekijat sivulle
            this.Frame.Navigate(typeof(CreditsPage));
        }
    }
}
