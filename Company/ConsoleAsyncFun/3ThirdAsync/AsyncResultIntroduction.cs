// <copyright file="AsyncResultIntroduction.cs" company="WSB-NLU">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ConsoleAsyncFun.ThirdAsync
{
    using System;

    /// <summary>
    /// This code isn't as old as the Thread and ThreadPool classes. When I ran this code for .NET Core 3.2, I received the exception: PlatformNotSupportedException.
    /// As you can see, .NET Core and likely.NET 5/6/7 won't even allow you to run such code.
    /// </summary>
    internal class AsyncResultIntroduction
    {
        /// <summary>
        ///  But don't worry, you haven't lost anything because this whole BeginInvoke and AsyncCallback looks like spaghetti code.
        /// This was the Asynchronous Programming Model (APM).
        /// </summary>
        public static void MainIAsyncResult()
        {
            Func<int, int> method =
                (int a) => { return a; };

            IAsyncResult handle = method.BeginInvoke(100, new AsyncCallback(WhenDone), new object());
        }

        private static void WhenDone(IAsyncResult ar)
        {
            var target = ar.AsyncState as Func<int, int>;
            if (target != null)
            {
                int result = target.EndInvoke(ar);
                Console.WriteLine(result);
            }
        }
    }
}