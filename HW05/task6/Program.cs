namespace task6
{
    class Program
    {
        static void Main()
        {
            Casino casino = new Casino();
            casino.StartDay();
            casino.EndDay();
        }
    }

    class Casino
    {
        private List<Player> players;
        private List<Player> allPlayers;
        private int totalPlayers;
        private Random random;
        private object lockObject;

        public Casino()
        {
            players = new List<Player>();
            allPlayers = new List<Player>();

            totalPlayers = new Random().Next(5, 11);
            random = new Random();
            lockObject = new object();
        }

        public void StartDay()
        {
            for (int i = 0; i < totalPlayers; i++)
            {
                Thread thread = new Thread(new ThreadStart(Play));
                thread.Start();
            }
        }

        private void Play()
        {
            Player player = new Player(random.Next(100, 501));
            lock (lockObject)
            {
                players.Add(player);
                allPlayers.Add(player);
                while (players.Count > 5)
                {
                    Monitor.Wait(lockObject);
                }
            }

            while (player.Money > 0)
            {
                int bet = random.Next(1, player.Money + 1);
                int number = random.Next(0, 37);
                int rouletteNumber = random.Next(0, 37);

                if (number == rouletteNumber)
                {
                    player.Money += bet;
                }
                else
                {
                    player.Money -= bet;
                }

                Console.WriteLine($"Гравець {player.Id}: ставка - {bet}, число - {number}, результат - {rouletteNumber}, гроші - {player.Money}");
            }

            lock (lockObject)
            {
                players.Remove(player);
                Monitor.PulseAll(lockObject);
            }
        }

        public void EndDay()
        {
            using (StreamWriter writer = new StreamWriter("report.txt"))
            {
                foreach (Player player in allPlayers)
                {
                    writer.WriteLine($"Гравець {player.Id} [{player.StartingMoney}] [{player.Money}]");
                }
            }
        }
    }

    class Player
    {
        private static int counter = 1;
        public int Id { get; private set; }
        public int Money { get; set; }
        public int StartingMoney { get; private set; }

        public Player(int money)
        {
            Id = counter++;
            Money = money;
            StartingMoney = money;
        }
    }
}