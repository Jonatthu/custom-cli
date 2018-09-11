using System;
using System.Threading;
using ShellProgressBar;

namespace ProgressBarTechniques
{
    public class Program
    {
        static void Main(string[] args)
        {
            const int totalTicks = 3;
            var options = new ProgressBarOptions
            {
                DisplayTimeInRealTime = true,
                ProgressBarOnBottom = true,
                ForegroundColor = ConsoleColor.Gray,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                EnableTaskBarProgress = true,
                BackgroundCharacter = '\u2593'
            };
            using (var pbar = new ProgressBar(totalTicks, "Building TyNet Api", options))
            {

                var d = new OneClass(pbar, "One Class");
                var c = new SecondClass(pbar, "Second Class");
                var e = new ThirdClass(pbar, "ThirdClass");
                pbar.Message = "TyNET has been completed.";
            }

            var dd = new OneClass(null, "One Class");
            var cd = new SecondClass(null, "Second Class");
            var ed = new ThirdClass(null, "ThirdClass");

            Console.WriteLine("Hello");

        }

    }


    public abstract class TheBase
    {
        private readonly ProgressBar progressBar;
        private readonly string proccessName;

        public TheBase(ProgressBar progressBar, string proccessName)
        {
            this.progressBar = progressBar;
            this.proccessName = proccessName;
        }

        ~TheBase()
        {
            Console.WriteLine(proccessName);
            //progressBar.Tick();
            //progressBar.Message = $"Doing the step {proccessName}";
            //Thread.Sleep(200);
        }
    }


    public class OneClass : TheBase
    {
        public OneClass(ProgressBar progressBar, string proccessName) : base(progressBar, proccessName)
        {
        }
    }

    public class SecondClass : TheBase
    {
        public SecondClass(ProgressBar progressBar, string proccessName) : base(progressBar, proccessName)
        {
        }
    }

    public class ThirdClass : TheBase
    {
        public ThirdClass(ProgressBar progressBar, string proccessName) : base(progressBar, proccessName)
        {
        }
    }

}
