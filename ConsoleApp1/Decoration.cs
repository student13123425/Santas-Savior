using Raylib_cs;
using System.Numerics;

namespace ConsoleApp1
{
    public class Decoration
    {
        private Texture2D texture;
        public Vec2D CenterPosition { get; set; }
        public float Height { get; set; }
        public float Rotation { get; set; } = 0f;
        public bool active = false;

        public Decoration(string texturePath, Vec2D centerPos, float height, bool active)
        {
            if (active)
                this.texture = Raylib.LoadTexture(texturePath);
            this.active = active;
            this.CenterPosition = centerPos;
            this.Height = height;
        }

        public void render(bool showDebug = false)
        {
            if (!active)
                return;

            float finalHeight = this.Height;
            float scale = finalHeight / (float)texture.Height;
            float finalWidth = texture.Width * scale;
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            Rectangle dest = new Rectangle(CenterPosition.X, CenterPosition.Y, finalWidth, finalHeight);
            Vector2 origin = new Vector2(finalWidth / 2, finalHeight / 2);

            Raylib.DrawTexturePro(texture, source, dest, origin, Rotation, Color.White);

            if (showDebug)
            {
                Raylib.DrawCircle((int)CenterPosition.X, (int)CenterPosition.Y, 5, Color.Red);
            }
        }

        public void Unload()
        {
            Raylib.UnloadTexture(texture);
        }
    }
}