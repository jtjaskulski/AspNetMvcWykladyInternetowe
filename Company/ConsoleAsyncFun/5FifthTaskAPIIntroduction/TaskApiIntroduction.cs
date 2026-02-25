// <copyright file="TaskApiIntroduction.cs" company="WSB-NLU">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ConsoleAsyncFun.FifthTaskAPIIntroduction
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Task Api.
    /// </summary>
    internal class TaskApiIntroduction
    {
        /// <summary>
        /// What are Task and Task.<T>?
        /// They are promises that an operation will complete.
        /// When you use the Task version, you create a task that doesn't return anything.
        /// When you create a generic Task<T>, then you must return a variable of type T.
        /// It existed before async and await.
        /// This complicates things a bit.
        /// If you start programming and mix the old Task API with the new one created for async and await, problems will arise.
        /// But more on that later. First, let's check if Task is indeed a background thread.
        /// Is it true? Yes, it is.
        /// </summary>
        public static void MainCheckingIfBackground()
            =>
            Task.Factory
                .StartNew(() => Console.WriteLine(Thread.CurrentThread.IsBackground))
                .Wait();

        /// <summary>
        /// Task and Task.<T> have their states:
        /// Created/WaitingForActivation/WaitingToRun: Created but not scheduled for execution
        /// Running: Currently executing
        /// WaitingForChildrenToComplete: Waiting for child tasks to complete
        /// RanToCompletion: Successfully completed
        /// Faulted: An exception occurred
        /// Canceled: The task was canceled
        /// Task and Task<T> have properties that simplify these states into 3:
        /// IsCanceled
        /// IsFaulted
        /// IsCompleted: True for RanToCompletion, Faulted, Canceled
        /// Care should be taken with the last property.
        /// The assertion of "completion" is true for both a task that encountered an exception and one that was canceled.
        /// Task contains numerous methods. I'll discuss only some of them.
        /// However, first, we need to discuss the major pitfalls associated with using Tasks. Some are obvious, some are not.
        /// </summary>
        public static void MainDelayProblem()
        {
            var d = DateTime.Now;
            Console.WriteLine(d.Minute + ":" + d.Second);
            var task = Task.Delay(7000);
            var d2 = DateTime.Now;
            Console.WriteLine(d2.Minute + ":" + d2.Second);
        }

        /// <summary>
        /// Looking at this code, you probably assume that Task.Delay runs for 7 seconds.
        /// But what does Task.Delay actually do in this case?
        /// By chance, this task didn't start on a different thread.
        /// Therefore, the main thread should continue without any delay.
        /// Moreover, Task.Delay() didn't start by itself.
        /// The status of the Task.Delay task is "WaitingForActivation".
        /// </summary>
        public static void MainWaiterDelayer()
        {
            var d = DateTime.Now;
            Console.WriteLine(d.Minute + ":" + d.Second);
            Task t = Task.Delay(7000);
            t.Wait();
            var d2 = DateTime.Now;
            Console.WriteLine(d2.Minute + ":" + d2.Second);
        }

        /// <summary>
        /// If you wanted to pause the main thread, you would need to use the "Wait" method.
        /// Additionally, the "Wait" method would start the Task for you since we didn't do it earlier.
        /// </summary>
        /// <returns>Task.</returns>
        public static async Task MainAwaiter()
        {
            var d = DateTime.Now;
            Console.WriteLine(d.Minute + ":" + d.Second);
            await Task.Delay(7000);
            var d2 = DateTime.Now;
            Console.WriteLine(d2.Minute + ":" + d2.Second);
        }

        /// <summary>
        /// However, for now, let's discuss how the Task API works and has worked.
        /// As you can see, there are many issues with understanding when a task is already running and when it's waiting to run.
        /// If you've used async and await before, you might think that tasks are warm by default and are immediately running, but that's not the case.
        /// </summary>
        public static void MainCheckingStatus()
        {
            var t = What();
            Console.WriteLine(t.Status);
        }

        /// <summary>
        /// It will show created.
        /// The constructor of the Task class only creates a task. It's not yet started.
        /// </summary>
        /// <returns>Task.</returns>
        public static Task What()
            => new (() => Console.WriteLine("1"));

        /// <summary>
        /// You can start a task in 3 ways.
        /// Here's the first and oldest method, which you don't want to use anymore.
        /// This method is called Start.
        /// </summary>
        public static void MainStarting()
        {
            Task task = new (Do); // new (Do); is short form of new Task(Do);
            task.Start();
        }

        /// <summary>
        /// Unfortunately, it's not compatible with "async and await",
        /// so it's best to forget about it.
        /// </summary>
        public static void Do()
            => Console.WriteLine("Main Starting");

        /// <summary>
        /// There is a more advanced method to start a Task.
        /// It's also not compatible with async and await,
        /// but it has many advanced options that can be useful.
        /// </summary>
        public static void MainStartNewFactory()
        {
            Task task = Task.Factory.StartNew(DoIt);
        }

        /// <summary>
        /// Going into Async side.
        /// </summary>
        public static void DoIt()
        {
            Console.WriteLine("Good Anakin, good. Kill him, kill him now");
            Console.WriteLine("I shouldn't");
            Console.WriteLine("Do it");
            Console.WriteLine("You did well anakin");
        }

        /// <summary>
        /// Last method is using Run(), which can be utilized with async and await.
        /// </summary>
        public static void MainRunning()
        {
            Task task = Task.Run(DoItNow);
        }

        /// <summary>
        /// Going into running side of force.
        /// </summary>
        public static void DoItNow()
        {
            Console.WriteLine("You and me baby ain't nothin' but mammals");
            Console.WriteLine("So let's do it like they do on the Discovery Channel");
            Console.WriteLine("Do it again now You and me baby ain't nothin' but mammals");
            Console.WriteLine("So let's do it like they do on the Discovery Channel");
        }

        /// <summary>
        /// Now that we know how to start a Task, there remains one question:
        /// how did developers used to synchronize the results of these commands to the main thread?
        /// They used the Wait() method for non-generic Tasks.
        /// For Tasks that were supposed to return something, they accessed the Result property.
        /// That's how it used to be. Now, if you wanted to do something similar,
        /// you could also use the GetAwaiter().GetResult() method.
        /// It all comes down to the keyword "await".
        /// The difference would be that when using GetAwaiter,
        /// you get the real exception.
        /// However, when using Result or the Wait method,
        /// that exception will be wrapped in an AggregateException.
        /// </summary>
        public static void MainTreadSynchronizing()
        {
            Task task = Task.Run(ILike);
            task.Wait();
            Task<int> task2 = Task.Run(Cakes);
            var r = task2.Result;
            Task<int> task3 = Task.Run(ReallyBigCakes);
            var res = task3.GetAwaiter().GetResult();
        }

        private static void ILike()
        {
            Console.WriteLine("I'm not returning anything.");
            Console.WriteLine("maybe honor of Jakub Jaskulski");
            Console.WriteLine("My honor and sanity is of type void.");
        }

        private static int Cakes() => 1;

        private static int ReallyBigCakes() => 2;
    }
}