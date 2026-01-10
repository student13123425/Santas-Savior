using System;
using System.Collections.Generic;
using Raylib_cs;

namespace ConsoleApp1
{
    public class Graf
    {
        public List<GrafNode> Nodes { get; set; } = new List<GrafNode>();
        public Line2D[] lines = new Line2D[0];
        private Texture2D? nodeTexture;

        public Graf()
        {
            update_lines();
        }

        void update_lines()
        {
            lines = GetLines();
        }

        public Graf(List<Line2D> lines)
        {
            foreach (var line in lines)
            {
                GrafNode from = GetOrAddNode(line.Start);
                GrafNode to = GetOrAddNode(line.End);
                AddEdge(from, to);
            }
            update_lines();
        }

        public int get_cloasest_node(Vec2D point)
        {
            float min_value = 9999;
            int index = 0;
            int min_node_index = 0;
            foreach (GrafNode node in Nodes)
            {
                float value = node.Point.DistanceTo(point);
                if (value < min_value)
                {
                    min_value = value;
                    min_node_index = index;
                }
                index++;
            }
            return min_node_index;
        }

        private GrafNode GetOrAddNode(Vec2D point)
        {
            foreach (var node in Nodes)
            {
                if (node.Point.DistanceTo(point) < 2.0f)
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
                to.AddConnection(from);
            }
            
            update_lines(); 
        }

        public Line2D[] GetLines()
        {
            List<Line2D> uniqueLines = new List<Line2D>();
            HashSet<GrafNode> processed = new HashSet<GrafNode>();

            foreach (var node in Nodes)
            {
                processed.Add(node);
                foreach (var connection in node.Connections)
                {
                    if (!processed.Contains(connection))
                    {
                        uniqueLines.Add(new Line2D(node.Point, connection.Point));
                    }
                }
            }
            return uniqueLines.ToArray();
        }

        public void render(bool is_debug)
        {
            if (!is_debug) return;

            if (nodeTexture == null) 
            {
                Image img = Raylib.GenImageColor(10, 10, Color.Blank);
                Raylib.ImageDrawCircle(ref img, 5, 5, 5, Color.Green);
                nodeTexture = Raylib.LoadTextureFromImage(img);
                Raylib.UnloadImage(img);
            }

            Rlgl.Begin(DrawMode.Lines);
            Rlgl.Color4ub(255, 0, 0, 255);

            foreach (Line2D line in lines)
            {
                Rlgl.Vertex2f(line.Start.X, line.Start.Y);
                Rlgl.Vertex2f(line.End.X, line.End.Y);
            }
            Rlgl.End();

            foreach (GrafNode node in Nodes)
            {
                Raylib.DrawTexture(nodeTexture.Value, (int)node.Point.X - 5, (int)node.Point.Y - 5, Color.White);
            }
        }

        public void Unload()
        {
            if (nodeTexture != null)
            {
                Raylib.UnloadTexture(nodeTexture.Value);
            }
        }

        public int[] GeneratePath(int startNodeIndex, int endNodeIndex)
        {
            if (startNodeIndex < 0 || startNodeIndex >= Nodes.Count || endNodeIndex < 0 || endNodeIndex >= Nodes.Count)
            {
                return new int[0];
            }

            Dictionary<GrafNode, int> nodeIndices = new Dictionary<GrafNode, int>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                nodeIndices[Nodes[i]] = i;
            }

            int count = Nodes.Count;
            float[] distances = new float[count];
            int[] previous = new int[count];
            List<int> unvisited = new List<int>();

            for (int i = 0; i < count; i++)
            {
                distances[i] = float.MaxValue;
                previous[i] = -1;
                unvisited.Add(i);
            }

            distances[startNodeIndex] = 0;

            while (unvisited.Count > 0)
            {
                int u = -1;
                float minDist = float.MaxValue;

                foreach (int i in unvisited)
                {
                    if (distances[i] < minDist)
                    {
                        minDist = distances[i];
                        u = i;
                    }
                }

                if (u == -1 || u == endNodeIndex)
                {
                    break;
                }

                unvisited.Remove(u);

                GrafNode uNode = Nodes[u];
                foreach (GrafNode neighbor in uNode.Connections)
                {
                    if (nodeIndices.TryGetValue(neighbor, out int v))
                    {
                        if (unvisited.Contains(v)) 
                        {
                            float dx = uNode.Point.X - neighbor.Point.X;
                            float dy = uNode.Point.Y - neighbor.Point.Y;
                            float weight = (float)Math.Sqrt(dx * dx + dy * dy);

                            float alt = distances[u] + weight;
                            if (alt < distances[v])
                            {
                                distances[v] = alt;
                                previous[v] = u;
                            }
                        }
                    }
                }
            }

            List<int> path = new List<int>();
            int current = endNodeIndex;

            if (previous[current] == -1 && current != startNodeIndex)
            {
                return new int[0];
            }

            while (current != -1)
            {
                path.Add(current);
                current = previous[current];
            }

            path.Reverse();
            return path.ToArray();
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