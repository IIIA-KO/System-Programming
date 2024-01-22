namespace task_1
{
    internal class Program
    {
        static readonly ManualResetEvent manualEvent = new ManualResetEvent(false);

        static void GeneratePairs(string pairsFilePath)
        {
            if (!File.Exists(pairsFilePath))
            {
                File.Create(pairsFilePath).Close();
            }

            int size = 1000;
            var pairs = new Tuple<int, int>[size];

            for (int i = 0; i < size; i++)
            {
                pairs[i] = new Tuple<int, int>
                    (
                        Random.Shared.Next(1, size + 1),
                        Random.Shared.Next(1, size + 1)
                    );
            }

            using (StreamWriter sw = new StreamWriter(pairsFilePath))
            {
                foreach (var item in pairs)
                {
                    sw.WriteLine($"{item.Item1} {item.Item2}");
                }
            }

            manualEvent.Set();
        }

        static void SumPairs(string fromFilePath, string toFilePath)
        {
            manualEvent.WaitOne();

            if (!File.Exists(fromFilePath))
            {
                throw new FileNotFoundException($"File {fromFilePath} was not found", fromFilePath);
            }

            List<int> pairsSums = new List<int>();

            try
            {
                using (StreamReader sr = new StreamReader(fromFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        var pair = line.Split(' ');

                        pairsSums.Add(int.Parse(pair[0]) + int.Parse(pair[1]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            using (StreamWriter sw = new StreamWriter(toFilePath))
            {
                foreach (var sum in pairsSums)
                {
                    sw.WriteLine(sum);
                }
            }

            manualEvent.Set();
        }

        static void ProductPairs(string fromFilePath, string toFilePath)
        {
            manualEvent.WaitOne();

            if (!File.Exists(fromFilePath))
            {
                throw new FileNotFoundException($"File {fromFilePath} was not found", fromFilePath);
            }

            List<int> pairsSums = new List<int>();

            try
            {
                using (StreamReader sr = new StreamReader(fromFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        var pair = line.Split(' ');

                        pairsSums.Add(int.Parse(pair[0]) * int.Parse(pair[1]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            using (StreamWriter sw = new StreamWriter(toFilePath))
            {
                foreach (var sum in pairsSums)
                {
                    sw.WriteLine(sum);
                }
            }

            manualEvent.Set();
        }

        static void Main(string[] args)
        {
            try
            {
                string pairs_txt = "pairs.txt";
                string pairsSums_txt = "pairsSums.txt";
                string pairsProds_txt = "pairsProds.txt";

                var thread1 = new Thread(() => GeneratePairs(pairs_txt));
                var thread2 = new Thread(() => SumPairs(pairs_txt, pairsSums_txt));
                var thread3 = new Thread(() => ProductPairs(pairs_txt, pairsProds_txt));

                thread1.Start();
                thread2.Start();
                thread3.Start();

                thread1.Join();
                thread2.Join();
                thread3.Join();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }
}