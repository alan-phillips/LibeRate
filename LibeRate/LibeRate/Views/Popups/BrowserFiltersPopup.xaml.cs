using LibeRate.ViewModels.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibeRate.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BrowserFiltersPopup : Popup
	{
		public BrowserFiltersPopup (Dictionary<string, object> CurrentFilterSettings)
		{
			InitializeComponent ();
            this.BindingContext = new BrowserFiltersPopupViewModel(CurrentFilterSettings);
        }

		private void Button_OnClicked(object sender, EventArgs e)
		{
			Dismiss(((BrowserFiltersPopupViewModel)BindingContext).Results);
		}
	}
}