using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ping_Pong
{
    public partial class Pong : Form
    {
        // Location Variables
        int cpuDirection = 5;
        int ballXCoordinate = 5;
        int ballYCoordinate = 5;
        // Score Variables
        int playerScore = 0;
        int cpuScore = 0;
        // Size Variables
        int bottomBoundary;
        int centerPoint;
        int xMidpoint;
        int yMidpoint;
        // Detection Variables
        bool playerDetectedUp;
        bool playerDetectedDown;
        // Special Keys
        int spaceBarClicked = 0;

        public Pong()
        {
            InitializeComponent();
            bottomBoundary = ClientSize.Height - player1.Height;
            xMidpoint = ClientSize.Width / 2;
            yMidpoint = ClientSize.Height / 2;
        }



        private void Pong_Load(object sender, EventArgs e)
        {

        }

        private void pongTimer_Tick(object sender, EventArgs e)
        {
            Random newBallSpot = new Random();
            int newSpot = newBallSpot.Next(100, ClientSize.Height - 100);
            // Adjust where the ball is
            pongBall.Top -= ballYCoordinate;
            pongBall.Left -= ballXCoordinate;
            // Make the CPU move
            cpuPlayer.Top += cpuDirection;

            // Make CPU better at the game, the higher the playerScore
            if(playerScore > 5)
            {
                cpuPlayer.Top = pongBall.Top + 30;
            }


            // Check if CPU has reached the top or the bottom
            if(cpuPlayer.Top < 0 || cpuPlayer.Top > bottomBoundary) { cpuDirection = -cpuDirection; }
            // Check if the ball has exited the left side of the screen
            if (pongBall.Left < 0)
            {
                pongBall.Left = xMidpoint;
                pongBall.Top = newSpot;
                ballXCoordinate = -ballXCoordinate;
                if(playerScore < 5) { ballXCoordinate -= 1; }
                cpuScore++;
                cpuScoreLabel.Text = cpuScore.ToString();
            }

            // Check if the ball has exited the right side of the screen
            if (pongBall.Left + pongBall.Width > ClientSize.Width)
            {
                pongBall.Left = xMidpoint;
                pongBall.Top = newSpot;
                ballXCoordinate = -ballXCoordinate;
                if(playerScore < 5) { ballXCoordinate += 1; }
                playerScore++;
                playerScoreLabel.Text = playerScore.ToString();
            }

            // Ensure the ball is within the boundaries of the screen
            if (pongBall.Top < 0 || pongBall.Top + pongBall.Height > ClientSize.Height) { ballYCoordinate = -ballYCoordinate; }

            // Check if the ball hits the player or CPU paddle
            if (pongBall.Bounds.IntersectsWith(player1.Bounds) || pongBall.Bounds.IntersectsWith(cpuPlayer.Bounds))
            {
                // Generate a random color for player
                Random rand = new Random();
                int max = byte.MaxValue + 1;
                int r = rand.Next(max);
                int g = rand.Next(max);
                int b = rand.Next(max);
                Color c = Color.FromArgb(r, g, b);

                // Generate a random color for CPU
                int r1 = rand.Next(max);
                int g1 = rand.Next(max);
                int b1 = rand.Next(max);
                Color c2 = Color.FromArgb(r1, g1, b1);

                // Change the color of the paddle
                player1.BackColor = c;
                cpuPlayer.BackColor = c2;

                // Send ball opposite direction
                ballXCoordinate = -ballXCoordinate; 
            }

            // Move player up
            if (playerDetectedUp == true && player1.Top > 0) { player1.Top -= 10; }
            // Move player down
            if (playerDetectedDown == true && player1.Top < bottomBoundary) { player1.Top += 10; }
            // Check for winner
            if (playerScore >= 10) { pongTimer.Stop(); }
        }

        private void Pong_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) { playerDetectedUp = false; }
            if (e.KeyCode == Keys.Down) { playerDetectedDown = false; }
        }

        private void Pong_KeyDown(object sender, KeyEventArgs e)
        {
            // If player presses the up arrow, move paddle upwards
            if(e.KeyCode == Keys.Up) { playerDetectedUp = true; }
            // If player presses the down arrow, move paddle downwards
            if(e.KeyCode == Keys.Down) { playerDetectedDown = true; }
            // If player presses the C key, open the character selection screen
            if(e.KeyCode == Keys.C)
            {
                Form character = new CharacterForm();
                character.Owner = this;
                pongTimer.Stop();
                character.Show();
            }

            // If player presses space bar, pause the game
            if(e.KeyCode == Keys.Space)
            {
                if(spaceBarClicked % 2 == 0)
                {
                    pongTimer.Stop();
                }
                else
                {
                    pongTimer.Start();
                }
            }
            spaceBarClicked++;
        }
    }
}
