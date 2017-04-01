using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_lab_2_CSP
{
    class CSP_Solver
    {
        public int[,] arr;
        private int size;

        public CSP_Solver(int[,] garr)
        {
            arr = garr;
            size = (int)Math.Sqrt(arr.Length);
        }

        public bool solveBackTracking(ref int[,] garr)
        {
            if (solver(ref garr))
            {
                return true;
            }
            return false;
        }

        public bool solver(ref int[,] arr)
        {
            int curr = -1;
            int curc = -1;
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (arr[row, col] == -1)
                    {
                        curr = row;
                        curc = col;
                        break;
                    }
                }
                if (curr != -1)
                    break;
            }

            if (curr == -1)
            {
                if (checkConstaints(arr) == 1)
                {
                    return true;
                }
                return false;
            }

            //try 0
            arr[curr, curc] = 0;
            if (checkConstaints(arr) != -1)
            {
                if (solver(ref arr) == true)
                    return true;
            }

            //try 1
            arr[curr, curc] = 1;
            if (checkConstaints(arr) != -1)
            {
                if (solver(ref arr) == true)
                    return true;
            }

            arr[curr, curc] = -1;
            return false;
        }


        private int checkConstaints(int[,] arr)
        {
            bool hasEmpty = false;

            //Check numbers of 0's and 1's
            //rows
            for (int col = 0; col < size; col++)
            {
                int rowS = 0;
                bool skip = false;
                for (int row = 0; row < size; row++)
                {
                    int val = arr[col, row];
                    if (val == -1)
                    {
                        hasEmpty = true;
                        skip = true;
                        break;
                    }
                    if (val == 0)
                        rowS--;
                    else
                        rowS++;
                }
                if (!skip)
                {
                    if (rowS != 0)
                        return -1;
                }
            }

            //columns
            for (int row = 0; row < size; row++)
            {
                int colS = 0;
                bool skip = false;
                for (int col = 0; col < size; col++)
                {
                    int val = arr[col, row];
                    if (val == -1)
                    {
                        hasEmpty = true;
                        skip = true;
                        break;
                    }
                    if (val == 0)
                        colS--;
                    else
                        colS++;
                }
                if (!skip)
                {
                    if (colS != 0)
                        return -1;
                }
            }


            //Check symbols not occuring more than twice in a row
            //rows
            for (int col = 0; col < size; col++)
            {
                int rowS = 0;
                for (int row = 0; row < size; row++)
                {
                    int val = arr[col, row];
                    if (val == -1)
                    {
                        hasEmpty = true;
                        rowS = 0;
                    }
                    else if (val == 0)
                    {
                        if (rowS > 0)
                        {
                            rowS = -1;
                        }
                        else
                        {
                            rowS--;
                        }
                    }
                    else
                    {
                        if (rowS < 0)
                        {
                            rowS = 1;
                        }
                        else
                        {
                            rowS++;
                        }
                    }
                    if (Math.Abs(rowS) > 2)
                    {
                        return -1;
                    }
                }
            }

            //columns
            for (int row = 0; row < size; row++)
            {
                int colS = 0;
                for (int col = 0; col < size; col++)
                {
                    int val = arr[col, row];
                    if (val == -1)
                    {
                        hasEmpty = true;
                        colS = 0;
                    }
                    else if (val == 0)
                    {
                        if (colS > 0)
                        {
                            colS = -1;
                        }
                        else
                        {
                            colS--;
                        }
                    }
                    else
                    {
                        if (colS < 0)
                        {
                            colS = 1;
                        }
                        else
                        {
                            colS++;
                        }
                    }
                    if (Math.Abs(colS) > 2)
                    {
                        return -1;
                    }
                }
            }


            //Check that each row and column is unique 
            List<bool> hasDiff = new List<bool>();

            //rows
            for (int row = 0; row < size; row++)
            {
                hasDiff.Clear();
                bool skip = false;
                for (int i = 0; i < size; i++)
                    hasDiff.Add(false);
                for (int i = 0; i <= row; i++)
                {
                    hasDiff[i] = true;
                }
                for (int col = 0; col < size; col++)
                {
                    if (arr[row, col] == -1)
                    {
                        skip = true;
                        break;
                    }
                    for (int check = row + 1; check < size; check++)
                    {
                        if (arr[row, check] == -1)
                        {
                            hasDiff[check] = true;
                        }
                        if (arr[row, col] != arr[check, col])
                        {
                            hasDiff[check] = true;
                        }
                    }
                }
                if (hasDiff.Contains(false) && !skip)
                {
                    return -1;
                }
            }

            //columns
            for (int col = 0; col < size; col++)
            {
                hasDiff.Clear();
                bool skip = false;
                for (int i = 0; i < size; i++)
                    hasDiff.Add(false);
                for (int i = 0; i <= col; i++)
                {
                    hasDiff[i] = true;
                }

                for (int row = 0; row < size; row++)
                {
                    if (arr[row, col] == -1)
                    {
                        skip = true;
                        break;
                    }
                    for (int check = col + 1; check < size; check++)
                    {
                        if (arr[row, check] == -1)
                        {
                            hasDiff[check] = true;
                        }
                        if (arr[row, col] != arr[row, check])
                        {
                            hasDiff[check] = true;
                        }
                    }
                }
                if (hasDiff.Contains(false) && !skip)
                {
                    return -1;
                }
            }


            if (hasEmpty)
                return 0;

            return 1;
        }
    }
}
