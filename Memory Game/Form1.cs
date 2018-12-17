using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory_Game
{
    public partial class Form1 : Form
    {
        #region Global parameters
        bool allowClick = false;
        PictureBox firstGuess;
        Random random = new Random();
        Timer clickTimer = new Timer();
        int time = 60;
        Timer timer = new Timer { Interval = 1000 };
        #endregion

        public Form1()
        {
            InitializeComponent();

        }


        /// <summary>
        /// Used as an array for the used pictures in the game
        /// </summary>
        private PictureBox[] PictureBoxes
        {
            get { return Controls.OfType<PictureBox>().ToArray(); }
        }

        /// <summary>
        /// Uploads the images from the resource to the PictureBox array
        /// </summary>
        private static IEnumerable<Image> Images
        {
            get
            {
                return new Image[]
                {
                    Properties.Resources.img1,
                    Properties.Resources.img2,
                    Properties.Resources.img3,
                    Properties.Resources.img4,
                    Properties.Resources.img5,
                    Properties.Resources.img6,
                    Properties.Resources.img7,
                    Properties.Resources.img8
                };
            }
        } 

        /// <summary>
        /// Starts the game timer, and check if the game is still going or if it is lost
        /// </summary>
        private void StartGameTime()
        {
            timer.Start();
            timer.Tick += delegate
            {
                time--;
                if (time < 0)
                {
                    timer.Stop();
                    MessageBox.Show("Out of time");
                    ResetImages();
                    StopGame();
                }
                lTime.Text = "00: " + time.ToString();
            };
        }

        /// <summary>
        /// Resets the images and the game time to default
        /// </summary>
        private void ResetImages()
        {
            foreach (var pic in PictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;
            }

            HideImages();
            SetRandomImages();
            time = 60;
        }

        /// <summary>
        /// Uses the question mark image to hide the images
        /// </summary>
        private void HideImages()
        {
            foreach (var pic in PictureBoxes)
            {
                pic.Image = Properties.Resources.question;

            }
        }

        /// <summary>
        /// Uses the random funtion to pick an empty picturebox location
        /// </summary>
        /// <returns></returns>
        private PictureBox GetFreeSlot()
        {
            int num;

            do
            {
                num = random.Next(0, PictureBoxes.Count());

            } while (PictureBoxes[num].Tag != null);
            return PictureBoxes[num];
        }

        /// <summary>
        /// Sets 2 identical images to 2 free slots
        /// </summary>
        private void SetRandomImages()
        {
            foreach (var image in Images)
            {
                GetFreeSlot().Tag = image;
                GetFreeSlot().Tag = image;
            }
        }

        /// <summary>
        /// Shows the images if the guess was incorect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickTimer_tick(object sender, EventArgs e)
        {
            HideImages();
            allowClick = true;
            clickTimer.Stop();

        }

        /// <summary>
        /// Checks if the clicked images math, and if the game is won
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickImage(object sender, EventArgs e)
        {
            if (!allowClick) return;

            var pic = (PictureBox)sender;

            if (firstGuess == null)
            {
                firstGuess = pic;
                pic.Image = (Image)pic.Tag;
                return;
            }

            pic.Image = (Image)pic.Tag;

            if (pic.Image == firstGuess.Image && pic != firstGuess)
            {
                pic.Visible = firstGuess.Visible = false;
                {
                    firstGuess = pic;
                }
                HideImages();
            }
            else
            {
                allowClick = false;
                clickTimer.Start();
            }

            firstGuess = null;
            if (PictureBoxes.Any(p => p.Visible)) return;
            timer.Stop();
            MessageBox.Show("You win");
            ResetImages();
            StopGame();
           

        }

        /// <summary>
        /// Stops the game, clears the timer, and clears the pictures
        /// </summary>
        private void StopGame()
        {
            allowClick = false;
            cbDificulty.Enabled = true;
            timer.Stop();
            bStart.Enabled = true;
            timer = null;
            timer = new Timer { Interval = 1000 };

            foreach (var pic in PictureBoxes)
            {
                pic.Image = null;
                pic.Tag = null;
            }
        }

        /// <summary>
        /// Starts the game depending on the selected dificulty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGame(object sender, EventArgs e)
        {
            switch (cbDificulty.Text)
            {
                case "Easy":
                    time = 60;
                    break;
                case "Medium":
                    time = 30;
                    break;
                case "Hard":
                    time = 20;
                    break;
                case "Hardcore":
                    time = 15;
                    break;
                default:
                    break;
            } // depending on the selected dificulty sets the timer
            allowClick = true;
            cbDificulty.Enabled = false;
            SetRandomImages();
            HideImages();
            StartGameTime();
            clickTimer.Interval = 750;
            clickTimer.Tick += ClickTimer_tick;
            bStart.Enabled = false;
        }


    }
}
