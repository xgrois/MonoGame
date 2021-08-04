using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameWithPhysics
{

    public class Game1 : Game
    {
        // Graphics
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _spriteBatchEffect;
        private SpriteFont _font;

        // Sprites
        private string _relPathSpriteBall = "Sprites/ball";
        private string _relPathSpritePlayer = "Sprites/player";
        private string _relPathSpritePlatform = "Sprites/platform";

        // Sounds
        private SoundEffect _hitSoundPlayerToPlatform;
        private SoundEffect _hitSoundPlayerToBall;
        private SoundEffect _hitSoundBallToBall;
        private SoundEffect _hitSoundBallToPlatform; // reuse _hitSoundPlayerToPlatform and change pitch

        // Inputs
        private GamePadState _oldPadState;
        private KeyboardState _oldKeyState;
        private MouseState _oldMouseState;

        // Simple camera controls
        private Vector3 _cameraPosition = new Vector3(0f, -1.7f, 0f); // camera is 1.7 meters below the ground
        float _cameraViewWidth = 30f; // camera is 12.5 meters wide.

        // Physics
        private World _world;
        private List<DynamicCircle> _balls;
        private List<FixedPlatform> _platforms;
        private DynamicCircle _player;

        private float _playerBodyRadius = 1.5f / 2f; // player diameter is 1.5 meters
        private Vector2 _platformBodySize = new Vector2(8f, 1f); // ground is 8x1 meters


#if !JOYSTICK
        const string Text = "Press A or D to rotate the ball\n" +
                            "Press Space to jump\n" +
                            "Use arrow keys to move the camera\n" +
                            "Left mouse click to throw a new ball";
#else
                const string Text = "Use left stick to move\n" +
                                    "Press A to jump\n" +
                                    "Use right stick to move camera\n";
#endif

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.Reach;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            
            /* World */
            _world = new World();

            /* Player */
            _player = new DynamicCircle(world: _world, position: new Vector2(0, _playerBodyRadius + 1f), radius: _playerBodyRadius, density: 1f);

            /* Circles */
            _balls = new List<DynamicCircle>();

            /* Platforms */

            _platforms = new List<FixedPlatform>()
            {
                new FixedPlatform(world: _world, position: new Vector2(-10f, -10f), bodySize: _platformBodySize, density: 1f),
                new FixedPlatform(world: _world, position: new Vector2(0f, 0f), bodySize: _platformBodySize, density: 1f),
                new FixedPlatform(world: _world, position: new Vector2(10f, -10f), bodySize: _platformBodySize, density: 1f),
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            /* Graphics */
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            // We use a BasicEffect to pass our view/projection in _spriteBatch
            _spriteBatchEffect = new BasicEffect(_graphics.GraphicsDevice);
            _spriteBatchEffect.TextureEnabled = true;

            /* Fonts */
            _font = Content.Load<SpriteFont>("Fonts/arial");

            /* Player */
            _player.Load(Content, _relPathSpritePlayer);
            _player.Body.FixtureList[0].Tag = "player"; // This tag is to identify the object in a collision

            /* Platforms */
            foreach (var platform in _platforms)
            {
                platform.Load(Content, _relPathSpritePlatform);
                platform.Body.FixtureList[0].Tag = "platform";
            }

            /* Sound effects */
            _hitSoundPlayerToPlatform = Content.Load<SoundEffect>("SoundEffects/player-hits-platform");
            _hitSoundPlayerToBall = Content.Load<SoundEffect>("SoundEffects/player-hits-ball");
            _hitSoundBallToBall = Content.Load<SoundEffect>("SoundEffects/ball-hits-ball");

            _player.Body.OnCollision += BodyPlayer_OnCollision;

        }

        private bool BodyPlayer_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
        {
            if (((string)sender.Tag == "player" && (string)other.Tag == "platform") || ((string)sender.Tag == "platform" && (string)other.Tag == "player"))
            {
                _hitSoundPlayerToPlatform.Play(volume: 0.4f, pitch: -0.5f, pan: 0f);
            }
            else if (((string)sender.Tag == "player" && (string)other.Tag == "ball") || ((string)sender.Tag == "ball" && (string)other.Tag == "player"))
            {
                _hitSoundPlayerToBall.Play(volume: 0.1f, pitch: 0f, pan: 0f);
            }

            return true;
        }

        private bool BodyBall_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
        {
            if ((string)sender.Tag == "ball" && (string)other.Tag == "ball")
            {
                _hitSoundBallToBall.Play(volume: 0.01f, pitch: 0f, pan: 0f);
            }
            else if (((string)sender.Tag == "ball" && (string)other.Tag == "platform") || ((string)sender.Tag == "platform" && (string)other.Tag == "ball"))
            {
                _hitSoundPlayerToPlatform.Play(volume: 0.1f, pitch: 0.2f, pan: 0f);
            }

            return true;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleGamePad(gameTime);
            HandleKeyboard(gameTime);
            HandleMouse(gameTime);

            //We update the world
            _world.Step(dt);

            base.Update(gameTime);
        }

        private void HandleGamePad(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            GamePadState padState = GamePad.GetState(0);

            if (padState.IsConnected)
            {
                if (padState.Buttons.Back == ButtonState.Pressed)
                    Exit();

                if (padState.Buttons.A == ButtonState.Pressed && _oldPadState.Buttons.A == ButtonState.Released)
                    _player.Body.ApplyLinearImpulse(new Vector2(0, 10));

                _player.Body.ApplyForce(padState.ThumbSticks.Left * 3f);
                _cameraPosition.X -= padState.ThumbSticks.Right.X;
                _cameraPosition.Y += padState.ThumbSticks.Right.Y;


                _oldPadState = padState;
            }
        }

        private void HandleKeyboard(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState state = Keyboard.GetState();

            var vp = GraphicsDevice.Viewport;

            // Move camera
            if (state.IsKeyDown(Keys.Left))
                _cameraPosition.X -= dt * _cameraViewWidth;

            if (state.IsKeyDown(Keys.Right))
                _cameraPosition.X += dt * _cameraViewWidth;

            if (state.IsKeyDown(Keys.Up))
                _cameraPosition.Y += dt * _cameraViewWidth;

            if (state.IsKeyDown(Keys.Down))
                _cameraPosition.Y -= dt * _cameraViewWidth;


            // We make it possible to rotate the player body
            if (state.IsKeyDown(Keys.A))
                _player.Body.ApplyTorque(10);

            if (state.IsKeyDown(Keys.D))
                _player.Body.ApplyTorque(-10);

            if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
                _player.Body.ApplyLinearImpulse(new Vector2(0, 10));

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            _oldKeyState = state;
        }

        private void HandleMouse(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            // Create new dynamic circles
            if ((mouseState.LeftButton == ButtonState.Pressed) && (_oldMouseState.LeftButton == ButtonState.Released))
            {
                Vector2 worldMousePosition = GetMousePositionInWorld(mouseState.X, mouseState.Y, GraphicsDevice.Viewport, _cameraViewWidth, _cameraPosition);
                DynamicCircle ball = new DynamicCircle(world: _world, position: worldMousePosition, radius: 0.5f, restitution: 0.5f, friction: 0.5f, density: 1f);
                ball.Load(Content, _relPathSpriteBall);
                ball.Body.FixtureList[0].Tag = "ball";
                ball.Body.OnCollision += BodyBall_OnCollision;
                _balls.Add(ball);
            }

            _oldMouseState = mouseState;
        }



        /// <summary>
        /// Converts mouse position in MG coordinate system to physics world coordinates and camera setup
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="viewport"></param>
        /// <param name="cameraViewWidth"></param>
        /// <param name="cameraPosition"></param>
        /// <returns></returns>
        private Vector2 GetMousePositionInWorld(float x, float y, Viewport viewport, float cameraViewWidth, Vector3 cameraPosition)
        {
            // First translate x,y coordinates where 0,0 = top,left 
            // to the x2,y2 coordinates where 0,0 = mid,mid
            float vphw = viewport.Width / 2f; // viewport half width
            float vphh = viewport.Height / 2f; // viewport half height

            float x2 = x - vphw;
            float y2 = (y - vphh) * -1;

            // Now scale x2,y2 coordinates to the rectangle of the camera (cameraWidth and cameraHeight)
            // (In this case x3,y3 represent meters now)
            float chw = cameraViewWidth / 2f; // camera half width
            float chh = (cameraViewWidth / viewport.AspectRatio) / 2f; // camera half height

            float x3 = x2 * (chw / vphw);
            float y3 = y2 * (chh / vphh);

            // Finally move x3,y3 to where the current cameraPosition is centered
            return new Vector2(x3 + cameraPosition.X, y3 + cameraPosition.Y);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Update camera View and Projection.
            var vp = GraphicsDevice.Viewport;
            _spriteBatchEffect.View = Matrix.CreateLookAt(_cameraPosition, _cameraPosition + Vector3.Forward, Vector3.Up);
            _spriteBatchEffect.Projection = Matrix.CreateOrthographic(_cameraViewWidth, _cameraViewWidth / vp.AspectRatio, 0f, -1f);

            // Draw player and ground. 
            // Our View/Projection requires RasterizerState.CullClockwise and SpriteEffects.FlipVertically.
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, RasterizerState.CullClockwise, _spriteBatchEffect);
            
            /* Draw player */
            _player.Draw(_spriteBatch);

            /* Draw platforms */
            foreach (var platform in _platforms)
            {
                platform.Draw(_spriteBatch);
            }
            
            /* Draw balls */
            foreach (var ball in _balls)
            {
                ball.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            // Display instructions
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, Text, new Vector2(14f, 14f), Color.Black);
            _spriteBatch.DrawString(_font, Text, new Vector2(12f, 12f), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
