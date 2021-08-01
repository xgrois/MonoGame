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

        public static void DrawLineConvexRegularPolygon(SpriteBatch spriteBatch, int sides, Vector2 center, float circumradius, Color color, float thinkness = 2f)
        {
            DrawCircunference(spriteBatch, center, circumradius, color, sides, thinkness);
        }

        public static void DrawCircunference(SpriteBatch spriteBatch, Vector2 center, float radius, Color color, int samples = 20, float thinkness = 2f)
        {
            if (samples <= 2) throw new Exception("Circunference needs at least 3 sample points.");

            double alpha = (2 * Math.PI) / samples;

            // For rotation, we translate the center of the circunference to the 0,0
            // Once rotated, we add the center
            Vector2 v1 = new Vector2(radius, 0);
            Vector2 v2 = v1.Rotate(alpha);

            DrawLine(spriteBatch, v1 + center, v2 + center, color, thinkness);

            for (int i = 0; i < samples - 1; i++)
            {
                v1 = v2;
                v2 = v1.Rotate(alpha);

                DrawLine(spriteBatch, v1 + center, v2 + center, color, thinkness);
            }


        }

        /// <summary>
        /// Rotates a given vector counterclockwise by the given angle (rads)
        /// Assumes the given vector is placed at 0,0
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Vector2 Rotate(this Vector2 vector, double alpha)
        {
            // counterclockwise rotation
            return new Vector2( (float)(vector.X * Math.Cos(alpha) - vector.Y * Math.Sin(alpha)), (float)(vector.X * Math.Sin(alpha) + vector.Y * Math.Cos(alpha)) );
        }

        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

    }
}
