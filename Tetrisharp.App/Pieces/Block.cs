//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Tetrisharp.App.Pieces
{
    public class Block
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public Block(int x, int y)
        {
            XPosition = x;
            YPosition = y;
        }

        public void SetPosition(int x, int y)
        {
            XPosition = x;
            YPosition = y;
        }

        public void Move(int x, int y)
        {
            XPosition += x;
            YPosition += y;
        }

        public int Value()
        {
            return XPosition + (YPosition*10);
        }

        public override string ToString()
        {
            return $"({XPosition},{YPosition})";
        }
    }
}
