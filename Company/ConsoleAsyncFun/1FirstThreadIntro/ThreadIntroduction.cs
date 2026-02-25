// <copyright file="ThreadIntroduction.cs" company="WSB-NLU">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ConsoleAsyncFun.FirstThreadIntro
{
    using System;
    using System.Threading;

    /// <summary>
    /// Class with methods to introduce to threads.
    /// </summary>
    internal class ThreadIntroduction
    {
        /// <summary>
        /// Class thread is the oldest approach to the multitreaded applications.
        /// Even Walenciuk stated that he didn't use this class during studies in 2008.
        /// To be honest, I was looking at it in 2012 as a mega-antique archaism.
        /// It's worth knowing what that means.
        /// Firstly, in .NET, there are Background and Foreground threads.
        /// Foreground threads block the application from closing, so you wouldn't want to use them.
        /// On the other hand, you have the assurance that the thread will complete its task, even if someone closes your application.
        /// Background threads, however, will be terminated when the application is closed.
        /// IsBackground allows us to set this.
        /// </summary>
        public static void MainDoThread()
        {
            var thread = new Thread(new ThreadStart(Do))
            {
                IsBackground = true,
            };
            thread.Start();
            thread.Join();
        }

        /// <summary>
        /// Is there any case where the Thread class would still be useful?
        /// The only scenario that comes to mind is an absurdly long-running task that needs to be executed in a separate thread.
        /// The Task API, for example, performs excellently for short to medium-length tasks.
        /// What else can be said about the Thread class? As you can see, it must have been around since.NET 1.1,
        /// because you can observe that it accepts a delegate in its constructor.
        /// However, this delegate is not generic but its own creation.
        /// If you wanted to create a thread that executes a method with a parameter, you would write code like this.
        /// The "Start" method initiates the thread, while the "Join" method synchronizes the thread with the main thread.
        /// It's also worth mentioning that .NET has its special threads like "GC" (Garbage Collection) and "Finalizer".
        /// Additionally, you can set the priority of a thread.
        /// </summary>
        public static void MainDoThread2()
        {
            Thread thread2 = new (new ParameterizedThreadStart(Do2))
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest,
            };
            thread2.Start("Krzysztof P");
            thread2.Join();
        }

        /// <summary>
        /// Thread.Sleep - You've probably used this static method before. It suspends the current thread for a specified number of milliseconds.
        /// However, if you've used this method even once, you may have noticed that the thread doesn't sleep exactly for the amount of time you specify.
        /// Why is that?.
        /// </summary>
        public static void MainSleepWithBackGround()
        {
            Thread thread = new (new ThreadStart(LongSleep))
            {
                IsBackground = true,
            };
            thread.Start();
        }

        /// <summary>
        /// Indeed, the processor and its core won't just wait patiently until your thread wakes up. At that moment, the processor will attend to another thread.
        /// So, if you use Thread.Sleep, you also need to consider the processor time that results from switching between threads.
        /// Now that you know this, what do you think will happen if we put the thread to sleep for 1 millisecond?
        /// One millisecond is indeed a very small unit of time. The question is whether such a small time unit will even be taken into account.
        /// Thread.Sleep(1) will force the processor to switch the thread, which will work for its "time quantum".
        /// Additionally, there's another trick.
        /// Thread.Sleep(0);
        /// Here, zero represents a special value.In this case, the processor will switch its thread if there is any thread in the "ready" state.
        /// </summary>
        public static void MainSleepWithForeGround()
        {
            Thread thread = new (new ThreadStart(ShortSleep))
            {
                IsBackground = false,
            };
            thread.Start();
        }

        /// <summary>
        /// Normally I prefer to use Task API, because it will not throw exception in main tread effectively stopping whole application like thread does.
        /// It is sometimes useful - because if your app is going south in calculations - you can catch it in main tread immediately.
        /// </summary>
        public static void MainExploding()
        {
            Thread thread = new (new ThreadStart(DoEx))
            {
                IsBackground = true,
            };
            thread.Start();
            thread.Join();
        }

        private static void DoEx()
            => throw new Exception();

        private static void ShortSleep()
        {
            Thread.Sleep(4000);
            Console.WriteLine("Short sleep!");
        }

        private static void LongSleep()
        {
            Thread.Sleep(40000);
            Console.WriteLine("Long sleep!");
        }

        private static void Do()
            => Console.WriteLine("Artur is arcy crucial :D");

        private static void Do2(object? parameter)
            => Console.WriteLine($"Mr {parameter}. is absolutely crucial!");
    }
}
