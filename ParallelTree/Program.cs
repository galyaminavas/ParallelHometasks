using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

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
            int[] arrInsert = new int[1000000];
            for (int i = 0; i < arrInsert.Length; i++)
            {
                arrInsert[i] = rnd.Next(1, 10000000);
            }
            int[] arrRemove = new int[500000];
            for (int i = 0; i < arrRemove.Length; i++)
            {
                arrRemove[i] = rnd.Next(1, 10000000);
            }

            swSeq.Start();
            for (int i = 0; i < arrInsert.Length; i++)
            {
                treeSeq.Insert(arrInsert[i], arrInsert[i]);
            }
            swSeq.Stop();
            Console.WriteLine("Sequential runtime for inserting: " + swSeq.Elapsed.ToString());

            swSeq.Restart();
            for (int i = 0; i < arrInsert.Length; i++)
            {
                treeSeq.Search(arrInsert[i]);
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
            //if (treeSeq.root == null)
            //    Console.WriteLine("Tree is empty");
            //else
            //    treeSeq.PrintLevelOrderTraversal();
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
            Parallel.ForEach(arrInsert, k =>
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
            //if (treePar.root == null)
            //    Console.WriteLine("Tree is empty");
            //else
            //    treePar.PrintLevelOrderTraversal();
            Console.WriteLine();

            var parTree = new Tree();
            int[] ins2 = new int[100000];
            for (int i = 0; i < ins2.Length; i++)
            {
                ins2[i] = rnd.Next(1, 10000000);
            }
            foreach (int a in ins2)
            {
                parTree.Insert(a, a);
            }
            Thread[] threads = new Thread[2];
            threads[0] = new Thread(() =>
            {
                foreach (int el in arrInsert)
                {
                    parTree.Insert(el, el);
                }
            });
            threads[1] = new Thread(() =>
            {
                foreach (int el in arrRemove)
                {
                    parTree.Remove(el);
                }
            });
            swPar.Restart();
            foreach (Thread thr in threads)
            {
                thr.Start();
            }
            foreach (Thread thr in threads)
            {
                thr.Join();
            }
            swPar.Stop();

            Console.WriteLine("Parallel runtime for inserting and removing: " + swPar.Elapsed.ToString());

            Console.ReadKey();
        }
    }
}
