using System;
using System.Collections.Generic;
using System.Threading;
using Parcs;

namespace Queens
{
    class QueensIModule : IModule
    {
        private static bool Safe(int new_row, int new_col, List<int> sol)
        {
            for (int r = 0; r < sol.Count; r++)
            {
                if ((sol[r] == new_col) ||
                    (sol[r] + r == new_col + new_row) ||
                    (sol[r] - r == new_col - new_row))
                {
                    return false;
                }
            }
            return true;
        }

        private static int CountQ(int n_row, int width, List<int> sol)
        {
            int all_res = 0;
            if (sol.Count == width)
            {
                all_res += 1;
            }
            else
            {
                for (int n_col = 0; n_col < width; n_col++)
                {
                    // Console.WriteLine(n_row.ToString() + " " + n_col.ToString() + "  " + String.Join(", ", sol));
                    if (Safe(n_row, n_col, sol))
                    {
                        // Console.WriteLine(n_row.ToString() + " " + n_col.ToString());
                        List<int> new_list = new List<int>(sol);
                        new_list.Add(n_col);
                        all_res += CountQ(n_row + 1, width, new_list);
                    }
                }
            }
            return all_res;
        }

        public void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            int a = info.Parent.ReadInt();
            int b = info.Parent.ReadInt();
            int n = info.Parent.ReadInt();

            Console.WriteLine(a.ToString() + " " + b.ToString());

            int res = 0;
            for (int i = a; i < b; i++)
            {
                List<int> list = new List<int>();
                list.Add(i);
                // Console.WriteLine(a.ToString() + " " + b.ToString() + "  " + String.Join(", ", list));
                res += CountQ(1, n, list);
            }
            info.Parent.WriteData(res);
        }
    }
}
