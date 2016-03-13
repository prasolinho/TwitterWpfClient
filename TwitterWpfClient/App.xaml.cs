using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TwitterWpfClient.Framework;

namespace TwitterWpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// http://stackoverflow.com/questions/1539958/wpf-showing-dialog-before-main-window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            bool? loginResult = true;
            IAppSettings settings = AppSettings.Instance;

            if (string.IsNullOrWhiteSpace(settings.Token) || string.IsNullOrWhiteSpace(settings.TokenSecret))
            {
                Uri authorizationUri = Service.Twitter.Instance.GetAuthorizationUri();
                Login loginWindow = new Login(authorizationUri);
                loginResult = loginWindow.ShowDialog();
            }

            if (loginResult == true)
            {
                Service.Twitter.Instance.AuthetnticateWithToken(settings.Token, settings.TokenSecret);

                var mainWindow = new MainWindow();
                //Re-enable normal shutdown mode.
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Unable to load data.", "Error", MessageBoxButton.OK);
                Current.Shutdown(-1);
            }
        }
    }
}
