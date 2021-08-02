using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWithMonoGame
{
    public static class SimpleLineShapes
    {
        private const float EPSILON = 0.000_000_1f;

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, float thinkness = 2f)
        {
            Vector2 diff = end - begin;
            spriteBatch.Draw(
                texture: ShapeTools.Pixel,
                position: begin,
                sourceRectangle: null,
                color: color,
                rotation: diff.ToAngle(),
                origin: Vector2.Zero,
                scale: new Vector2(diff.Length(), thinkness),
                effects: SpriteEffects.None,
                layerDepth: 0f
                );
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 begin, float length, float rotation, Color color, float thinkness = 2f)
        {
            spriteBatch.Draw(
                texture: ShapeTools.Pixel,
                position: begin,
                sourceRectangle: null,
                color: color,
                rotation: rotation,
                origin: Vector2.Zero,
                scale: new Vector2(length, thinkness),
                effects: SpriteEffects.None,
                layerDepth: 0f
                );
        }

        public static void DrawLineRectangle(SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color, float thinkness = 2f)
        {

            // A----B
            // |    |
            // |    |
            // D----C


            Vector2 vA = position;
            Vector2 vB = new Vector2(vA.X + size.X, vA.Y);
            Vector2 vC = new Vector2(vA.X + size.X, vA.Y + size.Y);
            Vector2 vD = new Vector2(vA.X, vA.Y + size.Y);
            

            // AB
            DrawLine(spriteBatch, vA, vB, color, thinkness);

            // BC
            DrawLine(spriteBatch, vB, vC, color, thinkness);

            // CD
            DrawLine(spriteBatch, vC, vD, color, thinkness);

            // DA
            DrawLine(spriteBatch, vD, vA, color, thinkness);


        }

        public static void DrawLineCircle(SpriteBatch spriteBatch, Vector2 centerPosition, float radius, Color color, int samples = 20, float thinkness = 2f)
        {
            if (samples <= 2) throw new Exception("Circunference needs at least 3 sample points.");

            float alpha = (2 * MathF.PI) / samples;

            // For rotation, we translate the center of the circunference to the 0,0 (top-left)
            // Once rotated, we add the center

            // Our first point is x = 0, y = radius in normal coordinates.
            // In MG, y-coord is reversed (- is up).
            Vector2 v1 = new Vector2(0, -radius);
            Vector2 v2;
            for (int i = 0; i < samples; i++)
            {
                v2 = v1.Rotate(alpha);
                DrawLine(spriteBatch, v1 + centerPosition, v2 + centerPosition, color, thinkness);
                v1 = v2;
            }
        }

        public static void DrawLineConvexRegularPolygon(SpriteBatch spriteBatch, int sides, Vector2 center, float circumradius, Color color, float thinkness = 2f)
        {
            DrawLineCircle(spriteBatch, center, circumradius, color, sides, thinkness);
        }


        /// <summary>
        /// Draw a custom polygon given the sequence of vertices in MG coordinate system.
        /// Applies to the given vertices the given transformation matrix. This is useful
        /// for rotate the polygon, translate it to a position, scale it...
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="vertexSequence"></param>
        /// <param name="transform"></param>
        /// <param name="color"></param>
        /// <param name="thinkness"></param>
        public static void DrawLineCustomPolygon(SpriteBatch spriteBatch, Vector2[] vertexSequence, Matrix transform, Color color, float thinkness = 2f)
        {
            Vector2 v1;
            Vector2 v2;
            for (int i = 0; i < vertexSequence.Length; i++)
            {
                v1 = vertexSequence[i];
                v2 = vertexSequence[(i + 1) % vertexSequence.Length];
                DrawLine(spriteBatch, Vector2.Transform(v1, transform), Vector2.Transform(v2, transform), color, thinkness);
            }
        }

        /// <summary>
        /// Rotates a given vector counterclockwise by the given angle (rads)
        /// Assumes the given vector is placed at 0,0
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Vector2 Rotate(this Vector2 vector, float alpha)
        {
            // counterclockwise rotation
            float x = vector.X * MathF.Cos(alpha) - vector.Y * MathF.Sin(alpha);
            float y = vector.X * MathF.Sin(alpha) + vector.Y * MathF.Cos(alpha);
            return new Vector2( IsAlmostZero(x) ? 0f : x, IsAlmostZero(y) ? 0f : y);
        }

        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        private static bool IsAlmostZero(float x)
        {
            return (-EPSILON <= x) && (x <= EPSILON);
        }

    }
}
