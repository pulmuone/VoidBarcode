using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VoidBarcode.Services.AutoUpdate;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VoidBarcode.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainView : ContentPage
	{
		public MainView ()
		{
			InitializeComponent ();

            var assembly = typeof(App).GetTypeInfo().Assembly;
            var assemblyName = new AssemblyName(assembly.FullName);

            this.lblVersion.Text = "Ver " + assemblyName.Version.ToString();

        }

        //async void는 await안됨.
        //MVVM할때 코드 변경할 예정
        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (await VersionCheck.Instance.IsUpdate())
            {
                await VersionCheck.Instance.UpdateCheck();
                IsEnabled = true;
                return;
            }
        }
    }
}