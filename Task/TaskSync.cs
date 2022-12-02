using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    class BankAccount
    {

        private int _balance;

        public int Balance
        {
            get { return _balance; }
            private set { _balance = value; }
        }

        public void Deposit(int amt)
        {
            /*lock (new object())
            {
                Balance += amt;
            }
*/
            Interlocked.Add(ref _balance, amt);
        }

        public void Withdraw(int amt)
        {

            //lock (new object())
            //{
            //    Balance -= amt;
            //}

            //or

            Interlocked.Add(ref _balance, -amt);
        }


    }

    public class TaskSync
    {
        internal static void Run()
        {
            
            List<Task> tasks = new List<Task>();
            BankAccount bankAccount = new BankAccount();

            for (int i = 0; i < 10; i++)
            {
                for (int k = 0; k < 1000; k++)
                {
                    tasks.Add(Task.Factory.StartNew(() => { bankAccount.Deposit(100); }) );
                }
                for (int j = 0; j < 1000; j++)
                {
                    tasks.Add(Task.Factory.StartNew(() => { bankAccount.Withdraw(100); }) );
                }
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"balance is {bankAccount.Balance}");


        }
    }
}
