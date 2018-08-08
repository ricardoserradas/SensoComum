using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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

		private async void OnSubjectView(object sender, EventArgs e)
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
			using (var loading = UserDialogs.Instance.Loading("Aguarde..."))
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
		}
	}
}
