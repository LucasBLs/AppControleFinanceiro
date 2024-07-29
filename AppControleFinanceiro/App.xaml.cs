using AppControleFinanceiro.View;

namespace AppControleFinanceiro;

public partial class App : Application
{
	public App(TransactionList listPage)
    {
        Current.UserAppTheme = AppTheme.Light;
        InitializeComponent();      

        MainPage = new NavigationPage(listPage);
	}
}
