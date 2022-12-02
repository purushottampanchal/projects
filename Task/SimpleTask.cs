using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskExample
{
    class SimpleTask
    {

        static void WriteChar(char c)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(c);
            }
        }


        public static void Run()
        {
            //2 Ways of creating tasks
            //CreateAndRunTask();

            //--------------------------- 

            //cancelling task
            //CancelAndWait();

            // composite tokens
            //CompositToken();


            //TaskWait();

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task t1 = new Task(() =>
            {
                throw new AccessViolationException() { Source = "t1 task" };
            }, token);


            Task t2 = new Task(() =>
            {

                throw new ArgumentException() { Source = "t2 task" };

            }, token);

            t1.Start();
            t2.Start();


            try
            {
                Task.WaitAll(new[] { t1, t2 }, token);
            }
            catch (AggregateException ae)
            {
                foreach (Exception ex in ae.InnerExceptions)
                {
                    Console.WriteLine($"Exception {ex.GetType()} occured in {ex.Source}");
                }
            }

            Console.ReadKey();
            cts.Cancel();


        }

        private static void TaskWait()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task t1 = new Task(() =>
            {

                Console.WriteLine("t1 Waiting for 5 sec");
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"t1 {i}");
                    token.ThrowIfCancellationRequested();
                }
                Console.WriteLine("t1 completed");
            }, token);

            t1.Start();

            Task t2 = new Task(() =>
            {
                Console.WriteLine("t2 Waiting for 3 sec");
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine($"t2 {i}");
                    Thread.Sleep(1000);
                    token.ThrowIfCancellationRequested();
                }
                Console.WriteLine("t2 Completed");

            }, token);

            t2.Start();

            //t2.Wait(1000);

            //throws exception if operation gets cancelled
            //Task.WaitAny(new[] { t1, t2 }, token);


            Console.ReadKey();
            cts.Cancel();
            //throws task cancelled exception

            try
            {
                Task.WaitAll(new[] { t1, t2 }, token);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception caught {e.Message}");
            }
        }

        private static void CompositToken()
        {
            var cts1 = new CancellationTokenSource();
            var cts2 = new CancellationTokenSource();
            var cts3 = new CancellationTokenSource();

            var composit = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token, cts3.Token);

            Task.Factory.StartNew(() =>
            {
                var i = 0;
                while (true)
                {
                    composit.Token.ThrowIfCancellationRequested();
                    Console.Write($"{i++}\t");
                }
            }, composit.Token);

            //we can use any cts to cancel

            Console.ReadKey();
            cts1.Cancel();
            //cts2.Cancel(); 
            //cts3.Cancel(); 
        }

        private static void CancelAndWait()
        {
            //to cancel task in middle we need cancellation tokens

            var cts = new CancellationTokenSource();
            var Tokens = cts.Token;

            //registering event of cancellation
            Tokens.Register(() =>
            {
                Console.WriteLine("Cancellation has been req");
            });


            Task.Factory.StartNew(() =>
            {
                var i = 0;
                while (true)
                {
                    if (Tokens.IsCancellationRequested)
                    {
                        //soft way ->  break;

                        //suggested way
                        throw new OperationCanceledException();
                    }

                    //alternate to above block 

                    //Tokens.ThrowIfCancellationRequested();

                    Console.WriteLine($"Press key to cancel the task {i++}");
                }
            }, Tokens);

            Task.Factory.StartNew(() =>
            {

                var b = Tokens.WaitHandle.WaitOne();
                Console.WriteLine($"Wait is done {b}");

            });

            Console.ReadKey();
            cts.Cancel();
        }

        private static void CreateAndRunTask()
        {
            Task.Factory.StartNew(() =>
            {
                WriteChar('c');
            });

            Task t = new Task(() => WriteChar('#'));
            t.Start();
        }
    }
}
