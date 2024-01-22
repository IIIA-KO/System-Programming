namespace task4
{
    internal class Program
    {
        static Mutex _mutex = new Mutex();

        static void WriteNumbersToFile(string path)
        {
            _mutex.WaitOne();
            try
            {

                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }

                using (StreamWriter sw = new StreamWriter(path))
                {
                    for (int i = 1; i <= 1000; i++)
                    {
                        sw.WriteLine(i.ToString());
                    }
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        static void WritePrimeNumberToFile(string fromFilePath, string toFilePath)
        {
            _mutex.WaitOne();
            try
            {

                if (!File.Exists(fromFilePath))
                {
                    throw new FileNotFoundException($"File {fromFilePath} was not found", fromFilePath);
                }

                var numbers = new List<int>();
                using (StreamReader sr = new StreamReader(fromFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        if (!int.TryParse(sr.ReadLine(), out int num))
                        {
                            throw new InvalidDataException("Invalid data format from file");
                        }

                        if (IsPrime(num))
                        {
                            numbers.Add(num);
                        }
                    }
                }

                using (StreamWriter sw = new StreamWriter(toFilePath))
                {
                    foreach (int num in numbers)
                    {
                        sw.WriteLine(num);
                    }
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
        static bool IsPrime(int num)
        {
            if (num <= 1)
            {
                return false;
            }

            for (int i = 2; i <= Math.Sqrt(num); i++)
            {
                if (num % i == 0)
                {
                    return false;
                }
            }

            return true;
        }


        static void WritePrimeNumbersEnding7ToFile(string fromFilePath, string toFilePath)
        {
            _mutex.WaitOne();

            try
            {

                if (!File.Exists(fromFilePath))
                {
                    throw new FileNotFoundException($"File {fromFilePath} was not found", fromFilePath);
                }

                var numbers = new List<int>();
                using (StreamReader sr = new StreamReader(fromFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        if (!int.TryParse(sr.ReadLine(), out int num))
                        {
                            throw new InvalidDataException("Invalid data format from file");
                        }

                        if (IsEndingWith7(num))
                        {
                            numbers.Add(num);
                        }
                    }
                }

                using (StreamWriter sw = new StreamWriter(toFilePath))
                {
                    foreach (int num in numbers)
                    {
                        sw.WriteLine(num);
                    }
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        static bool IsEndingWith7(int num)
        {
            if (num <= 0)
            {
                return false;
            }

            if (num == 7)
            {
                return true;
            }

            if (num % 10 == 7)
            {
                return true;
            }

            return false;
        }

        static void MakeReport(string numberFilePath, string primeNumberFilePath, string primeNumberEnding7FilePath)
        {
            _mutex.WaitOne();

            try
            {
                List<string> reports =
                [
                    $"Кількість чисел у файлі 'numbers.txt': {CountNumbersInFile(numberFilePath)}",
                    $"Розмір файлу 'numbers.txt' в байтах: {FileSizeInBytes(numberFilePath)}",
                    $"Вміст файлу 'numbers.txt': {FileContent(numberFilePath)}",
                    $"Кількість чисел у файлі 'primes.txt': {CountNumbersInFile(primeNumberFilePath)}",
                    $"Розмір файлу 'primes.txt' в байтах: {FileSizeInBytes(primeNumberFilePath)}",
                    $"Вміст файлу 'primes.txt': {FileContent(primeNumberFilePath)}",
                    $"Кількість чисел у файлі 'filtered_primes.txt': {CountNumbersInFile(primeNumberEnding7FilePath)}",
                    $"Розмір файлу 'filtered_primes.txt' в байтах: {FileSizeInBytes(primeNumberEnding7FilePath)}",
                    $"Вміст файлу 'filtered_primes.txt': {FileContent(primeNumberEnding7FilePath)}",
                ];

                using (StreamWriter writer = new StreamWriter("report.txt"))
                {
                    foreach (string report in reports)
                    {
                        writer.WriteLine(report);
                    }
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        static int CountNumbersInFile(string filename)
        {
            int count = 0;
            using (StreamReader reader = new StreamReader(filename))
            {
                while (reader.ReadLine() != null)
                {
                    count++;
                }
            }
            return count;
        }

        static long FileSizeInBytes(string filename)
        {
            return new FileInfo(filename).Length;
        }

        static string FileContent(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                return reader.ReadToEnd();
            }
        }

        static void Main(string[] args)
        {
            try
            {
                string numbers_txt = "numbers.txt";
                string primeNumbers_txt = "primeNumbers.txt";
                string primeNumbesrEndingWith7_txt = "primeNumbersEndingWith7.txt";

                var thread1 = new Thread(() => WriteNumbersToFile(numbers_txt));
                var thread2 = new Thread(() => WritePrimeNumberToFile(numbers_txt, primeNumbers_txt));
                var thread3 = new Thread(() => WritePrimeNumbersEnding7ToFile(primeNumbers_txt, primeNumbesrEndingWith7_txt));
                var thread4 = new Thread(() => MakeReport(numbers_txt, primeNumbers_txt, primeNumbesrEndingWith7_txt));

                thread1.Start();
                thread2.Start();
                thread3.Start();
                thread4.Start();

                thread1.Join();
                thread2.Join();
                thread3.Join();
                thread4.Join();
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
