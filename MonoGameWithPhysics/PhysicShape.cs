using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWithPhysics
{

    public abstract class PhysicShape
    {
        #region Textures
        public Texture2D Texture2D { get; set; }

        public Vector2 TextureSize
        {
            get
            {
                if (Texture2D == null)
                {
                    throw new System.Exception("Texture2D has not been assigned yet so we cannot return its size");
                }
                return new Vector2(Texture2D.Width, Texture2D.Height);
            }
        }

        public Vector2 TextureOrigin
        {
            get
            {
                if (Texture2D == null)
                {
                    throw new System.Exception("Texture2D has not been assigned yet so we cannot return its origin");
                }
                return new Vector2(Texture2D.Width / 2f, Texture2D.Height / 2f);
            }
        }

        public Color Color { get; set; } = Color.White;

        #endregion

        #region Physics

        public Body Body { get; set; }

        #endregion

        public virtual void Load(ContentManager content, string spriteLocation) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }

    }
}
