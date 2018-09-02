using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace SensoComum.Mobile.Forms
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			MainPage = new MainPage();
		}

		protected override void OnStart ()
		{
            // Handle when your app starts

            AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start("ios=4965a711-59b5-4b5b-9171-fa424cfe944c;" + "android=06c2f1ed-8ad4-4595-9830-9e4ea002cdd3", typeof(Analytics), typeof(Crashes), typeof(Push));

        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
