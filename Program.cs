using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    const int width = 40;
    const int height = 20;
    static int birdY = height / 2;
    static int velocity = 0;
    static int gravity = 1;
    static int score = 0;
    static bool gameOver = false;

    static List<int> pipesX = new List<int>();
    static List<int> pipesGapY = new List<int>();
    static Random rnd = new Random();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;

        InitPipes();

        Thread inputThread = new Thread(HandleInput);
        inputThread.Start();

        while (!gameOver)
        {
            Update();
            Draw();
            Thread.Sleep(100);
        }

        Console.Clear();
        Console.WriteLine("💥 GAME OVER 💥");
        Console.WriteLine($"Your Score: {score}");
    }

    static void InitPipes()
    {
        pipesX.Clear();
        pipesGapY.Clear();
        for (int i = 0; i < 3; i++)
        {
            pipesX.Add(width + i * 20);
            pipesGapY.Add(GetNewGapY());
        }
    }

    static int GetNewGapY()
    {
        // Her zaman ekran içinde kalacak şekilde 5 birimlik boşluk bırak
        return rnd.Next(3, height - 7);
    }

    static void HandleInput()
    {
        while (!gameOver)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Spacebar)
            {
                velocity = -3;
            }
        }
    }

    static void Update()
    {
        velocity += gravity;
        birdY += velocity;
        if (birdY < 0) birdY = 0;
        if (birdY >= height) gameOver = true;

        for (int i = 0; i < pipesX.Count; i++)
        {
            pipesX[i]--;

            if (pipesX[i] == 5)
            {
                if (birdY < pipesGapY[i] || birdY > pipesGapY[i] + 4)
                {
                    gameOver = true;
                }
                else
                {
                    score++;
                }
            }

            if (pipesX[i] < -1)
            {
                pipesX[i] = width + 10;
                pipesGapY[i] = GetNewGapY();
            }
        }
    }

    static void Draw()
    {
        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 5 && y == birdY)
                {
                    Console.Write("🐤");
                }
                else if (IsPipeAt(x, y))
                {
                    Console.Write("🟥");
                }
                else
                {
                    Console.Write("  "); // boşluk geniş olsun diye çift boşluk
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nScore: " + score);
    }

    static bool IsPipeAt(int x, int y)
    {
        for (int i = 0; i < pipesX.Count; i++)
        {
            if (x == pipesX[i])
            {
                if (y < pipesGapY[i] || y > pipesGapY[i] + 4)
                    return true;
            }
        }
        return false;
    }
}