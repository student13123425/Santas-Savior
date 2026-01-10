using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Game
    {
        public Level[] levels = new Level[4];
        public Player player;
        public Golumn donkey_kong;
        public Barel[] barels = new Barel[99999];
        public Drops[] drops = new Drops[50];
        int barels_spawned = 0;
        public MainMenu MainMenu;
        public GameOverMenu GameOverMenu;
        public YouWon YouWonScreen;
        public int current_level_id = -1;
        public bool is_debug = true;
        bool is_game_over = false;
        public TextureMap GlobalTextures;
        public AudioMap GlobalAudio;
        public SaveData SaveFile;
        public SantaClaus SantaClaus;
        public Jumper[] Jumpers = new Jumper[99999];
        public List<Enemy> Robots = new List<Enemy>();
        public Game()
        {
            levels[0] = new Level(0);
            levels[1] = new Level(1);
            levels[2] = new Level(2);
            levels[3] = new Level(3);

            player = new Player(new Vec2D(0, 0));
            donkey_kong = new Golumn(levels[0]);
            MainMenu = new MainMenu();
            GameOverMenu = new GameOverMenu();
            YouWonScreen = new YouWon();
            GlobalTextures = new TextureMap();
            GlobalAudio = new AudioMap();
            SaveFile = new SaveData();
            
            is_debug = SaveFile.is_debug;

            SantaClaus = new SantaClaus(levels[0].pricess_position);
        }

        public void render()
        {
            if (is_game_over)
            {
                GameOverMenu.render(this);
            }
            else if (current_level_id == -1)
            {
                MainMenu.render(this);
            }
            else if (current_level_id >= levels.Length)
            {
                YouWonScreen.render(this);
            }
            else
            {
                levels[current_level_id].render(this);
                donkey_kong.render(this);
                player.render(this);
                this.SantaClaus.render(this);
                render_enemys();
                render_drops();
            }
        }
        void render_enemys()
        {
            for (int i = 0; i < this.barels.Length; i++)
                this.barels[i].render(this);
            for (int i = 0; i < this.Jumpers.Length; i++)
                this.Jumpers[i].render(this);
            for (int i = 0; i < this.Robots.Count; i++)
                this.Robots[i].render(this);
        }
        void render_drops()
        {
            foreach (Drops drop in drops)
                drop.render(this.GlobalTextures);
        }

        public void reset_level()
        {
            if (this.current_level_id == -1)
                this.current_level_id = 0;

            if (this.current_level_id >= levels.Length)
                this.current_level_id = 0;

            this.player = new Player(new Vec2D(this.levels[current_level_id].player_start_pos.X, this.levels[current_level_id].player_start_pos.Y));
            this.donkey_kong = new Golumn(levels[current_level_id]);
            
            this.Robots.Clear();

            this.barels = new Barel[99999];
            for (int i = 0; i < this.barels.Length; i++)
                this.barels[i] = new Barel(levels[current_level_id].barel_spawn_point, false);
            for (int i = 0; i < this.Jumpers.Length; i++)
                this.Jumpers[i] = new Jumper(false,levels[current_level_id].JumperCollisionLine,levels[current_level_id].JumperSpawnPoint);
            this.drops = new Drops[50];
            var levelDrops = levels[current_level_id].LevelDrops;
            for (int i = 0; i < this.drops.Length; i++)
            {
                int drop_size = 60;
                if (i < levelDrops.Count)
                {
                    this.drops[i] = new Drops(levelDrops[i].Pos, this.GlobalTextures, levelDrops[i].Id, true);
                }
                else
                    this.drops[i] = new Drops(new Vec2D(0, 0), this.GlobalTextures, 0, false);
            }

            foreach (var bp in levels[current_level_id].breakPoints)
            {
                if (bp != null)
                {
                    bp.broke = false;
                    bp.is_active = true;
                }
            }
            this.is_game_over = false;
        }

        public void ThrowJumper()
        {
            foreach (Jumper J in  this.Jumpers)
                if (J.activate())
                    return;
        }
        public void onLive()
        {
            int currentHealth = player.GetHealth();

            if (currentHealth <= 0)
            {
                this.player_death();
            }
            else
            {
                this.reset_level();
                this.player.SetHealth(currentHealth);
            }
        }
        void update_enemys(Level level)
        {
            for (int i = 0; i < this.barels.Length; i++)
                this.barels[i].update(level);
            for (int i = 0; i < this.Jumpers.Length; i++)
                this.Jumpers[i].update(this);
            for (int i = 0; i < this.Robots.Count; i++)
                this.Robots[i].Update(this,true);
        }
        void update_drops()
        {
            for (int i = 0; i < this.drops.Length; i++)
                this.drops[i].update(this.player, this);
        }
        public void start_level(int current_level_id, bool save = true)
        {
            if (current_level_id >= levels.Length)
            {
                this.current_level_id = current_level_id;
                return;
            }

            if (save)
            {
                SaveFile.SetLevelID(current_level_id);
            }
            
            this.current_level_id = current_level_id;
            this.player = new Player(new Vec2D(this.levels[current_level_id].player_start_pos.X, this.levels[current_level_id].player_start_pos.Y));
            this.donkey_kong = new Golumn(levels[current_level_id]);
            
            this.Robots.Clear();

            this.barels = new Barel[99999];
            for (int i = 0; i < this.barels.Length; i++)
                this.barels[i] = new Barel(levels[current_level_id].barel_spawn_point, false);
            for (int i = 0; i < this.Jumpers.Length; i++)
                this.Jumpers[i] = new Jumper(false,levels[current_level_id].JumperCollisionLine,levels[current_level_id].JumperSpawnPoint);
            this.drops = new Drops[50];
            var levelDrops = levels[current_level_id].LevelDrops;
            for (int i = 0; i < this.drops.Length; i++)
            {
                if (i < levelDrops.Count)
                    this.drops[i] = new Drops(levelDrops[i].Pos, this.GlobalTextures, levelDrops[i].Id, true);
                else
                    this.drops[i] = new Drops(new Vec2D(0, 0), this.GlobalTextures, 0, false);
            }
            SantaClaus = new SantaClaus(levels[current_level_id].pricess_position);
        }

        public void spawn_barel(bool throw_side, bool isDown)
        {
            Vec2D spawn_point = new Vec2D(this.levels[this.current_level_id].barel_spawn_point.X, this.levels[this.current_level_id].barel_spawn_point.Y);
            if (isDown)
                spawn_point = new Vec2D(this.donkey_kong.DownSpawnBarrelPoint.X, this.donkey_kong.DownSpawnBarrelPoint.Y);
            this.barels[this.barels_spawned] = new Barel(spawn_point, true, isDown, throw_side);

            this.barels[this.barels_spawned].activete();
            this.barels_spawned += 1;
        }


        public void process_player_is_hit()
        {
            for (int i = 0; i < this.barels.Length; i++)
                if (barels[i].is_active && player.is_hit(barels[i].circle, this))
                {
                    this.barels[i].deactivate();
                    this.GlobalAudio.PlayerHit.Play(false);
                }
        }

        public void player_death()
        {
            this.is_game_over = true;
        }
        void process_level_end()
        {
            current_level_id += 1;
            if (current_level_id < levels.Length)
            {
                GlobalAudio.LevelCompleted.Play(false);
                start_level(this.current_level_id);
            }
            else
            {
                
            }
        }

        public void update()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Tab))
            {
                is_debug = !is_debug;
                SaveFile.SetDebug(is_debug);
            }

            if (!player.IsDying)
            {
                this.GlobalTextures.ConveyorTextures.ConvayerAnimation.Update();
            }
            
            this.GlobalAudio.Update();
            if (is_game_over)
            {
                this.GameOverMenu.update(this);
            }
            else if (current_level_id == -1)
            {
                int output = MainMenu.update(this);
                if (output == 0 || output == 1)
                {
                    int id = 0;
                    if (output == 0)
                    {
                        id = SaveFile.Load();
                    }
                    start_level(id);
                }
                else if (output == 2)
                    Environment.Exit(0);
            }
            else if (current_level_id >= levels.Length)
            {
                YouWonScreen.update(this);

                if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Space) || Raylib.IsKeyPressed(KeyboardKey.Escape))
                {
                    current_level_id = -1;
                    YouWonScreen.Reset();
                }
            }
            else
            {
                if (is_debug)
                {
                    if (Raylib.IsKeyDown(KeyboardKey.LeftControl) || Raylib.IsKeyDown(KeyboardKey.RightControl))
                    {
                        int levelToLoad = -1;

                        if (Raylib.IsKeyPressed(KeyboardKey.One)) levelToLoad = 0;
                        else if (Raylib.IsKeyPressed(KeyboardKey.Two)) levelToLoad = 1;
                        else if (Raylib.IsKeyPressed(KeyboardKey.Three)) levelToLoad = 2;
                        else if (Raylib.IsKeyPressed(KeyboardKey.Four)) levelToLoad = 3;
                        else if (Raylib.IsKeyPressed(KeyboardKey.Five)) levelToLoad = 4;
                        else if (Raylib.IsKeyPressed(KeyboardKey.Six)) levelToLoad = 5;
                        else if (Raylib.IsKeyPressed(KeyboardKey.Seven)) levelToLoad = 6;
                        else if (Raylib.IsKeyPressed(KeyboardKey.Eight)) levelToLoad = 7;
                        else if (Raylib.IsKeyPressed(KeyboardKey.Nine)) levelToLoad = 8;
                        else if (Raylib.IsKeyPressed(KeyboardKey.Zero)) levelToLoad = 9;

                        if (levelToLoad != -1)
                        {
                            start_level(levelToLoad, false);
                            if (current_level_id >= levels.Length) return;
                        }
                    }
                }
                if (current_level_id < levels.Length)
                {
                    if (!player.IsDying)
                    {
                        levels[current_level_id].update(this);
                        donkey_kong.update(this);
                    }

                    player.update(levels[current_level_id], this);

                    if (!player.IsDying)
                    {
                        update_enemys(levels[current_level_id]);
                        update_drops();
                        this.SantaClaus.update(this);
                        process_player_is_hit();
                        if (levels[this.current_level_id].is_level_end(this))
                            this.process_level_end();
                    }
                }
            }
        }
    }
}