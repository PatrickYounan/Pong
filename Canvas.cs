using System;
using System.Globalization;
using System.Numerics;
using Raylib_cs;

namespace Pong
{
    // Author: Patrick Younan
    public class Canvas
    {
        private static readonly int[] DIRECTIONS = {-1, 1};

        private readonly Vector2 moveSpeed = new(5f, 5f);
        private readonly Random random = new();

        private Vector2 ballSpeed = new(5f, 5f);
        private Vector2 score = new(0f, 0f);

        private const float ballRadius = 8f;

        private Rectangle player;
        private Rectangle enemy;

        private Vector2 startLine;
        private Vector2 endLine;

        private Vector2 ball;
        
        private Sound hit;
        private Sound win;

        public void Init()
        {
            ball = new Vector2(Raylib.GetScreenWidth() / 2 - 1, Raylib.GetScreenHeight() / 2 - 1);
            player = new Rectangle(Raylib.GetScreenWidth() - 20, Raylib.GetScreenHeight() / 2 - 70, 10, 140);
            enemy = new Rectangle(10, Raylib.GetScreenHeight() / 2 - 70, 10, 140);
            startLine = new Vector2(Raylib.GetScreenWidth() / 2f, 0);
            endLine = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight());
            
            hit = Raylib.LoadSound("res/hit.wav");
            win = Raylib.LoadSound("res/win.wav");
            Raylib.SetSoundVolume(hit, 0.2f);
            Raylib.SetSoundVolume(win, 0.2f);

            RandomizeBallSpeed();
        }

        private void RestartCanvas()
        {
            ball.X = Raylib.GetScreenWidth() / 2 - 1;
            ball.Y = Raylib.GetScreenHeight() / 2 - 1;
            RandomizeBallSpeed();
        }

        private int NextSpeed()
        {
            return DIRECTIONS[random.Next(DIRECTIONS.Length)];
        }

        private void RandomizeBallSpeed()
        {
            ballSpeed.X *= NextSpeed();
            ballSpeed.Y *= NextSpeed();
        }

        private void UpdatePlayerInput()
        {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_S)) player.y += moveSpeed.Y;
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_W)) player.y -= moveSpeed.Y;

            if (player.y <= 0) player.y = 0;
            if (player.y + player.height >= Raylib.GetScreenHeight()) player.y = Raylib.GetScreenHeight() - player.height;
        }

        private void UpdateEnemy()
        {
            if (enemy.y < ball.Y) enemy.y += moveSpeed.Y;
            else enemy.y -= moveSpeed.Y;

            if (enemy.y <= 0) enemy.y = 0;
            if (enemy.y + enemy.height >= Raylib.GetScreenHeight()) enemy.y = Raylib.GetScreenHeight() - enemy.height;
        }

        private void UpdateBallLogic()
        {
            ball.X += ballSpeed.X;
            ball.Y += ballSpeed.Y;

            if (ball.Y <= ballRadius || ball.Y + ballRadius >= Raylib.GetScreenHeight()) ballSpeed.Y *= -1;
            if (ball.X <= ballRadius)
            {
                score.Y++;
                Raylib.PlaySound(win);
                RestartCanvas();
            }
            else if (ball.X + ballRadius >= Raylib.GetScreenWidth())
            {
                score.X++;
                RestartCanvas();
            }

            if (!Raylib.CheckCollisionCircleRec(ball, ballRadius, player) && !Raylib.CheckCollisionCircleRec(ball, ballRadius, enemy)) return;
            ballSpeed.X *= -1;
            Raylib.PlaySound(hit);
        }

        public void UpdateAndRender()
        {
            UpdatePlayerInput();
            UpdateEnemy();
            UpdateBallLogic();

            Raylib.DrawRectangleRec(player, Color.BLUE);
            Raylib.DrawRectangleRec(enemy, Color.RED);
            Raylib.DrawLineEx(startLine, endLine, 1f, Color.BLACK);
            Raylib.DrawCircle((int) ball.X, (int) ball.Y, ballRadius, Color.WHITE);

            Raylib.DrawText(score.X.ToString(CultureInfo.CurrentCulture), Raylib.GetScreenWidth() / 2 - 30, 20, 20, Color.BLACK); // enemy score.
            Raylib.DrawText(score.Y.ToString(CultureInfo.CurrentCulture), Raylib.GetScreenWidth() / 2 + 15, 20, 20, Color.BLACK); // player score.
        }
    }
}