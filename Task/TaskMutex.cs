using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{

    class BankAccount2
    {
        private int _balance;

        public int Balance
        {
            get { return _balance; }
            private set { _balance = value; }
        }

        public void Withdraw(int amt) { Balance = Balance - amt; }
        public void Deposit(int amt) { Balance = Balance + amt; }
        public void Transfer(BankAccount2 ac, int amt)
        {
            Balance -= amt;
            ac.Balance += amt;
        }

    }

    class TaskMutex
    {
        internal static void Run()
        {
            //SingleMutex();

            //TwoMutex();

            string AppName = "MyOwnMutex";
            Mutex mutex;
            try
            {

                mutex = Mutex.OpenExisting(AppName);
                Console.WriteLine("App is already Running");
                Console.ReadKey();
            }
            catch (WaitHandleCannotBeOpenedException e)
            {

                Console.WriteLine("Starting program .. ");
                mutex = new Mutex(false, AppName);
                Console.ReadKey();
            }

        }

        private static void TwoMutex()
        {
            BankAccount2 ba = new BankAccount2();
            BankAccount2 ba2 = new BankAccount2();


            List<Task> tasks = new List<Task>();
            Mutex mutex = new Mutex();
            Mutex mutex2 = new Mutex();

            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        bool isLocked = mutex.WaitOne();
                        try
                        {
                            ba.Deposit(1);
                        }
                        finally
                        {
                            if (isLocked)
                                mutex.ReleaseMutex();
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        bool isLocked = mutex2.WaitOne();
                        try
                        {
                            ba2.Deposit(1);
                        }
                        finally
                        {
                            if (isLocked)
                                mutex2.ReleaseMutex();
                        }
                    }
                }));

            }

            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {

                        /*
                        bool isLocked = Mutex.WaitAll(new[] { mutex, mutex2 });
                        /*/

                        bool isLocked = WaitHandle.WaitAll(new[] { mutex, mutex2 });
                        //*/

                        try
                        {
                            ba.Transfer(ba2, 1);
                        }
                        finally
                        {
                            if (isLocked)
                            {
                                mutex.ReleaseMutex();
                                mutex2.ReleaseMutex();

                            }
                        }
                    }
                }));
            }


            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Remaining balance of ba is {ba.Balance}");
            Console.WriteLine($"Remaining balance of ba2 is {ba2.Balance}");
        }

        private static void SingleMutex()
        {
            BankAccount2 ba = new BankAccount2();
            List<Task> tasks = new List<Task>();
            Mutex mutex = new Mutex();

            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    bool isLocked = mutex.WaitOne();
                    try
                    {
                        ba.Deposit(10);
                    }
                    finally
                    {
                        if (isLocked)
                            mutex.ReleaseMutex();
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    bool isLocked = mutex.WaitOne();
                    try
                    {
                        ba.Withdraw(10);
                    }
                    finally
                    {
                        if (isLocked)
                            mutex.ReleaseMutex();
                    }
                }));
            }


            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Remaining balance is {ba.Balance}");
        }
    }
}
