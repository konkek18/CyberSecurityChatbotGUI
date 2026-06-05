using CyberSecurityChatbotGUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CyberSecurityChatbot
{
    public partial class MainForm : Form
    {
        private User _user;
        private Chatbot _chatbot;
        private TextBox txtInput;
        private RichTextBox rtxtChat;
        private Button btnSend;
        private Button btnExit;
        private Label lblTitle;
        private AudioPlayer _audio;

        public MainForm()
        {
            InitializeComponent();
            _audio = new AudioPlayer();
            _audio.PlayGreeting();

            // Ask for name
            string name = Microsoft.VisualBasic.Interaction.InputBox("Enter your name:", "Welcome", "");
            if (string.IsNullOrWhiteSpace(name)) name = "User";

            _user = new User { Name = name };
            _chatbot = new Chatbot(_user);

            // Display ASCII art in chat
            string asciiArt = @"
   ______             __                           ____           __ 
  / ____/   __  __   / /_   ___    _____          / __ )  ____   / /_
 / /       / / / /  / __ \ / _ \  / ___/         / __  | / __ \ / __/
/ /___    / /_/ /  / /_/ //  __/ / /            / /_/ / / /_/ // /_  
\____/    \__, /  /_.___/ \___/ /_/            /_____/  \____/ \__/  
         /____/                                                      

=== CYBERSECURITY AWARENESS BOT ===";

            rtxtChat.SelectionFont = new Font("Consolas", 9);
            rtxtChat.AppendText(asciiArt + "\n\n");
            rtxtChat.SelectionFont = new Font("Segoe UI", 10);
            rtxtChat.AppendText($"Bot: Welcome {_user.Name}! I'm your cybersecurity awareness bot. Ask me about passwords, phishing, scams, privacy, or 2FA.\n\n");
        }

        // Custom dark themes for cyber look
        private void InitializeComponent()
        {
            this.txtInput = new TextBox();
            this.rtxtChat = new RichTextBox();
            this.btnSend = new Button();
            this.btnExit = new Button();
            this.lblTitle = new Label();
            this.SuspendLayout();

            // Form settings
            this.Text = "Cybersecurity Awareness Chatbot";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 45);

            // Title label
            lblTitle.Text = "🛡️ CYBERSECURITY AWARENESS BOT 🛡️";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.ForeColor = Color.LimeGreen;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 50;
            lblTitle.BackColor = Color.FromArgb(20, 20, 35);

            // Chat display
            rtxtChat.Location = new Point(12, 60);
            rtxtChat.Size = new Size(660, 380);
            rtxtChat.BackColor = Color.FromArgb(40, 40, 55);
            rtxtChat.ForeColor = Color.White;
            rtxtChat.Font = new Font("Segoe UI", 10);
            rtxtChat.ReadOnly = true;
            rtxtChat.BorderStyle = BorderStyle.None;

            // Input textbox
            txtInput.Location = new Point(12, 450);
            txtInput.Size = new Size(540, 30);
            txtInput.BackColor = Color.FromArgb(50, 50, 65);
            txtInput.ForeColor = Color.White;
            txtInput.Font = new Font("Segoe UI", 11);
            txtInput.BorderStyle = BorderStyle.FixedSingle;
            txtInput.KeyPress += TxtInput_KeyPress;

            // Send button
            btnSend.Text = "SEND";
            btnSend.Location = new Point(560, 448);
            btnSend.Size = new Size(112, 35);
            btnSend.BackColor = Color.LimeGreen;
            btnSend.ForeColor = Color.Black;
            btnSend.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Click += BtnSend_Click;

            // Exit button
            btnExit.Text = "EXIT";
            btnExit.Location = new Point(560, 490);
            btnExit.Size = new Size(112, 35);
            btnExit.BackColor = Color.Crimson;
            btnExit.ForeColor = Color.White;
            btnExit.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Click += (s, e) => Application.Exit();

            // Add controls
            this.Controls.AddRange(new Control[] { lblTitle, rtxtChat, txtInput, btnSend, btnExit });

            this.ResumeLayout();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void TxtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendMessage();
                e.Handled = true;
            }
        }

        private void SendMessage()
        {
            string userInput = txtInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(userInput)) return;

            // Display user message
            rtxtChat.SelectionColor = Color.LightSkyBlue;
            rtxtChat.AppendText($"\nYou: {userInput}\n");

            // Get bot response
            string response = _chatbot.GetResponse(userInput);

            // Display bot response
            rtxtChat.SelectionColor = Color.LimeGreen;
            rtxtChat.AppendText($"Bot: {response}\n");

            // Scroll to bottom
            rtxtChat.ScrollToCaret();

            // Clear input
            txtInput.Clear();
            txtInput.Focus();

            // Check for exit
            if (userInput.ToLower() == "exit")
            {
                rtxtChat.AppendText("Bot: Goodbye! Stay safe online.\n");
                btnSend.Enabled = false;
            }
        }
    }
}