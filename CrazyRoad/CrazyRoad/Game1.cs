using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CrazyRoad
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random my_rand = new Random();

        //graphics support
        Texture2D grassPicture;
        Rectangle grassDrawRect;
        Texture2D roadPicture;
        Rectangle roadDrawRect;
        Texture2D linePicture;
        Rectangle lineDrawRect;
        Texture2D[] leftCarPictures = new Texture2D[GameConstants.CARS_NUMBER];
        Texture2D[] rightCarPictures = new Texture2D[GameConstants.CARS_NUMBER];

        //game objects
        Message introduction;
        GrandMother player1;
        GrandMother player2;
        List<Car> cars = new List<Car>();
        List<Blood> bloodInks = new List<Blood>();

        //timer and random support
        float elapsedTime = 0;
        float elapsedIntroTime = 0;
        int currentCar = 0;
        int prevCar;

        //score support
        int player_1_score = 0;
        int player_2_score = 0;
        Vector2 player1ScorePosition;
        Vector2 player2ScorePosition;
        SpriteFont scoreFont;

        // sound effects
        SoundEffectInstance backGroundMusic;
        SoundEffectInstance backGroundSound;
        SoundEffect manDieSound;
        SoundEffect womanDieSound;
        SoundEffect beepSound;
        SoundEffect gongSound;

        #endregion

        #region Constructors

        //Constructor
        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = GameConstants.WINDOW_WIDTH;
            this.graphics.PreferredBackBufferHeight = GameConstants.WINDOW_HEIGHT;
        }

        #endregion

        #region BODY
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            player1 = new GrandMother(Content, "Pictures/grandma", GameConstants.PLAYER_1_START_POSITION_X,
                                        GameConstants.PLAYERS_START_POSITION_Y, GameConstants.Player.One);
            player2 = new GrandMother(Content, "Pictures/grandpa", GameConstants.PLAYER_2_START_POSITION_X,
                                        GameConstants.PLAYERS_START_POSITION_Y, GameConstants.Player.Two);

            // position of score report on the canvas
            player1ScorePosition = new Vector2(GameConstants.PLAYER1_SCORE_POSITION_X, GameConstants.PLAYER1_SCORE_POSITION_Y);
            player2ScorePosition = new Vector2(GameConstants.PLAYER2_SCORE_POSITION_X, GameConstants.PLAYER2_SCORE_POSITION_Y);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            //load pictures
            this.grassPicture = Content.Load<Texture2D>("Pictures/grass");
            this.grassDrawRect = new Rectangle(-5, -5, this.grassPicture.Width, this.grassPicture.Height);
            this.roadPicture = Content.Load<Texture2D>("Pictures/road");
            this.roadDrawRect = new Rectangle(-5, GameConstants.ROAD_OFFSET, this.roadPicture.Width, this.roadPicture.Height);
            this.linePicture = Content.Load<Texture2D>("Pictures/line");
            this.lineDrawRect = new Rectangle((GameConstants.WINDOW_WIDTH - this.linePicture.Width) / 2, -7,
                                                    this.linePicture.Width, this.linePicture.Height);
            this.introduction = new Message(Content);

            // load sound effects
            this.manDieSound = Content.Load<SoundEffect>("Sounds/screem_man");
            this.womanDieSound = Content.Load<SoundEffect>("Sounds/screeem_wom");
            this.beepSound = Content.Load<SoundEffect>("Sounds/beep");
            SoundEffect sound = Content.Load<SoundEffect>("Sounds/road");
            this.backGroundSound = sound.CreateInstance();
            this.backGroundSound.IsLooped = true;
            this.backGroundSound.Volume = 0.3f;
            this.backGroundSound.Play();

            //load music
            SoundEffect music = Content.Load<SoundEffect>("Sounds/music");
            this.backGroundMusic = music.CreateInstance();
            this.backGroundMusic.IsLooped = true;
            this.backGroundMusic.Volume = 0.5f;
            this.backGroundMusic.Play();

            //load cars
            for (int i = 0; i < GameConstants.CARS_NUMBER; i++)
            {
                this.leftCarPictures[i] = Content.Load<Texture2D>("Cars/Car_Left" + (i + 1).ToString());
                this.rightCarPictures[i] = Content.Load<Texture2D>("Cars/Car_Right" + (i + 1).ToString());
            }

            //load fonts
            scoreFont = Content.Load<SpriteFont>("ScoreFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows players to control characters
            KeyboardState keyboard = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Movements support
            this.elapsedTime += gameTime.ElapsedGameTime.Milliseconds;


            // give possibility for cars spread on the road
            // playing introduction message
            if (this.elapsedIntroTime > GameConstants.PLAYERS_START_DELAY)
            {
                this.introduction.Active = false;
                this.player1.Update(gameTime, keyboard);
                this.player2.Update(gameTime, keyboard);
            }
            else
            {
                this.introduction.Active = true;
                this.showIntro();
                this.elapsedIntroTime += gameTime.ElapsedGameTime.Milliseconds;
            }

            // cars spawning
            if (this.elapsedTime > GameConstants.CARS_SPAWN_DELAY)
            {
                this.elapsedTime = 0;
                this.SpawnCar();
            }

            // updating game objects
            foreach (Car car in this.cars)
            {
                car.Update(gameTime);
            }
            foreach (Blood ink in this.bloodInks)
            {
                ink.Update(gameTime);
            }

            // check if someone win round
            if (this.player1.CollisionRactangle.Bottom < GameConstants.ROAD_OFFSET)
            {
                this.player_1_score += GameConstants.SCORE_GET_FINISH;
                this.elapsedIntroTime = 0;
                player1 = new GrandMother(Content, "Pictures/grandma", GameConstants.PLAYER_1_START_POSITION_X,
                                        GameConstants.PLAYERS_START_POSITION_Y, GameConstants.Player.One);
                player2 = new GrandMother(Content, "Pictures/grandpa", GameConstants.PLAYER_2_START_POSITION_X,
                                            GameConstants.PLAYERS_START_POSITION_Y, GameConstants.Player.Two);
            }
            else if (this.player2.CollisionRactangle.Bottom < GameConstants.ROAD_OFFSET)
            {
                this.player_2_score += GameConstants.SCORE_GET_FINISH;
                this.elapsedIntroTime = 0;
                player1 = new GrandMother(Content, "Pictures/grandma", GameConstants.PLAYER_1_START_POSITION_X,
                                        GameConstants.PLAYERS_START_POSITION_Y, GameConstants.Player.One);
                player2 = new GrandMother(Content, "Pictures/grandpa", GameConstants.PLAYER_2_START_POSITION_X,
                                            GameConstants.PLAYERS_START_POSITION_Y, GameConstants.Player.Two);
            }

            //check if someone was killed
            foreach (Car car in this.cars)
            {
                if (car.CollisionRactangle.Intersects(this.player1.CollisionRactangle))
                {
                    this.player_2_score += GameConstants.SCORE_COMPETITOR_DIES;
                    this.womanDieSound.Play(0.5f, 0.0f, 0.0f);
                    this.bloodInks.Add(new Blood(Content, this.player1.CollisionRactangle.Center.X,
                                                     this.player1.CollisionRactangle.Center.Y));
                    this.player1 = new GrandMother(Content, "Pictures/grandma", GameConstants.PLAYER_1_START_POSITION_X,
                                                        GameConstants.PLAYERS_START_POSITION_Y, GameConstants.Player.One);
                }
                else if (car.CollisionRactangle.Intersects(this.player2.CollisionRactangle))
                {
                    this.player_1_score += GameConstants.SCORE_COMPETITOR_DIES;
                    this.manDieSound.Play(0.5f, 0.0f, 0.0f);
                    this.bloodInks.Add(new Blood(Content, this.player2.CollisionRactangle.Center.X,
                                                          this.player2.CollisionRactangle.Center.Y));
                    this.player2 = new GrandMother(Content, "Pictures/grandpa", GameConstants.PLAYER_2_START_POSITION_X,
                                                        GameConstants.PLAYERS_START_POSITION_Y, GameConstants.Player.Two);
                }
            }

            //clean up inactive objects
            for (int i = this.cars.Count - 1; i >= 0; i--)
            {
                if (!this.cars[i].Active) this.cars.RemoveAt(i);
            }
            for (int i = this.bloodInks.Count - 1; i >= 0; i--)
            {
                if (!this.bloodInks[i].Active) this.bloodInks.RemoveAt(i);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();

            //background
            this.spriteBatch.Draw(this.grassPicture, this.grassDrawRect, Color.AliceBlue);
            this.spriteBatch.Draw(this.roadPicture, this.roadDrawRect, Color.AliceBlue);

            //blood            
            foreach (Blood ink in this.bloodInks)
            {
                ink.Draw(this.spriteBatch);
            }
            //line should be over the blood but under the other objects
            this.spriteBatch.Draw(this.linePicture, this.lineDrawRect, Color.AliceBlue);

            //players
            this.player1.Draw(this.spriteBatch);
            this.player2.Draw(this.spriteBatch);

            //cars
            foreach (Car car in cars)
            {
                car.Draw(this.spriteBatch);
            }

            //messages output
            this.spriteBatch.DrawString(this.scoreFont, "Grandma  " + this.player_1_score.ToString(),
                                        this.player2ScorePosition, Color.Gold);
            this.spriteBatch.DrawString(this.scoreFont, "Grandpa  " + this.player_2_score.ToString(),
                                        this.player1ScorePosition, Color.Gold);

            //numbers have to be over everything
            this.introduction.Draw(this.spriteBatch);

            this.spriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion

        #region Privat Methods

        //This function generates new car without overlappings
        private void SpawnCar()
        {
            //this is here to avoid overlapping of two consecutive cars
            this.prevCar = this.currentCar;
            Car tempCar;

            int randCar = my_rand.Next(GameConstants.CARS_NUMBER);                          //set picture for car
            GameConstants.Direction tempDirection = this.getDirection(my_rand.Next(2));     //set direction for car
            int linePosition = 0;                 // init line in apropriate direction
            int temp_x = 0;                       // init position
            int randVelocity = 0;                 // init random velocity

            //new car generation
            while (this.currentCar == this.prevCar)
            {
                if (tempDirection == GameConstants.Direction.Right)
                {
                    temp_x = -100;
                    int tempLine = my_rand.Next(3);
                    if (tempLine == GameConstants.SLOW_LINE)
                    {
                        linePosition = GameConstants.LINE3_POSITION;
                        currentCar = 3;
                        randVelocity = GameConstants.SLOW_CAR_SPEED;
                    }
                    else if (tempLine == GameConstants.MEDIAN_LINE)
                    {
                        linePosition = GameConstants.LINE2_POSITION;
                        currentCar = 4;
                        randVelocity = GameConstants.MEDIAN_CAR_SPEED;
                    }
                    else
                    {
                        linePosition = GameConstants.LINE1_POSITION;
                        currentCar = 5;
                        randVelocity = GameConstants.FAST_CAR_SPEED;
                    }
                }
                else // if direction of the car is to the left
                {
                    temp_x = GameConstants.WINDOW_WIDTH + 100;
                    int tempLine = my_rand.Next(3);
                    if (tempLine == GameConstants.SLOW_LINE)
                    {
                        linePosition = GameConstants.LINE4_POSITION;
                        currentCar = 0;
                        randVelocity = GameConstants.SLOW_CAR_SPEED;
                    }
                    else if (tempLine == GameConstants.MEDIAN_LINE)
                    {
                        linePosition = GameConstants.LINE5_POSITION;
                        currentCar = 1;
                        randVelocity = GameConstants.MEDIAN_CAR_SPEED;
                    }
                    else
                    {
                        linePosition = GameConstants.LINE6_POSITION;
                        currentCar = 2;
                        randVelocity = GameConstants.FAST_CAR_SPEED;
                    }
                }
            }

            //add this generated car
            if (tempDirection == GameConstants.Direction.Right)
            {
                tempCar = new Car(tempDirection, this.rightCarPictures[randCar], temp_x, linePosition, randVelocity);
            }
            else  //if direction of the car is to the left
            {
                tempCar = new Car(tempDirection, this.leftCarPictures[randCar], temp_x, linePosition, randVelocity);
            }
            this.cars.Add(tempCar);
        }

        //This function shows intro timer (3, 2, 1, START)
        private void showIntro()
        {
            if (this.elapsedIntroTime < GameConstants.INTRO_FIRST_DIGIT_DELAY)
            {
                if (this.elapsedIntroTime <= 0)
                {
                    this.beepSound.Play(0.5f, 0.0f, 0.0f);
                    this.elapsedIntroTime += 0.001f;
                }
                this.introduction.Update(3); // show number three
            }
            else if (this.elapsedIntroTime < GameConstants.INTRO_SECOND_DIGIT_DELAY)
            {
                if (this.elapsedIntroTime < GameConstants.INTRO_FIRST_DIGIT_DELAY + 10)
                {
                    this.beepSound.Play(0.5f, 0.0f, 0.0f);
                }
                introduction.Update(2); // show number two
            }
            else
            {
                if (elapsedIntroTime < GameConstants.INTRO_SECOND_DIGIT_DELAY + 10)
                {
                    beepSound.Play(0.5f, 0.0f, 0.0f);
                }
                introduction.Update(1); // show number one
            }
        }

        //function that calculates the direction for the cars
        private GameConstants.Direction getDirection(int number)
        {
            if (number == 0) return GameConstants.Direction.Left;
            else return GameConstants.Direction.Right;
        }

        #endregion
    }
}
