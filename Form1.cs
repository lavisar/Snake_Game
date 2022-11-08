using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();
            //Set settings to default
            new Settings();
            //Set game speed and start timer
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();
            //Start New game
            StartGame();
        }
        private void StartGame()
        {
            lblGameOver.Visible = false;

            //Set settings to default
            new Settings();
            //Create new player object
            Snake.Clear();
            Circle head = new Circle {X = 10, Y = 5};
            Snake.Add(head);
            //start themeSong
            playSimpleSound();
            lblScore.Text = Settings.Score.ToString();
            lblOutPut.Text = "Kỷ lục: " + File.ReadAllText("HighScore.txt");
            GenerateFood();
            //HighScore();
        }
        //Place random food object
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle {X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos)};
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            //Check for Game Over
            if (Settings.GameOver)
            {
                //Check if Enter is pressed
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();

        }
        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                //Draw snake
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black;     //Draw head
                    else
                        snakeColour = Brushes.Green;    //Rest of body
                    //Draw snake
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));
                    //Draw Food
                    canvas.FillEllipse(Brushes.Tomato,
                        new Rectangle(food.X * Settings.Width,
                             food.Y * Settings.Height, Settings.Width, Settings.Height));
                }
            }
            else
            {
                string gameOver = "Bạn đã thua! \nSố điểm đạt được: " + Settings.Score + "\nBấm'Enter'để chơi lại";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }
        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //Move head
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }
                    //Get maximum X and Y Pos
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;
                    //Detect collission with game borders.
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        playLoseSound();
                        Die();
                    }
                    //When snake eat her body => dead
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                           Snake[i].Y == Snake[j].Y)
                        {
                            playLoseSound();
                            Die();
                        }
                    }
                    //Detect collision with food piece
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    //Move body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Eat()
        {
            //Add circle to body
            Circle circle = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            //Update Score
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }
       
        private void Die()
        {
            // when player Die
            //playLoseSound();

            //dont touch it!
            if (Settings.Score > Settings.highScore)
            {
                HighScore();
            }
            
            Settings.GameOver = true;  
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblGameOver_Click(object sender, EventArgs e)
        {

        }

        /*
         * Music player  -- updated  13:00  30.10.22
         * Remember to using System.Media;
         * file music must be .wav and stored in 'resources' of the application folder
         * Added song to resources by following: https://stackoverflow.com/questions/4125698/how-to-play-wav-audio-file-from-resources
         */

        /// <summary>
        /// Because mySoundFile is a Stream, 
        /// you can take advantage of SoundPlayer's overloaded constructor, which accepts a Stream object
        /// System.IO.Stream str = Properties.Resources.mySoundFile;
        /// System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
        /// snd.Play();
        /// </summary>

        private void playSimpleSound()
        {
            /* -- Play in local
             * Code: SoundPlayer simpleSound = new SoundPlayer(@".\winForm\lavi\Snake\media\ThemeSong.wav");
             * -- Added into resources
             * Anyone have the project can play without changing the path
             */
            System.IO.Stream str = Properties.Resources.ThemeSong;
            SoundPlayer simpleSound = new SoundPlayer(str);
            simpleSound.PlayLooping();  //PlayLooping() is play looping a sound
        }
        private void playLoseSound()
        {
            //SoundPlayer simpleSound = new SoundPlayer(@".\winForm\lavi\Snake\media\whenLose.wav"); -- code to play sound in local computer            
            System.IO.Stream str = Properties.Resources.whenLose;
            SoundPlayer simpleSound = new SoundPlayer(str);
            simpleSound.Play();
        }

        ///<summary>
        ///When player die, the highest score will be save in HighScore.txt in resoucres
        ///if the score < highscore -- new highscore = score 
        ///I dont know how this function work magically, so dont touch it! --updated by lavi (time wasted to fix it > 5 hrs)
        ///</summary>
        
        //************
        //**  TODO  **
        //************
        //find a way to write highscore to resources to display in other device  -- done (add ReadMe)


        private void HighScore()
        {

            //This if statement check if highScore < Score to write new highScore to txt file                         
            if (Settings.Score > Settings.highScore)
            {                
                var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string filePath = Path.Combine(projectPath, "Resources");                

                Settings.highScore = Settings.Score;

                //File.WriteAllText(filePath, Settings.highScore.ToString());
                File.WriteAllText("HighScore.txt", Settings.highScore.ToString());
            }

            // PLEASE README
            //Problem in readfile -- other device cannot find file HighScore.txt
            /*
             *Fix: after cloning project in your computer, right click to your snake project in solution explorer
             *Choose properties > resources (in left side bar) > Add resources (in top bar) > 
             * > click on the \/ and choose add existing file > direct to your project folder and doubleClick to saveScore > doubleClick to HighScore.txt 
             * || Rebuil your project and start !  
             * Thank you!
             */

            //ReadFile from resources
            lblOutPut.Text = "Kỷ lục: " + File.ReadAllText("HighScore.txt"); 
        }
        
        private void pbCanvas_Click(object sender, EventArgs e)
        {

        }

    }
}
