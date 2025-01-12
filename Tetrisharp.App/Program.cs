//using ConsoleRenderer;
//using System.Diagnostics;
//using System.Diagnostics.Metrics;
//using System.Drawing;
using System.Runtime.Versioning;
using Tetrisharp.App.Pieces;

namespace Tetrisharp.App
{
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    internal class Program
    {
        private static Tetramino[] _pieceBox = new Tetramino[7];
        private static Tetramino piece;
        private static Field field;
        private static bool haveColision;
        private static Info info;

        static void Main(string[] args)
        {
            field = new Field();
            info = new Info();

            do
            {
                haveColision = false;
                piece = InitThePieces();
                
                do
                {
                    var delay = Task.Delay(500);
                    var detectarTecla = DetectarTecla();

                    Render();
                    detectarTecla.Wait();

                    GlobalGravity();
                    delay.Wait();                    

                } while (!haveColision);

                VerificarLinhasCompletas();

            } while (!field.OverFlow()) ;

            GameOverScreen();
        }

        private static void VerificarLinhasCompletas()
        {
            for (int i = 4; i <= 23; i++)
            {
                if (field.IsLineComplete(i))
                {
                    info.Lines++;
                    field.RemoveLine(i);
                    field.FallingDown(i);
                }
            }
        }

        private static void Render()
        {
            Console.Clear();
            var consoleGraphics = new ConsoleGraphics(field);
            consoleGraphics.AddBlocks(piece);
            consoleGraphics.Draw(info);
        }

        private static void GameOverScreen()
        {
            Console.WriteLine("GAME OVER");
            Console.ReadKey();
        }

        private static void GlobalGravity()
        {
            if (field.PredicColision(piece))
            {
                haveColision = true;
                Console.Beep();
                field.WeldTheBlocks(piece);
            }
            else
            {
                piece.Gravity();
            }
        }

        private static async Task DetectarTecla()
        {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                    if (keyInfo.Key == ConsoleKey.UpArrow)
                    {
                        if (field.CantSpin(piece))
                        {
                            Console.Beep();
                        }
                        else piece.ClockWiseSpin();
                    }

                    if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        if (field.HaveEdgesOnRight(piece))
                        {
                            Console.Beep();
                        }
                        else piece.MoveRight();
                    }

                    if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        if (field.HaveEdgesOnLeft(piece))
                        {
                            Console.Beep();
                        }
                        else piece.MoveLeft();
                    }

                    if (keyInfo.Key == ConsoleKey.DownArrow)
                    {
                        Console.Beep();
                        //piece.Gravity();
                        //piece.Gravity();
                    }

                    if (keyInfo.Key == ConsoleKey.Spacebar)
                    {
                        Console.ReadKey();
                    }
                }
                else Console.WriteLine($" ");
        }

        private static Tetramino InitThePieces()
        {
            //por convenção os indices da matriz é baseada no quarto quadrante do plano cartesiano
            //      0 1 2 3 4 5 6 7 8 9
            // 0  |_|_|_|_|_|_|_|_|_|_|
            // 1  |_|_|_|_|_|_|_|_|_|_|
            // 2  |_|_|_|_|_|_|_|_|_|_|
            // 3  |_|_|_|_|_|_|_|_|_|_|
            // ... 
            // 19 |_|_|_|_|_|_|_|_|_|_|

            var hero = new Tetramino(3, 3, 4, 3, 5, 3, 6, 3);
            //
            //  |_|_|_|_|
            //
            var teewee = new Tetramino(4, 2, 3, 3, 4, 3, 5, 3);
            //    |_|
            //  |_|_|_|
            //  
            var smashBoy = new Tetramino(4, 2, 5, 2, 4, 3, 5, 3);
            //    |_|_|
            //    |_|_|
            //    
            var blueRick = new Tetramino(3, 0, 3, 1, 4, 1, 5, 1);
            //  |_|
            //  |_|_|_|
            //
            var orangeRick = new Tetramino(0, 5, 1, 3, 1, 4, 1, 5);
            //      |_|
            //  |_|_|_|
            //
            var clevelandZ = new Tetramino(0, 3, 0, 4, 1, 4, 1, 5);
            //  |_|_|        |_|
            //    |_|_|    |_|_|
            //             |_|
            var RhodeIslandZ = new Tetramino(0, 4, 0, 5, 1, 3, 1, 4);
            //    |_|_|
            //  |_|_|
            //

            var heroSpined = new Tetramino(5, 0, 5, 1, 5, 2, 5, 3);
            var clevelandZSpined = new Tetramino(0, 3, 0, 4, 1, 4, 1, 5);


            _pieceBox[0] = heroSpined;
            _pieceBox[1] = teewee;
            _pieceBox[2] = smashBoy;
            _pieceBox[3] = blueRick;
            _pieceBox[4] = orangeRick;
            _pieceBox[5] = clevelandZ;
            _pieceBox[6] = RhodeIslandZ;

            var random = new Random();
            return new Tetramino(_pieceBox[random.Next(7)]);
        }

    }
}
