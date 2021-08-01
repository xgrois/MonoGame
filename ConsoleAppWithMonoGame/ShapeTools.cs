using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWithMonoGame
{
    public static class ShapeTools
    {
        public static Texture2D Pixel { get; private set; }

        public static void Load(GraphicsDevice graphicsDevice)
        {
            LoadPixel(graphicsDevice);
        }

        private static void LoadPixel(GraphicsDevice graphicsDevice)
        {
            Pixel = new Texture2D(graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }
    }
}
