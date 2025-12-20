using System;

namespace ConsoleApp1
{
    public class Vec2D
    {
        public const float EPS = 1e-6f;
        public float X { get; set; }
        public float Y { get; set; }
        public Vec2D() { X = 0; Y = 0; }
        public Vec2D(float x, float y) { X = x; Y = y; }
        public void Set(float x, float y) { X = x; Y = y; }

        public static Vec2D operator +(Vec2D a, Vec2D b) => new Vec2D(a.X + b.X, a.Y + b.Y);
        public static Vec2D operator -(Vec2D a, Vec2D b) => new Vec2D(a.X - b.X, a.Y - b.Y);
        public static Vec2D operator *(Vec2D v, float s) => new Vec2D(v.X * s, v.Y * s);
        public static Vec2D operator /(Vec2D v, float s) => new Vec2D(v.X / s, v.Y / s);

        public float Length => (float)Math.Sqrt(X * X + Y * Y);
        public float LengthSq => X * X + Y * Y;
        public static float Dot(Vec2D a, Vec2D b) => a.X * b.X + a.Y * b.Y;
        public static float Cross(Vec2D a, Vec2D b) => a.X * b.Y - a.Y * b.X;

        public bool CollideWith(Vec2D other)
        {
            return Math.Abs(X - other.X) <= EPS && Math.Abs(Y - other.Y) <= EPS;
        }
        public bool CollideWith(Rect2D rect)
        {
            return rect.Contains(this);
        }
        public bool CollideWith(Circle circle)
        {
            var d = this - circle.Pos;
            return d.LengthSq <= circle.Radius * circle.Radius + EPS;
        }
        public bool CollideWith(Line2D line)
        {
            return line.CollideWith(this);
        }
        public void MoveAtAngle(float angleDeg, float distance)
        {
            float rad = angleDeg * (float)Math.PI / 180f;
            X += distance * (float)Math.Cos(rad);
            Y += distance * (float)Math.Sin(rad);
        }
        public override string ToString() => $"Vec2D({X}, {Y})";

        public bool IsNull()
        {
            return (int)X == -1&&(int)Y == -1;
        }
    }

    public class Rect2D
    {
        public Vec2D Pos { get; set; }
        public Vec2D Size { get; set; }
        public Rect2D() { Pos = new Vec2D(); Size = new Vec2D(); }
        public Rect2D(Vec2D pos, Vec2D size) { Pos = pos; Size = size; }
        public Rect2D(float x, float y, float width, float height) { Pos = new Vec2D(x, y); Size = new Vec2D(width, height); }

        public float Left => Pos.X;
        public float Top => Pos.Y;
        public float Right => Pos.X + Size.X;
        public float Bottom => Pos.Y + Size.Y;

        public bool Contains(Vec2D p)
        {
            return p.X >= Left && p.X <= Right && p.Y >= Top && p.Y <= Bottom;
        }
        public bool CollideWith(Vec2D point)
        {
            return Contains(point);
        }
        public bool CollideWith(Rect2D other)
        {
            bool separated = other.Left > Right || other.Right < Left || other.Top > Bottom || other.Bottom < Top;
            return !separated;
        }
        public bool CollideWith(Circle circle)
        {
            float cx = Math.Max(Left, Math.Min(circle.Pos.X, Right));
            float cy = Math.Max(Top, Math.Min(circle.Pos.Y, Bottom));
            var dX = circle.Pos.X - cx;
            var dY = circle.Pos.Y - cy;
            return dX * dX + dY * dY <= circle.Radius * circle.Radius;
        }
        public bool CollideWith(Line2D line)
        {
            return line.CollideWith(this);
        }
        public override string ToString() => $"Rect2D(Pos={Pos}, Size={Size})";
    }

    public class Circle
    {
        public Vec2D Pos { get; set; }
        public float Diameter { get; set; }
        public Circle() { Pos = new Vec2D(); Diameter = 0; }
        public Circle(Vec2D pos, float diameter) { Pos = pos; Diameter = diameter; }
        public Circle(float x, float y, float diameter) { Pos = new Vec2D(x, y); Diameter = diameter; }

        public float Radius => Diameter * 0.5f;
        public float RadiusSq => Radius * Radius;

        public bool CollideWith(Vec2D point)
        {
            var d = point - Pos;
            return d.LengthSq <= RadiusSq;
        }
        public bool CollideWith(Rect2D rect)
        {
            return rect.CollideWith(this);
        }
        public bool CollideWith(Circle other)
        {
            var d = other.Pos - Pos;
            float r = Radius + other.Radius;
            return d.LengthSq <= r * r;
        }
        public bool CollideWith(Line2D line)
        {
            return line.CollideWith(this);
        }
        public override string ToString() => $"Circle(Pos={Pos}, Diameter={Diameter})";
    }

    public class Line2D
    {
        public Vec2D Start { get; set; }
        public Vec2D End { get; set; }
        public Line2D() { Start = new Vec2D(); End = new Vec2D(); }
        public Line2D(Vec2D start, Vec2D end) { Start = start; End = end; }
        public Line2D(float x1, float y1, float x2, float y2) { Start = new Vec2D(x1, y1); End = new Vec2D(x2, y2); }

        public Vec2D GetIntersectionPoint(Line2D other)
        {
            Vec2D p = this.Start;
            Vec2D r = this.End - this.Start;
            Vec2D q = other.Start;
            Vec2D s = other.End - other.Start;

            float rxs = Vec2D.Cross(r, s);

            if (Math.Abs(rxs) < Vec2D.EPS) return null;

            float t = Vec2D.Cross(q - p, s) / rxs;
            float u = Vec2D.Cross(q - p, r) / rxs;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                return p + (r * t);
            }

            return null;
        }

        private static int Orientation(Vec2D p, Vec2D q, Vec2D r)
        {
            float val = Vec2D.Cross(q - p, r - q);
            if (Math.Abs(val) < Vec2D.EPS) return 0;
            return (val > 0) ? 1 : 2;
        }
        private static bool OnSegment(Vec2D p, Vec2D q, Vec2D r)
        {
            if (r.X <= Math.Max(p.X, q.X) + Vec2D.EPS && r.X >= Math.Min(p.X, q.X) - Vec2D.EPS &&
                r.Y <= Math.Max(p.Y, q.Y) + Vec2D.EPS && r.Y >= Math.Min(p.Y, q.Y) - Vec2D.EPS)
                return true;
            return false;
        }
        public bool CollideWith(Vec2D point)
        {
            return Orientation(Start, End, point) == 0 && OnSegment(Start, End, point);
        }
        public bool CollideWith(Rect2D rect)
        {
            if (rect.Contains(Start) || rect.Contains(End)) return true;
            Line2D top = new Line2D(rect.Left, rect.Top, rect.Right, rect.Top);
            Line2D bottom = new Line2D(rect.Left, rect.Bottom, rect.Right, rect.Bottom);
            Line2D left = new Line2D(rect.Left, rect.Top, rect.Left, rect.Bottom);
            Line2D right = new Line2D(rect.Right, rect.Top, rect.Right, rect.Bottom);
            return CollideWith(top) || CollideWith(bottom) || CollideWith(left) || CollideWith(right);
        }
        public bool CollideWith(Circle circle)
        {
            Vec2D dir = End - Start;
            float lenSq = dir.LengthSq;
            if (lenSq < Vec2D.EPS)
                return (Start - circle.Pos).LengthSq <= circle.RadiusSq + Vec2D.EPS;
            float t = Math.Max(0, Math.Min(1, Vec2D.Dot(circle.Pos - Start, dir) / lenSq));
            Vec2D projection = Start + dir * t;
            return (projection - circle.Pos).LengthSq <= circle.RadiusSq + Vec2D.EPS;
        }
        public bool CollideWith(Line2D other)
        {
            int o1 = Orientation(Start, End, other.Start);
            int o2 = Orientation(Start, End, other.End);
            int o3 = Orientation(other.Start, other.End, Start);
            int o4 = Orientation(other.Start, other.End, End);

            if (o1 != o2 && o3 != o4) return true;
            if (o1 == 0 && OnSegment(Start, End, other.Start)) return true;
            if (o2 == 0 && OnSegment(Start, End, other.End)) return true;
            if (o3 == 0 && OnSegment(other.Start, other.End, Start)) return true;
            if (o4 == 0 && OnSegment(other.Start, other.End, End)) return true;

            return false;
        }
        public float get_angle()
        {
            float dx = End.X - Start.X;
            float dy = End.Y - Start.Y;
            float angle = (float)(Math.Atan2(dy, dx) * 180.0 / Math.PI);
            if (angle < 0) angle += 360f;
            return angle;
        }
        public override string ToString() => $"Line2D(Start={Start}, End={End})";
    }

    public static class Utils
    {
        public static int GetRandomInt(int n, int m)
        {
            return System.Random.Shared.Next(n, m + 1);
        }
    }
}