using Raylib_cs;
using System.Numerics;

namespace ConsoleApp1
{
    public static class LoadingScreen
    {
        public static void Render(float percentage, string text)
        {
            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();
            Raylib.ClearBackground(Color.Black);
            int fontSize = 40;
            int textWidth = Raylib.MeasureText(text, fontSize);
            Raylib.DrawText(text, (screenWidth - textWidth) / 2, screenHeight / 2 - 60, fontSize, Color.White);
            int barWidth = 600;
            int barHeight = 40;
            int barX = (screenWidth - barWidth) / 2;
            int barY = screenHeight / 2 + 20;

            Raylib.DrawRectangleLines(barX, barY, barWidth, barHeight, Color.White);

            if (percentage < 0) percentage = 0;
            if (percentage > 1) percentage = 1;

            int fillWidth = (int)(barWidth * percentage);
            int padding = 4;
            
            if (fillWidth > 0)
                Raylib.DrawRectangle(barX + padding, barY + padding, fillWidth - (padding * 2), barHeight - (padding * 2), Color.White);
            
            string percentText = $"{(int)(percentage * 100)}%";
            int percentSize = 20;
            int percentWidth = Raylib.MeasureText(percentText, percentSize);
            Raylib.DrawText(percentText, (screenWidth - percentWidth) / 2, barY + barHeight + 10, percentSize, Color.Gray);
        }
    }
}