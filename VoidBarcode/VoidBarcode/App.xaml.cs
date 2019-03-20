using VoidBarcode.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        protected async override void OnStart()
        {
            // Handle when your app starts

            //var status = PermissionStatus.Unknown;
            //status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

            //if (status != PermissionStatus.Granted)
            //{
            //    await Utils.CheckPermissions(Permission.Camera);
            //}

            //status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            //if (status != PermissionStatus.Granted)
            //{
            //    status = await Utils.CheckPermissions(Permission.Storage);
            //}
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
