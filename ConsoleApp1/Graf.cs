using System;
using System.Collections.Generic;
using Raylib_cs;

namespace ConsoleApp1
{
    public class Graf
    {
        public List<GrafNode> Nodes { get; set; } = new List<GrafNode>();
        public Line2D[] lines=new Line2D[0];
        public Graf()
        {
            update_lines();
        }

        void update_lines()
        {
            lines = GetLines();
        }
        public Graf(Line2D[] lines)
        {
            foreach (var line in lines)
            {
                GrafNode from = GetOrAddNode(line.Start);
                GrafNode to = GetOrAddNode(line.End);
                AddEdge(from, to);
            }

            update_lines();
        }

        private GrafNode GetOrAddNode(Vec2D point)
        {
            foreach (var node in Nodes)
            {
                if ((int)node.Point.X == (int)point.X && (int)node.Point.Y == (int)point.Y)
                {
                    return node;
                }
            }
            var newNode = new GrafNode(point);
            Nodes.Add(newNode);
            return newNode;
        }

        public void AddNode(Vec2D point)
        {
            Nodes.Add(new GrafNode(point));
        }

        public void AddEdge(GrafNode from, GrafNode to)
        {
            if (Nodes.Contains(from) && Nodes.Contains(to))
            {
                from.AddConnection(to);
            }

            update_lines();
        }

        public Line2D[] GetLines()
        {
            List<Line2D> lines = new List<Line2D>();
            foreach (var node in Nodes)
            {
                foreach (var connection in node.Connections)
                {
                    lines.Add(new Line2D(node.Point, connection.Point));
                }
            }
            return lines.ToArray();
        }

        public void render()
        {
            foreach (Line2D line in lines)
            {
                Vec2D start = line.Start;
                Vec2D end = line.End;
                Raylib.DrawLine((int)start.X, (int)start.Y, (int)end.X, (int)end.Y,Color.Red);
            }

            foreach (GrafNode node in Nodes)
            {
                Vec2D point = node.Point;
                Raylib.DrawCircle((int)point.X, (int)point.Y, 10, Color.Green);
            }
        }
    }

    public class GrafNode
    {
        public Vec2D Point { get; set; }
        public List<GrafNode> Connections { get; private set; }

        public GrafNode(Vec2D point)
        {
            Point = point;
            Connections = new List<GrafNode>();
        }

        public void AddConnection(GrafNode target)
        {
            if (!Connections.Contains(target))
            {
                Connections.Add(target);
            }
        }
    }
}