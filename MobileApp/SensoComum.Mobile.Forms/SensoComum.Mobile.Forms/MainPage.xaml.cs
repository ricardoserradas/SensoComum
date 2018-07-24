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
		public MainPage()
		{
			InitializeComponent();
		}

        private void OnSubjectView(object sender, EventArgs e)
        {
            int viewCount = 0;

            int.TryParse(subjectViewCount.Text, out viewCount);

            viewCount++;

            subjectViewCount.Text = viewCount.ToString();
        }
    }
}
