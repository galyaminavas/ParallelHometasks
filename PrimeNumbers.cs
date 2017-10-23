using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Primes
{
    class Program
    {
        static Object locker = new Object();

        static bool IsPrime(int n)
        {
            bool b = true;

            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0)
                {
                    b = false;
                    return b;
                }
            }
            return b;
        }

        //sequential version
        //performs too slow on 20.000.000
        static void PrimeNumbersSequential(List<int> list, int n, int m)
        {
            if (n < m)
            {
                for (int i = n; i <= m; i++)
                {
                    if (IsPrime(i))
                    {
                        lock (locker)
                        {
                            list.Add(i);
                        }
                    }
                }
            }
        }

        //parallel version for 10 ranges using Parallel.Invoke
        static void PrimeNumbersRanges10(List<int> list, int n, int m)
        {
            int a = n + (m - n) / 10;
            int b = a + (m - n) / 10;
            int c = b + (m - n) / 10;
            int d = c + (m - n) / 10;
            int e = d + (m - n) / 10;
            int f = e + (m - n) / 10;
            int g = f + (m - n) / 10;
            int h = g + (m - n) / 10;
            int i = h + (m - n) / 10;

            Parallel.Invoke(
                () => PrimeNumbersSequential(list, n, a),
                () => PrimeNumbersSequential(list, a + 1, b),
                () => PrimeNumbersSequential(list, b + 1, c),
                () => PrimeNumbersSequential(list, c + 1, d),
                () => PrimeNumbersSequential(list, d + 1, e),
                () => PrimeNumbersSequential(list, e + 1, f),
                () => PrimeNumbersSequential(list, f + 1, g),
                () => PrimeNumbersSequential(list, g + 1, h),
                () => PrimeNumbersSequential(list, h + 1, i),
                () => PrimeNumbersSequential(list, i + 1, m)
            );
        }

        static void PrimeNumbersSimpleThreads(List<int> list, int n, int m)
        {
            int a = n + (m - n) / 10;
            int b = a + (m - n) / 10;
            int c = b + (m - n) / 10;
            int d = c + (m - n) / 10;
            int e = d + (m - n) / 10;
            int f = e + (m - n) / 10;
            int g = f + (m - n) / 10;
            int h = g + (m - n) / 10;
            int i = h + (m - n) / 10;

            Thread[] threads = new Thread[10];
            threads[0] = new Thread(() => PrimeNumbersSequential(list, n, a));
            threads[1] = new Thread(() => PrimeNumbersSequential(list, a + 1, b));
            threads[2] = new Thread(() => PrimeNumbersSequential(list, b + 1, c));
            threads[3] = new Thread(() => PrimeNumbersSequential(list, c + 1, d));
            threads[4] = new Thread(() => PrimeNumbersSequential(list, d + 1, e));
            threads[5] = new Thread(() => PrimeNumbersSequential(list, e + 1, f));
            threads[6] = new Thread(() => PrimeNumbersSequential(list, f + 1, g));
            threads[7] = new Thread(() => PrimeNumbersSequential(list, g + 1, h));
            threads[8] = new Thread(() => PrimeNumbersSequential(list, h + 1, i));
            threads[9] = new Thread(() => PrimeNumbersSequential(list, i + 1, m));

            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        //only for 2 threads cause otherwise too many lines of the same code
        //tried to do in the loop to get 10 threads but something went wrong
        static void PrimeNumbersThreadPool1(List<int> list, int n, int m)
        {
            int half = (m - n) / 2;
            var handle1 = new ManualResetEvent(false);
            var handle2 = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(delegate
            {
                PrimeNumbersSequential(list, n, n + half);
                handle1.Set();
            });
            ThreadPool.QueueUserWorkItem(delegate
            {
                PrimeNumbersSequential(list, n + half + 1, m);
                handle2.Set();
            });
            //according to the dude on stackoverflow: 
            //"WaitHandle.WaitAll will fail if you have more than 64 items in an STA thread"
            WaitHandle.WaitAll(new WaitHandle[] { handle1, handle2 });
        }

        //like we had in class
        //same remarks as for the previous one
        static void PrimeNumbersThreadPool2(List<int> list, int n, int m)
        {
            int half = (m - n) / 2;
            int completed = 0;
            int toComplete = 2;
            ManualResetEvent allDone = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem((_ =>
            {
                PrimeNumbersSequential(list, n, n + half);
                if (Interlocked.Increment(ref completed) == toComplete)
                {
                    allDone.Set();
                }
            }));
            ThreadPool.QueueUserWorkItem((_ =>
            {
                PrimeNumbersSequential(list, n + half + 1, m);
                if (Interlocked.Increment(ref completed) == toComplete)
                {
                    allDone.Set();
                }
            }));

            allDone.WaitOne();
        }

        static void PrimeNumbersTasks(List<int> list, int n, int m)
        {
            int a = n + (m - n) / 10;
            int b = a + (m - n) / 10;
            int c = b + (m - n) / 10;
            int d = c + (m - n) / 10;
            int e = d + (m - n) / 10;
            int f = e + (m - n) / 10;
            int g = f + (m - n) / 10;
            int h = g + (m - n) / 10;
            int i = h + (m - n) / 10;

            int half = (m - n) / 2;

            Task task1 = Task.Run(() => PrimeNumbersSequential(list, n, a));
            Task task2 = Task.Run(() => PrimeNumbersSequential(list, a + 1, b));
            Task task3 = Task.Run(() => PrimeNumbersSequential(list, b + 1, c));
            Task task4 = Task.Run(() => PrimeNumbersSequential(list, c + 1, d));
            Task task5 = Task.Run(() => PrimeNumbersSequential(list, d + 1, e));
            Task task6 = Task.Run(() => PrimeNumbersSequential(list, e + 1, f));
            Task task7 = Task.Run(() => PrimeNumbersSequential(list, f + 1, g));
            Task task8 = Task.Run(() => PrimeNumbersSequential(list, g + 1, h));
            Task task9 = Task.Run(() => PrimeNumbersSequential(list, h + 1, i));
            Task task10 = Task.Run(() => PrimeNumbersSequential(list, i + 1, m));

            Task.WaitAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10);
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the left border of range");
            int n = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the right border of range");
            int m = int.Parse(Console.ReadLine());

            //sequential
            var primesSeq = new List<int>();
            Stopwatch swSeq = new Stopwatch();
            swSeq.Start();
            PrimeNumbersSequential(primesSeq, n, m);
            swSeq.Stop();
            string tSeq = swSeq.Elapsed.ToString();
            Console.WriteLine("Sequential runtime is: " + tSeq);
            double averageSeq = primesSeq.Average();
            Console.WriteLine(averageSeq);

            //10 ranges using Parallel.Invoke
            var primesR = new List<int>();
            Stopwatch sw10 = new Stopwatch();
            sw10.Start();
            PrimeNumbersRanges10(primesR, n, m);
            sw10.Stop();
            string t10 = sw10.Elapsed.ToString();
            Console.WriteLine("Ranges runtime is: " + t10);
            double averageR = primesR.Average();
            Console.WriteLine(averageR);

            //simple threads
            var primesST = new List<int>();
            Stopwatch swST = new Stopwatch();
            swST.Start();
            PrimeNumbersSimpleThreads(primesST, n, m);
            swST.Stop();
            string tST = swST.Elapsed.ToString();
            Console.WriteLine("Simple threads runtime is: " + tST);
            double averageST = primesST.Average();
            Console.WriteLine(averageST);

            //thread pool1
            var primesP1 = new List<int>();
            Stopwatch swP1 = new Stopwatch();
            swP1.Start();
            PrimeNumbersThreadPool1(primesP1, n, m);
            swP1.Stop();
            string tP1 = swP1.Elapsed.ToString();
            Console.WriteLine("Thread pool1 runtime is: " + tP1);
            double averageP1 = primesP1.Average();
            Console.WriteLine(averageP1);

            //thread pool2
            var primesP2 = new List<int>();
            Stopwatch swP2 = new Stopwatch();
            swP2.Start();
            PrimeNumbersThreadPool2(primesP2, n, m);
            swP2.Stop();
            string tP2 = swP2.Elapsed.ToString();
            Console.WriteLine("Thread pool2 runtime is: " + tP2);
            double averageP2 = primesP2.Average();
            Console.WriteLine(averageP2);

            //tasks
            var primesT = new List<int>();
            Stopwatch swT = new Stopwatch();
            swT.Start();
            PrimeNumbersTasks(primesT, n, m);
            swT.Stop();
            string tT = swT.Elapsed.ToString();
            Console.WriteLine("Tasks runtime is: " + tT);
            double averageT = primesT.Average();
            Console.WriteLine(averageT);

            //this one is odd, cause the average value different from the others for some ranges
            //though it was in class...
            /*var primes = new List<int>();
            Stopwatch swF = new Stopwatch();
            swF.Start();
            primes = (from e in Enumerable.Range(n,m).AsParallel()
                      where IsPrime(e)
                      select e).ToList();
            swF.Stop();
            string tF = swF.Elapsed.ToString();
            Console.WriteLine("Parallel Final RunTime: " + tF);
            double average = primes.Average();
            Console.WriteLine(average);*/

            Console.ReadKey();

        }
    }
}
