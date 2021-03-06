﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ai_lab_2_CSP
{
    class CSP_Solver
    {
        private int size;
        private int colors;
        private List<List<int>> domains;
        private List<List<int>> biDomains;

        public CSP_Solver(int[,] garr)
        {
            size = (int)Math.Sqrt(garr.Length);
            colors = (size % 2 == 0) ? (2 * size) : (2 * size + 1);
            domains = new List<List<int>>();
            for (int i = 0; i < garr.Length; i++)
            {
                domains.Add(new List<int>());
            }
            for (int i = 0; i < garr.Length; i++)
            {
                domains[i] = resetDomain();
            }

            biDomains = new List<List<int>>();
            for (int i = 0; i < garr.Length; i++)
            {
                biDomains.Add(new List<int>());
            }
            for (int i = 0; i < garr.Length; i++)
            {
                biDomains[i].Add(0);
                biDomains[i].Add(1);
            }
        }

        private List<int> resetDomain()
        {
            List<int> res = new List<int>();
            for (int i = 0; i < colors; i++)
            {
                res.Add(i);
            }
            return res;
        }

        public bool solveGraphBT(ref int[,] arr)
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
                if (checkConstaintsGraph(arr) == 1)
                {
                    return true;
                }
                return false;
            }

            for (int i = 0; i < colors; i++)
            {
                arr[curr, curc] = i;
                if (checkConstaintsGraph(arr) != -1)
                {
                    if (solveGraphBT(ref arr) == true)
                        return true;
                }
            }
            arr[curr, curc] = -1;
            return false;
        }

        public bool solveForwardCheckingGraph(ref int[,] arr)
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
                return true;

            for (int i = 0; i < domains[curr * size + curc].Count; i++)
            {
                arr[curr, curc] = domains[curr * size + curc][i];
                List<List<int>> listOfOldDoms = new List<List<int>>();
                copyArray(ref domains, ref listOfOldDoms);
                bool hasNullDomain = false;

                hasNullDomain = checkDomains(curr, curc, ref domains, arr);

                if (solveForwardCheckingGraph(ref arr) == true && !hasNullDomain)
                    return true;
                copyArray(ref listOfOldDoms, ref domains);
            }
            arr[curr, curc] = -1;
            return false;
        }

        private bool checkDomains(int row, int col, ref List<List<int>> domains, int[,] arr)
        {         
            if (col != size - 1)
            {
                var dom = domains[row * size + (col + 1)];
                for (int idxInDomain = 0; idxInDomain < dom.Count; idxInDomain++)
                {
                    int numb = dom[idxInDomain];
                    if (checkConstraintsGFC(arr, row, col+1, numb) == -1)
                    {
                        dom[idxInDomain] = -1;
                    }
                }
                dom.RemoveAll(x => (x == -1));
                if (dom.Count == 0)
                {
                    return false;
                }
            }
            if (row != size - 1)
            {
                var dom = domains[(row + 1) * size + col];
                for (int idxInDomain = 0; idxInDomain < dom.Count; idxInDomain++)
                {
                    int numb = dom[idxInDomain];
                    if (checkConstraintsGFC(arr, row + 1, col, numb) == -1)
                    {
                        dom[idxInDomain] = -1;
                    }
                }
                dom.RemoveAll(x => (x == -1));
                if (dom.Count == 0)
                {
                    return false;
                }
            }
            if (col != 0 && row != size - 1)
            {
                var dom = domains[(row + 1) * size + (col - 1)];
                for (int idxInDomain = 0; idxInDomain < dom.Count; idxInDomain++)
                {
                    int numb = dom[idxInDomain];
                    if (checkConstraintsGFC(arr, row + 1, col - 1, numb) == -1)
                    {
                        dom[idxInDomain] = -1;
                    }
                }
                dom.RemoveAll(x => (x == -1));
                if (dom.Count == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool checkBiDomains(int row, int col, ref List<List<int>> domains, int[,] arr)
        {
            bool hasNullDomain = false;
            for (int idxOfCell = row * size + col + 1; idxOfCell < domains.Count; idxOfCell++)
            {
                var dom = biDomains[idxOfCell];
                int nextr = idxOfCell / size;
                int nextc = idxOfCell % size;
                if (arr[nextr, nextc] != -1)
                    continue;
                for (int idxInDomain = 0; idxInDomain < dom.Count; idxInDomain++)
                {
                    int numb = dom[idxInDomain];
                    if (checkConstraintsBFC(arr, nextr, nextc, numb) == -1)
                    {
                        dom[idxInDomain] = -1;
                    }
                }
                dom.RemoveAll(x => (x == -1));
                if (dom.Count == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void copyArray(ref List<List<int>> from, ref List<List<int>> to)
        {
            to.Clear();
            for (int t = 0; t < from.Count; t++)
            {
                to.Add(new List<int>());
                for (int t2 = 0; t2 < from[t].Count; t2++)
                {
                    to[t].Add(from[t][t2]);
                }
            }
        }

        public bool solveBackTracking(ref int[,] arr, int idx)
        {
            if (idx == arr.Length)
                if (checkConstaints(arr) == 1)
                    return true;
            if (idx == -1)
                return false;

            int curr = idx / size;
            int curc = idx % size;

            if (arr[curr, curc] != -1)
                return solveBackTracking(ref arr, idx + 1);



            //try 0
            arr[curr, curc] = 0;
            if (checkConstaints(arr) != -1)
            {
                if (solveBackTracking(ref arr, idx + 1) == true)
                    return true;
            }

            //try 1
            arr[curr, curc] = 1;
            if (checkConstaints(arr) != -1)
            {
                if (solveBackTracking(ref arr, idx + 1) == true)
                    return true;
            }

            arr[curr, curc] = -1;
            return false;
        }

        public bool solveForwardChecking(ref int[,] arr, int idx)
        {
            if (idx == arr.Length)
                return true;
            if (idx == -1)
                return false;

            int curr = idx / size;
            int curc = idx % size;

            if (arr[curr, curc] != -1)
                return solveForwardChecking(ref arr, idx + 1);

            for (int i = 0; i < biDomains[idx].Count; i++)
            {
                arr[curr, curc] = biDomains[curr * size + curc][i];
                List<List<int>> listOfOldDoms = new List<List<int>>();
                copyArray(ref biDomains, ref listOfOldDoms);
                bool hasNullDomain = false;

                hasNullDomain = !checkBiDomains(curr, curc, ref biDomains, arr);

                //Debug.WriteLine("Current: " + curr * size + curc + " with value: " + arr[curr,curc]);
                //Debug.WriteLine("Domains:");
                //for (int j = 0; j < biDomains.Count; j++)
                //{
                //    Debug.Write("Object " + j + ": ");
                //    foreach(int a in biDomains[j])
                //    {
                //        Debug.Write(a + ", ");
                //    }
                //    Debug.WriteLine("");
                //}


                if (solveForwardChecking(ref arr, idx+1) == true && !hasNullDomain)
                    return true;

                copyArray(ref listOfOldDoms, ref biDomains);
            }
            arr[curr, curc] = -1;
            return false;
        }

        private int checkConstaintsGraph(int[,] arr)
        {
            List<List<int>> pairs = new List<List<int>>();
            pairs.Clear();
            for (int i = 0; i < colors; i++)
            {
                pairs.Add(new List<int>());
            }

            bool hasEmpty = false;
            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    int val = arr[col, row];
                    if (val == -1)
                    {
                        hasEmpty = true;
                        break;
                    }
                    if (col != 0)
                    {
                        int nval = arr[col - 1, row];
                        if (nval == -1)
                        {
                            hasEmpty = true;
                        }
                        else if (nval == val)
                            return -1;
                    }
                    if (col != size - 1)
                    {
                        int nval = arr[col + 1, row];
                        if (nval == -1)
                        {
                            hasEmpty = true;
                        }
                        else if (nval == val)
                            return -1;
                    }
                    if (row != 0)
                    {
                        int nval = arr[col, row - 1];
                        if (nval == -1)
                        {
                            hasEmpty = true;
                        }
                        else if (nval == val)
                            return -1;
                    }
                    if (row != size - 1)
                    {
                        int nval = arr[col, row + 1];
                        if (nval == -1)
                        {
                            hasEmpty = true;
                        }
                        else if (nval == val)
                            return -1;
                    }

                }
            }

            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    int val = arr[col, row];
                    if (val == -1)
                    {
                        break;
                    }
                    if (col != 0)
                    {
                        int nval = arr[col - 1, row];
                        if (nval == -1)
                        {
                            hasEmpty = true;
                        }
                        else
                        {
                            pairs[val].Add(nval);
                            pairs[nval].Add(val);
                        }
                    }
                    if (col != size - 1)
                    {
                        int nval = arr[col + 1, row];
                        if (nval == -1)
                        {
                            hasEmpty = true;
                        }
                        else
                        {
                            pairs[val].Add(nval);
                            pairs[nval].Add(val);
                        }
                    }
                    if (row != 0)
                    {
                        int nval = arr[col, row - 1];
                        if (nval == -1)
                        {
                            hasEmpty = true;
                        }
                        else
                        {
                            pairs[val].Add(nval);
                            pairs[nval].Add(val);
                        }
                    }
                    if (row != size - 1)
                    {
                        int nval = arr[col, row + 1];
                        if (nval == -1)
                        {
                            hasEmpty = true;
                        }
                        else
                        {
                            pairs[val].Add(nval);
                            pairs[nval].Add(val);
                        }
                    }

                    foreach (List<int> a in pairs)
                    {
                        if (a.Count() < 2)
                        {
                            continue;
                        }

                        List<int> listTemp = new List<int>();
                        listTemp.Clear();
                        for (int i = 0; i < colors; i++)
                        {
                            listTemp.Add(0);
                        }

                        foreach (int tmp in a)
                        {
                            listTemp[tmp]++;
                        }

                        if (listTemp.Max() > 2)
                            return -1;
                    }

                }
            }



            if (hasEmpty)
                return 0;

            return 1;
        }

        private int checkConstraintsGFC(int[,] arr, int row, int col, int val)
        {
            arr[row, col] = val;
            int res = checkConstaintsGraph(arr);
            arr[row, col] = -1;
            return res;
        }

        private int checkConstraintsBFC(int[,] arr, int row, int col, int val)
        {
            arr[row, col] = val;
            int res = checkConstaints(arr);
            arr[row, col] = -1;
            return res;
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

                    if (Math.Abs(rowS) > size)
                        return -1;
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

                    if (Math.Abs(colS) > size)
                        return -1;
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
