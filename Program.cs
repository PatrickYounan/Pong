using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Pong
{
    // Author: Patrick Younan
    internal static class Program
    {
        private static void Main()
        {
            InitAudioDevice();
            InitWindow(960, 600, "Pong - C# Raylib");
            SetTargetFPS(60);

            Canvas canvas = new();
            canvas.Init();

            while (!WindowShouldClose())
            {
                BeginDrawing();
                ClearBackground(Color.DARKGREEN);

                canvas.UpdateAndRender();

                EndDrawing();
            }

            CloseWindow();
        }
    }
}