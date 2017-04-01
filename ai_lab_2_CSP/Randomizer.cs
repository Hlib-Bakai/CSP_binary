using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_lab_2_CSP
{
    class Randomizer
    {
        static public void Randomize(out int[,] arr, int size, int M)
        {
            arr = new int[size, size];
            Random r = new Random();
            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    arr[row, col] = -1;
                }
            }
            for (int i = 0; i < M; )
            {
                int col = r.Next(0, size);
                int row = r.Next(0, size);
                if (arr[row, col] == -1)
                {
                    i++;
                    arr[row, col] = r.Next(0, 2);
                }
            }


        }
    }
}
