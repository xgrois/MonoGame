using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWithPhysics
{
    public class FixedPlatform : PhysicShape
    {

        public BodyType BodyType { get; } = BodyType.Static;

        public Vector2 Scale
        {
            get
            {
                if (Texture2D == null)
                {
                    throw new System.Exception("Texture2D has not been assigned yet so we cannot return its origin");
                }
                return BodySize / TextureSize;
            }
        }

        public Vector2 BodySize { get; set; }

        public FixedPlatform(World world, Vector2 position, Vector2 bodySize, string tag, float density = 1f)
        {
            Tag = tag;

            BodySize = bodySize;
            Body = world.CreateBody(position: position, rotation: 0f, bodyType: BodyType);

            var pfixture = Body.CreateRectangle(width: bodySize.X, height: bodySize.Y, density: 1f, offset: Vector2.Zero);
            
            // Give it some bounce and friction
            pfixture.Restitution = 0.3f;
            pfixture.Friction = 0.5f;
        }

        public  override void Load(ContentManager content, string spriteLocation)
        {
            Texture2D = content.Load<Texture2D>(spriteLocation);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: Texture2D,
                position: Body.Position,
                sourceRectangle: null,
                color: Color,
                rotation: Body.Rotation,
                origin: TextureOrigin,
                scale: Scale,
                effects: SpriteEffects.FlipVertically,
                layerDepth: 0f);
        }
    }
}
