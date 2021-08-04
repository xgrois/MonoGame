using tainicom.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWithPhysics
{
    public class DynamicCircle : PhysicShape
    {

        public BodyType BodyType { get; } = BodyType.Dynamic;

        public Vector2 Scale
        {
            get
            {
                if (Texture2D == null)
                {
                    throw new System.Exception("Texture2D has not been assigned yet so we cannot return its origin");
                }
                return new Vector2(BodyRadius * 2f) / TextureSize;
            }
        }

        public float BodyRadius
        {
            get
            {
                if (Body == null)
                {
                    throw new System.Exception("Body hasn't been assigned yet.");
                }
                else if (Body.FixtureList.Count == 0)
                {
                    throw new System.Exception("Body exists but Fixture hasn't been assigned yet.");
                }
                return Body.FixtureList[0].Shape.Radius; // We assume unique Fixture per Body
            }
        }

        public DynamicCircle(World world, Vector2 position, float radius, float restitution = 0.8f, float friction = 0.5f, float density = 1f)
        {

            Body = world.CreateBody(position: position, rotation: 0f, bodyType: BodyType);

            var pfixture = Body.CreateCircle(radius, density);

            // Give it some bounce and friction
            pfixture.Restitution = restitution;
            pfixture.Friction = friction;

        }


        public override void Load(ContentManager content, string spriteLocation)
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
                layerDepth: 0.5f);
        }

    }
}
