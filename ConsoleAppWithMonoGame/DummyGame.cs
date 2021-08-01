using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleAppWithMonoGame
{
    public class DummyGame : Game
    {

        #region Fields
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            SimpleLineShapes.DrawLine(
                spriteBatch: _spriteBatch, 
                begin: new Vector2(100, 100), 
                end: new Vector2(100,200), 
                color: Color.White,
                thinkness: 10f);

            SimpleLineShapes.DrawLineRectangle(
                spriteBatch: _spriteBatch,
                position: new Vector2(30, 30),
                size: new Vector2(100, 100),
                color: Color.Red,
                thinkness: 2f);

            SimpleLineShapes.DrawCircunference(
                spriteBatch: _spriteBatch, 
                center: new Vector2(200, 200), 
                radius: 50f, 
                color: Color.Green, 
                samples: 20, 
                thinkness: 2);

            SimpleLineShapes.DrawLineConvexRegularPolygon(
                spriteBatch: _spriteBatch, 
                sides: 5, 
                center: new Vector2(500, 200), 
                circumradius: 30f, 
                color: Color.Violet, 
                thinkness: 2f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

    }
}
