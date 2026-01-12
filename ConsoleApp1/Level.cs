using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ConsoleApp1
{
    public struct LevelDropData
    {
        public Vec2D Pos;
        public int Id;
        public LevelDropData(Vec2D pos, int id)
        {
            this.Pos = pos;
            this.Id = id;
        }
    }

    public class Level
    {
        public Platform[] platforms = new Platform[50];
        public Stairs[] stairs = new Stairs[50];
        public Decoration[] decorations = new Decoration[50];
        public OilBarel barel;
        public Vec2D player_start_pos;
        public Line2D[] Frame = new Line2D[2];
        public Vec2D barel_spawn_point;
        public Rect2D BarelDesponLocation;
        public Vec2D SnowGolumnSpawnLocation;
        public Rect2D LevelEndRect;
        public bool is_throwing_barrels = false;
        public bool is_throwing_jumpers = false;
        public Vec2D SantaClausePosition;
        public Elevator[] elevators = new Elevator[50];
        public List<LevelDropData> LevelDrops = new List<LevelDropData>();
        public ConveyerBelt[] conveyerBelts = new ConveyerBelt[50];
        public PlatformBreakPoint[] breakPoints = new PlatformBreakPoint[50];
        public Vec2D[] enemySpawnPoints = new Vec2D[50];
        public int ID;
        public int jumper_X = -1;
        public Vec2D JumperSpawnPoint = new Vec2D(-1, -1);
        public Line2D JumperCollisionLine = new Line2D(new Vec2D(-1, -1), new Vec2D(-1, 1));
        public Graf graf;


        public List<Line2D> generate_graf_lines()
        {
            List<Line2D> output = new List<Line2D>();
            int segmentSize = 5; 

            foreach(Platform platform in platforms)
            {
                if (platform.collison_lines[0] == null) continue;
                Line2D[] lines = platform.get_line_segments(segmentSize);
                foreach (Line2D line in lines)
                  output.Add(line);  
            }

            foreach (ConveyerBelt conveyerBelt in conveyerBelts)
            {
                if(!conveyerBelt.is_active) continue;
                Line2D[] lines = conveyerBelt.get_line_segments(segmentSize);
                foreach (Line2D line in lines)
                    output.Add(line);  
            }
            foreach (Stairs stair in stairs)
            {
                if(!stair.active)continue;
                Line2D[] lines = stair.get_line_segments(output, segmentSize);
                foreach (Line2D line in lines)
                    output.Add(line);
            }
            return output;
        }

        public Level(int ID)
        {
            this.ID = ID;
            this.SnowGolumnSpawnLocation = new Vec2D(0, 0);
            for (int i = 0; i < enemySpawnPoints.Length; i++)
                enemySpawnPoints[i] = new Vec2D(-1, -1);
            for (int i = 0; i < platforms.Length; i++)
                platforms[i] = new Platform(true, 0, new Vec2D(0, 0), 0, ID);
            for (int i = 0; i < stairs.Length; i++)
                stairs[i] = new Stairs(0, 0, 0, true, false, ID, [], []);
            for (int i = 0; i < decorations.Length; i++)
                decorations[i] = new Decoration("", new Vec2D(0, 0), 0, false);
            for (int i = 0; i < elevators.Length; i++)
                elevators[i] = new Elevator(0, [0, 0], false, false);
            for (int i = 0; i < conveyerBelts.Length; i++)
                conveyerBelts[i] = new ConveyerBelt(new Vec2D(0, 0), 50, true, 3, 0, false);

            for (int i = 0; i < breakPoints.Length; i++)
                breakPoints[i] = new PlatformBreakPoint(new Vec2D(0, 0), 40, false);

            if (ID == 0)
            {
                this.barel = new OilBarel(new Vec2D(25, 1050 - 80), false, 2, true);
                this.is_throwing_barrels = true;

                this.decorations[0] = new Decoration("./sprites/BarrelStack.png", new Vec2D(65, 300 - 80), 165, true);
                this.SnowGolumnSpawnLocation = new Vec2D(250, 300);
                this.barel_spawn_point = new Vec2D(400, 300 - 55);
                platforms[9] = new Platform(false, 2, new Vec2D(400, 160), 0, ID);
                this.SantaClausePosition = new Vec2D(493, 160);
                this.LevelEndRect = new Rect2D(600, 000, 40, 300);

                stairs[11] = new Stairs(400, 300, 12, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[12] = new Stairs(539, 300, 12, false, true, ID, this.platforms, this.conveyerBelts);

                platforms[7] = new Platform(false, 8, new Vec2D(372, 300), -20, ID);
                platforms[8] = new Platform(false, 4, new Vec2D(0, 300), 0, ID);

                platforms[6] = new Platform(false, 15, new Vec2D(100, 450), 20, ID);
                platforms[5] = new Platform(false, 12, new Vec2D(0, 600), -00, ID);
                platforms[4] = new Platform(false, 14, new Vec2D(100, 750), 20, ID);
                platforms[3] = new Platform(false, 12, new Vec2D(0, 900), -20, ID);
                platforms[0] = new Platform(false, 8, new Vec2D(0, 1050), 0, ID);
                platforms[1] = new Platform(false, 7, new Vec2D(744, 1050), 20, ID);

                player_start_pos = new Vec2D(200, 1050 - 80);
                BarelDesponLocation = new Rect2D(0, 1050 - 80, 5, 80);

                stairs[0] = new Stairs(300, 1050, 5, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[1] = new Stairs(300, 1050, 5, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[2] = new Stairs(900, 1050, 5, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[4] = new Stairs(450, 900, 5, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[5] = new Stairs(700, 750, 5, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[6] = new Stairs(1050, 750, 5, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[7] = new Stairs(200, 600, 5, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[8] = new Stairs(500, 600, 5, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[9] = new Stairs(1050, 450, 5, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[10] = new Stairs(600, 450, 6, false, true, ID, this.platforms, this.conveyerBelts);
            }
            else if (ID == 1)
            {
                this.is_throwing_barrels = false;

                int groundY = 1050;
                int topY = 250;
                int rightStackX = 868;

                this.SnowGolumnSpawnLocation = new Vec2D(100, topY - 5);
                this.barel_spawn_point = new Vec2D(150, topY - 5);
                this.SantaClausePosition = new Vec2D(250, 145);
                this.LevelEndRect = new Rect2D(rightStackX-400, topY - 340, 40, 340);

                player_start_pos = new Vec2D(100, groundY - 80);
                BarelDesponLocation = new Rect2D(-100, -100, 1, 1);

                platforms[0] = new Platform(false, 50, new Vec2D(0, groundY), 0, ID);
                conveyerBelts[0] = new ConveyerBelt(new Vec2D(-15, groundY - 200), 31, true, 3, 40);
                conveyerBelts[3] = new ConveyerBelt(new Vec2D(-15, groundY - 800), 31, true, 3, 40, true, 40, true);
                conveyerBelts[1] = new ConveyerBelt(new Vec2D(-15, groundY - 600), 14, true, 2, 25);
                conveyerBelts[2] = new ConveyerBelt(new Vec2D(700, groundY - 600), 14, false, 2, 25);
                
                this.barel = new OilBarel(new Vec2D(640, groundY - 605), false, 1, true);

                platforms[1] = new Platform(false, 4, new Vec2D(250, groundY - 400), 0, ID);
                platforms[2] = new Platform(false, 4, new Vec2D(750, groundY - 400), 0, ID);
                platforms[4] = new Platform(false, 3, new Vec2D(200, 150), 0, ID);

                stairs[0] = new Stairs(200 - 70, groundY, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[1] = new Stairs(466 + 50, groundY, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[2] = new Stairs(733 - 10, groundY, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[3] = new Stairs(1000 + 70, groundY, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[4] = new Stairs(1075, groundY - 400, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[5] = new Stairs(250, groundY - 400, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[6] = new Stairs(600 - 20, groundY - 400, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[7] = new Stairs(750, groundY - 400, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[13] = new Stairs(850, 429, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[8] = new Stairs(425, groundY - 200, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[9] = new Stairs(720 + 125, groundY - 200, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[10] = new Stairs(200, 210, 12, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[11] = new Stairs(350, 210, 12, false, true, ID, this.platforms, this.conveyerBelts);
            }
            else if (ID == 2)
            {
                this.barel = new OilBarel(new Vec2D(0, 0), true, 0, false);
                this.is_throwing_barrels = false;
                int ground_y = 1100 - 50;
                this.SnowGolumnSpawnLocation = new Vec2D(150, 350);
                this.SantaClausePosition = new Vec2D(390, 200);
                this.barel_spawn_point = new Vec2D(0, 0);
                this.LevelEndRect = new Rect2D(500, 0, 40, 340);

                player_start_pos = new Vec2D(100, ground_y - 80);
                BarelDesponLocation = new Rect2D(-100, -100, 1, 1);

                elevators[0] = new Elevator(350, [350, 1050 + 60], true, true);
                elevators[1] = new Elevator(700, [350, 1050 + 60], true, false);

                platforms[1] = new Platform(false, 25, new Vec2D(0, ground_y), 0, 0);
                int gap = 200;
                for (int i = 0; i < 3; i++)
                {
                    int pos_y = ground_y - 150 - (gap * i);
                    if (i == 0)
                        player_start_pos = new Vec2D(100, pos_y - 80);
                    platforms[2 + i] = new Platform(false, 2, new Vec2D(50, pos_y), 0, 0);
                }
                platforms[5] = new Platform(false, 2, new Vec2D(410, 500 + 100), 0, 0);
                platforms[6] = new Platform(false, 2, new Vec2D(410 + 50, 700 + 100), 0, 0);

                platforms[7] = new Platform(false, 2, new Vec2D(850, 490), 0, 0);
                platforms[21] = new Platform(false, 2, new Vec2D(900, 725), 0, 0);
                platforms[8] = new Platform(false, 2, new Vec2D(850, 950), 0, 0);
                for (int i = 0; i < 3; i++)
                {
                    Vec2D offset = new Vec2D(90, -80);
                    platforms[9 + i] = new Platform(false, 1, new Vec2D(1050 + offset.X * i, 950 + offset.Y * i), 0, 0);
                    offset = new Vec2D(-90, -80);
                    platforms[13 + i] = new Platform(false, 1, new Vec2D(1230 + offset.X * i, 650 + offset.Y * i), 0, 0);

                }
                platforms[20] = new Platform(false, 12, new Vec2D(-80, 350), 0, 0);
                platforms[22] = new Platform(false, 2, new Vec2D(300, 200), 0, 0);
                platforms[23] = new Platform(false, 2, new Vec2D(500, 250), 0, 0);
                stairs[0] = new Stairs(50, 900, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[1] = new Stairs(190, 700, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[2] = new Stairs(900, 700, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[3] = new Stairs(190, 700, 6, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[4] = new Stairs(990, 480, 4, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[5] = new Stairs(300, 350, 15, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[6] = new Stairs(440, 350, 15, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[7] = new Stairs(638, 350, 3, false, true, ID, this.platforms, this.conveyerBelts);

                JumperSpawnPoint = new Vec2D(-100, 100);
                int line_y = 350;
                JumperCollisionLine = new Line2D(new Vec2D(0, line_y), new Vec2D(1030, line_y));

                is_throwing_jumpers = true;
                this.jumper_X = 1200;
            }
            else if (ID == 3)
            {
                this.barel = new OilBarel(new Vec2D(300,170), false, 1, true);
                this.is_throwing_barrels = false;

                this.SnowGolumnSpawnLocation = new Vec2D(666, 250);
                this.barel_spawn_point = new Vec2D(0, 0);

                this.SantaClausePosition = new Vec2D(500, 250);
                this.LevelEndRect = new Rect2D(-1, -1, 0, 0);

                player_start_pos = new Vec2D(100, 1050 - 80);
                BarelDesponLocation = new Rect2D(-100, -100, 1, 1);
                platforms[0] = new Platform(false, 14, new Vec2D(15, 1050), 0, ID);
                platforms[1] = new Platform(false, 13, new Vec2D(62, 850), 0, ID);
                platforms[2] = new Platform(false, 12, new Vec2D(108, 650), 0, ID);
                platforms[3] = new Platform(false, 11, new Vec2D(155, 450), 0, ID);
                platforms[4] = new Platform(false, 10, new Vec2D(201, 250), 0, ID);

                int bpIndex = 0;
                float tileSize = 93f;
                float bpSize = 40f;

                void AddPair(float x, float y, int w)
                {
                    int offset = 400;
                    float leftX =offset;
                    float rightX = 1333-offset;
                    breakPoints[bpIndex++] = new PlatformBreakPoint(new Vec2D(leftX, y), bpSize, true);
                    breakPoints[bpIndex++] = new PlatformBreakPoint(new Vec2D(rightX, y), bpSize, true);
                }

                AddPair(62, 850, 13);
                AddPair(108, 650, 12);
                AddPair(155, 450, 11);
                AddPair(201, 250, 10);

                int stairH = 6;
                int offset = 100;
                stairs[0] = new Stairs(90, 1050, stairH, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[1] = new Stairs(1200, 1050, stairH, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[2] = new Stairs(125, 850, stairH, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[3] = new Stairs(1150, 850, stairH, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[4] = new Stairs(175, 650, stairH, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[5] = new Stairs(1100, 650, stairH, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[6] = new Stairs(225, 450, stairH, false, true, ID, this.platforms, this.conveyerBelts);
                stairs[7] = new Stairs(1075, 450, stairH, false, true, ID, this.platforms, this.conveyerBelts);
            }

            Frame[0] = new Line2D(new Vec2D(0, 0), new Vec2D(0, 1100));
            Frame[1] = new Line2D(new Vec2D(1333, 0), new Vec2D(1333, 1100));
            graf = new Graf(generate_graf_lines());
        }

        public void render(Game game)
        {
            Raylib.DrawRectangle(0, 0, 999999, 9999999, Color.Black);
            foreach (Decoration decoration in decorations)
                decoration.render();
            foreach (Stairs stair in stairs)
                stair.render(game);
            foreach (Platform platform in platforms)
                platform.render(game);

            foreach (PlatformBreakPoint bp in breakPoints)
                bp.render(game);

            foreach (ConveyerBelt belt in this.conveyerBelts)
                belt.render(game);
            foreach (Elevator elevator in elevators)
                elevator.render(game);
            barel.render(game);

            if (game.is_debug)
            {
                Raylib.DrawRectangleLines((int)BarelDesponLocation.Pos.X, (int)BarelDesponLocation.Pos.Y, (int)BarelDesponLocation.Size.X, (int)BarelDesponLocation.Size.Y, Color.Red);
                Raylib.DrawCircle((int)barel_spawn_point.X, (int)barel_spawn_point.Y, 5, Color.Red);
                Raylib.DrawCircle((int)SnowGolumnSpawnLocation.X, (int)SnowGolumnSpawnLocation.Y, 5, Color.Red);

                if (LevelEndRect != null)
                    Raylib.DrawRectangleLines((int)LevelEndRect.Pos.X, (int)LevelEndRect.Pos.Y, (int)LevelEndRect.Size.X, (int)LevelEndRect.Size.Y, Color.Purple);

                Raylib.DrawCircle((int)JumperSpawnPoint.X, (int)JumperSpawnPoint.Y, 5, Color.Orange);
                Raylib.DrawLineV(new Vector2(JumperCollisionLine.Start.X, JumperCollisionLine.Start.Y), new Vector2(JumperCollisionLine.End.X, JumperCollisionLine.End.Y), Color.Green);
            }
            this.graf.render(game.is_debug);
        }
        public bool is_level_end(Game game)
        {
            if (this.ID == 3)
            {
                foreach (PlatformBreakPoint b in this.breakPoints)
                    if (b.is_active && !b.broke)
                        return false;
                return true;
            }
            return game.player.colision_rect.CollideWith(this.LevelEndRect);
        }
        public void update(Game game)
        {
            foreach (Elevator elevator in elevators)
                elevator.update(game);
            foreach (ConveyerBelt belt in this.conveyerBelts)
                belt.update(game, this);
            foreach (PlatformBreakPoint bp in breakPoints)
                bp.update(game);
        }
    }
}