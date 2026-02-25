// <copyright file="ThreadPoolIntroduction.cs" company="WSB-NLU">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ConsoleAsyncFun.SecondThreadpool
{
    using System;
    using System.Threading;

    /// <summary>
    /// The ThreadPool class represents a pool of threads, which in a sense is a queue of threads managed by .NET.
    /// ThreadPool manages two types of threads:
    /// Worker Threads: These are threads performing general tasks.
    /// I/O completion port thread: These execute I/O operations like disk operations.
    /// ThreadPool is used by various components in .NET such as WCF, ASP.NET, ASP.NET Core, TPL, PLinq, essentially throughout the entire .NET ecosystem.
    /// You can't create your own thread pool, and that makes sense. As you can see, the thread mechanism is very complicated.
    /// If you were to poorly write an application that, for example, creates an enormous number of threads,
    /// not only would you crash your application, but you could also destabilize the entire system it runs on.
    /// So why not create a default ThreadPool mechanism that handles this for you?
    /// ThreadPool has a mechanism for maintaining an optimal number of threads.
    /// For example, in an ASP.NET Core application, imagine that each HTTP request actually corresponds to a new thread.
    /// </summary>
    internal class ThreadPoolIntroduction
    {
        /// <summary>
        /// By using the ThreadPool, you can retrieve the maximum and minimum number of threads available for the .NET platform.
        /// Fun fact: here, I'm using inline out declarations :D
        /// This number depends on the .NET platform and whether it's a 64-bit or 32-bit version.
        /// For.NET Core 3.2, the maximum number is 32767.
        /// Now, it's worth noting what this minimum number represents. The minimum doesn't determine the starting number of threads for every.NET application.
        /// Instead, it indicates how quickly new threads should be created in your application.
        /// When this number is exceeded, it creates new threads intermittently.
        /// For.NET Core 3.2, the minimum is the number of threads your processor has (for me, it's 16).
        /// It's important to remember this because you have a tangible proof that your application might run slower if it needs more than, for example, 16 threads.
        /// By using ThreadPool, you can also add asynchronous tasks.
        /// </summary>
        public static void ThreadPoolOperation()
        {
            ThreadPool.GetMinThreads(
                out int workerThreadsMin,
                out int iOCPThreadMin);
            Console.WriteLine($"Min {workerThreadsMin}, Min IO threads:{iOCPThreadMin}");
            ThreadPool.GetMaxThreads(
                out int workerThreadsMax,
                out int iOCPThreadMax);
            Console.WriteLine($"Max {workerThreadsMax}, Max IO threads:{iOCPThreadMax}");
            ThreadPool.SetMaxThreads(1000, 32767);
            ThreadPool.SetMinThreads(16, 16);
            ThreadPool.GetMinThreads(
                out int workerThreadsMin2,
                out int iOCPThreadMin2);
            Console.WriteLine($"Min {workerThreadsMin2}, Min IO threads:{iOCPThreadMin2}");
            ThreadPool.GetMaxThreads(
                out int workerThreadsMax2,
                out int iOCPThreadMax2);
            Console.WriteLine($"Max {workerThreadsMax2}, Max IO threads:{iOCPThreadMax2}");
            Action<string> a = (state) => { Console.WriteLine("What is state of" + state); };
            ThreadPool.QueueUserWorkItem(a, "StateOk", true);
            ThreadPool.QueueUserWorkItem(FuctionToQueue);
        }

        /// <summary>
        /// Let's see what will happen if we throw exception from threadpool?.
        /// </summary>
        public static void MainExceptional()
        {
            AppDomain.CurrentDomain.UnhandledException
                += OnUnhandleException;
            ThreadPool.QueueUserWorkItem(ThrowException);
        }

        /// <summary>
        /// I think your app will die ;).
        /// </summary>
        /// <param name="state">State of thread.</param>
        public static void ThrowException(object? state)
            => throw new Exception();

        private static void OnUnhandleException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Termination? : " + e.IsTerminating);
            Console.WriteLine("Reason : " + e.ExceptionObject);
        }

        private static void FuctionToQueue(object? state)
            => Console.WriteLine("I'm standing in queue");
    }
}
