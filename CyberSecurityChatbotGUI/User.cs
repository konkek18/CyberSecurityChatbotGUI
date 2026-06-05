namespace CyberSecurityChatbot
{
    internal class User
    {
        public string Name { get; set; }
        // Stores user name and favourite topic for memory feature
        public string FavoriteTopic { get; set; }
        public string LastTopic { get; set; }
        public string DetectedSentiment { get; set; }
    }
}