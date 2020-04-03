using ORKK.Data.Vaults;
using System.Windows;

namespace ORKK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DataVault.FillVaults();
        }
    }
}
