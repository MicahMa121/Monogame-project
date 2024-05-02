using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Monogame_project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        enum Screen
        {
            intro,
            game,
            result
        }
        Screen screen;

        Texture2D characterSpriteSheet, coinTexture, fireballSpriteSheet, slimeSpriteSheet;
        List<Texture2D> characterTextures = new List<Texture2D>();
        List<Texture2D> fireballTextures = new List<Texture2D>();
        List<Texture2D> slimeTextures = new List<Texture2D>();
        Texture2D rectangleTexture, heartTexture;
        KeyboardState keyboardState;
        MouseState mouseState, prevMouseState;

        SpriteFont spriteFont;

        int width, height;

        int dimensionX, dimensionY;

        Rectangle[,] board = new Rectangle[12,9];
        Box[,] game = new Box[12, 9];

        Player player;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screen = Screen.intro;
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();
            width = _graphics.PreferredBackBufferWidth; height = _graphics.PreferredBackBufferHeight;

            base.Initialize();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Rectangle rect = new Rectangle(i * height / 9 + height / 180, j * height / 9 + height / 180, height / 9 - height / 90, height / 9 - height / 90);
                    board[i, j] = rect;
                    Box box = new Box(rect, Color.LawnGreen, rectangleTexture);
                    game[i, j] = box;
                }
            }
            player = new Player(characterTextures, new Vector2(board[0, 0].X, board[0,0].Y),board[0,0], 5, rectangleTexture,0,0);
            player._game = game;

            //test
            game[3, 6].AbleToPass = false;
            game[3, 5].AbleToPass = false;
            game[3, 4].AbleToPass = false;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            rectangleTexture = Content.Load<Texture2D>("rectangle");
            coinTexture = Content.Load<Texture2D>("coin");
            characterSpriteSheet = Content.Load<Texture2D>("pixel_character");
            spriteFont = Content.Load<SpriteFont>("SpriteFont");
            slimeSpriteSheet = Content.Load<Texture2D>("slimeSpriteSheet");
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    int width = slimeSpriteSheet.Width / 6, height = slimeSpriteSheet.Height / 6;
                    Rectangle sourceRect = new Rectangle(i * width, j * height, width, height);
                    Texture2D cropTexture = new Texture2D(GraphicsDevice, width, height);
                    Color[] data = new Color[width * height];
                    slimeSpriteSheet.GetData(0, sourceRect, data, 0, data.Length);
                    cropTexture.SetData(data);
                    if (slimeTextures.Count < 35)
                    {
                        slimeTextures.Add(cropTexture);
                    }

                }
            }
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    int width = characterSpriteSheet.Width / 4, height = characterSpriteSheet.Height / 4;
                    Rectangle sourceRect = new Rectangle(i * width, j * height, width, height);
                    Texture2D cropTexture = new Texture2D(GraphicsDevice, width, height);
                    Color[] data = new Color[width * height];
                    characterSpriteSheet.GetData(0, sourceRect, data, 0, data.Length);
                    cropTexture.SetData(data);
                    if (characterTextures.Count < 16)
                    {
                        characterTextures.Add(cropTexture);
                    }

                }
            }
            fireballSpriteSheet = Content.Load<Texture2D>("fireball");
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    int width = fireballSpriteSheet.Width / 2, height = fireballSpriteSheet.Height / 2;
                    Rectangle sourceRect = new Rectangle(i * width, j * height, width, height);
                    Texture2D cropTexture = new Texture2D(GraphicsDevice, width, height);
                    Color[] data = new Color[width * height];
                    fireballSpriteSheet.GetData(0, sourceRect, data, 0, data.Length);
                    cropTexture.SetData(data);
                    if (fireballTextures.Count < 4)
                    {
                        fireballTextures.Add(cropTexture);
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    game[i, j].Update();
                    if (board[i,j].Contains(mouseState.Position)&& prevMouseState.LeftButton != ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        player.UpdateLocation(i, j);
                    }
                }
            }
            player.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            for (int i = 0; i < 12; i++)
            {
                for (int j= 0; j < 9; j++)
                {
                    game[i, j].Draw(_spriteBatch);
                }
            }
            player.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}