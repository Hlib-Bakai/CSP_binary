﻿using System;
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
        static Brush[] boardColor = { Brushes.LightGray, Brushes.Orange, Brushes.White };
        static int pixelSize = 60;


        static public void FillBoard(Graphics g, int[,] arr)
        {
            int size = (int)Math.Sqrt(arr.Length);
            int boardX = 350 - (size * pixelSize) / 2;
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
                g.DrawLine(new Pen(borderColor), boardX, boardY + i * pixelSize, boardX + boardWidth, boardY + i * pixelSize);
                g.DrawLine(new Pen(borderColor), boardX + i * pixelSize, boardY, boardX + i * pixelSize, boardY + boardHeight);
            }
            //draw border
            g.DrawRectangle(new Pen(borderColor, 1), new Rectangle(boardX, boardY, boardWidth, boardHeight));

            //fill numbers
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