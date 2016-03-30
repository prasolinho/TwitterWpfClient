using System;
using System.Diagnostics;
using System.Windows;
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
