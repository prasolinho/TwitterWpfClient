using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TwitterWpfClient.Framework;

namespace TwitterWpfClient
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private Uri authorizationUri;

        public Login(Uri authorizationUri)
        {
            InitializeComponent();
            this.authorizationUri = authorizationUri;
        }

        private void btnRedirectToTwitter_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(authorizationUri.ToString());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPin.Text))
            {
                MessageBox.Show("Nie wprowadzono PINu");
                return;
            }

            TweetSharp.OAuthAccessToken accessToken = Service.Twitter.Instance.GetAccessToken(txtPin.Text);
            bool? result = accessToken.Token != "?" && accessToken.TokenSecret != "?";
            if (result.HasValue && result.Value)
            {
                AppSettings.Instance.AddSetting("token", accessToken.Token);
                AppSettings.Instance.AddSetting("tokenSecret", accessToken.TokenSecret);

            }
            DialogResult = result;
        }

    }
}
