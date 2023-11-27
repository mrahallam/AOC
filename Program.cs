using System.Diagnostics;

namespace AOC2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set Current Day Number and whether to use the actual data or test data file
            int DayNumber = 20;
            bool TestData = true;
            List<string> InputData = SupportRoutines.LoadDataIntoArray(DayNumber, TestData);
            Stopwatch t = Stopwatch.StartNew();
            switch (DayNumber)
            {
                case 1:
                    Solutions.Day01aSolution(ref InputData);
                    Solutions.Day01bSolution(ref InputData);
                    break;
                case 2:
                    Solutions.Day02aSolution(ref InputData);
                    Solutions.Day02bSolution(ref InputData);
                    break;
                case 3:
                    Solutions.Day03aSolution(ref InputData);
                    Solutions.Day03bSolution(ref InputData);
                    break;
                case 4:
                    Solutions.Day04aSolution(ref InputData);
                    Solutions.Day04bSolution(ref InputData);
                    break;
                case 5:
                    Solutions.Day05aSolution(ref InputData);
                    Solutions.Day05bSolution(ref InputData);
                    break;
                case 6:
                    Solutions.Day06aSolution(ref InputData);
                    Solutions.Day06bSolution(ref InputData);
                    break;
                case 7:
                    Solutions.Day07Solution(ref InputData);
                    Solutions.Day07SolutionNSW(ref InputData);
                    break;
                case 8:
                    Solutions.Day08aSolution(ref InputData);
                    Solutions.Day08bSolution(ref InputData);
                    break;
                case 9:
                    Solutions.Day09aSolution(ref InputData);
                    Solutions.Day09bSolution(ref InputData);
                    // Solutions.Day09aSolutionBounded(ref InputData);
                    break;
                case 10:
                    Solutions.Day10Solution(ref InputData);
                    break;
                case 11:
                    Solutions.Day11aSolution(ref InputData);
                    Solutions.Day11bSolution(ref InputData);
                    break;
                case 13:
                    Solutions.Day13Solution();
                    break;
                case 17:
                    Solutions.Day17Solution('a', ref InputData);
                    Solutions.Day17Solution('b', ref InputData);
                    break;
                case 18:
                    Solutions.Day18Solution(ref InputData);
                    break;
                case 19:
                    Solutions.Day19Solution('a', ref InputData);
                    Solutions.Day19Solution('b', ref InputData);
                    break;
                case 20:
                    Solutions.Day20Solution('a', ref InputData);
                    Solutions.Day20Solution('b', ref InputData);
                    break;
                default:
                    Console.WriteLine("Error: There is no solution for this day yet.");
                    break;
            }
            t.Stop();
            Console.WriteLine("=== Completed in " + t.ElapsedMilliseconds.ToString() + "ms ===");
            Console.WriteLine("===============================");

            // Test random stuff down here..
            //Console.WriteLine("\nRandom stuff down here..");
            //var graph = SupportRoutines.LoadFileIntoArray("graph");
            //Solutions.Dijkstra(ref graph);
        }
    }
}