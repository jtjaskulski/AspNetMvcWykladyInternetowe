// <copyright file="AsyncAwaitIntroduction.cs" company="WSB-NLU">
// Copyright (c) WSB-NLU. All rights reserved.
// </copyright>

#pragma warning disable SA1615 // Element return value should be documented
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously - this is example.
namespace ConsoleAsyncFun.SeventhAsyncAwait
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Async and Await
    /// As for Task API, when it comes to using async and await, it looks like this.
    /// Async and await have been around in C# since 2012. Interestingly, the async and await mechanism also exists in:
    /// - TypeScript
    /// - Python
    /// - JavaScript
    /// - Rust
    /// It's interesting that while .NET as a Microsoft framework sometimes evokes mixed feelings, async and await is something that has transcended to other programming languages.
    /// </summary>
    internal class AsyncAwaitIntroduction
    {
        /// <summary>
        /// Example method.
        /// </summary>
        public static int GiveNumberek() => -1;

        /// <summary>
        /// Example method.
        /// </summary>
        public static async Task<int> GiveNumberekAsync() => -1;

        /// <summary>
        /// As you can see, the Asynchronous method returns Task.<int> and is marked with the async keyword.
        /// If you want to call such an asynchronous method, you use the await keyword.
        /// </summary>
        public static async Task<int> GiveMeRandomNumberAsync()
        {
            _ = await GiveAnotherNumberAsync();
            return await GiveNumberAsync();
        }

        /// <summary>
        /// Example method.
        /// </summary>
        public static async Task<int> GiveNumberAsync() => 1;

        /// <summary>
        /// Example method.
        /// </summary>
        public static async Task<int> GiveAnotherNumberAsync() => 2;

        /// <summary>
        /// Once upon a time, I wrote simple examples of async and await.
        /// There was always a problem with simple console applications because they didn't quite understand the async keyword in the Main method.
        /// Fortunately, as of C# 7.1, the async keyword plays nicely with the Main method in console applications, so you can write demo examples without any problems.
        /// </summary>
        ///
        /// <remarks>
        /// Examples of Main method declarations:
        /// - public static void Main() { }
        /// - public static int Main() { }
        /// - public static void Main(string[] args) { }
        /// - public static int Main(string[] args) { }
        /// - public static async Task Main() { }
        /// - public static async Task.<int> Main() { }
        /// - public static async Task Main(string[] args) { }
        /// - public static async Task<int> Main(string[] args) { }
        /// Source: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/main-command-line
        /// </remarks>
        public static async Task MainWatkiSprawdzajacyAsynceTestujacy()
        {
            WriteThread("Begin Main");
            var a = await M1Async();
            WriteThread("Middle Main");
            var b = await M1Async();
            Console.WriteLine(a + b);
            WriteThread("End Main");
            Console.ReadKey();
        }

        /// <summary>
        /// Isn't there a thread involved?
        /// I remember back in 2012, I attended a presentation on WinRT - applications in the Microsoft Store.
        /// There, the idea was presented to have the entire framework use Tasks and the async and await keywords.
        /// However, a certain thought crossed my mind. Is this going to be efficient?
        /// Assuming that every async and await operation creates a new thread,
        /// making every method asynchronous seems like a foolish idea.
        /// We'll come back to this issue shortly. But I want to ask you an even bigger question.
        /// Does every async method create a new thread?
        /// We can check this with the following code.
        /// </summary>
        public static async Task<int> M1Async()
        {
            WriteThread("Begin M1Async");
            var i = await M2Async();
            WriteThread("End M1Async");
            return i + 1;
        }

        /// <summary>
        /// Another example of this.
        /// </summary>
        public static async Task<int> M2Async()
        {
            WriteThread("At M2Async");
            return -1;
        }

        /// <summary>
        /// Example method.
        /// </summary>
        /// <param name="helpfultoken">Token.</param>
        public static void WriteThread(string helpfultoken)
            => Console.WriteLine(helpfultoken + " : " + Environment.CurrentManagedThreadId);

        /// <summary>
        /// You might be surprised.
        /// Even though I'm running two asynchronous methods here, I'm still operating on a SINGLE thread.
        /// So the console should return the ID of the main thread, which is 1.
        /// I recommend reading to understand how this is possible:
        /// - https://stackoverflow.com/questions/37419572/if-async-await-doesnt-create-any-additional-threads-then-how-does-it-make-appl
        /// - https://blog.stephencleary.com/2013/11/there-is-no-thread.html
        /// A quote from the article:
        /// "The idea that “there must be a thread somewhere processing the asynchronous operation” is not the truth."
        /// It turns out that the operating system and processor can determine whether a given asynchronous command is trivial or not.
        /// If it's trivial, no new thread will be created.
        /// Of course, this is a very simplified explanation. Look at this code.
        /// This time, one asynchronous method will run for 4 seconds.
        /// </summary>
        public static async Task MainWatkiTworzacy()
        {
            WriteThread("Begin Main");
            var a = await M3Async();
            WriteThread("Middle Main");
            var b = await M3Async();
            Console.WriteLine(a + b);
            WriteThread("End Main");
            Console.ReadKey();
        }

        /// <summary>
        /// Example method.
        /// </summary>
        public static async Task<int> M3Async()
        {
            WriteThread("Begin M1Async");
            await Task.Delay(2000);
            WriteThread("End M1Async");
            return 0;
        }

        /// <summary>
        /// As you can see, even the thread we started with is not the same thread we ended with.
        /// But should we make every method asynchronous with async and await?
        /// The answer is "NO".
        /// Async and Await always create a state machine, so it's worth using these keywords only when they are really needed.
        /// By the way, this state machine - Task and await can be used for result caching.
        /// If you try to await a task that has already completed, you will receive the previous result.
        /// </summary>
        public static async Task MainAwaiterWaiter()
        {
            Task<string> t =
            File.ReadAllTextAsync(@"D:\numbers2.txt");

            await t;
            await t;
            await t;
            await t;
        }

        /// <summary>
        /// Example method.
        /// </summary>
        public static Task<int> M1() => Task.FromResult(1);

        /// <summary>
        /// Example method.
        /// </summary>
        public static async Task<int> M2() => await M1();

        /// <summary>
        /// As I mentioned earlier, the async and await keywords create a state machine.
        /// So why not send this Task higher up?
        /// Perhaps at this stage, it doesn't need to execute the "await" yet.
        /// It seems very simple, but it's worth being cautious about sending Tasks up the chain.
        /// </summary>
        /// <param name="url">Url.</param>
        public static async Task<string> GetWithKeywordsAsync(string url)
        {
            using var client = new HttpClient();
            return await client.GetStringAsync(url);
        }

        /// <summary>
        /// In this example, the "GetElidingKeywordsAsync" method is incorrect.
        /// Why? Because the "using" statement will dispose of the HttpClient,
        /// meaning our HttpClient no longer exists at this point.
        /// However, here we are trying to pass an already closed HttpClient to an unstarted Task.
        /// So, the async eliding technique won't be useful here.
        /// </summary>
        /// <param name="url">Url.</param>
        public static Task<string> GetElidingKeywordsAsync(string url)
        {
            using var client = new HttpClient();
            return client.GetStringAsync(url);
        }

        /// <summary>
        /// And what about exceptions? You certainly can't return them like this:
        /// <code>
        /// static Task<int> M1()
        /// {
        ///     return Task.FromException(new Exception());
        /// }
        /// </code>
        /// Such code is correct, and by combining Task API and "async" and "await", you can check if an exception occurred.
        /// </summary>
        public static async Task MainCathingExceptions()
        {
            var task = M2Exceptional();

            if (task.IsFaulted)
            {
                Console.WriteLine("Error");
            }
        }

        /// <summary>
        /// Example method.
        /// </summary>
        public static Task<int> M1Exceptional() => throw new Exception();

        /// <summary>
        /// Example method.
        /// </summary>
        public static async Task<int> M2Exceptional() => await M1Exceptional();

        /// <summary>
        /// This code just reminds us of one thing. If something goes wrong, in asynchronous code, you can always throw an exception.
        /// Never ever throw NULL as a response to improper behavior.
        /// This greatly complicates matters with the Task instance.
        /// Similarly to the Task API, you can pass a cancellation token.
        /// </summary>
        public static async Task MainCancelling()
        {
            CancellationTokenSource tokenSource = new ();
            CancellationToken token = tokenSource.Token;
            tokenSource.Cancel();
            _ = await ReadFile(token);
        }

        /// <summary>
        /// Example method.
        /// </summary>
        /// <param name="cancellation">Cancellation token.</param>
        public static async Task<string> ReadFile(CancellationToken cancellation = default)
        {
            var t = await File.ReadAllTextAsync(@"D:\numbers2.txt", cancellation);
            return t;
        }
    }
}
#pragma warning restore SA1615 // Element return value should be documented
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously