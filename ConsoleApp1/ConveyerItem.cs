using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ConveyerItem
    {
        Vec2D pos;
        int id;
        Rect2D rect = new Rect2D();
        float error_margin;

        public ConveyerItem(Vec2D pos, int id, float error_margin = 14f)
        {
            this.pos = pos;
            this.id = id;
            this.error_margin = error_margin;
        }

        public void update(Game game, int offset, int[] end_points)
        {
            this.pos.X += (float)offset * Raylib.GetFrameTime();

            if (offset > 0)
            {
                if (this.pos.X > end_points[1])
                {
                    this.pos.X = end_points[0];
                }
            }
            else
            {
                if (this.pos.X < end_points[0])
                {
                    this.pos.X = end_points[1];
                }
            }

            if (game.player.is_hit(this.rect, game))
            {
                
            }
        }

        private void update_rect(float width, float height)
        {
            float w_error = ((error_margin / 100) * width);
            float h_error = ((error_margin / 100) * height);
            float w = width - w_error*2;
            float h = height - h_error;
            
            Vec2D rect_pos = new Vec2D(pos.X - (width / 2) + w_error, pos.Y - height + h_error);
            Vec2D rect_size = new Vec2D(w, h);
            rect = new Rect2D(rect_pos, rect_size);
        }

        public void render(Game game)
        {
            int height = 45;
            TextureObject t = game.GlobalTextures.SlideItem[id];
            float width = t.DrawBottomCenter(height, false, this.pos);
            this.update_rect(width, height);

            if (game.is_debug)
            {
                Raylib.DrawRectangleLines(
                    (int)rect.Pos.X,
                    (int)rect.Pos.Y,
                    (int)rect.Size.X,
                    (int)rect.Size.Y,
                    Color.Red
                );
            }
        }
    }
}