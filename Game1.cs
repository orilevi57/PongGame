using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PongGame;


public class Game1 : Game
{
    private Texture2D _whitePixel;
    private Rectangle _ball;
    private Rectangle _paddle1;
    private Rectangle _paddle2;
    private Vector2 _ballSpeed;
    private int _screenWidth = 800;
    private int _screenHeight = 480;
    private int _paddleSpeed = 5;
    private SpriteFont _font;
    private int _score1 = 0;
    private int _score2 = 0;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _ball = new Rectangle(_screenWidth / 2 - 10, _screenHeight / 2 - 10, 20, 20);
        _paddle1 = new Rectangle(30, _screenHeight / 2 - 40, 10, 80);
        _paddle2 = new Rectangle(_screenWidth - 40, _screenHeight / 2 - 40, 10, 80);
        _ballSpeed = new Vector2(4f, 4f);
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("DefaultFont");

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
        _whitePixel.SetData(new[] { Color.White });
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState k = Keyboard.GetState();

        if (k.IsKeyDown(Keys.Escape))
            Exit();

        // Move player 1 paddle (W/S)
        if (k.IsKeyDown(Keys.W) && _paddle1.Y > 0)
            _paddle1.Y -= _paddleSpeed;
        if (k.IsKeyDown(Keys.S) && _paddle1.Y + _paddle1.Height < _screenHeight)
            _paddle1.Y += _paddleSpeed;

        // Move player 2 paddle (Up/Down)
        if (k.IsKeyDown(Keys.Up) && _paddle2.Y > 0)
            _paddle2.Y -= _paddleSpeed;
        if (k.IsKeyDown(Keys.Down) && _paddle2.Y + _paddle2.Height < _screenHeight)
            _paddle2.Y += _paddleSpeed;

        // Move ball
        _ball.X += (int)_ballSpeed.X;
        _ball.Y += (int)_ballSpeed.Y;

        // Bounce ball off top/bottom
        if (_ball.Y <= 0 || _ball.Y + _ball.Height >= _screenHeight)
            _ballSpeed.Y *= -1;

        // Bounce ball off paddles
        if (_ball.Intersects(_paddle1) || _ball.Intersects(_paddle2))
            _ballSpeed.X *= -1;

        // Check if ball is out of bounds to update score and reset
        if (_ball.X < 0)
        {
            _score2++;
            ResetBall();
        }
        if (_ball.X > _screenWidth)
        {
            _score1++;
            ResetBall();
        }

        base.Update(gameTime);
    }

    private void ResetBall()
    {
        _ball.X = _screenWidth / 2 - 10;
        _ball.Y = _screenHeight / 2 - 10;
        _ballSpeed = new Vector2(4f * (_ballSpeed.X > 0 ? -1 : 1), 4f);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        // Draw paddles and ball using the white pixel texture tinted by color
        _spriteBatch.Draw(_whitePixel, _paddle1, Color.LightBlue);
        _spriteBatch.Draw(_whitePixel, _paddle2, Color.Red);
        _spriteBatch.Draw(_whitePixel, _ball, Color.White);

        // Draw scores if you have a font loaded
        // _spriteBatch.DrawString(_font, $"{_score1}", new Vector2(200, 20), Color.White);
        // _spriteBatch.DrawString(_font, $"{_score2}", new Vector2(_screenWidth - 240, 20), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
