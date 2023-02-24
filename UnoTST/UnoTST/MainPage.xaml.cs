using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnoTST.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UnoTST
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void vnHead_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {

        }

        private void vnHead_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (sender is NavigationView)
            {
                var nvHeader = sender as NavigationView;
                var SelectedItem = nvHeader.SelectedItem as NavigationViewItem;

                if (SelectedItem?.Tag?.ToString() == "media")
                {
                    Frame.Navigate(typeof(HttpTesting), null, new DrillInNavigationTransitionInfo());
                }
                //else if (SelectedItem?.Tag?.ToString() == "home")
                //{
                //    mainFrame.Navigate(typeof(Home), null, new DrillInNavigationTransitionInfo());
                //}
                //else if (SelectedItem?.Tag?.ToString() == "media")
                //{
                //    mainFrame.Navigate(typeof(Media), null, new DrillInNavigationTransitionInfo());
                //}
                //else if (SelectedItem?.Tag?.ToString() == "plugins")
                //{
                //    mainFrame.Navigate(typeof(Plugins), null, new DrillInNavigationTransitionInfo());
                //}
            }
        }
    }
}
