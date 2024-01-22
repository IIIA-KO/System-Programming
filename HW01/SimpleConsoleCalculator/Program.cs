namespace SimpleConsoleCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: CalculatorApp <int operand1> <operator> <int operand2>");
                Environment.Exit(int.MinValue);
            }

            if (int.TryParse(args[0], out int value1) && int.TryParse(args[2], out int value2))
            {
                switch (args[1])
                {
                    case "+":
                        Environment.Exit(value1 + value2);
                        break;

                    case "-":
                        Environment.Exit(value1 - value2);
                        break;

                    case "*":
                        Environment.Exit(value1 * value2);
                        break;

                    case "/":
                        if (value2 == 0)
                        {
                            Console.WriteLine("Error: Division by zero");
                            Environment.Exit(int.MinValue);
                        }
                        else
                        {
                            Environment.Exit(value1 / value2);
                        }
                        break;

                    default:
                        Console.WriteLine("Error: invalid operator format");
                        Environment.Exit(int.MinValue);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid input format");
                Environment.Exit(int.MinValue);
            }
        }
    }
}