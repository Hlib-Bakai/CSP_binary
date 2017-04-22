using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_lab_2_CSP
{
    class Drawer
    {
        static Brush borderColor = Brushes.Black;
        static Brush[] boardColor = {
            Brushes.White,
            Brushes.MediumVioletRed,
            Brushes.Orange,
            Brushes.LightBlue,
            Brushes.Brown,
            Brushes.LightCoral,
            Brushes.LightSeaGreen,
            Brushes.Lime,
            Brushes.Goldenrod,
            Brushes.Fuchsia,
            Brushes.MediumPurple,
            Brushes.Khaki,
            Brushes.SpringGreen,
            Brushes.Olive,
            Brushes.PaleTurquoise,
            Brushes.Peru,
            Brushes.Red,
            Brushes.SandyBrown,
            Brushes.CornflowerBlue};

        static public void FillBoardGraph(Graphics g, int[,] arr)
        {
            var myPen = new Pen(borderColor);
            int size = (int)Math.Sqrt(arr.Length);
            int colors = (size % 2 == 0) ? (2 * size) : (2 * size + 1);
            int pixelSize = 60;
            int boardX = 350 - (size * pixelSize) / 2;
            if (boardX < 10)
                boardX = 10;
            int boardY = 40;
            int boardWidth = size * pixelSize;
            int boardHeight = size * pixelSize;

            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    int value = arr[row, col];
                    if (value == -1)
                        g.FillEllipse(boardColor[0], boardX + col * pixelSize, boardY + row * pixelSize, pixelSize, pixelSize);
                    else
                        g.FillEllipse(boardColor[1 + value], boardX + col * pixelSize, boardY + row * pixelSize, pixelSize, pixelSize);
                }
            }

            for (int i = 0; i < size; i++)
            {
                //draw lines
                g.DrawLine(myPen, boardX, boardY + i * pixelSize, boardX + boardWidth, boardY + i * pixelSize);
                g.DrawLine(myPen, boardX + i * pixelSize, boardY, boardX + i * pixelSize, boardY + boardHeight);
            }
            //draw border
            g.DrawRectangle(myPen, new Rectangle(boardX, boardY, boardWidth, boardHeight));

            Font myFont = new Font("Arial", 18);
            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    int value = arr[row, col];
                    if (value != -1)
                        g.DrawString(value.ToString(), myFont, Brushes.Black, new Point(boardX + col * pixelSize + pixelSize / 3, boardY + row * pixelSize + pixelSize / 3));
                }
            }
        }

        static public void FillBoard(Graphics g, int[,] arr)
        {
            var myPen = new Pen(borderColor);
            int size = (int)Math.Sqrt(arr.Length);
            int pixelSize = (60 - ((size / 10) * 10) + ((size / 50) * 3));
            int boardX = 350 - (size * pixelSize) / 2;
            if (boardX < 10)
                boardX = 10;
            int boardY = 40;
            int boardWidth = size * pixelSize;
            int boardHeight = size * pixelSize;


            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    int value = arr[row, col];
                    if (value == -1)
                        g.FillRectangle(boardColor[2], boardX + col * pixelSize, boardY + row * pixelSize, pixelSize, pixelSize);
                    else
                        g.FillRectangle(boardColor[value], boardX + col * pixelSize, boardY + row * pixelSize, pixelSize, pixelSize);
                }
            }

            for (int i = 0; i < size; i++)
            {
                //draw lines
                g.DrawLine(myPen, boardX, boardY + i * pixelSize, boardX + boardWidth, boardY + i * pixelSize);
                g.DrawLine(myPen, boardX + i * pixelSize, boardY, boardX + i * pixelSize, boardY + boardHeight);
            }
            //draw border
            g.DrawRectangle(myPen, new Rectangle(boardX, boardY, boardWidth, boardHeight));

            //fill numbers
            if (size > 15)
                return;
            Font myFont = new Font("Arial", 18);
            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    int value = arr[row, col];
                    if (value != -1)
                        g.DrawString(value.ToString(), myFont, Brushes.Black, new Point(boardX + col * pixelSize + pixelSize / 3, boardY + row * pixelSize + pixelSize / 3));
                }
            }
        }
    }
}
