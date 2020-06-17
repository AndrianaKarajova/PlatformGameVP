using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace PlatformGame
{
    public partial class Form1 : Form
    {
        private SoundPlayer _soundPlayer;

        System.Media.SoundPlayer sound = new System.Media.SoundPlayer(PlatformGame.Properties.Resources.PlatformGameMusic);
        bool goLeft, goRight, jumping, isGameOver;
        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;

        int horizontalSpeed = 5;
        int verticalSpeed = 3;
        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;
        int coinCount = 0;

        public HighScore h;
        string player_name;
        int ind = 0;

        public SoundPlayer SoundPlayer { get => _soundPlayer; set => _soundPlayer = value; }

        public Form1()
        {
            InitializeComponent();
            sound.PlayLooping();
            
            timer1.Start();
            timer1.Interval = 1000;

            h = new HighScore();
            player_name = "";

        }
        

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: ";
            label12.Text = "" + score;
            player.Top += jumpSpeed;
           

            if (goLeft == true)
            {
                player.Left -= playerSpeed;

            }
            if (goRight == true)
            {
                player.Left += playerSpeed;
            }
            if (jumping == true && force < 0)
            {
                jumping = false;

            }
            if (jumping == true)
            {
                jumpSpeed = -8;
                force -= 2;
            }
            else
            {
                jumpSpeed = 10;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 8;
                            player.Top = x.Top - player.Height;

                            if ((string)x.Name == "horizontalPlatform" && goLeft == false || (string)x.Name == "horizontalPlatform" && goRight == false)
                            {
                                player.Left -= horizontalSpeed;
                            }
                        }
                        x.BringToFront();
                    }

                    if ((string)x.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            coinCount += 1;
                            if (easyToolStripMenuItem.Checked == true)
                            {
                                score += 1;
                            }
                            else if (mediumToolStripMenuItem.Checked == true)
                            {
                                score += 2;
                            }
                            else
                            {
                                score += 3;
                            }
                        }
                    }

                    if ((string)x.Tag == "enemy")
                    {
                        //dokolku igracot go dopre enemy objektot, igrata zavrsuva i se pojavuva poraka
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            gameTimer.Stop();
                            isGameOver = true;
                            End_Text.Visible = true;
                            PlayAgain_button.Visible = true;
                            txtScore.Text = "Score: " + Environment.NewLine + "Game over!";
                            label12.Text = "" + score;
                        }
                    }
                }
            }

            horizontalPlatform.Left -= horizontalSpeed;

            //dinamicko horizontalno dvizenje na platformata do krajot na goleminata na prozorecot 
            if (horizontalPlatform.Left < 0 || horizontalPlatform.Left + horizontalPlatform.Width > this.ClientSize.Width)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            verticalPlatform.Top += verticalSpeed;

            //vertikalno dvizenje na plaformata od edno do drugo rastojanie
            if (verticalPlatform.Top < 110 || verticalPlatform.Top > 375)
            {
                verticalSpeed = -verticalSpeed;
            }

            enemyOne.Left -= enemyOneSpeed;

            //dvizenje na prviot enemy od edniot do drugiot kraj na platformata
            if (enemyOne.Left < pictureBox5.Left || enemyOne.Left + enemyOne.Width > pictureBox5.Left + pictureBox5.Width)
            {
                enemyOneSpeed = -enemyOneSpeed;
            }

            enemyTwo.Left -= enemyTwoSpeed;

            //dvizenje na vtoriot enemy od edniot do drugiot kraj na platformata
            if (enemyTwo.Left < pictureBox2.Left || enemyTwo.Left + enemyTwo.Width > pictureBox2.Left + pictureBox2.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
            }

            //dokolku igracot padne nadvor od prozorecot igrata zavrsuva i se ispisuva poraka
            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                timer1.Stop();
                progressBar1.Value = 0;
                gameTimer.Stop();
                isGameOver = true;
                End_Text.Visible = true;
                PlayAgain_button.Visible = true;
                txtScore.Text = "Score: " + Environment.NewLine + "Game over!";
                label12.Text = "" + score;
            }

            //dokolku igracot stigne do vratata i gi ima sobrano site coins, igrata zavrsuva
            if (player.Bounds.IntersectsWith(door.Bounds) && coinCount == 20)
            {
                timer1.Stop();
                progressBar1.Value = 0;
                gameTimer.Stop();
                isGameOver = true;
                PlayAgain_button.Visible = true;
                txtScore.Text = "Score: " + Environment.NewLine + "Congratulations! You won!";
                label12.Text = "" + score;
            }
            //vo sprotivno stoi porakata da gi sobere i preostanatite coins
            else
            {
                txtScore.Text = "Score: "  + Environment.NewLine + "Collect all the coins!";
                label12.Text = ""+score;
            }
        }

        private void PlayAgain_button_Click(object sender, EventArgs e)
        {
            RestartGame();
            PlayAgain_button.Visible = false;
            PlayAgain_button.Enabled = false;

            if (PlayAgain_button.Enabled == false)
            {
                PlayAgain_button.Enabled = true;
            }
            
        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediumToolStripMenuItem.Checked = false;
            hardToolStripMenuItem.Checked = false;
            horizontalSpeed = 3;
            verticalSpeed = 2;
            enemyOneSpeed = 3;
            enemyTwoSpeed = 2;
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {

            easyToolStripMenuItem.Checked = false;
            hardToolStripMenuItem.Checked = false;
            horizontalSpeed = 5;
            verticalSpeed = 3;
            enemyOneSpeed = 5;
            enemyTwoSpeed = 3;
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            easyToolStripMenuItem.Checked = false;
            mediumToolStripMenuItem.Checked = false;
            horizontalSpeed = 7;
            verticalSpeed = 5;
            enemyOneSpeed = 7;
            enemyTwoSpeed = 5;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
           // progressBar1.Value = 100;

            if (progressBar1.Value <= progressBar1.Maximum)
            {
                if (isGameOver != true)
                { 
                        progressBar1.Value = progressBar1.Value - 4;
                }

            }
            if (progressBar1.Value == progressBar1.Minimum)
            {
                timer1.Stop();
                isGameOver = true;
                End_Text.Visible = true;
                PlayAgain_button.Visible = true;
                txtScore.Text = "Score: "  + Environment.NewLine + "Game over!";
                label12.Text = "" + score;
            }
        }

        private void addHighScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AddHighScore forma = new AddHighScore();
            forma.ShowDialog();
            player_name = forma.name;
            if (player_name == null)
                player_name = "Anonymous";
            int x = 0;
            int.TryParse(label12.Text, out x);
            if (ind == 10)
            {
                if (h.scores[9] < x)
                {
                    h.scores[9] = x;
                    h.names[9] = player_name;
                }
            }
            else
            {
                h.names[ind] = player_name;
                h.scores[ind++] = x;
            }
            h.sort();
            HighScores ff = new HighScores(this);

            MessageBox.Show("Good job, " + player_name + "\n Your score is: " + score);
            ff.ShowDialog();
        }



        // private void cbMusic_CheckedChanged(object sender, EventArgs e)
        // {
        //     if (cbMusic.Checked)
        //     {


        //         cbMusic.Text = "Stop Music";
        //         SoundPlayer.Play();
        //         SoundPlayer.PlayLooping();
        //     }
        //     else
        //    {
        //        cbMusic.Text = "Play Music";
        //        SoundPlayer.Stop();
        //    }
        //}

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                jumping = true;
            }
            //if(e.KeyCode == Keys.Space && jumping == true)
            // {
            //    jumping = true;
            //}
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }

        }

        private void RestartGame()
        {
            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            End_Text.Visible = false;
            easyToolStripMenuItem.Checked = false;
            mediumToolStripMenuItem.Checked = true;
            hardToolStripMenuItem.Checked = false;

            score = 0;
            coinCount = 0;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 100;
            timer1.Start();

            txtScore.Text = "Score: " ;
            label12.Text = "" + score;

            // treba parickite da se pojavat povtorno dokolku korisnikot prethodno gi ima zemeno
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }

            //treba da se namesti pozicijata na player-ot,enemies i na platforms na pocetok
            player.Left = 8;
            player.Top = 439;

            enemyOne.Left = 339;
            enemyTwo.Left = 235;

            horizontalPlatform.Left = 166;
            verticalPlatform.Top = 337;

            gameTimer.Start();

        }

    }
}
