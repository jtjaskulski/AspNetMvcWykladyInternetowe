// <copyright file="EventAsynchronousPatternIntroduction.cs" company="WSB-NLU">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ConsoleAsyncFun.FourthEventAsynchronousPatternIntroduction
{
    using System;
    using System.ComponentModel;
    using System.Net;

    /// <summary>
    /// Before the Task API, there was another asynchronous programming model known as the event-based model.
    /// It's best illustrated using the BackgroundWorker class.
    /// </summary>
    internal class EventAsynchronousPatternIntroduction
    {
        /// <summary>
        /// As you can see, BackgroundWorker accepts three events:
        /// Worker_RunWorkerCompleted: This event fires when the task has been completed.
        /// WorkerDoSomething: This event runs in a separate thread.
        /// Worker_ProgressChanged: This event informs about the progress of the task.
        /// </summary>
        public static void MainBackGround()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += WorkerDoSomething;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync(worker);
        }

        /// <summary>
        /// While the code may still get convoluted, especially if you have 5 or more BackgroundWorkers for some reason,
        /// this solution still appears to be the most human-readable and understandable for now.
        /// Unfortunately, as in other examples, exceptions still pose a problem.
        /// </summary>
        public static void MainWebClient()
        {
#pragma warning disable SYSLIB0014 // Type or member is obsolete, but I needed good example
            WebClient client = new ();
#pragma warning restore SYSLIB0014 // Type or member is obsolete
            client.DownloadStringCompleted += OnDownloadCompl;
            client.DownloadStringAsync(new Uri(@"https://wsbnlu.clouda.edu.pl/"));
            Console.ReadKey();
        }

        private static void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Work completed");
        }

        private static void WorkerDoSomething(object? sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Working on something.");
        }

        private static void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {

            Console.WriteLine("Work in progress: " + e.ProgressPercentage);
        }

        private static void OnDownloadCompl(object? sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("Cancelled");
            }
            else if (e.Error != null)
            {
                Console.WriteLine(e.Error.Message);
            }
            else
            {
                Console.WriteLine(e.Result);
            }
        }
    }
}
