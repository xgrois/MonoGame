using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWithPhysics
{
    public class RectangleEntity : PhysicShape
    {



        public Vector2 Scale
        {
            get
            {
                if (Texture2D == null)
                {
                    throw new System.Exception("Texture2D has not been assigned yet so we cannot return its origin.");
                }
                return BodySize / TextureSize;
            }
        }

        public Vector2 BodySize { get; set; }

        public BodyType BodyType
        {
            get
            {
                if (Body != null) return Body.BodyType;
                else throw new System.Exception("BodyType cannot be returned since no Body is yet attached.");
            }
            private set { }
        }

        public RectangleEntity(World world, Vector2 position, Vector2 bodySize, BodyType bodyType, float restitution = 0.8f, float friction = 0.5f, float density = 1f)
        {
            
            BodySize = bodySize;
            Body = world.CreateBody(position: position, rotation: 0f, bodyType: bodyType);
            BodyType = bodyType;

            Fixture fixture = Body.CreateRectangle(width: bodySize.X, height: bodySize.Y, density: 1f, offset: Vector2.Zero);

            // Give it some bounce and friction
            fixture.Restitution = restitution;
            fixture.Friction = friction;
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
