using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Graf
    {
        public List<GrafNode> Nodes { get; set; } = new List<GrafNode>();

        public Graf()
        {
        }

        public Graf(Line2D[] lines)
        {
            foreach (var line in lines)
            {
                GrafNode from = GetOrAddNode(line.Start);
                GrafNode to = GetOrAddNode(line.End);
                AddEdge(from, to);
            }
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