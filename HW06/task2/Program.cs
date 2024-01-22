namespace task2
{
    internal class Program
    {
        static int busCapacity = 20;
        
        static int peopleAtStop = 0;
        
        static object lockObject = new object();

        static AutoResetEvent busArrived = new AutoResetEvent(false);

        static void Main()
        {
            Thread busThread = new Thread(BusArrives);
            Thread peopleThread = new Thread(PeopleArrive);

            busThread.Start();
            peopleThread.Start();

            busThread.Join();
            peopleThread.Join();
        }

        static void BusArrives()
        {
            while (true)
            {
                Console.WriteLine("Автобус прибув");
                busArrived.Set();
                Thread.Sleep(1000);
            }
        }

        static void PeopleArrive()
        {
            Random random = new Random();
            while (true)
            {
                lock (lockObject)
                {
                    peopleAtStop += random.Next(1, 15);
                    Console.WriteLine($"На зупинці {peopleAtStop} людей");
                }

                busArrived.WaitOne();

                lock (lockObject)
                {
                    int peopleBoarding = Math.Min(busCapacity, peopleAtStop);
                    peopleAtStop -= peopleBoarding;
                    Console.WriteLine($"{peopleBoarding} людей заходять в автобус");
                }
            }
        }
    }
}
