using Raylib_cs;
using System;
using System.Numerics;

namespace ConsoleApp1
{
    public class TextureRenderer
    {
        public float DrawTextureCenter(Texture2D texture, float pixelSize, bool isWidth, Vec2D position, bool flip = false, float rotation = 0f)
        {
            float finalWidth, finalHeight;
            float returnValue;

            if (isWidth)
            {
                finalWidth = pixelSize;
                float scale = pixelSize / (float)texture.Width;
                finalHeight = texture.Height * scale;
                returnValue = finalHeight;
            }
            else
            {
                finalHeight = pixelSize;
                float scale = pixelSize / (float)texture.Height;
                finalWidth = texture.Width * scale;
                returnValue = finalWidth;
            }

            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            if (flip) source.Width *= -1;

            Rectangle dest = new Rectangle(position.X, position.Y, finalWidth, finalHeight);
            Vector2 origin = new Vector2(finalWidth / 2, finalHeight / 2);

            Raylib.DrawTexturePro(texture, source, dest, origin, rotation, Color.White);

            return returnValue;
        }

        public float DrawTextureBottomCenter(Texture2D texture, float pixelSize, bool isWidth, Vec2D position, bool flip = false)
        {
            float finalWidth, finalHeight;
            float returnValue;

            if (isWidth)
            {
                finalWidth = pixelSize;
                float scale = pixelSize / (float)texture.Width;
                finalHeight = texture.Height * scale;
                returnValue = finalHeight;
            }
            else
            {
                finalHeight = pixelSize;
                float scale = pixelSize / (float)texture.Height;
                finalWidth = texture.Width * scale;
                returnValue = finalWidth;
            }

            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            if (flip) source.Width *= -1;

            Rectangle dest = new Rectangle(position.X, position.Y, finalWidth, finalHeight);
            Vector2 origin = new Vector2(finalWidth / 2, finalHeight);

            Raylib.DrawTexturePro(texture, source, dest, origin, 0f, Color.White);

            return returnValue;
        }

        public float DrawTextureTopCenter(Texture2D texture, float pixelSize, bool isWidth, Vec2D position, bool flip = false)
        {
            float finalWidth, finalHeight;
            float returnValue;

            if (isWidth)
            {
                finalWidth = pixelSize;
                float scale = pixelSize / (float)texture.Width;
                finalHeight = texture.Height * scale;
                returnValue = finalHeight;
            }
            else
            {
                finalHeight = pixelSize;
                float scale = pixelSize / (float)texture.Height;
                finalWidth = texture.Width * scale;
                returnValue = finalWidth;
            }

            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            if (flip) source.Width *= -1;

            Rectangle dest = new Rectangle(position.X, position.Y, finalWidth, finalHeight);
            Vector2 origin = new Vector2(finalWidth / 2, 0);

            Raylib.DrawTexturePro(texture, source, dest, origin, 0f, Color.White);

            return returnValue;
        }

        public float DrawTextureLeftCenter(Texture2D texture, float pixelSize, bool isWidth, Vec2D position, bool flip = false)
        {
            float finalWidth, finalHeight;
            float returnValue;

            if (isWidth)
            {
                finalWidth = pixelSize;
                float scale = pixelSize / (float)texture.Width;
                finalHeight = texture.Height * scale;
                returnValue = finalHeight;
            }
            else
            {
                finalHeight = pixelSize;
                float scale = pixelSize / (float)texture.Height;
                finalWidth = texture.Width * scale;
                returnValue = finalWidth;
            }

            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            if (flip) source.Width *= -1;

            Rectangle dest = new Rectangle(position.X, position.Y, finalWidth, finalHeight);
            Vector2 origin = new Vector2(0, finalHeight / 2);

            Raylib.DrawTexturePro(texture, source, dest, origin, 0f, Color.White);

            return returnValue;
        }

        public float DrawTextureRightCenter(Texture2D texture, float pixelSize, bool isWidth, Vec2D position, bool flip = false)
        {
            float finalWidth, finalHeight;
            float returnValue;

            if (isWidth)
            {
                finalWidth = pixelSize;
                float scale = pixelSize / (float)texture.Width;
                finalHeight = texture.Height * scale;
                returnValue = finalHeight;
            }
            else
            {
                finalHeight = pixelSize;
                float scale = pixelSize / (float)texture.Height;
                finalWidth = texture.Width * scale;
                returnValue = finalWidth;
            }

            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            if (flip) source.Width *= -1;

            Rectangle dest = new Rectangle(position.X, position.Y, finalWidth, finalHeight);
            Vector2 origin = new Vector2(finalWidth, finalHeight / 2);

            Raylib.DrawTexturePro(texture, source, dest, origin, 0f, Color.White);

            return returnValue;
        }

        public void DrawTextureRect(Texture2D texture, Rect2D rect, bool flip = false, float rotation = 0f)
        {
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            if (flip) source.Width *= -1;

            Rectangle dest = new Rectangle(rect.Pos.X, rect.Pos.Y, rect.Size.X, rect.Size.Y);
            Vector2 origin = Vector2.Zero;

            Raylib.DrawTexturePro(texture, source, dest, origin, rotation, Color.White);
        }

        public void DrawTextureRectCentered(Texture2D texture, Rect2D rect, bool flip = false, float rotation = 0f)
        {
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            if (flip) source.Width *= -1;

            Rectangle dest = new Rectangle(rect.Pos.X + rect.Size.X / 2, rect.Pos.Y + rect.Size.Y / 2, rect.Size.X, rect.Size.Y);
            Vector2 origin = new Vector2(rect.Size.X / 2, rect.Size.Y / 2);

            Raylib.DrawTexturePro(texture, source, dest, origin, rotation, Color.White);
        }
    }
}