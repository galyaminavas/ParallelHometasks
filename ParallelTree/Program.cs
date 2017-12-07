using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BSTParallel
{
    class Program
    {
        static void Main(string[] args)
        {
            var treeSeq = new Tree();
            Stopwatch swSeq = new Stopwatch();
            var treePar = new Tree();
            Stopwatch swPar = new Stopwatch();

            Random rnd = new Random();
            int[] arrInsert = new int[10000000];
            for (int i = 0; i < arrInsert.Length; i++)
            {
                arrInsert[i] = rnd.Next(1, 5000000);
            }
            int[] arrSearch = new int[1000000];
            for (int i = 0; i < arrSearch.Length; i++)
            {
                arrSearch[i] = rnd.Next(1, 5000000);
            }
            int[] arrRemove = new int[1000000];
            for (int i = 0; i < arrSearch.Length; i++)
            {
                arrRemove[i] = rnd.Next(1, 5000000);
            }


            swSeq.Start();
            for (int i = 0; i < arrInsert.Length; i++)
            {
                treeSeq.Insert(arrInsert[i], arrInsert[i]);
            }
            swSeq.Stop();
            Console.WriteLine("Sequential runtime for inserting: " + swSeq.Elapsed.ToString());

            swSeq.Restart();
            for (int i = 0; i < arrSearch.Length; i++)
            {
                treeSeq.Search(arrSearch[i]);
            }
            swSeq.Stop();
            Console.WriteLine("Sequential runtime for searching: " + swSeq.Elapsed.ToString());

            swSeq.Restart();
            for (int i = 0; i < arrRemove.Length; i++)
            {
                treeSeq.Remove(arrRemove[i]);
            }
            swSeq.Stop();
            Console.WriteLine("Sequential runtime for removing: " + swSeq.Elapsed.ToString());

            if (treeSeq.root == null)
                Console.WriteLine("Tree is empty");

            Console.WriteLine();


            swPar.Start();
            Parallel.ForEach(arrInsert, k =>
            {
                int j = k;
                treePar.Insert(j, j);
            });
            swPar.Stop();
            Console.WriteLine("Parallel runtime for inserting: " + swPar.Elapsed.ToString());

            swPar.Restart();
            Parallel.ForEach(arrSearch, k =>
            {
                int j = k;
                treePar.Search(j);
            });
            swPar.Stop();
            Console.WriteLine("Parallel runtime for searching: " + swPar.Elapsed.ToString());

            swPar.Restart();
            Parallel.ForEach(arrRemove, k =>
            {
                int j = k;
                treePar.Remove(j);
            });
            swPar.Stop();
            Console.WriteLine("Parallel runtime for removing: " + swPar.Elapsed.ToString());

            if (treePar.root == null)
                Console.WriteLine("Tree is empty");

            Console.ReadKey();
        }
    }
}
