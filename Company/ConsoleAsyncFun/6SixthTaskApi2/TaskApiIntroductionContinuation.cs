// <copyright file="TaskApiIntroductionContinuation.cs" company="WSB-NLU">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ConsoleAsyncFun.SixthTaskApi
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The difference between Task.Run and Task.Factory.StartNew.
    /// </summary>
    internal class TaskApiIntroductionContinuation
    {
        /// <summary>
        /// Task.Factory.StartNew, although legacy, still has some advanced options.
        /// For example, you can set the option: LongRunning.
        /// Although Tasks were designed for medium and short tasks,
        /// this option might help you when you want to execute a LONG task in a separate thread.
        /// However, some sources recommend using the Thread class for long tasks.
        /// A more interesting option is "AttachedToParent". It changes the behavior of tasks inside tasks.
        /// </summary>
        public static void AttachedTest()
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent Start");
                var child = Task.Factory.StartNew(
                    () =>
                        {
                            Console.WriteLine("ChildTask Start");
                            Thread.Sleep(4000);
                            Console.WriteLine("ChildTask End");
                        }, TaskCreationOptions.AttachedToParent);

                Console.WriteLine("Parent END");
            });

            parent.Wait();
            Console.WriteLine("DEMO END");
        }

        /// <summary>
        /// In this example, you'll see that the child task is attached to the parent.
        /// What will appear in the console?
        /// First, the parent task will execute, then the child task will execute.
        /// Afterward, the entire demo will end.
        /// If you set a different option, such as DenyChildAttach.
        ///
        /// In that case, the child and parent tasks will be treated separately.
        /// They are therefore separate Tasks. Consequently, the order will be different.
        /// First, the parent command will execute,
        /// then the supposedly child Task will start,
        /// but since it takes 4 seconds,
        /// it won't finish before the entire application ends.
        /// </summary>
        public static void RunMyChild()
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent Start");

                var child = Task.Factory.StartNew(
                    () =>
                        {
                            Console.WriteLine("ChildTask Start");
                            Thread.Sleep(4000);
                            Console.WriteLine("ChildTask End");
                        }, TaskCreationOptions.DenyChildAttach);

                Console.WriteLine("Parent END");
            });

            parent.Wait();
            Console.WriteLine("DEMO END");
        }

        /// <summary>
        /// Here are the only reasons you'd want to use Task.Factory.StartNew:
        /// What happens when an exception occurs in a Task
        /// In previous examples, we emphasized that exceptions can crash the entire application if not caught.
        /// With Tasks, it's different.
        /// </summary>
        public static void DeleteThis()
        {
            List<Exception> exceptions = new ()
            {
                new TimeoutException("It timed out", new ArgumentException("ID missing")),
            };

            // all done, now create the AggregateException and throw it
            AggregateException aggEx = new (exceptions);
            throw aggEx;
        }

        /// <summary>
        /// The only thing to remember is that Tasks wrap exceptions in an AggregateException type.
        /// This packaging mechanism was created because there's a chance that inside one Task, there are other Tasks
        /// and exceptions may occur within them (plural).
        /// Additionally, from .NET 4.0 onwards, you can send a collection of exceptions this way if you want.
        /// </summary>
        /// <exception cref="AccessViolationException">My beautiful exception.</exception>
        public static void NieWywalKodu()
        {
            Task<int> task = Task.Run(() =>
            {
                throw new AccessViolationException();
#pragma warning disable CS0162 // Unreachable code detected - this is for example.
                return -1;
#pragma warning restore CS0162 // Unreachable code detected
            });

            Console.ReadKey();

            try
            {
                _ = task.Result;
            }
            catch (AggregateException ex)
            {
#pragma warning disable CS0184 // 'is' expression's given expression is never of the provided type - this is for example.
                if (ex.InnerExceptions is AccessViolationException)
                {
                    Console.WriteLine(ex.Message);
                }
#pragma warning restore CS0184 // 'is' expression's given expression is never of the provided type
            }
        }

        /// <summary>
        /// Yes, even if you don't catch Task.Result in a try-catch block, the application won't explode.
        /// The GC will handle exceptions that you didn't catch.
        /// By using "TaskScheduler.UnobservedTaskException", you can see all the exceptions
        /// that you missed in the form of an event.
        /// </summary>
        /// <exception cref="Exception">My beautiful exception.</exception>
        public static void ObserveUnobserved()
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            var task = Task.Run(() => { throw new Exception(); });
        }

        /// <summary>
        /// Example Exception.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event.</param>
        public static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            Console.WriteLine("You missed that boy:" + e.Exception);
            e.SetObserved();
        }

        /// <summary>
        /// ContinueWith, or why async and await were created.
        /// </summary>
        /// <remarks>
        /// Before Async and Await, there had to be a way to run tasks one after the other.
        /// How was this done without blocking the main thread using the "Result" property or the "Wait()" method?
        /// This is where the ContinueWith method came in.
        /// Here's an example of running one Task when the previous one has completed.
        /// </remarks>
        public static void ContinueWithExample()
        {
            Task<int> t = Task.Run(() =>
            {
                return 44;
            });

            Task<int> t2 = t.ContinueWith(a =>
            {
                return t.Result + 66;
            });

            Console.WriteLine(t2.Result);
            Console.Read();
        }

        /// <summary>
        /// By using the ContinueWith method, you can even specify whether certain code should be executed based on the state of the previous Task.
        /// You can create a single lambda expression for a Task when it has completed successfully.
        /// </summary>
        public static void SpecifyState()
        {
            Task<int> t = Task.Run(() =>
            {
                return 44;
            });

            Task<int> t2 = t.ContinueWith(
                a =>
                {
                    return t.Result + 66;
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

            Task t3 = t.ContinueWith(
                a =>
                {
                    Console.WriteLine("Error");
                }, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// And then you can create a second lambda expression for a Task when it hasn't completed successfully.
        /// Of course, this can make the code a bit messy. Just think how many ContinueWith expressions you would have to write if you had complex logic involving several Tasks.
        /// In my opinion, this is one of the reasons why Async and Await were created.
        /// </summary>
        ///
        /// <remarks>
        /// CancellationTokenSource
        /// Each Task can be canceled. You do this through a cancellation token.
        /// It's worth specifying its behavior as "ThrowIfCancellationRequested",
        /// because you wouldn't want to constantly check the state of this token.
        /// It's better to say from the outset that canceling the task results in an exception.
        /// </remarks>
        public static void CancellationFun()
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            var t = Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();
            });

            try
            {
                t.Wait();
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
            }
            finally
            {
                tokenSource2.Dispose();
            }
        }

        /// <summary>
        /// It's also worth noting that you can cancel a task that hasn't even started yet.
        /// In this case, the Task doesn't even enter the scheduling queue, so who knows, maybe for performance reasons, you'd want to have this option.
        /// </summary>
        public static void ArturPleaseDoNotCancellMe()
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            tokenSource2.Cancel();

            var t = Task.Run(() => { }, ct);

            try
            {
                t.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.InnerException);
            }
            finally
            {
                tokenSource2.Dispose();
            }
        }

        /// <summary>
        /// WaitAll and WaitAny
        /// Task API also offers methods to handle a collection of Tasks.
        /// Metoda WaitAll wymusi poczekanie na wszystkie Taski i dopiero wtedy program ruszy dalej.
        /// The WaitAny method waits for the fastest task to complete before proceeding, ignoring the results of all other tasks.
        /// </summary>
        public static void TaskAWaitingDelayStarting()
        {
            Task t1 = Task.Delay(2000);
            Task t2 = Task.Delay(4000);
            Task t3 = Task.Delay(6000);
            Task.WaitAll(t1, t2, t3);
            Task.WaitAny(t1, t2, t3);
        }

        /// <summary>
        /// Unwrap
        /// The Unwrap method was created to simplify unwrapping tasks into tasks for async and await.
        /// How does it look? Look at this code.
        /// </summary>
        public static void GigaUnwrapper()
        {
            Task<Task<Task<int>>> t =
                Task.Factory.StartNew(
                    () =>
                    {
                        return Task.Factory.StartNew(
                        () =>
                        Task.Run(() => { return 1; }));
                    });
            var result = t.Result.Result.Result;
        }

        /// <summary>
        /// You can always unwrap it.
        /// You can do Unwrap() and Unwrap().
        /// If an exception occurs during unwrapping, you will receive it in its original form.
        /// It won't be wrapped in an AggregateException.
        /// </summary>
        public static void MegaUnwrapper()
        {
            Task<Task<Task<int>>> t =
                Task.Factory.StartNew(
                    () => Task.Factory.StartNew(
                        () => Task.Run(() => { return 1; })));

            var unwrap = t.Unwrap();
            var unwrap2 = unwrap.Unwrap();
            var result = unwrap2.Result;
        }
    }
}
