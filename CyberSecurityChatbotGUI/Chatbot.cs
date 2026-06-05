using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    internal class Chatbot
    {
        private User _user;
        private Dictionary<string, List<string>> _responses;
        private Dictionary<string, string[]> _sentimentResponses;
        private Random _random;

        public Chatbot(User user)
        {
            _user = user;
            _random = new Random();
            InitializeResponses();
            InitializeSentimentResponses();
        }

        private void InitializeResponses()
        {
            _responses = new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
                {
                    "Use strong passwords with 12+ characters, numbers, and symbols.",
                    "Never reuse passwords across different accounts.",
                    "Enable two-factor authentication (2FA) for extra security.",
                    "Use a password manager to generate and store unique passwords."
                },
                ["phishing"] = new List<string>
                {
                    "Don't click suspicious links in emails or messages.",
                    "Check the sender's email address carefully – scammers fake legitimate ones.",
                    "Never share personal info via email or text.",
                    "Hover over links to see the real URL before clicking."
                },
                ["scam"] = new List<string>
                {
                    "If something sounds too good to be true, it probably is.",
                    "Never send money or gift cards to someone you met online.",
                    "Block and report suspicious callers or messages.",
                    "Verify unexpected offers by contacting the company directly."
                },
                ["privacy"] = new List<string>
                {
                    "Review your social media privacy settings regularly.",
                    "Don't overshare personal info like your address or birthday.",
                    "Use a VPN on public Wi-Fi to protect your data.",
                    "Check which apps have access to your personal information."
                },
                ["2fa"] = new List<string>
                {
                    "2FA adds a second layer of security to your accounts.",
                    "Use an authenticator app instead of SMS when possible.",
                    "Enable 2FA on your email, banking, and social media accounts."
                }
            };
        }

        private void InitializeSentimentResponses()
        {
            _sentimentResponses = new Dictionary<string, string[]>
            {
                ["worried"] = new string[]
                {
                    "It's completely understandable to feel worried. Let me give you a practical tip to help.",
                    "Cybersecurity can feel overwhelming, but small steps make a big difference. Here's something useful:"
                },
                ["curious"] = new string[]
                {
                    "Great question! It's smart to be curious about staying safe online. Here's what you should know:",
                    "I love that you're asking questions! Here's an important cybersecurity insight:"
                },
                ["frustrated"] = new string[]
                {
                    "I hear you – cybersecurity can be frustrating. Let me simplify things for you:",
                    "Don't worry, you've got this. Here's a straightforward tip that might help:"
                }
            };
        }

        public string GetResponse(string input)
        {
            input = input.ToLower();

            // Sentiment detection first
            string sentiment = DetectSentiment(input);
            _user.DetectedSentiment = sentiment;

            // Handle conversation flow ("tell me more", "another tip")
            if (input.Contains("tell me more") || input.Contains("another tip") || input.Contains("explain more"))
            {
                if (!string.IsNullOrEmpty(_user.LastTopic))
                {
                    return GetRandomResponseForTopic(_user.LastTopic);
                }
                return "Sure! What topic would you like to learn more about? (password, phishing, scam, privacy, or 2fa)";
            }

            // Keyword matching
            if (input.Contains("password"))
            {
                _user.LastTopic = "password";
                _user.FavoriteTopic = _user.FavoriteTopic ?? "password";
                return GetSentimentWrappedResponse("password");
            }
            if (input.Contains("phish"))
            {
                _user.LastTopic = "phishing";
                _user.FavoriteTopic = _user.FavoriteTopic ?? "phishing";
                return GetSentimentWrappedResponse("phishing");
            }
            if (input.Contains("scam"))
            {
                _user.LastTopic = "scam";
                _user.FavoriteTopic = _user.FavoriteTopic ?? "scam";
                return GetSentimentWrappedResponse("scam");
            }
            if (input.Contains("privacy"))
            {
                _user.LastTopic = "privacy";
                _user.FavoriteTopic = _user.FavoriteTopic ?? "privacy";
                return GetSentimentWrappedResponse("privacy");
            }
            if (input.Contains("2fa") || input.Contains("two factor") || input.Contains("two-factor"))
            {
                _user.LastTopic = "2fa";
                return GetSentimentWrappedResponse("2fa");
            }

            // Greeting / small talk
            if (input.Contains("how are you"))
                return "I'm here to help you stay safe online! How can I assist with cybersecurity today?";

            if (input.Contains("hello") || input.Contains("hi") || input.Contains("hey"))
                return $"Hello {_user.Name}! I'm your cybersecurity awareness bot. Ask me about passwords, phishing, scams, privacy, or 2FA.";

            if (input.Contains("purpose") || input.Contains("what do you do"))
                return "My purpose is to help you stay safe online by sharing cybersecurity tips and advice.";

            if (input.Contains("thank"))
                return $"You're welcome, {_user.Name}! Stay safe online. Any other cybersecurity questions?";

            // Memory recall – refer to favorite topic
            if (!string.IsNullOrEmpty(_user.FavoriteTopic))
            {
                return $"Since you're interested in {_user.FavoriteTopic}, here's a tip: {GetRandomResponseForTopic(_user.FavoriteTopic)}";
            }

            // Default/error handling
            return "I'm not sure I understand. Can you try rephrasing? You can ask about passwords, phishing, scams, privacy, or 2FA.";
        }

        private string DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("nervous") || input.Contains("anxious"))
                return "worried";
            if (input.Contains("curious") || input.Contains("interesting") || input.Contains("tell me") || input.Contains("learn"))
                return "curious";
            if (input.Contains("frustrated") || input.Contains("confused") || input.Contains("hard") || input.Contains("difficult"))
                return "frustrated";
            return "neutral";
        }

        private string GetSentimentWrappedResponse(string topic)
        {
            string sentiment = _user.DetectedSentiment;
            string tip = GetRandomResponseForTopic(topic);

            if (_sentimentResponses.ContainsKey(sentiment))
            {
                string[] sentimentMessages = _sentimentResponses[sentiment];
                string intro = sentimentMessages[_random.Next(sentimentMessages.Length)];
                return intro + " " + tip;
            }

            return tip;
        }

        private string GetRandomResponseForTopic(string topic)
        {
            if (_responses.ContainsKey(topic))
            {
                List<string> responses = _responses[topic];
                return responses[_random.Next(responses.Count)];
            }
            return "Let me share a general cybersecurity tip: always keep your software updated!";
        }
    }
}