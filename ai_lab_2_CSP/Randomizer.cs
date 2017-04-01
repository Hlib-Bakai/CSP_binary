using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_lab_2_CSP
{
    class Randomizer
    {
        static public void Randomize(out int[,] arr, int size)
        {
            arr = new int[size, size];
            Random r = new Random();
            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    arr[row, col] = r.Next(0, 3) - 1;
                }
            }
        }
    }
}
