using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Platform
    {
        bool is_empty = true;
        int width;
        int offset;
        Vec2D start;
        Vec2D size;
        public Line2D[] collison_lines = new Line2D[4];

        public Platform(bool is_empty, int width_segments, Vec2D pos, int offset, int id)
        {
            this.is_empty = is_empty; 
            this.width = width_segments;
            this.start = pos;
            this.offset = offset; 
            
            size = new Vec2D(93, 40);

            if (this.is_empty || width_segments <= 0) return;

            const float TILE_W = 93f;
            const float TILE_H = 40f;
            
            float offsetPerTile = (float)this.offset / width_segments;

            float leftX = start.X;
            float rightX = start.X + TILE_W * width_segments;

            float topY_left = start.Y;
            float topY_right = start.Y - offsetPerTile * (width_segments - 1);

            float botY_left = start.Y + TILE_H;
            float botY_right = start.Y + TILE_H - offsetPerTile * (width_segments - 1);

            Vec2D topLeft = new Vec2D(leftX, topY_left);
            Vec2D topRight = new Vec2D(rightX, topY_right);
            Vec2D bottomRight = new Vec2D(rightX, botY_right);
            Vec2D bottomLeft = new Vec2D(leftX, botY_left);

            collison_lines[0] = new Line2D(topLeft, topRight);
            collison_lines[1] = new Line2D(topRight, bottomRight);
            collison_lines[2] = new Line2D(bottomRight, bottomLeft);
            collison_lines[3] = new Line2D(bottomLeft, topLeft);
        }

        public bool check_colision_rect(Rect2D rect, bool is_top_only = false)
        {
            if (is_empty)
                return false;
            if (is_top_only)
                return collison_lines[0].CollideWith(rect);
            foreach (Line2D l in this.collison_lines)
                if (l.CollideWith(rect))
                    return true;
            return false;
        }

        public bool check_colision_circle(Circle circle, bool is_top_only = false)
        {
            if (is_empty)
                return false;
            if (is_top_only)
                return collison_lines[0].CollideWith(circle);
            foreach (Line2D l in this.collison_lines)
                if (l.CollideWith(circle))
                    return true;
            return false;
        }

        public void render(Game game)
        {
            if (is_empty)
                return;

            float offset_per_interation = ((float)offset / (float)width);

            for (int i = 0; i < width; i++)
            {
                float x = start.X + i * 93;
                float y = start.Y - offset_per_interation * i;

                Rect2D tileRect = new Rect2D(x, y, 93, 40);

                game.GlobalTextures.platform[0].DrawRect(tileRect);
            }

#if DEBUG
            for (int i = 0; i < 4; i++)
            {
                Raylib.DrawLine((int)collison_lines[i].Start.X, (int)collison_lines[i].Start.Y, (int)collison_lines[i].End.X, (int)collison_lines[i].End.Y, Color.SkyBlue);
            }
#endif
        }
    }
}