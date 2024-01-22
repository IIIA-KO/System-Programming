namespace task_4
{
    record Person(Bus expectedBus);
    //record Bus(string number, int peopleCount = 0, int capacity = 20);

    class Bus
    {
        public string Number { get; set; }
        public int PeopleCount { get; set; }
        public int Capacity { get; set; }

        public Bus(string number, int peopleCount = 0, int capacity = 20)
        {
            Number = number;
            PeopleCount = peopleCount;
            Capacity = capacity;
        }
    }


    internal class Program
    {
        static IReadOnlyList<Bus> Buses { get; } = new List<Bus>()
        {
            new Bus("#123", Random.Shared.Next(10), 30),
            new Bus("#456"),
            new Bus("#789", 5)
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
                    Console.WriteLine($"Автобус {Buses[i].Number} прибув з {Buses[i].PeopleCount}/{Buses[i].Capacity} людьми всередині");
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

                        int peopleBoarding = Math.Min((bus.Capacity - bus.PeopleCount), waitingPeople.Count);
                        bus.PeopleCount += peopleBoarding;
                        People.RemoveRange(0, peopleBoarding);

                        Console.WriteLine($"{peopleBoarding} людей заходять в автобус");
                    }
                }
            }
        }
    }
}