using System.Diagnostics;
using System.Windows.Input;
using zcarey_Advent_of_Code_2023;

static void Benchmark(Func<string, object> func, string input, int day, int part)
{
    Console.WriteLine($"Running Day {day} part {part}:");
    Console.WriteLine("Running...");
    Stopwatch timer = new Stopwatch();
    timer.Start();
    object result = func(input);
    timer.Stop();
    TimeSpan delta = new TimeSpan(timer.Elapsed.Ticks);
    Console.WriteLine($"Execution time: {delta.Hours}:{delta.Minutes} h:m {delta.Seconds}:{delta.Milliseconds}::{delta.Nanoseconds} s:ms::ns");
    Console.WriteLine("Answer: " + result.ToString());
}

Type[] DayTypes = new Type[25];
var types = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(s => s.GetTypes())
    .Where(p => typeof(AdventOfCodeProblem).IsAssignableFrom(p))
    .Where(t => t.GetConstructor(Type.EmptyTypes) != null);

foreach (var type in types)
{
    if (type.Name.StartsWith("Day"))
    {
        int day;
        if (int.TryParse(type.Name.Substring(3), out day))
        {
            DayTypes[day] = type;
        }
    }
}

while (true)
{
    Console.WriteLine("Enter the day (1-25) to run followed by the part (1-2). If no part is entered, both parts will be ran.");
    string input = Console.ReadLine() ?? "";

    string[] inputArgs = input.Split(' ');
    if (inputArgs.Length < 1)
    {
        Console.WriteLine("Enter at least one argument.");
        continue;
    }

    int dayInput;
    int partInput;
    if (!int.TryParse(inputArgs[0], out dayInput))
    {
        Console.WriteLine("Bad input[0]");
        continue;
    }

    if (inputArgs.Length > 1)
    {
        if (!int.TryParse(inputArgs[1], out partInput))
        {
            Console.WriteLine("Bad input[1]");
            continue;
        }
        if (partInput < 1 || partInput > 3)
        {
            Console.WriteLine("Bad input[1]");
            continue;
        }
    } else
    {
        partInput = 3;
    }

    if (DayTypes[dayInput] == null)
    {
        Console.WriteLine("No code for that day was found!");
        continue;
    }

    AdventOfCodeProblem? problem = (AdventOfCodeProblem)Activator.CreateInstance(DayTypes[dayInput]);
    if (problem == null)
    {
        Console.WriteLine("Error occured instantiating type!");
        continue;
    }

    // Load input data
    string inputText;
    try
    {
        inputText = File.ReadAllText($"input/Day{dayInput:00}.txt");
    }catch(Exception e)
    {
        Console.WriteLine("Failed to load input data! " + e.Message);
        continue;
    }

    if ((partInput & 1) > 0)
    {
        Benchmark(problem.Part1, inputText, dayInput, 1);
        Console.WriteLine();
        Console.WriteLine();
    }
    if ((partInput & 2) > 0)
    {
        Benchmark(problem.Part2, inputText, dayInput, 2);
        Console.WriteLine();
        Console.WriteLine();
    }

    break;
}
