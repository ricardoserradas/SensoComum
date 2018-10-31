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

            this.serviceManager = new ApiServiceManager();

            RefreshSum();

		}

		private async void OnSubjectView()
		{
			int viewCount = 0;

			try
            {
                this.serviceManager.SumToService();

                int.TryParse(subjectViewCount.Text, out viewCount);

                viewCount++;

                subjectViewCount.Text = viewCount.ToString();
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro na soma", $"Erro ao somar no serviço: {ex.Message}", "OK");
            }
        }

        private async void OnRefreshSum(object sender, EventArgs e)
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
            var currentSum = await this.serviceManager.RefreshDataAsync();

            subjectViewCount.Text = currentSum;
        }
    }
}
