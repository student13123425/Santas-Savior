using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Color = Raylib_cs.Color;
using Rectangle = Raylib_cs.Rectangle;

namespace ConsoleApp1
{
    public class Button
    {
        Rect2D rectangle;
        Texture2D hover;
        bool can_hover;
        bool is_hover = false;
        float animation = 0;
        bool ignore_hover_offset = false;
        int input_width;
        Vec2D pos;

        public Button(string texture_path_normal, string texture_path_hover, Vec2D pos, int width, bool can_hover = false, bool ignore_hover_offset = false)
        {
            this.ignore_hover_offset = ignore_hover_offset;
            this.can_hover = can_hover;
            input_width = width;
            this.pos = pos;
        }

        bool get_if_clicked()
        {
            bool is_clicked = Raylib.IsMouseButtonReleased(MouseButton.Left);
            return get_if_hover() && is_clicked;
        }

        bool get_if_hover()
        {
            if (rectangle == null)
                return false;
            Vec2D pos = new Vec2D(Raylib.GetMouseX(), Raylib.GetMouseY());
            return rectangle.Contains(pos);
        }

        public void render(Texture2D texture, TextureRenderer renderer)
        {
            float hover_offset = -10;
            if (ignore_hover_offset)
                hover_offset *= 0;

            float scale = (float)input_width / texture.Width;
            float currentOffset = (int)(hover_offset * animation);

            rectangle = new Rect2D(
                new Vec2D(pos.X, pos.Y + currentOffset),
                new Vec2D((float)input_width, texture.Height * scale)
            );

            renderer.DrawTextureRect(texture, rectangle, false);
        }

        void animation_update()
        {
            float speed = 10 * Raylib.GetFrameTime();
            if (!is_hover)
                speed *= -1;
            animation += speed;
            animation = Math.Clamp(animation, 0f, 1f);
        }

        public bool update()
        {
            is_hover = get_if_hover();
            animation_update();
            return get_if_clicked();
        }
    }
}