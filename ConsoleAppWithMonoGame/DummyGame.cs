using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConsoleAppWithMonoGame
{
    public class DummyGame : Game
    {

        #region Fields
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private float _rotationalSpeed = MathHelper.Pi; // rad/s
        private float _rotationAngle = 0f; // rads
        #endregion

        #region Properties

        #endregion


        #region Constructors
        public DummyGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion

        #region MainMethods
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ShapeTools.Load(GraphicsDevice);

        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.R))
                _rotationAngle = (_rotationAngle + _rotationalSpeed * dt) % MathHelper.TwoPi;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            SimpleLineShapes.DrawLine(
                spriteBatch: _spriteBatch, 
                begin: new Vector2(100, 400), 
                end: new Vector2(100,200), 
                color: Color.White,
                thinkness: 10f);

            SimpleLineShapes.DrawLineRectangle(
                spriteBatch: _spriteBatch,
                position: new Vector2(30, 30),
                size: new Vector2(100, 100),
                color: Color.Red,
                thinkness: 2f);

            SimpleLineShapes.DrawLineCircle(
                spriteBatch: _spriteBatch, 
                centerPosition: new Vector2(200, 200), 
                radius: 50f, 
                color: Color.Green, 
                samples: 20, 
                thinkness: 2);

            SimpleLineShapes.DrawLineCircle(
                spriteBatch: _spriteBatch,
                centerPosition: new Vector2(200, 200),
                radius: 50f,
                color: Color.Blue,
                samples: 5,
                thinkness: 1);

            SimpleLineShapes.DrawLineConvexRegularPolygon(
                spriteBatch: _spriteBatch,
                sides: 3,
                center: new Vector2(600, 100),
                circumradius: 60f,
                color: Color.Yellow,
                thinkness: 1f);

            SimpleLineShapes.DrawLineConvexRegularPolygon(
                spriteBatch: _spriteBatch, 
                sides: 5, 
                center: new Vector2(500, 200), 
                circumradius: 30f, 
                color: Color.Violet, 
                thinkness: 2f);

            Vector2[] vertices = new Vector2[] { new Vector2(0f, -10f), new Vector2(-10f, 10f), new Vector2(-5f, 5f), new Vector2(5f, 5f), new Vector2(10f, 10f) };
            SimpleLineShapes.DrawLineCustomPolygon(
                spriteBatch: _spriteBatch,
                vertexSequence: vertices,
                transform: Matrix.CreateTranslation(50f, 50f, 0f),
                color: Color.White,
                thinkness: 2f);

            SimpleLineShapes.DrawLineCustomPolygon(
                spriteBatch: _spriteBatch,
                vertexSequence: vertices,
                transform: Matrix.CreateRotationZ(MathHelper.PiOver2) * Matrix.CreateTranslation(100f, 100f, 0f),
                color: Color.White,
                thinkness: 2f);

            SimpleLineShapes.DrawLineCustomPolygon(
                spriteBatch: _spriteBatch,
                vertexSequence: vertices,
                transform: Matrix.CreateScale(2f) * Matrix.CreateRotationZ(_rotationAngle) * Matrix.CreateTranslation(200f, 200f, 0f),
                color: Color.White,
                thinkness: 2f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

    }
}
