namespace TwitterWpfClient.Models
{
    /// <summary>
    /// Powiadomienie np. @devtalk, @tomaszprasolek
    /// </summary>
    public class Mention
    {
        public int Order { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string ScreenName { get; set; }
    }
}