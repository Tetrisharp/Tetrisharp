//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Reflection.Metadata.BlobBuilder;

namespace Tetrisharp.App.Pieces
{
    public class Tetramino
    {
        public Block[] blocks;

        public Tetramino(Tetramino newPiece)
        {
            blocks = new Block[4];

            blocks[0] = new Block(newPiece.blocks[0].XPosition, newPiece.blocks[0].YPosition);
            blocks[1] = new Block(newPiece.blocks[1].XPosition, newPiece.blocks[1].YPosition);
            blocks[2] = new Block(newPiece.blocks[2].XPosition, newPiece.blocks[2].YPosition);
            blocks[3] = new Block(newPiece.blocks[3].XPosition, newPiece.blocks[3].YPosition);
        }

        public Tetramino(int ax, int ay, int bx, int by, int cx, int cy, int dx, int dy)
        {
            blocks = new Block[4];
            blocks[0] = new Block(ax, ay);
            blocks[1] = new Block(bx, by);
            blocks[2] = new Block(cx, cy);
            blocks[3] = new Block(dx, dy);
        }

        public void Gravity() //MoveDown
        {
            foreach (var block in blocks)
            {
                block.YPosition++;
            }
        }

        public void MoveLeft()
        {
            foreach (var block in blocks)
            {
                block.XPosition--;
            }
        }

        public void MoveRight()
        {
            foreach (var block in blocks)
            {
                block.XPosition++;
            }
        }

        public void ClockWiseSpin()
        {
            var minX = blocks.Min(_ => _.XPosition);
            var maxX = blocks.Max(_ => _.XPosition);
            
            var minY = blocks.Min(_ => _.YPosition);
            var maxY = blocks.Max(_ => _.YPosition);

            double axisX = (maxX + minX) / 2;
            double axisY = (maxY + minY) / 2;

            foreach (var block in blocks)
            {
                var oldX = block.XPosition;
                var oldY = block.YPosition;

                block.XPosition = (int)(axisX - axisY + oldY);
                block.YPosition = (int)(axisX + axisY - oldX);
            }
        }

        public override string ToString()
        {
            return $"{{ ({blocks[0].XPosition},{blocks[0].YPosition})"
                   + $" ({blocks[1].XPosition},{blocks[1].YPosition})"
                   + $" ({blocks[2].XPosition},{blocks[2].YPosition})"
                   + $" ({blocks[3].XPosition},{blocks[3].YPosition}) }}";
        }
    }
}
