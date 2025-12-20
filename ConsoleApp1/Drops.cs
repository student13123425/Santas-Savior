using Raylib_cs;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Drops
    {
        public Vec2D pos;
        public int id;
        public int size;
        public Rect2D collision_rect;

        public bool isActive;

        public Dictionary<int, int> DropScores;

        public Drops(Vec2D pos, TextureMap map, int id, bool isActive)
        {
            this.isActive = isActive;
            this.pos = pos;
            this.size = 50; 
            this.id = id;
            InitializeScores();
            InitializeCollisionRect(map);
        }

        public Drops(Vec2D pos, TextureMap map, bool isActive)
        {
            this.isActive = isActive;
            this.pos = pos;
            this.size = 50;

            InitializeScores();

            Random rnd = new Random();
            this.id = rnd.Next(0, map.SantaClaus.drops.Length);

            InitializeCollisionRect(map);
        }

        private void InitializeScores()
        {
            DropScores = new Dictionary<int, int>()
            {
                { 0, 200 },
                { 1, 800 },
                { 2, 100 },
                { 3, 500 },
                { 4, 500 },
                { 5, 1500 },
                { 6, 800 },
                { 7, 1000 },
                { 8, 1200 }
            };
        }

        private void InitializeCollisionRect(TextureMap map)
        {
            if (this.size <= 0 || map.SantaClaus.drops == null || this.id < 0 || this.id >= map.SantaClaus.drops.Length)
            {
                this.collision_rect = new Rect2D(0, 0, 0, 0);
                return;
            }

            TextureObject textureObj = map.SantaClaus.drops[this.id];

            if (textureObj.Height <= 0)
            {
                this.collision_rect = new Rect2D(0, 0, 0, 0);
                return;
            }

            float finalHeight = this.size;
            float scale = this.size / (float)textureObj.Height;
            float finalWidth = textureObj.Width * scale;

            float topLeftX = this.pos.X - (finalWidth / 2);
            float topLeftY = this.pos.Y - (finalHeight / 2);

            this.collision_rect = new Rect2D(
                new Vec2D(topLeftX, topLeftY),
                new Vec2D(finalWidth, finalHeight)
            );
        }

        public void render(TextureMap map)
        {
            if (!isActive) return;

            map.SantaClaus.drops[this.id].DrawCenter(
                this.size,
                false,
                this.pos
            );
        }

        public void update(Player player, Game instance)
        {
            if (!isActive) return;

            if (collision_rect.Size.X <= 0 || collision_rect.Size.Y <= 0)
                return;

            if (collision_rect.CollideWith(player.colision_rect))
            {
                this.process_collision(player);
                instance.GlobalAudio.Pickup.Play(false);
            }
        }

        public void process_collision(Player player)
        {
            player.score += GetScore();
            isActive = false;
        }

        public int GetScore()
        {
            if (!isActive) return 0;
            if (this.DropScores.ContainsKey(this.id))
                return this.DropScores[this.id];
            return 0;
        }
    }
}