namespace TwitterWpfClient.Framework
{
    public interface IAppSettings
    {
        string ConsumerKey
        {
            get;
        }

        string ConsumerSecret
        {
            get;
        }

        string Token
        {
            get;
        }

        string TokenSecret
        {
            get;
        }

        bool AddSetting(string key, string value);
    }
}