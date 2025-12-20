using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Stairs
    {
        int height;
        Vec2D pos;
        bool is_empty = false;
        bool active = true;
        Rect2D rect;
        Rect2D GoDownWindowRect;
        public Vec2D[] StartAndEnd;

        Platform[] platforms;
        ConveyerBelt[] belts;

        public Stairs(int x, int y, int h, bool is_empty, bool active, int id, Platform[] level_data, ConveyerBelt[] belts)
        {
            this.active = active;
            this.is_empty = is_empty;
            this.height = h;
            this.pos = new Vec2D(x, y);
            this.platforms = level_data;
            this.belts = belts;
            this.rect = new Rect2D(pos.X, pos.Y - 35 * height + 35, 47, 35 * height);

            if (!is_empty)
            {
                this.StartAndEnd = this.getEndPoints(level_data, belts);
            }
            else
            {
                this.StartAndEnd = new Vec2D[] { new Vec2D(), new Vec2D() };
            }

            this.GoDownWindowRect = new Rect2D(pos.X, pos.Y - 35 * height - 60, 47, 60);
        }

        public int getPlayerColision(Rect2D rect)
        {
            if (!this.active)
                return 0;

            if (!is_empty)
            {
                this.StartAndEnd = this.getEndPoints(this.platforms, this.belts);
            }

            bool is_top = this.StartAndEnd[0].CollideWith(rect);
            bool is_bottom = this.StartAndEnd[1].CollideWith(rect);
            int output = 0;
            if (is_top)
                output = 1;
            else if (is_bottom)
                output = 2;
            return output;
        }

        public Vec2D[] getEndPoints(Platform[] platforms, ConveyerBelt[] belts)
        {
            if (platforms == null) return [new Vec2D(), new Vec2D()];

            Line2D line = this.getLine();
            int center_x = get_center_x();

            int[] TopAndBottom = [0, 9999];

            foreach (Platform platform in platforms)
            {
                if (platform == null || platform.collison_lines == null) continue;

                Line2D l = platform.collison_lines[0];

                if (l == null) continue;

                Vec2D intersection = l.GetIntersectionPoint(line);

                if (intersection != null)
                {
                    float point = intersection.Y;
                    float offset = 10f;
                    if (point > TopAndBottom[0])
                        TopAndBottom[0] = (int)(point - offset);

                    if (point < TopAndBottom[1])
                        TopAndBottom[1] = (int)(point - offset);
                }
            }

            if (belts != null)
            {
                foreach (ConveyerBelt belt in belts)
                {
                    if (belt == null || belt.rect.Size.X == 0) continue;

                    Vec2D start = new Vec2D(belt.rect.Pos.X, belt.rect.Pos.Y);
                    Vec2D end = new Vec2D(belt.rect.Pos.X + belt.rect.Size.X, belt.rect.Pos.Y);
                    Line2D l = new Line2D(start, end);

                    Vec2D intersection = l.GetIntersectionPoint(line);

                    if (intersection != null)
                    {
                        float point = intersection.Y;
                        float offset = 10f;
                        if (point > TopAndBottom[0])
                            TopAndBottom[0] = (int)(point - offset);

                        if (point < TopAndBottom[1])
                            TopAndBottom[1] = (int)(point - offset);
                    }
                }
            }

            return [new Vec2D(center_x, TopAndBottom[0]), new Vec2D(center_x, TopAndBottom[1])];
        }

        public Line2D getLine()
        {
            int extension = 100;
            int center_x = get_center_x();
            Vec2D start_point = new Vec2D(center_x, rect.Pos.Y - extension);
            Vec2D end_point = new Vec2D(center_x, rect.Pos.Y + rect.Size.Y + extension);
            return new Line2D(start_point, end_point);
        }

        public bool is_colision(Rect2D r, bool is_only_core)
        {
            if (!active)
                return false;
            if (is_only_core)
                return (r.CollideWith(rect));
            return (r.CollideWith(rect) || r.CollideWith(GoDownWindowRect));
        }

        public int get_center_x()
        {
            return (int)(rect.Pos.X + (rect.Size.X / 2));
        }

        public float[] get_bounds(float player_size)
        {
            float[] output = new float[2];
            output[0] = rect.Pos.Y - player_size;
            output[1] = rect.Pos.Y + rect.Size.Y - player_size;
            return output;
        }

        public void render(Game game)
        {
            if (is_empty) return;

#if DEBUG
            Raylib.DrawRectangleLines((int)rect.Pos.X, (int)rect.Pos.Y, (int)rect.Size.X, (int)rect.Size.Y, Color.Yellow);
            if (StartAndEnd != null && StartAndEnd.Length >= 2)
            {
                Raylib.DrawLine((int)StartAndEnd[0].X, (int)StartAndEnd[0].Y,
                                (int)StartAndEnd[1].X, (int)StartAndEnd[1].Y, Color.Green);
                Raylib.DrawCircle((int)StartAndEnd[0].X, (int)StartAndEnd[0].Y, 5, Color.Red);
                Raylib.DrawCircle((int)StartAndEnd[1].X, (int)StartAndEnd[1].Y, 5, Color.Green);
            }
            Line2D scanLine = getLine();
            Raylib.DrawLine((int)scanLine.Start.X, (int)scanLine.Start.Y,
                          (int)scanLine.End.X, (int)scanLine.End.Y, Color.Blue);
#endif
            for (int i = 0; i < height; i++)
            {
                Rect2D segmentRect = new Rect2D(pos.X, pos.Y - 35 * i, 47, 35);
                game.GlobalTextures.stairs[0].DrawRect(segmentRect);
            }
        }
    }
}