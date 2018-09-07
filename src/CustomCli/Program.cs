using System;
using McMaster.Extensions.CommandLineUtils;

namespace CustomCli
{
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
            }

        }

    }
}
