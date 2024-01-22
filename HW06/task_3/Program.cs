namespace task_3
{
    record Person(Bus expectedBus);
    record Bus(string number, int capacity = 20);

    internal class Program
    {
        static IReadOnlyList<Bus> Buses { get; } = new List<Bus>()
        {
            new Bus("#123"),
            new Bus("#456"),
            new Bus("#789")
        };

        static List<Person> People { get; } = new List<Person>();

        static object lockObject = new object();

        static Dictionary<Bus, AutoResetEvent> busArrivedEvents = new Dictionary<Bus, AutoResetEvent>()
        {
            { Buses[0], new AutoResetEvent(false) },
            { Buses[1], new AutoResetEvent(false) },
            { Buses[2], new AutoResetEvent(false) },
        };

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
                for (int i = 0; i < Buses.Count; i++)
                {
                    Console.WriteLine($"Автобус {Buses[i].number} прибув");
                    busArrivedEvents[Buses[i]].Set();
                    Thread.Sleep(1000);
                }
            }
        }

        static void PeopleArrive()
        {
            Random random = new Random();
            while (true)
            {
                lock (lockObject)
                {
                    for (int i = 1; i <= 16; i++)
                    {
                        var expectedBus = Buses[Random.Shared.Next(Buses.Count)];

                        People.Add(new Person(expectedBus));
                    }

                    Console.WriteLine($"На зупинці {People.Count} людей");
                }

                foreach (var bus in Buses)
                {
                    busArrivedEvents[bus].WaitOne();

                    lock (lockObject)
                    {
                        var waitingPeople = People.FindAll(p => p.expectedBus == bus);

                        int peopleBoarding = Math.Min(bus.capacity, waitingPeople.Count);
                        People.RemoveRange(0, peopleBoarding);

                        Console.WriteLine($"{peopleBoarding} людей заходять в автобус");
                    }
                }
            }
        }
    }
}