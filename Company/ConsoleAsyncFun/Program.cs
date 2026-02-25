using ConsoleAsyncFun.FifthTaskAPIIntroduction;
using ConsoleAsyncFun.FirstThreadIntro;
using ConsoleAsyncFun.FourthEventAsynchronousPatternIntroduction;
using ConsoleAsyncFun.SecondThreadpool;
using ConsoleAsyncFun.SeventhAsyncAwait;
using ConsoleAsyncFun.SixthTaskApi;
using ConsoleAsyncFun.ThirdAsync;

namespace ConsoleAsyncFun
{

    /// <summary>
    /// That is main executable class in project.
    /// </summary>
    internal class Program
    {
        private static async Task Main(string[] args)
        {

            await AsyncAwaitIntroduction.MainWatkiTworzacy();
            Console.WriteLine($"Hello, World!");
        }
    }
}
