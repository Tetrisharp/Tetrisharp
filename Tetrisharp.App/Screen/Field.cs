//using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Tetrisharp.App.Pieces
{
    public class Field
    {
        public List<Block> blocks = new List<Block>();

        public void WeldTheBlocks(Tetramino piece)
        {
            foreach (var block in piece.blocks)
            {
                blocks.Add(block);
            }
        }

        public bool OverFlow()
        {
            foreach (var block in this.blocks)
            {
                if (block.YPosition==4) return true; //top or edge
            }
            return false;
        }

        public bool HaveEdgesOnRight(Tetramino piece)
        {
            var target = new Tetramino(piece);
            target.MoveRight();

            foreach (var tblock in target.blocks)
            {
                if (tblock.XPosition > 9) return true;

                foreach (var block in this.blocks)
                {
                    if (tblock.XPosition == block.XPosition && tblock.YPosition == block.YPosition) return true;
                }
            }
            return false;
        }

        public bool CantSpin(Tetramino piece)
        {
            var target = new Tetramino(piece);
            target.ClockWiseSpin();

            foreach (var tblock in target.blocks)
            {
                if (tblock.XPosition < 0 || tblock.XPosition > 9 || tblock.YPosition > 23) return true;

                foreach (var block in this.blocks)
                {
                    if (tblock.XPosition == block.XPosition && tblock.YPosition == block.YPosition) return true;
                }
            }
            return false;
        }

        public bool HaveEdgesOnLeft(Tetramino piece)
        {
            var target = new Tetramino(piece);
            target.MoveLeft();

            foreach (var tblock in target.blocks)
            {
                if (tblock.XPosition < 0) return true;

                foreach (var block in this.blocks)
                {
                    if (tblock.XPosition == block.XPosition && tblock.YPosition == block.YPosition) return true;
                }
            }
            return false;
        }

        public bool PredicColision(Tetramino piece)
        {
            var target = new Tetramino( piece);
            target.Gravity();

            foreach (var tblock in target.blocks)
            {
                if (tblock.YPosition > 23) return true; //floor

                foreach (var block in this.blocks)
                {
                    if (tblock.XPosition == block.XPosition && tblock.YPosition == block.YPosition) return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            this.blocks.Sort((a,b)=> a.Value().CompareTo(b.Value()));

            StringBuilder builder = new StringBuilder();

            foreach(var block in blocks)
            {
                builder.Append(block.ToString());
            }

            return builder.ToString();
        }

        public bool IsLineComplete(int i)
        {
            int count = 0;
            foreach(var block in blocks)
            {
                if (block.YPosition == i) count++;
            }
            return count == 10;
        }

        public void RemoveLine(int i)
        {
            blocks.RemoveAll(_ => _.YPosition==i);
        }

        internal void FallingDown(int i)
        {
            foreach (var block in blocks)
            {
                if (block.YPosition < i) block.Move(0,1);
            }
        }
    }
}
