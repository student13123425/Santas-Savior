using Raylib_cs;
using ConsoleApp1;
using System;

namespace HelloWorld
{
    internal static class Program
    {
        [System.STAThread]
        public static void Main()
        {
            string baseDir = AppContext.BaseDirectory;
            Console.WriteLine($"App base directory: {baseDir}");
            
            Raylib.InitWindow(1333, 1100, "Donkey Kong remake");
            Raylib.InitAudioDevice();

            Game game = new Game();
            
            bool isLoaded = false;
            LoadingStatus loadingStatus = new LoadingStatus { Percentage = 0, Title = "Initializing..." };

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                if (!isLoaded)
                {
                    loadingStatus = game.GlobalTextures.UpdateLoad();
                    LoadingScreen.Render(loadingStatus.Percentage, loadingStatus.Title);
                    if (loadingStatus.IsFinished)
                        isLoaded = true;
                }
                else
                {
                    game.update();
                    game.render();
                }
                Raylib.DrawFPS(10, 10);
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}