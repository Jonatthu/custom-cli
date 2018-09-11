using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentLib
{
    class Program
    {
        static ConcurrentDictionary<int, string> cd = new ConcurrentDictionary<int, string>();

        static void Main(string[] args)
        {
            string[] lines = new string[] { "Hello World", "Jurassic World", "World class", "Wonder world" };

            Parallel.ForEach<string>(lines,
                (string line) =>
                {
                    string[] words = line.Split(' ');
                    foreach (string word in words)
                    {
                        if (string.IsNullOrWhiteSpace(word)) continue;

                        string canonicalWord = word.ToLowerInvariant();

                        wordCount.AddOrUpdate(canonicalWord, 1, (k, currentCount) => { return currentCount + 1; });
                    }
                });

            foreach (var item in wordCount)
            {
                Console.WriteLine(item.Key + "-" + item.Value);
            }

            Console.ReadKey();

        }
        static ConcurrentDictionary<string, uint> wordCount = new ConcurrentDictionary<string, uint>();
    }
}
