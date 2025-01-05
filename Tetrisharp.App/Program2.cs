using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Runtime.Versioning;

namespace Tetrisharp.App
{
    [SupportedOSPlatform("windows")]
    internal class Program2
    {
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Hello, World!");
        //    Console.ReadLine();
        //}


        //render buffer. !- do not change directly -!
        private static string buffer = "";
        private static float[,] render;

        private static readonly float targetFPS = 60; //change to get higher/lower fps
        private static readonly Vector canvasSize = new(59, 59); //change for different canvas size


        public static float deltaTime = 0; //time between frames
        public static float time = 0; //total time
        public static Vector halfCanvas = canvasSize / 2; //center of canvas

        public static float[,] Generate2DArray(Vector size)
        {
            float[,] output = new float[(int)size.x, (int)size.y];

            return output;
        }
        public static string GetPixel(float value)
        {
            string[] symbols = ["  ", "░░", "▒▒", "▓▓", "██"];
            return symbols[(int)Math.Floor(Math.Clamp(value * symbols.Length, 0, symbols.Length - 1))];
        }
        public static float Invert01(float value)
        {
            return 1 - value;
        }
        public static bool InBoundary(Vector pos)
        {
            return (int)MathF.Round(pos.x) >= 0 && (int)MathF.Round(pos.x) < canvasSize.x &&
                (int)MathF.Round(pos.y) >= 0 && (int)MathF.Round(pos.y) < canvasSize.y;
        }
        public static void Normalize()
        {
            for (int y = 0; y < canvasSize.y; y++)
            {
                for (int x = 0; x < canvasSize.x; x++)
                {
                    if (render[x, y] != 0)
                        render[x, y] = 1;
                }
            }
        }
        public static float Clamp(float value, float min = 0, float max = 1)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        //Render functions. Use them for rendering objects

        /// <summary>
        /// draw pixel
        /// </summary>
        /// <param name="pos">pixel position (from top-left)</param>
        /// <param name="opacity">pixel transparency</param>
        /// <param name="overlap">draw as transparent (true) or opaque (false)</param>
        public static void DrawPixel(Vector pos, float opacity = 1, bool overlap = false)
        {
            if (InBoundary(pos))
            {
                if (overlap)
                    render[(int)MathF.Round(pos.x), (int)MathF.Round(pos.y)] = Clamp(render[(int)MathF.Round(pos.x), (int)MathF.Round(pos.y)] + opacity);
                else
                    render[(int)MathF.Round(pos.x), (int)MathF.Round(pos.y)] = Clamp(opacity);
            }

        }
        /// <summary>
        /// draw circle
        /// </summary>
        /// <param name="pos">position of circle</param>
        /// <param name="radius">radius of circle</param>
        /// <param name="opacity">transparency of circle</param>
        /// <param name="overlap">draw as transparent (true) or opaque (false)</param>
        public static void DrawCircle(Vector pos, float radius, float opacity = 1, bool overlap = false)
        {
            for (int y = 0; y < canvasSize.y; y++)
            {
                for (int x = 0; x < canvasSize.x; x++)
                {
                    if (!InBoundary(new Vector(x, y)))
                        continue;
                    if ((x - pos.x) * (x - pos.x) + (y - pos.y) * (y - pos.y) <= radius * radius)
                        DrawPixel(new(x, y), opacity, overlap);
                }
            }
        }
        /// <summary>
        /// draw line
        /// </summary>
        /// <param name="a">start position</param>
        /// <param name="b">end position</param>
        /// <param name="opacity">transparency of line</param>
        /// <param name="overlap">draw as transparent (true) or opaque (false)</param>
        public static void DrawLine(Vector a, Vector b, float opacity = 1, bool overlap = false)
        {
            if (MathF.Abs(b.x - a.x) < MathF.Abs(b.y - a.y))
            {
                if (a.y < b.y)
                {
                    float incline = (a.x - b.x) / (a.y - b.y);

                    for (int y = (int)a.y; y <= b.y; y++)
                    {
                        int x = (int)MathF.Floor((y - b.y) * incline + b.x);
                        DrawPixel(new(x, y), opacity, overlap);
                    }
                }
                else
                {
                    float incline = (a.x - b.x) / (a.y - b.y);

                    for (int y = (int)b.y; y <= a.y; y++)
                    {
                        int x = (int)MathF.Floor((y - b.y) * incline + b.x);
                        DrawPixel(new(x, y), opacity, overlap);
                    }
                }
            }
            else
            {
                float incline = (a.y - b.y) / (a.x - b.x);

                if (a.x < b.x)
                {
                    for (int x = (int)a.x; x <= b.x; x++)
                    {
                        int y = (int)MathF.Floor((x - b.x) * incline + b.y);
                        DrawPixel(new(x, y), opacity, overlap);
                    }
                }
                else
                {
                    for (int x = (int)b.x; x <= a.x; x++)
                    {
                        int y = (int)MathF.Floor((x - b.x) * incline + b.y);
                        DrawPixel(new(x, y), opacity, overlap);
                    }
                }

            }
        }
        /// <summary>
        /// draw rectangle
        /// </summary>
        /// <param name="pos">position of rectangle</param>
        /// <param name="size">sides of rectangle</param>
        /// <param name="opacity">transparency of rectangle</param>
        /// <param name="overlap">draw as transparent (true) or opaque (false)</param>
        public static void DrawRect(Vector pos, Vector size, float opacity = 1, bool overlap = false)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    Vector pixelPos = new(MathF.Ceiling(x + pos.x - size.x / 2), MathF.Ceiling(y + pos.y - size.y / 2));
                    DrawPixel(pixelPos, opacity, overlap);
                }
            }
        }
        /// <summary>
        /// smoothing objects
        /// </summary>
        /// <param name="smoothZero">did smoothing effect transparent pixels</param>
        public static void Smooth(bool smoothZero = true)
        {
            float[,] output = Generate2DArray(canvasSize);
            for (int y = 0; y < canvasSize.y; y++)
            {
                for (int x = 0; x < canvasSize.x; x++)
                {
                    if (!smoothZero && render[x, y] == 0)
                    {
                        output[x, y] = 0;
                        continue;
                    }

                    float sum = render[x, y];
                    if ((x > 0) && (x < canvasSize.x - 1))
                        sum += render[x - 1, y] + render[x + 1, y];
                    else if (x == 0)
                        sum += render[x + 1, y];
                    else
                        sum += render[x - 1, y];

                    if ((y > 0) && (y < canvasSize.y - 1))
                        sum += render[x, y - 1] + render[x, y + 1];
                    else if (y == 0)
                        sum += render[x, y + 1];
                    else
                        sum += render[x, y - 1];

                    output[x, y] = sum / 5;
                }
            }
            render = output;
        }

        //dont edit this functions
        public static void ClearRender()
        {
            for (int y = 0; y < canvasSize.y; y++)
            {
                for (int x = 0; x < canvasSize.x; x++)
                {
                    render[x, y] = 0;
                }
            }
        }
        public static void Render()
        {
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < canvasSize.y; y++)
            {
                for (int x = 0; x < canvasSize.x; x++)
                {
                    buffer += GetPixel(render[x, y]);
                }
                buffer += "\n";
            }

            Console.Write(buffer);
            buffer = "";
        }

        //edit for rendering objects you need
        public static void ProcessRender()
        {

            //uncomment to see different examples

            // --- draw grid --- //
            //for (int x = 0; x < canvasSize.x; x += 5)
            //{
            //    DrawLine(new(x - 1, halfCanvas.y - 1), new(x - 1, halfCanvas.y + 1), .2f);
            //}
            //for (int y = 0; y < canvasSize.y; y += 5)
            //{
            //    DrawLine(new(halfCanvas.x - 1, y - 1), new(halfCanvas.x + 1, y - 1), .2f);
            //}
            //DrawLine(new(0, halfCanvas.y), new(canvasSize.x - 1, halfCanvas.y), .4f);
            //DrawLine(new(halfCanvas.x, 0), new(halfCanvas.x, canvasSize.y - 1), .4f);

            // --- draw rotating line --- //
            //DrawLine(halfCanvas, halfCanvas + new Vector(MathF.Sin(time) * 10, -MathF.Cos(time) * 10));

            // --- draw sin --- //
            //for (float x = 0; x < canvasSize.x; x += 0.2f)
            //{
            //    DrawPixel(new(x, MathF.Sin((x - halfCanvas.x) / 2) * 5 + halfCanvas.y));
            //}

            // --- draw snowman --- //
            DrawLine(halfCanvas - Vector.up * 2 + Vector.left * 6, halfCanvas - Vector.down * 4 + Vector.left * 12, .6f);
            DrawLine(halfCanvas - Vector.up * 2 + Vector.right * 7, halfCanvas - Vector.down * 4 + Vector.right * 13, .6f);
            DrawCircle(halfCanvas - Vector.down * 9, 9);
            DrawCircle(halfCanvas - Vector.up * 2, 7);
            DrawCircle(halfCanvas - Vector.up * 10, 5);
            DrawCircle(halfCanvas - Vector.up * 11 + Vector.left * 2, 1, 0);
            DrawCircle(halfCanvas - Vector.up * 11 + Vector.right * 2, 1, 0);
        }

        public static void Main2()
        {
            //if (Console.BufferHeight < canvasSize.y + 1 || Console.BufferWidth < canvasSize.x)
            //    Console.SetBufferSize((int)canvasSize.x * 2, (int)canvasSize.y + 1);

            Console.SetWindowSize((int)canvasSize.x * 2, (int)canvasSize.y + 1);
            render = Generate2DArray(canvasSize);


            Console.ForegroundColor = ConsoleColor.DarkGreen;
            float spf = 1 / targetFPS;

            //Render loop
            while (true)
            {
                var watch = Stopwatch.StartNew();

                time += (deltaTime / 1000);
                ClearRender();

                ProcessRender();

                Render();

                watch.Stop();

                float timeRest = (1 / targetFPS) * 1000 - watch.ElapsedMilliseconds;
                deltaTime = MathF.Max(watch.ElapsedMilliseconds, (1 / targetFPS) * 1000);

                float smoothing = 0.9f; // larger=more smoothing
                spf = (spf * smoothing) + ((deltaTime / 1000) * (1.0f - smoothing));

                string deltaTimeString = "", fpsString = "";
                if (deltaTime < 10)
                    deltaTimeString += "0";
                deltaTimeString += MathF.Round(deltaTime);
                if (1 / spf < 10)
                    fpsString += "0";
                fpsString += MathF.Round(1 / spf);

                Console.Write($"Render time: {deltaTimeString}ms, FPS: {fpsString} ");
                Thread.Sleep(Math.Clamp((int)timeRest, 0, 1000));
            }
        }
    }

    class Vector(float x, float y)
    {
        public float x = x;
        public float y = y;

        public static readonly Vector one = new(1, 1);
        public static readonly Vector zero = new(0, 0);
        public static readonly Vector up = new(0, 1);
        public static readonly Vector down = new(0, -1);
        public static readonly Vector left = new(-1, 0);
        public static readonly Vector right = new(1, 0);

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y);
        }
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y);
        }
        public static Vector operator *(Vector a, float b)
        {
            return new Vector(a.x * b, a.y * b);
        }
        public static Vector operator /(Vector a, float b)
        {
            return new Vector(a.x / b, a.y / b);
        }

    }
}
