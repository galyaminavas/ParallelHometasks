using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtomicSnapshots
{
    class Program
    {
        static void Main(string[] args)
        {
            //check for two registers
            SWMR sw_mr = new SWMR(2);
            Random rnd = new Random();

            Thread[] threads = new Thread[2];

            threads[0] = new Thread(() =>
            {
                sw_mr.Scan(0);
                sw_mr.Update(0, rnd.Next(1, 100));
                sw_mr.Update(0, rnd.Next(1, 100));
                sw_mr.Update(0, rnd.Next(1, 100));
                sw_mr.Scan(0);
                sw_mr.Update(0, rnd.Next(1, 100));
                sw_mr.Update(0, rnd.Next(1, 100));
                sw_mr.Scan(0);
                sw_mr.Update(0, rnd.Next(1, 100));
                sw_mr.Update(0, rnd.Next(1, 100));
                sw_mr.Update(0, rnd.Next(1, 100));
            });

            threads[1] = new Thread(() =>
            {
                sw_mr.Update(1, rnd.Next(1, 100));
                sw_mr.Scan(1);
                sw_mr.Update(1, rnd.Next(1, 100));
                sw_mr.Update(1, rnd.Next(1, 100));
                sw_mr.Update(1, rnd.Next(1, 100));
                sw_mr.Scan(1);
                sw_mr.Update(1, rnd.Next(1, 100));
                sw_mr.Update(1, rnd.Next(1, 100));
                sw_mr.Scan(1);
            });

            foreach(Thread thr in threads)
            {
                thr.Start();
            }
            foreach(Thread thr in threads)
            {
                thr.Join();
            }

            List<String> timeJournal = sw_mr.journal;
            timeJournal.Sort();
            foreach(string s in timeJournal)
            {
                Console.WriteLine(s);
            }


            Console.ReadKey();
        }
    }
}
