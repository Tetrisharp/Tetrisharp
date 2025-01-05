//using System;
using System.Text;
using Tetrisharp.App.Pieces;

namespace Tetrisharp.App
{
    internal class ConsoleGraphics
    {
        List<Block> blocks;

        public ConsoleGraphics(Field field)
        {
            blocks = new List<Block>();
            blocks.AddRange(field.blocks);
        }

        public void AddBlocks(Tetramino piece)
        {
            blocks.AddRange(piece.blocks);
        }

        public void Draw(Info info)
        {
            this.blocks.Sort((a, b) => a.Value().CompareTo(b.Value()));

            StringBuilder builder = new StringBuilder();

            //•	╔ (canto superior esquerdo) -Código ASCII: 201
            //•	═ (linha horizontal dupla) -Código ASCII: 205
            //•	╗ (canto superior direito) -Código ASCII: 187
            //•	║ (linha vertical dupla) -Código ASCII: 186
            //•	╚ (canto inferior esquerdo) -Código ASCII: 200
            //•	╝ (canto inferior direito) -Código ASCII: 188
            //•	╠ (junção esquerda) -Código ASCII: 204
            //•	╣ (junção direita) -Código ASCII: 185
            //•	╦ (junção superior) -Código ASCII: 203
            //•	╩ (junção inferior) -Código ASCII: 202
            //•	╬ (junção central) -Código ASCII: 206

            builder.Append($"\tlines:{info.Lines}\r\n");
            builder.Append("╔══════════════════════╗\r\n");

            for (int y = 4; y <= 23; y++)
            {
                builder.Append("║ ");//
                for (int x = 0; x <= 9; x++)
                {
                    if (this.blocks.Any(_ => _.XPosition == x && _.YPosition == y))
                    {
                        //builder.Append("██");//219
                        //builder.Append("▓▓");//178
                        builder.Append("▒▒");//177
                                             //builder.Append("░░");//176
                    }
                    else
                    {
                        builder.Append("  ");
                    }
                }
                builder.Append(" ║\r\n");
            }

            builder.Append("╚══════════════════════╝\r\n");

            Console.WriteLine(builder.ToString());

            //return $"     0 1 2 3 4 5 6 7 8 9 \r\n" +
            //       $" 00                      \r\n" +
            //       $"-01                      \r\n" +
            //       $"-02                      \r\n" +
            //       $"-03                      \r\n" +
            //       $"-04 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-05 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-06 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-07 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-08 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-09 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-10 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-11 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-12 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-13 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-14 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-15 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-16 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-17 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-18 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-19 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-20 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-21 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-22 |_|_|_|_|_|_|_|_|_|_|\r\n" +
            //       $"-23 |_|_|_|_|_|_|_|_|_|_|\r\n";

        }
    }
}