using JogoGourmet.GameLogic;

namespace JogoGourmet
{
    public partial class MainForm : Form
    {
        private GameEngine gameEngine;
        private Button startButton;
        private Label middleText;

        public MainForm()
        {
            InitializeComponent();
            SetupForm();
            InitializeControls();
        }

        private void SetupForm()
        {
            this.ClientSize = new Size(300, 100);
            this.Text = "Jogo Gourmet";
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeControls()
        {
            gameEngine = new GameEngine();

            middleText = new Label() { Text = "Pense em um prato que gosta", TextAlign = ContentAlignment.TopCenter, Dock = DockStyle.Fill, AutoSize = false, Padding = new Padding(0, 20, 0, 0) };

            startButton = new Button() { Text = "Iniciar Jogo", Dock = DockStyle.None, Location = new Point((this.ClientSize.Width - 100) / 2, this.ClientSize.Height - 50) };
            startButton.Click += (sender, e) => gameEngine.StartGame();

            this.Controls.Add(startButton);
            this.Controls.Add(middleText);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Fechando o jogo");
        }
    }
}

