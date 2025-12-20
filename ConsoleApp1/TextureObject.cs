using Raylib_cs;

namespace ConsoleApp1
{
    public class TextureObject
    {
        public Texture2D Texture { get; private set; }
        public Vec2D Offset { get; set; }
        private TextureRenderer _renderer;

        public int Width => Texture.Width;
        public int Height => Texture.Height;

        public TextureObject(Texture2D texture, TextureRenderer renderer, Vec2D offset = null)
        {
            Texture = texture;
            _renderer = renderer;
            Offset = offset ?? new Vec2D(0, 0);
        }

        public float DrawCenter(float pixelSize, bool isWidth, Vec2D position, bool flip = false, float rotation = 0f)
        {
            return _renderer.DrawTextureCenter(Texture, pixelSize, isWidth, position + Offset, flip, rotation);
        }

        public float DrawBottomCenter(float pixelSize, bool isWidth, Vec2D position, bool flip = false)
        {
            return _renderer.DrawTextureBottomCenter(Texture, pixelSize, isWidth, position + Offset, flip);
        }

        public float DrawTopCenter(float pixelSize, bool isWidth, Vec2D position, bool flip = false)
        {
            return _renderer.DrawTextureTopCenter(Texture, pixelSize, isWidth, position + Offset, flip);
        }

        public float DrawLeftCenter(float pixelSize, bool isWidth, Vec2D position, bool flip = false)
        {
            return _renderer.DrawTextureLeftCenter(Texture, pixelSize, isWidth, position + Offset, flip);
        }

        public float DrawRightCenter(float pixelSize, bool isWidth, Vec2D position, bool flip = false)
        {
            return _renderer.DrawTextureRightCenter(Texture, pixelSize, isWidth, position + Offset, flip);
        }

        public void DrawRect(Rect2D rect, bool flip = false, float rotation = 0f)
        {
            Rect2D offsetRect = new Rect2D(rect.Pos + Offset, rect.Size);
            _renderer.DrawTextureRect(Texture, offsetRect, flip, rotation);
        }

        public void DrawRectCentered(Rect2D rect, bool flip = false, float rotation = 0f)
        {
            Rect2D offsetRect = new Rect2D(rect.Pos + Offset, rect.Size);
            _renderer.DrawTextureRectCentered(Texture, offsetRect, flip, rotation);
        }
    }
}