using System;
using System.Net;
using System.Xml.Linq;

namespace AOC2022
{
    internal class SupportRoutines
    {
        public class Node
        {
            public long n;
            public Node left;
            public Node right;

            public Node(long n)
            {
                this.n = n;
                this.left = null;
                this.right = null;
            }
        }
        public class Monkey
        {
            //member variables
            public int name;
            public Queue<long> startingItems;
            public char operation;
            public int operationValue;
            public int test;
            public int ifTrue;
            public int ifFalse;
            public int CurrentWorryLevel;
            public int inspectionCount;

            //constuctor for initializing fields
            public Monkey()
            {
                name = 0;
                startingItems = new Queue<long>();
                operation = '+';
                operationValue = 1;
                test = 1;
                ifTrue = 1;
                ifFalse = 1;
                CurrentWorryLevel = 0;
                inspectionCount = 0;
            }

            // method for displaying a monkey (functionality)
            public void displayData()
            {
                Console.WriteLine("name = " + name);
                Console.WriteLine("Starting items=" + string.Join(", ", startingItems));
                Console.WriteLine("Operation=" + operation + " " + operationValue);
                Console.WriteLine("test=" + test);
                Console.WriteLine("If true, throw to monkey " + ifTrue);
                Console.WriteLine("If false, throw to monkey " + ifFalse);
                Console.WriteLine("Current worry level=" + CurrentWorryLevel);
                Console.WriteLine("Inspection count =" + inspectionCount);
                Console.WriteLine();
            }
            public int Test(int worryLevel)
            {
                return worryLevel % test == 0 ? ifTrue : ifFalse;

            }
        }
    public class fso
        {
            private string _name = "";
            private int _size = 0;
            private bool _file = false;
            private fso _parent;
            private IDictionary<string, fso> _members = new Dictionary<string, fso>();
            public Boolean IsFile
            {
                get { return _file; }
            }
            public fso(string name, int filesize, bool file, fso parent)
            {
                _name = name;
                _size = filesize;
                _file = file;
                if (!file)
                    _parent = parent;
            }
            public fso CD(string dirname)
            {
                if (dirname == "..")
                    return _parent;
                else
                    return _members[dirname];
            }
            public void AddFileOrDirectory(string name, int filesize, bool file)
            {
                if (!_members.ContainsKey(name))
                    _members.Add(name, new fso(name, filesize, file, this));
            }
            public int Size()
            {
                if (_file)
                    return _size;
                else
                {
                    int totalsize = 0;
                    foreach (fso member in _members.Values)
                        totalsize += member.Size();
                    return totalsize;
                }
            }
            public int SizeLessThan(int requiredsize)
            {
                int totalsize = 0;
                if (!_file)
                {
                    int thissize = this.Size();
                    if (thissize <= requiredsize)
                        totalsize += thissize;
                    foreach (fso member in _members.Values)
                        if (!member.IsFile)
                            totalsize += member.SizeLessThan(requiredsize);
                }
                return totalsize;
            }
            public void DirectoryToDelete(int SizeNeeded, ref int Smallestsize)
            {
                if (!_file)
                {
                    int thissize = this.Size();
                    if ((thissize >= SizeNeeded) & (thissize < Smallestsize))
                        Smallestsize = thissize;
                    foreach (fso member in _members.Values)
                        if (!member.IsFile)
                            member.DirectoryToDelete(SizeNeeded, ref Smallestsize);
                }
            }
        }

        public static long lcm_of_array_elements(int[] element_array)
        {
            long lcm_of_array_elements = 1;
            int divisor = 2;

            while (true)
            {

                int counter = 0;
                bool divisible = false;
                for (int i = 0; i < element_array.Length; i++)
                {

                    // lcm_of_array_elements (n1, n2, ... 0) = 0.
                    // For negative number we convert into
                    // positive and calculate lcm_of_array_elements.
                    if (element_array[i] == 0)
                    {
                        return 0;
                    }
                    else if (element_array[i] < 0)
                    {
                        element_array[i] = element_array[i] * (-1);
                    }
                    if (element_array[i] == 1)
                    {
                        counter++;
                    }

                    // Divide element_array by devisor if complete
                    // division i.e. without remainder then replace
                    // number with quotient; used for find next factor
                    if (element_array[i] % divisor == 0)
                    {
                        divisible = true;
                        element_array[i] = element_array[i] / divisor;
                    }
                }

                // If divisor able to completely divide any number
                // from array multiply with lcm_of_array_elements
                // and store into lcm_of_array_elements and continue
                // to same divisor for next factor finding.
                // else increment divisor
                if (divisible)
                {
                    lcm_of_array_elements = lcm_of_array_elements * divisor;
                }
                else
                {
                    divisor++;
                }

                // Check if all element_array is 1 indicate
                // we found all factors and terminate while loop.
                if (counter == element_array.Length)
                {
                    return lcm_of_array_elements;
                }
            }
        }

        // find vertex with min dist from set of vertices not in shortest path tree
        public static int minDistance(int[] dist,
                        bool[] sptSet, int V)
        {
            // Initialize min value
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < V; v++)
                if (sptSet[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v;
                }

            return min_index;
        }

        // Dijkstra's single source shortest path algorithm for a graph represented with adjacency matrix
        public static Dictionary<int, int> dijkstra(int[,] graph, int src, int V)
        {
            // array to hold shortest distance from src to i
            int[] dist = new int[V];

            // true if i included in shortest path tree or shortest distance from src to i in finalized
            bool[] sptSet = new bool[V];

            // all distances infinite and stpSet[] as false
            for (int i = 0; i < V; i++)
            {
                dist[i] = int.MaxValue;
                sptSet[i] = false;
            }

            // distance from self always 0
            dist[src] = 0;

            // Find shortest path for all vertices
            for (int count = 0; count < V - 1; count++)
            {
                // choose min dist vertex from set not yet processed, u=0 for first iteration
                int u = minDistance(dist, sptSet, V);

                // flag the picked vertex as processed
                sptSet[u] = true;

                // update dist value of the adjacent vertices of the picked vertex.
                for (int v = 0; v < V; v++)

                    // update dist[v] only if:
                    //     is not in sptSet,
                    //     there is an edge from u to v,
                    //     total weight of path from src to v through u < current dist[v]
                    if (!sptSet[v] && graph[u, v] != 0 &&
                         dist[u] != int.MaxValue && dist[u] + graph[u, v] < dist[v])
                        dist[v] = dist[u] + graph[u, v];
            }

            // return the completed distance dictionary
            Dictionary<int, int> solution = new Dictionary<int, int>();
            for (int i = 0; i < V; i++)
            {
                solution[i] = dist[i];
            }
            return solution;
        }
        public static List<string> LoadFileIntoArray(string FileName)
        {
            List<string> InputData = new List<string>();
            string FilePath = "..//..//..//Data//" + FileName + ".txt";
            // string FilePath = "Data//" + FileName + ".txt";
            foreach (string line in System.IO.File.ReadLines(FilePath))
            {
                InputData.Add(line);
            }
            Console.WriteLine("==== data loaded from '" + FileName + ".txt' ====");
            return InputData;
        }
        public static List<string> LoadDataIntoArray(int DayNumber, bool TestData)
        {
            List<string> InputData = new List<string>();
            // string FilePath = "..//..//..//Data//" + (TestData ? "TestData" : "InputData") + DayNumber.ToString() + ".txt";
            string FilePath = "Data//" + (TestData ? "TestData" : "InputData") + DayNumber.ToString() + ".txt";
            foreach (string line in System.IO.File.ReadLines(FilePath))
            {
                InputData.Add(line);
            }
            Console.WriteLine("==== Day " + DayNumber.ToString() + (TestData ? " Test" : "") + " data loaded ====");
            return InputData;
        }
        // swap two values in an array
        static void swap(List<int> arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
        /*   This function takes last element as pivot, places
             the pivot element at its correct position in sorted
             array, and places all smaller (smaller than pivot)
             to left of pivot and all greater elements to right
             of pivot */
        static int partition(List<int> arr, int low, int high)
        {
            //pivot element
            int pivot = arr[high];
            //index of smaller element
            int i = (low - 1);
            for (int j = low; j <= high - 1; j++)
            {
                //is current element smaller than the pivot
                if (arr[j] < pivot)
                {
                    //increment index of smaller element
                    i++;
                    swap(arr, i, j);
                }
            }
            swap(arr, i + 1, high);
            return (i + 1);
        }
        //quicksort recursive function
        public static void quickSort(List<int> arr, int low, int high)
        {
            if (low < high)
            {
                //partition index, arr[p] is now in correct position
                int i = partition(arr, low, high);

                //sort elements before and after partition
                quickSort(arr, low, i - 1);
                quickSort(arr, i + 1, high);
            }
        }
        // count true in bool[,]
        public static int countVisible(bool[,] boolGrid)
        {
            int count = 0;
            for (int i = 0; i < boolGrid.GetLength(0); i++)
            {
                for (int j = 0; j < boolGrid.GetLength(1); j++)
                {
                    count += boolGrid[i, j] ? 1 : 0;
                }
            }
            return count;
        }
        // print grid of integers
        public static void printGridInt(int[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    line += grid[i, j];
                }
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
        //print grid of bools
        public static void printGridBool(bool[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    line += grid[i, j] ? 1 : 0;
                }
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
    }
}
