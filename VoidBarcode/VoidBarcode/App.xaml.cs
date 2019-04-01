using VoidBarcode.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace VoidBarcode
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //여러 페이지 할경우 NavigationPage으로.
            MainPage = new MainView();
        }

        protected override void OnStart()
        {
             AppCenter.Start("android=525c2b00-d035-4983-8855-70319a2cd2b5;" + "uwp={Your UWP App secret here};" + "ios={Your iOS App secret here}", typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
