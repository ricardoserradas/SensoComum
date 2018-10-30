using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;

namespace SensoComum.Mobile.Forms
{
	public partial class MainPage : ContentPage
	{
		ApiServiceManager serviceManager;

		public MainPage()
		{
			InitializeComponent();

            // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/platform/device

            this.serviceManager = new ApiServiceManager();
            sumSubjectView.GestureRecognizers.Add(new TapGestureRecognizer {
                Command = new Command(() => { OnSubjectView(); }),
                NumberOfTapsRequired = 1
            });

            refreshSum.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => { OnRefreshSum(); }),
                NumberOfTapsRequired = 1
            });

            try
            {
                RefreshSum();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error while loading the app.", $"Error: {ex.Message}", "OK");
            }
		}

		private async void OnSubjectView()
		{
			int viewCount = 0;

			try
			{
				using (var loading = UserDialogs.Instance.Loading("Aguarde..."))
				{
					await this.serviceManager.SumToService(); 
				}

				int.TryParse(subjectViewCount.Text, out viewCount);

				viewCount++;

				subjectViewCount.Text = viewCount.ToString();
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erro na soma", $"Erro ao somar no serviço: {ex.Message}", "OK");
                Crashes.TrackError(ex);
			}
		}

		private async void OnRefreshSum()
		{
			try
			{
				await RefreshSum();
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erro na atualização", $"Erro ao obter atualização de soma: {ex.Message}", "OK");
			}
		}

		private async Task RefreshSum()
		{
			using (var loading = UserDialogs.Instance.Loading("Aguarde..."))
			{
                try
                {
                    var serviceSumString = await this.serviceManager.RefreshDataAsync();

                    int currentSum;
                    int serviceSum;

                    int.TryParse(serviceSumString, out serviceSum);
                    int.TryParse(subjectViewCount.Text, out currentSum);

                    if (serviceSum > currentSum)
                    {
                        subjectViewCount.Text = serviceSumString;
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    throw;
                }
			}
		}
	}
}
