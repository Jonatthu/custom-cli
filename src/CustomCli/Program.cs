using System;
using System.Text;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using ShellProgressBar;

namespace CustomCli
{
    // PS C:\Users\jnungaray\Desktop\CustomCli> dotnet publish -c Release -r win10-x64
    // PS C:\Users\jnungaray\Desktop\CustomCli\src\CustomCli\bin\Release\netcoreapp2.1\win10-x64> .\CustomCli.exe serve -h
    [Command(Name = "gtr", Description = "Generatthor comes to generate your entire solution.")]
    [Subcommand("build", typeof(Build))]
    [Subcommand("serve", typeof(Serve))]
    public class Program
    {
        public static void Main(string[] args)
        {
            CommandLineApplication.Execute<Program>(args);
        }

        [Argument(0, name: "command", description: "Creates an instance")]
        private string command { get; }

        [Option(Description = "The subject")]
        public string Subject { get; }

        [Option(ShortName = "n")]
        public int Count { get; }

        private void OnExecute()
        {
            var subject = Subject ?? "world";
            for (var i = 0; i < Count; i++)
            {
                Console.WriteLine($"Hello {subject}!");
            }

            const int totalTicks = 15;
            var options = new ProgressBarOptions
            {
                //ProgressCharacter = '─',
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
                TickToCompletion(pbar, totalTicks, sleep: 200);
                pbar.Message = "TyNET has been completed.";
            }
        }

        private void TickToCompletion(ProgressBar pbar, int totalTicks, int sleep)
        {
            for (int i = 0; i < totalTicks; i++)
            {
                pbar.Tick();
                pbar.Message = $"Doing the step {i + 1}";
                Thread.Sleep(sleep);
            }
        }

        [Command("build", Description = "Builds and generates the Generated Project.")]
        private class Build
        {
            [Argument(0, name: "command", description: "Creates an instance")]
            private string command { get; }

            [Option(Description = "The subject")]
            public string Subject { get; }

            [Option(ShortName = "n")]
            public int Count { get; }

            private void OnExecute()
            {
                var subject = Subject ?? "world";
                for (var i = 0; i < Count; i++)
                {
                    Console.WriteLine($"Hello {subject}!");
                }
            }

        }

        [Command("serve", Description = "Serves and generates the Generated Project.")]
        private class Serve
        {
            [Argument(0, name: "command", description: "Creates an instance")]
            private string command { get; }

            [Option(Description = "The subject")]
            public string Subject { get; }

            [Option(ShortName = "n")]
            public int Count { get; }

            private void OnExecute()
            {
                var subject = Subject ?? "world";
                for (var i = 0; i < Count; i++)
                {
                    Console.WriteLine($"Hello {subject}!");
                }

                const int totalTicks = 10;
                var options = new ProgressBarOptions
                {
                    ProgressCharacter = '─',
                    ProgressBarOnBottom = true
                };
                using (var pbar = new ProgressBar(totalTicks, "Initial message", options))
                {
                    pbar.Tick(); //will advance pbar to 1 out of 10.
                    //we can also advance and update the progressbar text
                    pbar.Tick("Step 2 of 10");
                }
            }

        }

    }

}
