using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Monogame_project
{
    internal class Box
    {
        private Rectangle _rectangle;
        private Color _color;
        private Texture2D _texture;
        public Vector2 Position;
        public bool AbleToPass { get; set; }
        public Box(Rectangle rectangle, Color color, Texture2D texture)
        {
            _rectangle = rectangle;
            _color = color;
            _texture = texture;
            AbleToPass = true;
            Position = new Vector2(rectangle.X,rectangle.Y);
        }
        public void Update()
        {
            if (!AbleToPass)
            {
                _color = Color.Red;
            }
            if (AbleToPass)
            {
                _color = Color.LawnGreen;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, _color);
        }
    }
}
