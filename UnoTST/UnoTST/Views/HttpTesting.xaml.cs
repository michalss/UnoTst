using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using UnoTST.Models;
using UnoTST.ViewsModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UnoTST.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HttpTesting : Page
    {

        public HttpTesting()
        {
            this.InitializeComponent();

            var container = ((App)App.Current).Container;
            DataContext = (HttpTestingViewModel)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(HttpTestingViewModel));
        }

        private void listRepositories_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selected = e.ClickedItem as MRepository;
            if (selected != null)
            {
                var dataContext = (HttpTestingViewModel)this.DataContext;
                Log.Logger(LogType.debug, nameof(listRepositories_ItemClick), "STEP1 repo");
                dataContext.LoadPlugingRepository(selected!);
            }
        }

    }
}
