using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AtomicSnapshots
{
    class SWMR
    {
        //unbounded wait-free algorithm realisation
        public int registersCount;

        public struct Register
        {
            public int data;
            public int version;
            public int[] snapshot;

            public Register(int d, int v, int[] sn)
            {
                data = d;
                version = v;
                snapshot = sn;
            }
        }

        public List<String> journal = new List<String>();
        private Stopwatch sw = new Stopwatch();

        public Register[] regs;

        public SWMR(int regCount)
        {
            registersCount = regCount;
            regs = new Register[registersCount];
            for (int i = 0; i < registersCount; i++)
            {
                regs[i] = new Register(0, 0, new int[registersCount]);
            }
            sw.Start();
        }

        public Register[] Collect()
        {
            Register[] snapshot = new Register[registersCount];
            for (int i = 0; i < registersCount; i++)
            {
                snapshot[i] = regs[i];
            }
            return snapshot;
        }

        public int[] Scan(int regId)
        {
            bool[] moved = new bool[registersCount];

            while(true)
            {
                for (int j = 0; j < registersCount; j++)
                {
                    moved[j] = false;
                }

                Register[] a = Collect();
                Register[] b = Collect();

                for (int j = 0; j < regs.Length; j++)
                {
                    if (a[j].version != b[j].version)
                    {
                        if (moved[j] == true)
                        {
                            journal.Add("On time: " + sw.Elapsed + " read: [" + string.Join(", ", b[j].snapshot) + "] using #" + regId + " register");
                            return b[j].snapshot;
                        }
                        else
                        {
                            moved[j] = true;
                        }
                    }
                }

                int[] snapsh = new int[registersCount];
                for (int j = 0; j < registersCount; j++)
                {
                    snapsh[j] = b[j].data;
                }
                journal.Add("On time: " + sw.Elapsed + " read: [" + string.Join(", ", snapsh) + "] using #" + regId + " register");
                return snapsh;
            } 
        }

        public void Update(int regId, int value)
        {
            int[] s = Scan(regId);
            regs[regId] = new Register(value, regs[regId].version + 1, s);
            journal.Add("On time: " + sw.Elapsed + " wrote in #" + regId + " register new value of [" + value + "]");
        }
    }
}
