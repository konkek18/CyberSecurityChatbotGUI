using System;
using System.Media;
using System.IO;

namespace CyberSecurityChatbot
{
    internal class AudioPlayer
    {
        public void PlayGreeting()
        {
          // Load and play the greeting WAV file from project folder 
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Greeting.wav");

                Console.WriteLine("Looking for audio at: " + path);

                if (File.Exists(path))
                {
                    SoundPlayer player = new SoundPlayer(path);
                    player.Load();
                    player.PlaySync();
                }
                else
                {
                    Console.WriteLine("Audio file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Audio error: " + ex.Message);
            }
        }
    }
}