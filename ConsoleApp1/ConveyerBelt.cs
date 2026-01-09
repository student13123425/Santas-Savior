using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ConveyerBelt
    {
        float speed = 200f;
        int tile_count;
        int height;
        Vec2D pos;
        bool side;
        public Rect2D rect = new Rect2D();
        public Rect2D effect_rect = new Rect2D();
        public ConveyerItem[] items;
        bool is_first_update = true;
        int item_end_points_offset;
        int[] end_points = [-9999, 9999];

        public bool is_active;
        bool forced_stop;

        void stop()
        {
            forced_stop = true;
        }
        
        public ConveyerBelt(Vec2D pos, int tile_count, bool side, int items_count, int item_end_points_offset, bool is_active = true, int height = 40, bool forced_stop = false)
        {
            this.pos = pos;
            this.tile_count = tile_count;
            this.height = height;
            this.side = side;
            this.item_end_points_offset = item_end_points_offset;
            this.is_active = is_active;
            this.forced_stop = forced_stop;

            float estimatedTotalWidth = (tile_count + 2) * height; 
            CalculateBounds(estimatedTotalWidth);

            if (forced_stop)
            {
                items = new ConveyerItem[0];
            }
            else
            {
                items = new ConveyerItem[items_count];
                for (int i = 0; i < items.Length; i++)
                {
                    int ID = Utils.GetRandomInt(0, 1);
                    items[i] = new ConveyerItem(new Vec2D(0, 0), ID);
                }
            }
        }

        public void render(Game game)
        {
            if (!is_active)
                return;
            int center_y = (int)pos.Y + (height / 2);

            TextureObject animTexture;
            if (forced_stop)
                animTexture = game.GlobalTextures.ConveyorTextures.ConvayerAnimation.GetFrame(0);
            else
                animTexture = game.GlobalTextures.ConveyorTextures.ConvayerAnimation.GetCurrentTexture();

            float currentXOffset = animTexture.DrawLeftCenter(
                height,
                false,
                new Vec2D(this.pos.X, center_y)
            );

            for (int i = 0; i < tile_count; i++)
            {
                currentXOffset += game.GlobalTextures.ConveyorTextures.ConveyerSideTile.DrawLeftCenter(
                    height,
                    false,
                    new Vec2D(this.pos.X + currentXOffset, center_y)
                );
            }

            float endWidth = animTexture.DrawLeftCenter(
                height,
                false,
                new Vec2D(this.pos.X + currentXOffset, center_y),
                true
            );

            float totalWidth = currentXOffset + endWidth;

            CalculateBounds(totalWidth);

            if (is_first_update)
            {
                DistributeItems();
                is_first_update = false;
            }
            foreach (ConveyerItem item in items)
                item.render(game);

#if DEBUG
            Raylib.DrawCircle(end_points[0], (int)pos.Y, 10, Color.Blue);
            Raylib.DrawCircle(end_points[1], (int)pos.Y, 10, Color.Blue);
            Raylib.DrawLine(end_points[0], (int)pos.Y, end_points[1], (int)pos.Y, Color.Red);
#endif
        }

        private void CalculateBounds(float totalWidth)
        {
            rect = new Rect2D(this.pos.X, this.pos.Y, totalWidth, height);
            effect_rect = new Rect2D(this.pos.X, this.pos.Y, totalWidth, 3);
            end_points[0] = (int)rect.Pos.X - item_end_points_offset;
            end_points[1] = (int)rect.Pos.X + (int)rect.Size.X + item_end_points_offset;
        }

        private void DistributeItems()
        {
            if (items.Length == 0) return;

            int totalDistance = end_points[1] - end_points[0];
            int step = totalDistance / items.Length;

            for (int i = 0; i < items.Length; i++)
            {
                int xPos = end_points[0] + (i * step);
                items[i] = new ConveyerItem(new Vec2D(xPos, pos.Y), 0);
            }
        }

        public void update(Game game, Level level)
        {
            if (!is_active)
                return;
            int s = (int)speed;
            if (!this.side)
                s *= -1;

            if (game.player.is_on_ground_colision_rect.CollideWith(this.rect))
            {
                game.player.slide(s * Raylib.GetFrameTime(), level);
            }

            foreach (ConveyerItem item in items)
                item.update(game, (int)s, this.end_points);
        }

        public Line2D[] get_line_segments(int segment_len)
        {
            if (!is_active || segment_len <= 0) return new Line2D[0];

            int n = (int)(rect.Size.X / segment_len);

            if (n <= 0) return new Line2D[] { new Line2D(new Vec2D(rect.Left, rect.Top), new Vec2D(rect.Right, rect.Top)) };

            Line2D topLine = new Line2D(new Vec2D(rect.Left, rect.Top), new Vec2D(rect.Right, rect.Top));
            Line2D[] segments = new Line2D[n];

            for (int i = 0; i < n; i++)
            {
                float t1 = (float)i / n;
                float t2 = (float)(i + 1) / n;

                Vec2D p1 = topLine.Interpolate(t1);
                Vec2D p2 = topLine.Interpolate(t2);

                segments[i] = new Line2D(p1, p2);
            }
            return segments;
        }
    }
}