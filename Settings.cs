namespace Snake
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };

    public class Settings
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public int HighScore { get; set;  }
        public static bool GameOver { get; set; }
        public static Direction direction { get; set; }

        /// <summary>
        /// This Function is set the default setting of Snake. ex: Speed, score, point...
        /// TODO: add an setting label to setting in form without opening the code
        /// </summary>
        
        public Settings()
        {
            Width = 16;      //16
            Height = 16;     //16
            Speed = 9;       //Speed of Snake  
            Score = 0;       //Begin Score
            Points = 100;    //when the snake eat circle, score += 100

            //Test HighScore
            //HighScore = System.IO.;

            GameOver = false;
            direction = Direction.Down;
        }
    }


}
