using System.Drawing;

namespace AOC2022;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.IO;
using static SupportRoutines;

internal class Solutions
{
    public static void Day20Solution(char part, ref List<string> Data)
    {
        //List<string> input = new List<string>();
        //for (int i = 0; i < Data.Count; i++)
        //{
        //    input.Add(Data[i]);
        //}

        //var result1 = Mixing(input);
        //var result2 = Mixing(input, 811589153, 10);

        //if (part == 'a')
        //{
        //    Console.WriteLine("===============================");
        //    Console.WriteLine("Part a solution: " + (result1));
        //    Console.WriteLine("===============================");
        //}
        //else
        //{

        //    Console.WriteLine("===============================");
        //    Console.WriteLine("Part b solution: " + (result2));
        //    Console.WriteLine("===============================");
        //}

        //static Int64 Mixing(List<string> input, Int64 decryptionKey = 1, int mixCount = 1)
        //{
        //    var parsedInput = input.Select(e => Int64.Parse(e) * decryptionKey).ToList();
        //    var encryptedFile = new List<(Int64 value, int index)>();

        //    for (int i = 0; i < parsedInput.Count; i++)
        //        encryptedFile.Add((parsedInput[i], i));

        //    var listToMix = new List<(Int64 value, int index)>(encryptedFile);
        //    var count = encryptedFile.Count;

        //    for (int mc = 0; mc < mixCount; mc++)
        //    {
        //        for (int i = 0; i < count; i++)
        //        {
        //            var number = encryptedFile[i];
        //            var oldIndex = listToMix.IndexOf(number);
        //            var newIndex = (oldIndex + number.value) % (count - 1);

        //            if (newIndex < 0)
        //                newIndex = count + newIndex - 1;

        //            listToMix.Remove(number);
        //            listToMix.Insert((int)newIndex, number);
        //        }
        //    }

        //var indexZero = listToMix.FindIndex(e => e.value == 0);
        //var index1000 = (1000 + indexZero) % count;
        //var index2000 = (2000 + indexZero) % count;
        //var index3000 = (3000 + indexZero) % count;

        //var coordinatesSum = listToMix[index1000].value + listToMix[index2000].value + listToMix[index3000].value;

        //return coordinatesSum;
        //}
        List<Node> mixList = new List<Node>();
        for (int i = 0; i < Data.Count; i++)
        {
            if (part == 'a')
            {
                mixList.Add(new Node(Convert.ToInt64(Data[i])));
            }
            else if (part == 'b')
            {
                mixList.Add(new Node(Convert.ToInt64(Data[i]) * 811589153));
            }

        }
        for (int i = 0; i < mixList.Count; i++)
        {
            mixList[i].right = mixList[(i + 1) % mixList.Count];
            mixList[i].left = mixList[(i - 1 + mixList.Count) % mixList.Count];
        }
        int m = mixList.Count - 1;
        Node zeroNode = null;
        for (int cycleCount = 0; cycleCount < (part == 'a' ? 1 : 10); cycleCount++)
        {
            for (int i = 0; i < mixList.Count; i++)
            {
                Node k = mixList[i];
                if (k.n == 0)
                {
                    zeroNode = k;
                    Console.WriteLine("zeroNode = " + zeroNode.right.n);
                    continue;
                }
                Node p = k;
                if (k.n > 0)
                {
                    for (int j = 0; j < k.n % m; j++)
                    {
                        p = p.right;
                    }
                    if (k == p)
                    {
                        continue;
                    }
                    k.right.left = k.left;
                    k.left.right = k.right;
                    p.right.left = k;
                    k.right = p.right;
                    p.right = k;
                    k.left = p;
                }
                else
                {
                    for (int j = 0; j < -k.n % m; j++)
                    {
                        p = p.left;
                    }
                    if (k == p)
                    {
                        continue;
                    }
                    k.left.right = k.right;
                    k.right.left = k.left;
                    p.left.right = k;
                    k.left = p.left;
                    p.left = k;
                    k.right = p;
                }
                foreach (var node in mixList)
                {
                    Console.Write("(" + node.left.n + ", " + node.n + ", " + node.right.n + ")");
                }
                Console.WriteLine();
            }
            //Console.WriteLine("zeroNode = " + zeroNode.n);
        }
        long t = 0;
        mixList.Clear();
        //Console.WriteLine(zeroNode.left.left.left.left.left.left.n + ", " + zeroNode.left.left.left.left.left.n + ", " + zeroNode.left.left.left.left.n + ", " + zeroNode.left.left.left.n + ", " + zeroNode.left.left.n + ", " + zeroNode.left.n + ", " + zeroNode.n + ", " + zeroNode.right.n + ", " + zeroNode.right.right.n);
        //Console.WriteLine(mixList.Count());
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 1000; j++)
            {
                zeroNode = zeroNode.right;
            }
            //Console.WriteLine("t = " + zeroNode.n);
            t += zeroNode.n;
        }

        //Console.WriteLine(t);
        if (part == 'a')
        {
            Console.WriteLine("===============================");
            Console.WriteLine("Part a solution: " + (t));
            Console.WriteLine("===============================");
        }
        else
        {

            Console.WriteLine("===============================");
            Console.WriteLine("Part b solution: " + (t));
            Console.WriteLine("===============================");
        }

    }
    public static void Day19Solution(char part, ref List<string> Data)
    {
        if (part == 'a')
        {
            var total = 0;
            for (int i = 0; i < Data.Count; i++)
            {
                var bluePrint = new List<List<Tuple<int, int>>>();
                var maxSpend = new List<int>() { 0, 0, 0 };
                foreach (string section in Data[i].Split(": ")[1].Split(". "))
                {
                    var recipe = new List<Tuple<int, int>>();
                    var pattern = @"(\d+) (\w+)";
                    var robots = new List<string>() { "ore", "clay", "obsidian" };
                    var values = Regex.Matches(section, pattern);
                    foreach (var match in values)
                    {
                        var stringToSplit = match.ToString().Split(" ");
                        var recipeElement = new Tuple<int, int>(Convert.ToInt32(stringToSplit[0]), robots.IndexOf(stringToSplit[1]));
                        recipe.Add(recipeElement);
                        maxSpend[recipeElement.Item2] = Math.Max(maxSpend[recipeElement.Item2], recipeElement.Item1);

                    }
                    bluePrint.Add(recipe);
                }
                var geodeCount = dfs(bluePrint, maxSpend, new Dictionary<string, int>(), 24, new List<int>() { 1, 0, 0, 0 }, new List<int>() { 0, 0, 0, 0 });
                total += (i + 1) * geodeCount;
            }
            Console.WriteLine("===============================");
            Console.WriteLine("Part a solution: " + (total));
            Console.WriteLine("===============================");
        }
        if (part == 'b')
        {
            var total = 1;
            for (int i = 0; i < Math.Min(3, Data.Count); i++)
            {
                var bluePrint = new List<List<Tuple<int, int>>>();
                var maxSpend = new List<int>() { 0, 0, 0 };
                foreach (string section in Data[i].Split(": ")[1].Split(". "))
                {
                    var recipe = new List<Tuple<int, int>>();
                    var pattern = @"(\d+) (\w+)";
                    var robots = new List<string>() { "ore", "clay", "obsidian" };
                    var values = Regex.Matches(section, pattern);
                    foreach (var match in values)
                    {
                        var stringToSplit = match.ToString().Split(" ");
                        var recipeElement = new Tuple<int, int>(Convert.ToInt32(stringToSplit[0]), robots.IndexOf(stringToSplit[1]));
                        recipe.Add(recipeElement);
                        maxSpend[recipeElement.Item2] = Math.Max(maxSpend[recipeElement.Item2], recipeElement.Item1);
                    }

                    bluePrint.Add(recipe);
                }
                var geodeCount = dfs(bluePrint, maxSpend, new Dictionary<string, int>(), 32, new List<int>() { 1, 0, 0, 0 }, new List<int>() { 0, 0, 0, 0 });
                Console.WriteLine("geodeCount = " + geodeCount);
                total *= geodeCount;
            }
            Console.WriteLine("Part b solution: " + (total));
            Console.WriteLine("===============================");
        }
        static int dfs(
                List<List<Tuple<int, int>>> bluePrint,
                List<int> maxSpend,
                Dictionary<string, int> cache,
                int time,
                List<int> bots,
                List<int> amt)
        {
            if (time == 0)
            {
                return amt[3];
            }
            var key = time + ", " + String.Join(", ", bots) + ", " + String.Join(", ", amt);
            //Console.WriteLine(key);
            //Thread.Sleep(100);
            if (cache.ContainsKey(key))
            {
                //Console.WriteLine("Found the key!");
                return cache[key];
            }
            var maxVal = amt[3] + bots[3] * time;
            for (int i = 0; i < bluePrint.Count; i++)
            {
                // Optimization, don't make more than max spend
                if (i != 3 && bots[i] >= maxSpend[i])
                {
                    continue;
                }
                var wait = 0;
                bool found = false;
                for (int j = 0; j < bluePrint[i].Count; j++)
                {
                    if (bots[bluePrint[i][j].Item2] == 0)
                    {
                        found = true;
                        break;
                    }
                    wait = Math.Max(wait, Convert.ToInt32(Math.Ceiling((bluePrint[i][j].Item1 - amt[bluePrint[i][j].Item2]) / Convert.ToDouble(bots[bluePrint[i][j].Item2]))));
                }
                if (!found)
                {
                    var remtime = time - wait - 1;
                    if (remtime <= 0)
                    {
                        continue;
                    }
                    List<int> bots_ = bots.ToList();
                    var amt_ = new List<int>();
                    var zippedList = amt.Zip(bots, (item1, item2) => new { Item1 = item1, Item2 = item2 });
                    foreach (var item in zippedList)
                    {
                        amt_.Add(item.Item1 + item.Item2 * (wait + 1));
                    }
                    for (int j = 0; j < bluePrint[i].Count; j++)
                    {
                        amt_[bluePrint[i][j].Item2] -= bluePrint[i][j].Item1;
                    }
                    bots_[i] += 1;
                    // Optimization, discard excess resources
                    for (int j = 0; j < 3; j++)
                    {
                        amt_[j] = Math.Min(amt_[j], maxSpend[j] * remtime);
                    }
                    maxVal = Math.Max(maxVal, dfs(bluePrint, maxSpend, cache, remtime, bots_, amt_));
                }
            }
            cache.Add(key, maxVal);
            //Console.WriteLine("Cache count = " + cache.Count);
            return maxVal;
        }
    }
    public static void Day17Solution(char part, ref List<string> Data)
    {
        // The shapes
        List<List<(int, int)>> shapes = new() {
            new List<(int, int)>() { (2, 0), (3, 0), (4, 0), (5, 0) },
            new List<(int, int)>() { (3, 0), (2, 1), (3, 1), (4, 1), (3, 2) },
            new List<(int, int)>() { (4, 2), (4, 1), (2, 0), (3, 0), (4, 0) },
            new List<(int, int)>() { (2, 0), (2, 1), (2, 2), (2, 3) },
            new List<(int, int)>() { (2, 0), (3, 0), (2, 1), (3, 1) }
            };
        Dictionary<Point, bool> tower = new Dictionary<Point, bool>(); // Dictionary to hold all the points in tower
        Dictionary<string, int> cycle = new Dictionary<string, int>();
        Dictionary<int, int> rockNumberTowerHeight = new Dictionary<int, int>();
        var CurrentShapeIndex = 0;
        var currentShape = TupleToPoint(shapes[CurrentShapeIndex]);
        var floor = 0;
        var rockCount = 1;
        var dirCounter = 0;
        var maxHeight = 0;
        var rocksToDrop = part == 'a' ? 2022 : 1_000_000_000_000;
        List<Point> TupleToPoint(List<(int, int)> TupleShape)
        {
            var PointShape = new List<Point>();
            foreach (var TuplePoint in TupleShape)
            {
                PointShape.Add(new Point(TuplePoint.Item1, TuplePoint.Item2));
            }
            return PointShape;
        }
        List<Point> translateShapeInitialPos(List<Point> shape)
        {
            for (int k = shape.Count - 1; k >= 0; k--)
            {
                shape[k] = new Point(shape[k].X, shape[k].Y + floor + 3);
            }
            return shape;
        }
        List<int> getBounds(List<Point> shape)
        {
            var minWidth = int.MaxValue;
            var maxWidth = 0;
            var minDepth = 0;
            var maxDepth = int.MaxValue;
            foreach (var point in shape)
            {
                if (point.X < minWidth)
                {
                    minWidth = point.X;
                }
                if (point.X > maxWidth)
                {
                    maxWidth = point.X;
                }
                if (point.Y > minDepth)
                {
                    minDepth = point.Y;
                }
                if (point.Y < maxDepth)
                {
                    maxDepth = point.Y;
                }
            }
            return new List<int> { minWidth, maxWidth, minDepth, maxDepth };
        }
        List<Point> translateShapePos(List<Point> shape, string direction)
        {
            var testShape = new List<Point>();
            bool canMove = true;
            if (direction == "<")
            {
                for (int k = shape.Count - 1; k >= 0; k--)
                {
                    var testPoint = new Point(shape[k].X - 1, shape[k].Y);
                    if (!tower.ContainsKey(testPoint) && testPoint.X >= 0)
                    {
                        testShape.Add(testPoint);
                    }
                    else
                    {
                        canMove = false;
                    }
                }
            }
            else if (direction == ">")
            {
                for (int k = shape.Count - 1; k >= 0; k--)
                {
                    var testPoint = new Point(shape[k].X + 1, shape[k].Y);
                    if (!tower.ContainsKey(testPoint) && testPoint.X <= 6)
                    {
                        testShape.Add(testPoint);
                    }
                    else
                    {
                        canMove = false;
                    }
                }
            }
            if (canMove)
            {
                shape = testShape;
            }
            dirCounter++;
            return shape;
        }
        // Print the game
        void PrintGameState()
        {
            for (int i = maxHeight; i >= 0; i--)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (tower.ContainsKey(new Point(j, i)))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine(" " + i);
            }
        }
        currentShape = translateShapeInitialPos(currentShape); //position the first shape relative to the floor
        // loop through the specified number of rocks
        while (rockCount <= rocksToDrop)
        {
            var dirIndex = dirCounter % Data[0].Length;
            currentShape = translateShapePos(currentShape, Data[0][dirIndex].ToString());
            var testShape = new List<Point>();
            bool validDrop = true;
            for (int k = currentShape.Count - 1; k >= 0; k--)
            {
                var testPoint = new Point(currentShape[k].X, currentShape[k].Y - 1);
                if (!tower.ContainsKey(testPoint))
                {
                    testShape.Add(testPoint);
                }
                else
                {
                    validDrop = false;
                }
            }
            if (validDrop && getBounds(currentShape)[3] > 0)
            {
                currentShape = testShape;
            }
            else
            {
                // put the shape into the tower dictionary, now that it has come to rest
                foreach (Point p in currentShape)
                {
                    // check to see if we have a full row, if so, delete all points in the tower below this row
                    if (!tower.ContainsKey(p))
                    {
                        tower.Add(p, true);
                        if (tower.ContainsKey(new Point((p.X + 1) % 7, p.Y))
                            && tower.ContainsKey(new Point((p.X + 2) % 7, p.Y))
                            && tower.ContainsKey(new Point((p.X + 3) % 7, p.Y))
                            && tower.ContainsKey(new Point((p.X + 4) % 7, p.Y))
                            && tower.ContainsKey(new Point((p.X + 5) % 7, p.Y))
                            && tower.ContainsKey(new Point((p.X + 6) % 7, p.Y)))
                        {
                            foreach (var item in tower.Where(kvp => kvp.Key.Y < p.Y).ToList())
                            {
                                tower.Remove(item.Key);
                            }
                        }
                    }
                    // update the maximum height of the tower
                    if (p.Y > maxHeight)
                    {
                        maxHeight = p.Y;
                    }
                }
                rockNumberTowerHeight.Add(rockCount, maxHeight + 1); // Dictionary to track the height of the tower after each rock comes to rest
                string cycleString = dirIndex.ToString() + CurrentShapeIndex.ToString(); // String to represent the top 30 rows of the game, index of direction and shape, for pattern matching
                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (tower.ContainsKey(new Point(j, maxHeight - i)))
                        {
                            cycleString += "#";
                        }
                        else
                        {
                            cycleString += ".";
                        }
                    }
                }
                if (!cycle.ContainsKey(cycleString))
                {
                    cycle.Add(cycleString, rockCount); // Add if it's not a repeat
                }
                else
                {
                    if (part == 'b') // Found a cycle, do the maths!
                    {
                        Console.WriteLine("Rock number " + rockCount.ToString() + ", height " + rockNumberTowerHeight[rockCount - 1] + " is a repeat of rock number " + cycle[cycleString].ToString() + ", height " + rockNumberTowerHeight[cycle[cycleString] - 1]);
                        var cycleCount = ((rocksToDrop) - (cycle[cycleString] - 1)) / ((rockCount - 1) - (cycle[cycleString] - 1));
                        var cycleHeight = rockNumberTowerHeight[rockCount - 1] - rockNumberTowerHeight[cycle[cycleString] - 1];
                        var remainder = ((rocksToDrop) - (cycle[cycleString] - 1)) % ((rockCount - 1) - (cycle[cycleString] - 1));
                        var initialAndRemainderHeight = rockNumberTowerHeight[Convert.ToInt32(remainder) + cycle[cycleString] - 1];
                        var cycleTotalHeight = cycleCount * cycleHeight;
                        Console.WriteLine("Part b solution: " + cycleTotalHeight + initialAndRemainderHeight);
                        Console.WriteLine("===============================");
                        break;
                    }
                }
                // update floor height and get next shape
                floor = maxHeight + 1;
                rockCount++;
                CurrentShapeIndex = (CurrentShapeIndex + 1) % 5;
                currentShape = TupleToPoint(shapes[CurrentShapeIndex]);
                currentShape = translateShapeInitialPos(currentShape);
            }
        }
        if (part == 'a')
        {
            //PrintGameState();
            Console.WriteLine("Part a solution: " + (maxHeight + 1));
            Console.WriteLine("===============================");
        }
    }
public static void Day18Solution(ref List<string> Data)
    {
        List<Tuple<int, int, int>> cubes = new();
        List<Tuple<int, int, int>> enclosingCube = new();
        List<Tuple<int, int, int>> airSpace = new();
        List<string> matching = new();
        // fill list of cubes
        for (int i = 0; i < Data.Count; i++)
        {
            string[] tempString = Data[i].Split(",");
            cubes.Add(new Tuple<int, int, int>(int.Parse(tempString[0]),
                int.Parse(tempString[1]),
                int.Parse(tempString[2])));
        }
        // helper functions
        bool isMatch(Tuple<int, int, int> t1, Tuple<int, int, int> t2)
        {
            if (t1.Item1 == t2.Item1
                    && t1.Item2 == t2.Item2
                    && Math.Abs(t1.Item3 - t2.Item3) == 1)
            {
                return true;
            }
            else if (t1.Item2 == t2.Item2
                && t1.Item3 == t2.Item3
                && Math.Abs(t1.Item1 - t2.Item1) == 1)
            {
                return true;
            }
            else if (t1.Item1 == t2.Item1
                && t1.Item3 == t2.Item3
                && Math.Abs(t1.Item2 - t2.Item2) == 1)
            {
                return true;
            }
            else return false;
        }
        // return neighbouring cubes
        List<Tuple<int, int, int>> getNeighbours(Tuple<int, int, int> cube)
        {
            List<Tuple<int, int, int>> neighbours = new();
            neighbours.Add(new Tuple<int, int, int>(cube.Item1 + 1, cube.Item2, cube.Item3));
            neighbours.Add(new Tuple<int, int, int>(cube.Item1 - 1, cube.Item2, cube.Item3));
            neighbours.Add(new Tuple<int, int, int>(cube.Item1, cube.Item2 + 1, cube.Item3));
            neighbours.Add(new Tuple<int, int, int>(cube.Item1, cube.Item2 - 1, cube.Item3));
            neighbours.Add(new Tuple<int, int, int>(cube.Item1, cube.Item2, cube.Item3 + 1));
            neighbours.Add(new Tuple<int, int, int>(cube.Item1, cube.Item2, cube.Item3 - 1));
            return neighbours;
        }// count internal edges
        int matchCount = 0;
        for (int i = 0; i < cubes.Count; i++)
        {
            for (int j = i+1; j < cubes.Count; j++)
            {
                if (isMatch(cubes[j], cubes[i]))
                {
                    matchCount++;
                }
            }
        }//get surface area of scanned droplets
        int surfaceArea = (cubes.Count * 6) - (matchCount * 2);
        Console.WriteLine("cubes = " + cubes.Count +
            ", shared sides = " + (matchCount * 2) +
            ", surface area = " + surfaceArea);

        // Part 2
        // get min and max x,y,z values
        int minX = int.MaxValue; int minY = int.MaxValue;
        int minZ = int.MaxValue; int maxX = int.MinValue;
        int maxY = int.MinValue; int maxZ = int.MinValue;
        for (int i = 0; i < cubes.Count; i++)
        {
            minX = cubes[i].Item1 < minX ? cubes[i].Item1 : minX;
            minY = cubes[i].Item2 < minY ? cubes[i].Item2 : minY;
            minZ = cubes[i].Item3 < minZ ? cubes[i].Item3 : minZ;
            maxX = cubes[i].Item1 > maxX ? cubes[i].Item1 : maxX;
            maxY = cubes[i].Item2 > maxY ? cubes[i].Item2 : maxY;
            maxZ = cubes[i].Item3 > maxZ ? cubes[i].Item3 : maxZ;
        }
        //flood fill start
        Queue<Tuple<int, int, int>> stillToCheck = new();
        List<Tuple<int, int, int>> outsideAir = new();
        stillToCheck.Enqueue(new Tuple<int, int, int>(minX - 1, minY - 1, minZ - 1));
        outsideAir.Add(new Tuple<int, int, int>(minX - 1, minY - 1, minZ - 1));
        while (stillToCheck.Count>0)
        {
            var current = stillToCheck.Dequeue();
            var newFoundAir = getNeighbours(current);
            for (int i=0;i<newFoundAir.Count;i++)
            {
                if (newFoundAir[i].Item1 >= minX-1 && newFoundAir[i].Item1 <= maxX+1
                    && newFoundAir[i].Item2 >= minY-1 && newFoundAir[i].Item2 <= maxY+1
                    && newFoundAir[i].Item3 >= minZ-1 && newFoundAir[i].Item3 <= maxZ+1
                    && !cubes.Contains(newFoundAir[i])
                    && !outsideAir.Contains(newFoundAir[i]))
                {
                    outsideAir.Add(newFoundAir[i]);
                    stillToCheck.Enqueue(newFoundAir[i]);
                }
            }
        }
        // get rid of the cubes outside the enclosing cube
        for (int i = outsideAir.Count - 1; i >= 0; i--)
        {
            if (outsideAir[i].Item1 < minX || outsideAir[i].Item1 > maxX
                || outsideAir[i].Item2 < minY || outsideAir[i].Item2 > maxY
                || outsideAir[i].Item3 < minZ || outsideAir[i].Item3 > maxZ)
            {
                outsideAir.RemoveAt(i);
            }
        }
        // get all cubes in enclosing cube
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int z = minZ; z <= maxZ; z++)
                {
                    enclosingCube.Add(new Tuple<int, int, int>(x, y, z));
                }
            }
        }
        // loop through all cubes and check to see if in
        // original cube list, or outside air-space
        for (int i = 0; i < enclosingCube.Count; i++)
        {
            if (!outsideAir.Contains(enclosingCube[i])
                && !cubes.Contains(enclosingCube[i]))
            {
                //identify internal air space cubes
                airSpace.Add(enclosingCube[i]);
            }
        }
        matchCount = 0; // reset matchCount
        // count internal edges
        for (int i = 0; i < airSpace.Count; i++)
        {
            for (int j = i + 1; j < airSpace.Count; j++)
            {
                if (isMatch(airSpace[j], airSpace[i]))
                {
                    matchCount++;
                }
            }
        }
        // get air-space surface area
        int innerSurfaceArea = ((airSpace.Count * 6) - (matchCount * 2));
        // subtract it from part 1 surface area
        Console.WriteLine("airspace cubes = " + airSpace.Count +
            ", shared sides = " + (matchCount * 2) +
            ", surface area = " + innerSurfaceArea +
            ", outer surface area = " + (surfaceArea - innerSurfaceArea));
    }
    // ripped off this!
    // https://github.com/oddrationale/AdventOfCode2022CSharp/blob/main/Day13.ipynb
    public static void Day13Solution()
    {

        var input = File.ReadAllText("..//..//..//Data//InputData13.txt")
            .Split("\n\n")
            .Select(pair => pair.Split("\n"))
            .Select(pair => (Left: JsonDocument.Parse(pair[0]).RootElement, Right: JsonDocument.Parse(pair[1]).RootElement));

        int comparePackets(JsonElement left, JsonElement right)
        {
            if (left.ValueKind == JsonValueKind.Number && right.ValueKind == JsonValueKind.Number)
            {
                return left.GetInt32() - right.GetInt32();
            }
            else if (left.ValueKind == JsonValueKind.Number)
            {
                return comparePackets(JsonDocument.Parse($"[{left.GetInt32()}]").RootElement, right);
            }
            else if (right.ValueKind == JsonValueKind.Number)
            {
                return comparePackets(left, JsonDocument.Parse($"[{right.GetInt32()}]").RootElement);
            }
            else
            {
                foreach (var (nextLeft, nextRight) in Enumerable.Zip(left.EnumerateArray(), right.EnumerateArray()))
                {
                    var current = comparePackets(nextLeft, nextRight);
                    if (current == 0)
                    {
                        continue;
                    }
                    else
                    {
                        return current;
                    }
                }

                return left.GetArrayLength() - right.GetArrayLength();
            }
        }

        var Part1 = input
            .Select((pair, i) => (Order: comparePackets(pair.Left, pair.Right), Index: i + 1))
            .Where(t => t.Order < 0)
            .Select(t => t.Index)
            .Sum();
        Console.WriteLine("Part a solution: " + Part1);
        Console.WriteLine("===============================");

        var packets = input
    .SelectMany(pair => new JsonElement[] { pair.Left, pair.Right })
    .Append(JsonDocument.Parse("[[2]]").RootElement)
    .Append(JsonDocument.Parse("[[6]]").RootElement)
    .OrderBy(packet => packet, Comparer<JsonElement>.Create((l, r) => comparePackets(l, r)));

        var divider1 = packets
            .Select((packet, index) => (Packet: packet, Index: index + 1))
            .First(item => comparePackets(item.Packet, JsonDocument.Parse("[[2]]").RootElement) == 0).Index;

        var divider2 = packets
            .Select((packet, index) => (Packet: packet, Index: index + 1))
            .First(item => comparePackets(item.Packet, JsonDocument.Parse("[[6]]").RootElement) == 0).Index;

        var Part2 = divider1 * divider2;

        Console.WriteLine("Part b solution: " + Part2);
        Console.WriteLine("===============================");

    }
    public static void Day11aSolution(ref List<string> Data)
    {
        int nRounds = 20;
        SupportRoutines.Monkey monkey = new SupportRoutines.Monkey();
        List<SupportRoutines.Monkey> round = new List<SupportRoutines.Monkey>();
        for (int i = 0; i < Data.Count; i++)
        {
            string[] numbers = Regex.Split(Data[i], @"\D+");
            int num = 0;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    num = int.Parse(value);
                }
            }
            if (i % 7 == 0)
            {
                monkey.name = num;
            }
            if (i % 7 == 1)
            {
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        int j = int.Parse(value);
                        monkey.startingItems.Enqueue(j);
                    }
                }
            }
            if (i % 7 == 2)
            {
                if (Data[i].Contains('+'))
                {
                    monkey.operation = '+';
                    monkey.operationValue = num;
                }
                else if (Data[i].Contains("* old"))
                {
                    monkey.operation = '*';
                    monkey.operationValue = monkey.CurrentWorryLevel;
                }
                else
                {
                    monkey.operation = '*';
                    monkey.operationValue = num;
                }
            }
            if (i % 7 == 3)
            {
                monkey.test = num;
            }
            if (i % 7 == 4)
            {
                monkey.ifTrue = num;
            }
            if (i % 7 == 5)
            {
                monkey.ifFalse = num;
            }
            if (i % 7 == 6 || i == Data.Count - 1)
            {
                round.Add(monkey);
                monkey = new SupportRoutines.Monkey();
            }
        }
        for (int j = 0; j < nRounds; j++)
        {
            foreach (var turn in round)
            {
                while (turn.startingItems.Count > 0)
                {
                    long item = turn.startingItems.Dequeue();
                    turn.inspectionCount++;
                    long worryLevel = 0;
                    if (turn.operation == '+')
                    {
                        worryLevel = (item + turn.operationValue) / 3;
                    }
                    else
                    {
                        long n = turn.operationValue == 0 ? item : turn.operationValue;
                        worryLevel = (item * n) / 3;
                    }
                    if (worryLevel % turn.test == 0)
                    {
                        foreach (var t in round)
                        {
                            if (t.name == turn.ifTrue)
                            {
                                t.startingItems.Enqueue(worryLevel % (13 * 17 * 19 * 23));
                            }
                        }
                    }
                    else
                    {
                        foreach (var t in round)
                        {
                            if (t.name == turn.ifFalse)
                            {
                                t.startingItems.Enqueue(worryLevel%(13*17*19*23));
                            }
                        }
                    }
                }
            }
        }
        List<int> inspectionCounts = new List<int>();
        foreach (var turn in round)
        {
            inspectionCounts.Add(turn.inspectionCount);
        }
            
        inspectionCounts.Sort();
        //Console.WriteLine(string.Join(", ",inspectionCounts));
        int solution = inspectionCounts[inspectionCounts.Count-1];
        solution *= inspectionCounts[inspectionCounts.Count - 2];

        Console.WriteLine("Part a solution: " + solution);
        Console.WriteLine("===============================");
    }
    public static void Day11bSolution(ref List<string> Data)
    {
        int nRounds = 10000;
        SupportRoutines.Monkey monkey = new SupportRoutines.Monkey();
        List<SupportRoutines.Monkey> round = new List<SupportRoutines.Monkey>();
        for (int i = 0; i < Data.Count; i++)
        {
            string[] numbers = Regex.Split(Data[i], @"\D+");
            int num = 0;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    num = int.Parse(value);
                }
            }
            if (i % 7 == 0)
            {
                monkey.name = num;
            }
            if (i % 7 == 1)
            {
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        int j = int.Parse(value);
                        monkey.startingItems.Enqueue(j);
                    }
                }
            }
            if (i % 7 == 2)
            {
                if (Data[i].Contains('+'))
                {
                    monkey.operation = '+';
                    monkey.operationValue = num;
                }
                else if (Data[i].Contains("* old"))
                {
                    monkey.operation = '*';
                    monkey.operationValue = monkey.CurrentWorryLevel;
                }
                else
                {
                    monkey.operation = '*';
                    monkey.operationValue = num;
                }
            }
            if (i % 7 == 3)
            {
                monkey.test = num;
            }
            if (i % 7 == 4)
            {
                monkey.ifTrue = num;
            }
            if (i % 7 == 5)
            {
                monkey.ifFalse = num;
            }
            if (i % 7 == 6 || i == Data.Count - 1)
            {
                round.Add(monkey);
                monkey = new SupportRoutines.Monkey();
            }
        }
        // get the lcm
        int[] divisors = new int[round.Count];
        for ( int j = 0; j < round.Count; j++)
        {
            divisors[j] = round.ElementAt(j).test;
        }
        int lcm = Convert.ToInt32(SupportRoutines.lcm_of_array_elements(divisors));

        for (int j = 0; j < nRounds; j++)
        {
            foreach (var turn in round)
            {
                while (turn.startingItems.Count > 0)
                {
                    long item = turn.startingItems.Dequeue();
                    turn.inspectionCount++;
                    long worryLevel = 0;
                    if (turn.operation == '+')
                    {
                        worryLevel = (item + turn.operationValue)%lcm;
                    }
                    else
                    {
                        long n = turn.operationValue == 0 ? item : turn.operationValue;
                        worryLevel = (item * n)%lcm;
                    }
                    if (worryLevel % turn.test == 0)
                    {
                        foreach (var t in round)
                        {
                            if (t.name == turn.ifTrue)
                            {
                                t.startingItems.Enqueue(worryLevel);
                            }
                        }
                    }
                    else
                    {
                        foreach (var t in round)
                        {
                            if (t.name == turn.ifFalse)
                            {
                                t.startingItems.Enqueue(worryLevel);
                            }
                        }
                    }
                }
            }
        }
        List<long> inspectionCounts = new List<long>();
        foreach (var turn in round)
        {
            inspectionCounts.Add(turn.inspectionCount);
            //turn.displayData();
        }
        inspectionCounts.Sort();
        //Console.WriteLine(string.Join(", ", inspectionCounts));
        long solution = inspectionCounts[inspectionCounts.Count - 1];
        solution *= inspectionCounts[inspectionCounts.Count - 2];

        Console.WriteLine("Part b solution: " + solution);
        Console.WriteLine("===============================");
    }
    public static void Day10Solution(ref List<string> Data)
    {
        int x = 1;
        int clock = 1;
        int score = 0;
        string output = "#";
        void updateString()
        {
            if ((clock-1)%40 >= x - 1 && (clock-1)%40 <= x + 1)
            {
                output += "#";
                //Console.WriteLine("clock = " + clock + "; x = " + x + "drawing #");
            }
            else
            {
                output += ".";
                //Console.WriteLine("clock = " + clock + "; x = " + x + "drawing .");
            }
        }
        Dictionary<string, int> rules = new Dictionary<string, int> { { "noop", 0 }, { "addx", 1 } };
        Queue<Tuple<string, int>> queue = new Queue<Tuple<string, int>>();
        // queue everything up
        for (int i = 0; i < Data.Count; i++)
        {
            string action = Data[i].Split(" ")[0];
            int value = Data[i].Split(" ")[0] == "noop" ? 0 : int.Parse(Data[i].Split(" ")[1]);
            queue.Enqueue(new Tuple<string, int>(action, value));
        }
        while (queue.Count > 0)
        {
            var q = queue.Dequeue();
            if (q.Item2 == 0)
            { 
                clock++;
                updateString();
                if (clock == 20 || (clock - 20) % 40 == 0)
                {
                    //Console.WriteLine("The clock is " + clock + " X = " + x + "; signal strength = " + (clock * x));
                    score += clock * x;
                }
            }
            else
            {
                for (int j = 0; j < rules[q.Item1]; j++)
                {
                    clock++;
                    updateString();
                    if (clock == 20 || (clock - 20) % 40 == 0)
                    {
                        //Console.WriteLine("The clock is " + clock + " X = " + x + "; signal strength = " + (clock * x));
                        score += clock * x;
                    }
                }
                x += q.Item2;
                clock++;
                updateString();
                if (clock == 20 || (clock - 20) % 40 == 0)
                {
                    //Console.WriteLine("The clock is " + clock + " X = " + x + "; signal strength = " + (clock * x));
                    score += clock * x;
                }
            }
        }
        Console.WriteLine("Part a solution: " + score);
        Console.WriteLine("===============================");
        /*int counter = 0;
        string line = "";
        Console.WriteLine("Part b solution: ");
        for (int i = 0; i < output.Length; i++)
        {
            line += output.ElementAt(i);
            counter += 1;
            if (counter == 40)
            {
                counter = 0;
                Console.WriteLine(line);
                line = "";
            }
        }
        Console.WriteLine("===============================");
        */
        Console.WriteLine("Part b solution: ");
        for (int i = 0; i < output.Length; i++)
        {
            if (i % 40 == 0)
            {
                Console.WriteLine();
            }
            else
            {
                Console.Write(output.ElementAt(i));
            }
        }
        Console.WriteLine("===============================");
    }
    public static void Day09aSolution(ref List<string> Data)
    {
        Point head = new Point(0, 0);
        Point tail = head;
        Dictionary<string, Point> Directions = new Dictionary<string, Point>();
        Dictionary<string, Point> Diagonals = new Dictionary<string, Point>();
        Dictionary<Point, int> visitPointCount = new Dictionary<Point, int>();
        visitPointCount.Add(tail, 1);
        //populate directions
        Directions["L"] = new Point(-1, 0);
        Directions["R"] = new Point(1, 0);
        Directions["D"] = new Point(0, 1);
        Directions["U"] = new Point(0, -1);
        //populate diagonal directions
        Diagonals["LU"] = new Point(-1, -1);
        Diagonals["RU"] = new Point(1, -1);
        Diagonals["LD"] = new Point(-1, 1);
        Diagonals["RD"] = new Point(1, 1);

        bool tooFar(Point head, Point tail)
        {
            if (Math.Abs(head.X - tail.X) > 1
                    || Math.Abs(head.Y - tail.Y) > 1)
            {
                //Console.WriteLine("too far!" + head.ToString() + tail.ToString());
                return true;
            }
            else
            {
                //Console.WriteLine("not too far!" + head.ToString() + tail.ToString());
                return false;
            }
        }
        //Console.WriteLine("Head = " + head.ToString() + "; Tail = " + tail.ToString());

        for (int i = 0; i < Data.Count; i++)
        {
            string dir = Data[i].Split(" ")[0];
            int dist = int.Parse(Data[i].Split(" ")[1]);
            for (int j = 0; j < dist; j++)
            {
                head.X += Directions[dir].X;
                head.Y += Directions[dir].Y;
                //Console.WriteLine("Head = " + head.ToString());
                if (tooFar(head, tail))
                {
                    bool updatedTail = false;
                    foreach (var d in Directions)
                    {
                        Point newTail = new Point(tail.X + d.Value.X, tail.Y + d.Value.Y);
                        if (!tooFar(head, newTail)

                            && (
                                head.X - newTail.X == 0
                                || head.Y - newTail.Y == 0))
                        {
                            tail.X += d.Value.X;
                            tail.Y += d.Value.Y;
                            //Console.WriteLine(tail.ToString());
                            //Console.WriteLine("Setting updated to true and breaking from loop");
                            updatedTail = true;
                            break;
                        }
                    }
                    if (!updatedTail)
                    {
                        //Console.WriteLine("entering diagonals");
                        foreach (var d in Diagonals)
                        {
                            Point newTail = new Point(tail.X + d.Value.X, tail.Y + d.Value.Y);
                            if (!tooFar(head, newTail))
                            {
                                tail.X += d.Value.X;
                                tail.Y += d.Value.Y;
                                //Console.WriteLine(tail.ToString());
                                updatedTail = true;
                                break;
                            }
                        }
                    }
                    if (updatedTail)
                    {
                        if (visitPointCount.ContainsKey(tail))
                        {
                            visitPointCount[tail] += 1;
                        }
                        else
                        {
                            visitPointCount.Add(tail, 1);
                        }
                    }
                }
                //Console.WriteLine("Tail = " + tail.ToString());
            }
        }
        Console.WriteLine("Part a solution: " + visitPointCount.Count);
        Console.WriteLine("===============================");
    }
    public static void Day09bSolution(ref List<string> Data)
    {
        Point head = new Point(0, 0);
        Point[] tail = new Point[10];
        for (int i = 0; i < tail.Length; i++)
        {
            // initialise all tail to origin
            tail[i] = head;
        }
        Dictionary<string, Point> Directions = new Dictionary<string, Point>();
        Dictionary<string, Point> Diagonals = new Dictionary<string, Point>();
        Dictionary<Point, int> visitPointCount = new Dictionary<Point, int>();
        visitPointCount.Add(tail[tail.Length - 1], 1);
        //populate directions
        Directions["L"] = new Point(-1, 0);
        Directions["R"] = new Point(1, 0);
        Directions["D"] = new Point(0, 1);
        Directions["U"] = new Point(0, -1);
        //populate diagonal directions
        Diagonals["LU"] = new Point(-1, -1);
        Diagonals["RU"] = new Point(1, -1);
        Diagonals["LD"] = new Point(-1, 1);
        Diagonals["RD"] = new Point(1, 1);

        // test to see if the points are too far apart
        bool tooFar(Point p1, Point p2)
        {
            if (Math.Abs(p1.X - p2.X) > 1
                    || Math.Abs(p1.Y - p2.Y) > 1)
            {
                //Console.WriteLine("too far!" + head.ToString() + tail.ToString());
                return true;
            }
            else
            {
                //Console.WriteLine("not too far!" + head.ToString() + tail.ToString());
                return false;
            }
        }
        //Console.WriteLine("Head = " + head.ToString() + "; Tail = " + tail.ToString());

        for (int i = 0; i < Data.Count; i++)
        {
            //Console.WriteLine(Data[i]);
            string dir = Data[i].Split(" ")[0]; // direction
            int dist = int.Parse(Data[i].Split(" ")[1]); //distance travelled in that direction
            for (int j = 0; j < dist; j++)
            {
                // no need for head, just a tail+1
                tail[0].X += Directions[dir].X;
                tail[0].Y += Directions[dir].Y;
                // array of updated state of each tail item
                bool[] updatedTail = new bool[10];
                // loop through each tail item
                for (int k = 1; k < tail.Length; k++)
                {
                    if (tooFar(tail[k - 1], tail[k]))
                    {
                        // if up/down/left/right possible, do that
                        foreach (var d in Directions)
                        {
                            Point newTail = new Point(tail[k].X + d.Value.X, tail[k].Y + d.Value.Y);
                            if (!tooFar(tail[k - 1], newTail)

                                && (
                                    tail[k - 1].X - newTail.X == 0
                                    || tail[k - 1].Y - newTail.Y == 0))
                            {
                                tail[k].X += d.Value.X;
                                tail[k].Y += d.Value.Y;
                                updatedTail[k] = true;
                                break;
                            }
                        }
                        // else, do diagonal direction
                        if (!updatedTail[k])
                        {
                            //Console.WriteLine("entering diagonals");
                            foreach (var d in Diagonals)
                            {
                                Point newTail = new Point(tail[k].X + d.Value.X, tail[k].Y + d.Value.Y);
                                if (!tooFar(tail[k - 1], newTail))
                                {
                                    tail[k].X += d.Value.X;
                                    tail[k].Y += d.Value.Y;
                                    //Console.WriteLine(tail.ToString());
                                    updatedTail[k] = true;
                                    break;
                                }
                            }
                        }
                        // if that tail item has been updated, and it's the last item in the tail, update the visited grid
                        if (updatedTail[k] && k == tail.Length - 1)
                        {
                            //Console.WriteLine(tail[k].ToString());
                            if (visitPointCount.ContainsKey(tail[k]))
                            {
                                visitPointCount[tail[k]] += 1;
                            }
                            else
                            {
                                visitPointCount.Add(tail[k], 1);
                            }
                        }
                    }

                }
            }
        }
        Console.WriteLine("Part b solution: " + visitPointCount.Count);
        Console.WriteLine("===============================");
    }
    public static void Day09aSolutionBounded(ref List<string> Data)
    {
        int maxWidth = 6;
        int maxHeight = 5;
        Point head = new Point(0, maxHeight - 1);
        Point tail = head;
        Dictionary<string, Tuple<string, Point>> Directions = new Dictionary<string, Tuple<string, Point>>();
        Dictionary<string, Tuple<string, Point>> Diagonals = new Dictionary<string, Tuple<string, Point>>();
        //populate directions
        Directions["L"] = new Tuple<string, Point>("R", new Point(-1, 0));
        Directions["R"] = new Tuple<string, Point>("L", new Point(1, 0));
        Directions["D"] = new Tuple<string, Point>("U", new Point(0, 1));
        Directions["U"] = new Tuple<string, Point>("D", new Point(0, -1));
        //populate diagonal directions
        Diagonals["LU"] = new Tuple<string, Point>("R", new Point(-1, -1));
        Diagonals["RU"] = new Tuple<string, Point>("L", new Point(1, -1));
        Diagonals["LD"] = new Tuple<string, Point>("U", new Point(-1, 1));
        Diagonals["RD"] = new Tuple<string, Point>("D", new Point(1, 1));

        // test if head has gone too far
        bool tooFar(Point head, Point tail)
        {
            if (Math.Abs(head.X - tail.X) > 1
                    || Math.Abs(head.Y - tail.Y) > 1)
            {
                //Console.WriteLine("too far!" + head.ToString() + tail.ToString());
                return true;
            }
            else
            {
                //Console.WriteLine("not too far!" + head.ToString() + tail.ToString());
                return false;
            }
        }
        // create grid
        int[,] grid = new int[maxHeight, maxWidth];
        // create visited grid
        bool[,] visitedGrid = new bool[maxHeight, maxWidth];

        // position head
        grid[head.Y, head.X] = 1;
        //SupportRoutines.printGridInt(grid);
        for (int i = 0; i < Data.Count; i++)
        {
            if (tail != head)
            {
                grid[tail.Y, tail.X] = 2;
            }
            string dir = Data[i].Split(" ")[0];
            int dist = int.Parse(Data[i].Split(" ")[1]);
            //Console.WriteLine(dir + " " + dist);
            for (int j = 0; j < dist; j++)
            {
                // move head
                grid[head.Y, head.X] = 0;
                var nextPos = head;
                nextPos.X += Directions[dir].Item2.X;
                nextPos.Y += Directions[dir].Item2.Y;
                if (nextPos.X < 0 || nextPos.Y < 0 || nextPos.X == maxWidth || nextPos.Y == maxHeight)
                {
                    dir = Directions[dir].Item1;
                    head.X += Directions[dir].Item2.X;
                    head.Y += Directions[dir].Item2.Y;
                }
                else
                {
                    head.X += Directions[dir].Item2.X;
                    head.Y += Directions[dir].Item2.Y;
                }
                //Console.WriteLine(head.ToString());
                grid[head.Y, head.X] = 1;
                SupportRoutines.printGridInt(grid);
                // move tail
                if (tail != head)
                {
                    grid[tail.Y, tail.X] = 0;
                }

                if (tooFar(head, tail))
                {
                    bool updatedTail = false;
                    foreach (var d in Directions)
                    {
                        Point newTail = new Point(tail.X + d.Value.Item2.X, tail.Y + d.Value.Item2.Y);
                        if (!tooFar(head, newTail)
                            && newTail.X >= 0
                            && newTail.Y >= 0
                            && (
                                head.X - newTail.X == 0
                                || head.Y - newTail.Y == 0))
                        {
                            tail.X += d.Value.Item2.X;
                            tail.Y += d.Value.Item2.Y;
                            //Console.WriteLine(tail.ToString());
                            //Console.WriteLine("Setting updated to true and breaking from loop");
                            updatedTail = true;
                            break;
                        }
                    }
                    if (!updatedTail)
                    {
                        //Console.WriteLine("entering diagonals");
                        foreach (var d in Diagonals)
                        {
                            Point newTail = new Point(tail.X + d.Value.Item2.X, tail.Y + d.Value.Item2.Y);
                            if (!tooFar(head, newTail) && newTail.X >= 0 && newTail.Y >= 0)
                            {
                                tail.X += d.Value.Item2.X;
                                tail.Y += d.Value.Item2.Y;
                                //Console.WriteLine(tail.ToString());
                                updatedTail = true;
                                break;
                            }
                        }
                    }
                    if (updatedTail && !visitedGrid[tail.Y, tail.X])
                    {
                        visitedGrid[tail.Y, tail.X] = true;
                    }
                }
                if (tail != head)
                {
                    grid[tail.Y, tail.X] = 2;
                }
                SupportRoutines.printGridInt(grid);
            }
        }
        SupportRoutines.printGridBool(visitedGrid);
        Console.WriteLine("Part a solution: " + (SupportRoutines.countVisible(visitedGrid) + 1));
        Console.WriteLine("===============================");
    }
    public static void Dijkstra(ref List<string> Data)
    {
        int y = Data.Count;
        int x = Data[0].Split(",").Length;
        int[,] graph = new int[y, x];
        for (int i = 0; i < Data.Count; i++)
        {
            string[] row = Data[i].Split(",");
            for (int j = 0; j < x; j++)
            {
                graph[j, i] = Convert.ToInt32(row.ElementAt(j));
            }
        }
        foreach (var item in SupportRoutines.dijkstra(graph, 0, x))
        {
            Console.WriteLine(item.Key + " ==> " + item.Value);
        }
    }
    public static void Day01aSolution(ref List<string> Data)
    {
        int largest = 0;
        int subTotal = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            //end of elf
            if (Data[i] == "")
            {
                //set largest to subTotal, if subTotal is larger
                if (subTotal > largest)
                {
                    largest = subTotal;
                }
                subTotal = 0;

            }
            else
            {
                subTotal += Convert.ToInt32(Data[i]);
                //is new sub-total larger than largest, and end of file reached
                if (subTotal > largest && i == Data.Count - 1)
                {
                    largest = subTotal;
                    subTotal = 0;
                }
            }
        }
        Console.WriteLine("Part a solution: " + largest);
        Console.WriteLine("===============================");
    }
    public static void Day01bSolution(ref List<string> Data)
    {
        int subTotal = 0;
        int topThreeSum = 0;
        List<int> totals = new List<int>();
        for (int i = 0; i < Data.Count; i++)
        {
            //end of elf
            if (Data[i] == "")
            {
                //add new subtotal to list of totals
                totals.Add(subTotal);
                subTotal = 0;

            }
            else
            {
                subTotal += Convert.ToInt32(Data[i]);
                //did we find the end of the file
                if (i == Data.Count - 1)
                {
                    totals.Add(subTotal);
                }
            }
        }
        //No linq
        //Using .Sort()
        //totals.Sort();
        //Using quickSort implementation
        SupportRoutines.quickSort(totals, 0, totals.Count - 1);
        int n = 3; //get top n
        for (int i = totals.Count - 1; i > totals.Count - (n + 1); i--)
        {
            topThreeSum += totals[i];
        }
        //using Linq
        //var topThree = totals.OrderByDescending(i => i).Take(3).ToList();
        //foreach (int total in topThree)
        //{
        //    topThreeSum += total;
        //}
        Console.WriteLine("Part b solution: " + topThreeSum);
        Console.WriteLine("===============================");
    }
    public static void Day02aSolution(ref List<string> Data)
    {
        //define the mapping of hand to actual hand, points and win condition
        Dictionary<string, Tuple<string, int, string>> map = new Dictionary<string, Tuple<string, int, string>>();
        map["X"] = new Tuple<string, int, string>("A", 1, "C");//rock
        map["Y"] = new Tuple<string, int, string>("B", 2, "A");//paper
        map["Z"] = new Tuple<string, int, string>("C", 3, "B");//scissors

        int score = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            string[] round = Data[i].Split(" ");
            if (round[0] == map[round[1]].Item1) //draw, 3
            {
                score += (3 + map[round[1]].Item2);
            }
            else if (round[0] == map[round[1]].Item3) //win, 6
            {
                score += (6 + map[round[1]].Item2);
            }
            else //lose, 0
            {
                score += (0 + map[round[1]].Item2);
            }
        }
        Console.WriteLine("Part a solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day02bSolution(ref List<string> Data)
    {
        //dictionary of hand, win/loss/points
        Dictionary<string, Tuple<string, string, int>> map = new Dictionary<string, Tuple<string, string, int>>();
        map["A"] = new Tuple<string, string, int>("C", "B", 1);//rock
        map["B"] = new Tuple<string, string, int>("A", "C", 2);//paper
        map["C"] = new Tuple<string, string, int>("B", "A", 3);//scissors
        int score = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            string[] round = Data[i].Split(" ");
            //player wins
            if (round[1] == "Z")
            {
                //win score + points for hand that matches elf hand loss hand
                score += (6 + map[map[round[0]].Item2].Item3);
            }
            //player loses
            else if (round[1] == "X") //lose,3
            {
                //lose score + points for hand that matches elf hand win hand
                score += (0 + map[map[round[0]].Item1].Item3);
            }
            //player draws
            else
            {
                //draw score + points for hand that matches elf hand
                score += (3 + map[round[0]].Item3);
            }
        }
        Console.WriteLine("Part b solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day03aSolution(ref List<string> Data)
    {
        int score = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            int halfway = Data[i].Length / 2;
            // Creating array of string length 
            List<char> compartment1 = new List<char>();
            List<char> compartment2 = new List<char>();

            // Copy character by character into List of chars 
            for (int j = 0; j < halfway; j++)
            {
                compartment1.Add(Data[i][j]);
            }
            for (int j = halfway; j < Data[i].Length; j++)
            {
                compartment2.Add(Data[i][j]);
            }
            var commonValues = compartment1.Intersect(compartment2);
            foreach (char value in commonValues)
            {
                if (char.IsLower(value))
                {
                    score += ((int)value - 96);
                }
                else
                {
                    score += ((int)value - 38);
                }
            }

        }
        Console.WriteLine("Part a solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day03bSolution(ref List<string> Data)
    {
        int score = 0;
        //3 lists of chars to store the three elf rucksacks
        List<char> rucksack1 = new List<char>();
        List<char> rucksack2 = new List<char>();
        List<char> rucksack3 = new List<char>();

        //iterate through the list
        for (int i = 0; i < Data.Count; i++)
        {
            //use modulus to get elf 1,2 and 3, then reset
            if (i % 3 == 0)
            {
                rucksack1.Clear();
                for (int j = 0; j < Data[i].Length; j++)
                {
                    //elf 1 rucksack contents
                    rucksack1.Add(Data[i][j]);
                }

            }
            if (i % 3 == 1)
            {
                rucksack2.Clear();
                for (int j = 0; j < Data[i].Length; j++)
                {
                    //elf 2 rucksack contents
                    rucksack2.Add(Data[i][j]);
                }
            }
            if (i % 3 == 2)
            {
                rucksack3.Clear();
                for (int j = 0; j < Data[i].Length; j++)
                {
                    //elf 3 rucksack contents
                    rucksack3.Add(Data[i][j]);
                }
                //get items common to all three lists using set intersections
                List<char> commonValues = rucksack1.Intersect(rucksack2).Intersect(rucksack3).ToList();
                if (char.IsLower(commonValues[0]))
                {
                    score += ((int)commonValues[0] - 96);
                }
                else
                {
                    score += ((int)commonValues[0] - 38);
                }
            }

        }
        Console.WriteLine("Part b solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day04aSolution(ref List<string> Data)
    {
        int score = 0;
        //iterate through the list
        for (int i = 0; i < Data.Count; i++)
        {
            int lowerBound1 = Convert.ToInt32(Data[i].Split(",")[0].Split('-')[0]);
            int upperBound1 = Convert.ToInt32(Data[i].Split(",")[0].Split('-')[1]);
            int lowerBound2 = Convert.ToInt32(Data[i].Split(",")[1].Split('-')[0]);
            int upperBound2 = Convert.ToInt32(Data[i].Split(",")[1].Split('-')[1]);
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            for (int j = lowerBound1; j <= upperBound1; j++)
            {
                left.Add(j);
            }
            for (int j = lowerBound2; j <= upperBound2; j++)
            {
                right.Add(j);
            }
            var intersection = left.Intersect(right).ToList();
            if (left.Count == intersection.Count || right.Count == intersection.Count)
            {
                //Console.WriteLine(Data[i] + "overlap found!");
                score++;
            }
        }
        Console.WriteLine("Part a solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day04bSolution(ref List<string> Data)
    {
        int score = 0;
        //iterate through the list
        for (int i = 0; i < Data.Count; i++)
        {
            int lowerBound1 = Convert.ToInt32(Data[i].Split(",")[0].Split('-')[0]);
            int upperBound1 = Convert.ToInt32(Data[i].Split(",")[0].Split('-')[1]);
            int lowerBound2 = Convert.ToInt32(Data[i].Split(",")[1].Split('-')[0]);
            int upperBound2 = Convert.ToInt32(Data[i].Split(",")[1].Split('-')[1]);
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            for (int j = lowerBound1; j <= upperBound1; j++)
            {
                left.Add(j);
            }
            for (int j = lowerBound2; j <= upperBound2; j++)
            {
                right.Add(j);
            }
            var intersection = left.Intersect(right).ToList();
            if (intersection.Count > 0)
            {
                //Console.WriteLine(Data[i] + "overlap found!");
                score++;
            }
        }
        Console.WriteLine("Part b solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day05aSolution(ref List<string> Data)
    {
        int lineNumber = 0;
        SortedDictionary<int, List<string>> stacks = new SortedDictionary<int, List<string>>();
        for (int i = 0; i < Data.Count; i++)
        {
            lineNumber++;
            if (char.IsNumber(Data[i][1]))
            {
                break;
            }
            else
            {
                for (int j = 0; j < Data[i].Length; j++)
                {
                    int index = ((j + 3) / 4);
                    if (char.IsLetter(Data[i][j]))
                    {
                        if (stacks.ContainsKey(index))
                        {
                            stacks[index].Add(Data[i][j].ToString());
                        }
                        else
                        {
                            stacks[index] = new List<string> { Data[i][j].ToString() };
                        }
                    }
                }
            }
        }
        //foreach (var stack in stacks)
        //{
        //    Console.WriteLine(stack.Key + " ==> " + string.Join(" , ", stack.Value));
        //}
        for (int i = lineNumber; i < Data.Count; i++)
        {
            if (Data[i].Length > 1 && char.IsLetter(Data[i][1]))
            {
                int howMany = 0;
                int moveFrom = 0;
                int moveTo = 0;
                List<int> steps = new List<int>();
                string[] numbers = Regex.Split(Data[i], @"\D+");
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        int j = int.Parse(value);
                        steps.Add(j);
                    }
                }
                if (steps.Count == 3)
                {
                    howMany = steps[0];
                    moveFrom = steps[1];
                    moveTo = steps[2];
                }
                for (int j = 0; j < howMany; j++)
                {
                    string crate = stacks[moveFrom][0];
                    stacks[moveFrom].RemoveAt(0);
                    if (stacks.ContainsKey(moveTo))
                    {
                        stacks[moveTo].Insert(0, crate);
                    }
                    else
                    {
                        stacks[moveTo] = new List<string> { crate };
                    }
                }
            }
            //foreach (var stack in stacks)
            //{
            //    Console.WriteLine(stack.Key + " ==> " + string.Join(" , ", stack.Value));
            //}
        }
        string answer = "";
        foreach (var stack in stacks)
        {
            if (stack.Value.Count > 0)
            {
                answer += stack.Value[0];
            }
        }
        // move 3 from 1 to 3
        Console.WriteLine("Part a solution: " + answer);
        Console.WriteLine("===============================");
    }
    public static void Day05bSolution(ref List<string> Data)
    {
        int lineNumber = 0;
        SortedDictionary<int, List<string>> stacks = new SortedDictionary<int, List<string>>();
        for (int i = 0; i < Data.Count; i++)
        {
            lineNumber++;
            if (char.IsNumber(Data[i][1]))
            {
                break;
            }
            else
            {
                for (int j = 0; j < Data[i].Length; j++)
                {
                    int index = ((j + 3) / 4);
                    if (char.IsLetter(Data[i][j]))
                    {
                        if (stacks.ContainsKey(index))
                        {
                            stacks[index].Add(Data[i][j].ToString());
                        }
                        else
                        {
                            stacks[index] = new List<string> { Data[i][j].ToString() };
                        }
                    }
                }
            }
        }
        //foreach (var stack in stacks)
        //{
        //    Console.WriteLine(stack.Key + " ==> " + string.Join(" , ", stack.Value));
        //}
        for (int i = lineNumber; i < Data.Count; i++)
        {
            if (Data[i].Length > 1 && char.IsLetter(Data[i][1]))
            {
                int howMany = 0;
                int moveFrom = 0;
                int moveTo = 0;
                List<int> steps = new List<int>();
                string[] numbers = Regex.Split(Data[i], @"\D+");
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        int j = int.Parse(value);
                        steps.Add(j);
                    }
                }
                if (steps.Count == 3)
                {
                    howMany = steps[0];
                    moveFrom = steps[1];
                    moveTo = steps[2];
                }
                List<string> crates = stacks[moveFrom].GetRange(0, howMany);
                crates.Reverse();
                stacks[moveFrom].RemoveRange(0, howMany);
                if (stacks.ContainsKey(moveTo))
                {
                    foreach (string crate in crates)
                    {
                        stacks[moveTo].Insert(0, crate);
                    }
                }
                else
                {
                    stacks[moveTo] = crates;
                }
            }
            //foreach (var stack in stacks)
            //{
            //    Console.WriteLine(stack.Key + " ==> " + string.Join(" , ", stack.Value));
            //}
        }
        string answer = "";
        foreach (var stack in stacks)
        {
            if (stack.Value.Count > 0)
            {
                answer += stack.Value[0];
            }
        }
        Console.WriteLine("Part b solution: " + answer);
        Console.WriteLine("===============================");
    }
    public static void Day06aSolution(ref List<string> Data)
    {
        int packetLength = 4;
        for (int i = 0; i < Data.Count; i++)
        {
            int index = 0;
            for (int j = packetLength - 1; j < Data[i].Length; j++)
            {
                List<char> quad = new List<char>();
                for (int k = 0; k < packetLength; k++)
                {
                    quad.Add(Data[i][j - k]);
                }
                var CharSet = new HashSet<char>(quad);
                //Console.WriteLine(string.Join(", ", quad) + " ==> " + string.Join(", ", CharSet));
                if (CharSet.Count == packetLength)
                {
                    index = j + 1;
                    break;
                }
            }
            Console.WriteLine("Part a solution: " + index);
            Console.WriteLine("===============================");
        }
    }
    public static void Day06bSolution(ref List<string> Data)
    {
        int packetLength = 14;
        for (int i = 0; i < Data.Count; i++)
        {
            int index = 0;
            for (int j = packetLength - 1; j < Data[i].Length; j++)
            {
                List<char> quad = new List<char>();
                for (int k = 0; k < packetLength; k++)
                {
                    quad.Add(Data[i][j - k]);
                }
                var CharSet = new HashSet<char>(quad);
                //Console.WriteLine(string.Join(", ", quad) + " ==> " + string.Join(", ", CharSet));
                if (CharSet.Count == packetLength)
                {
                    index = j + 1;
                    break;
                }
            }
            Console.WriteLine("Part b solution: " + index);
            Console.WriteLine("===============================");
        }
    }
    public static void Day07Solution(ref List<string> Data)
    {
        int result = 0;
        int diskSpace = 70000000;
        int unusedSpaceReq = 30000000;
        // build the path as a list of string items
        List<string> path = new List<string>();
        // each path root has a total value, stored in a dict
        Dictionary<string, int> folders = new Dictionary<string, int>();
        // loop through all lines
        for (int i = 0; i < Data.Count; i++)
        {
            // remove last path entry 
            if (Data[i].Contains("$ cd .."))
            {
                path.RemoveAt(path.Count - 1);
            }
            // add new directory to path
            else if (Data[i].Contains("$ cd"))
            {
                path.Add(Data[i].Split(" ")[2]);
            }
            // if there is a number (file of size)..
            else if (char.IsNumber(Data[i][0]))
            {
                // get the number
                int fileSize = Convert.ToInt32(Data[i].Split(" ")[0]);
                // loop through current path items, starting with the root
                for (int j = 0; j < path.Count; j++)
                {
                    // build a list of path items to that index
                    List<string> pathSlice = new List<string>();
                    for (int k = 0; k < j + 1; k++)
                    {
                        pathSlice.Add(path[k]);
                    }
                    //join that list as a string and set it as the dict key
                    string pathKey = string.Join("/", pathSlice);
                    //if the key exists, add value to the total for that path
                    if (folders.ContainsKey(pathKey))
                    {
                        folders[pathKey] += fileSize;
                    }
                    //if the key doesn't, create it and set the inital value to value
                    else
                    {
                        folders.Add(pathKey, fileSize);
                    }
                }
            }
        }
        // Part a, loop through the dict and add any directory of size < 10000 to the result
        foreach (var item in folders)
        {
            if (item.Value < 100000)
            {
                result += item.Value;
            }
            //Console.WriteLine(item.Key + " ==> " + item.Value);
        }
        Console.WriteLine("Part a solution: " + result);
        Console.WriteLine("===============================");

        // Part b, if not enough remaining space..
        result = 0;
        int remainingSpace = diskSpace - folders["/"];
        //Console.WriteLine("Remaining space is " + remainingSpace);
        if (remainingSpace < unusedSpaceReq)
        {
            //loop through the dict, ordered on value (ASC)
            foreach (var item in folders.OrderBy(key => key.Value))
            {
                //Console.WriteLine(item.Key + " ==> " + item.Value);
                // if removing that directory gives a satisfactory result, return its size
                if (remainingSpace + item.Value >= unusedSpaceReq)
                {
                    result = item.Value;
                    break;
                }
            }
        }
        Console.WriteLine("Part b solution: " + result);
        Console.WriteLine("===============================");
    }

    // Nick's fancy class based recursive solution
    public static void Day07SolutionNSW(ref List<string> Data)
    {
        SupportRoutines.fso root = new SupportRoutines.fso("/", 0, false, null);
        SupportRoutines.fso current = root;
        bool lsmode = false;
        foreach (string s in Data)
        {
            string[] statement = s.Split(' ');
            if (statement[0] == "$")
            {
                lsmode = false;
                switch (statement[1])
                {
                    case "cd":
                        if (statement[2] == "/")
                            current = root;
                        else
                            current = current.CD(statement[2]);
                        break;
                    case "ls":
                        lsmode = true;
                        break;
                }
            }
            else if (lsmode)
                if (statement[0] == "dir")
                    current.AddFileOrDirectory(statement[1], 0, false);
                else
                    current.AddFileOrDirectory(statement[1], Convert.ToInt32(statement[0]), true);
        }
        Console.WriteLine("NSW Part a solution: " + (root.SizeLessThan(100000)));
        Console.WriteLine("===============================");
        int Smallestsize = 70000000;
        root.DirectoryToDelete((30000000 - (70000000 - root.Size())), ref Smallestsize);
        Console.WriteLine("NSW Part b solution: " + (Smallestsize));
        Console.WriteLine("===============================");
    }
    public static void Day08aSolution(ref List<string> Data)
    {
        int y = Data.Count;
        int x = Data[0].Length;
        int[,] trees = new int[y, x]; //the forest
        bool[,] visible = new bool[y, x]; // bool grid, true is visible from 1/4 directions
        bool[,] visibleX = new bool[y, x]; // bool grid, true is visible from 1/4 directions
        bool[,] visibleY = new bool[y, x]; // bool grid, true is visible from 1/4 directions
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                trees[i, j] = int.Parse(Data[i][j].ToString());
                if (j == 0 || j == x - 1 || i == 0 || i == y - 1)
                {
                    visible[i, j] = true;
                    visibleX[i, j] = true;
                    visibleY[i, j] = true;
                }
                else
                {
                    visible[i, j] = false;
                    visibleX[i, j] = false;
                    visibleY[i, j] = false;
                }
            }
        }
        // update the bool grid, true if already true, or visible from this direction
        void goFromLeft(ref bool[,] visibleX)
        {
            for (int i = 0; i < y; i++)
            {
                int currentMaxHeight = trees[i, 0];
                for (int j = 1; j < x; j++)
                {
                    if (trees[i, j] > currentMaxHeight)
                    {
                        visibleX[i, j] = true;
                    }
                    if (currentMaxHeight < trees[i, j])
                    {
                        currentMaxHeight = trees[i, j];
                    }
                }
            }
        }
        void goFromRight(ref bool[,] visibleX)
        {
            for (int i = 0; i < y; i++)
            {
                int currentMaxHeight = trees[i, y - 1];
                for (int j = x - 2; j >= 0; j--)
                {
                    if (j > 0 && visible[i, j])
                    {
                        //stop when we get to those marked visible from the left
                        break;
                    }
                    if (trees[i, j] > currentMaxHeight)
                    {
                        visibleX[i, j] = true;
                    }
                    if (currentMaxHeight < trees[i, j])
                    {
                        currentMaxHeight = trees[i, j];
                    }
                }
            }
        }
        void goFromBottom(ref bool[,] visibleY)
        {
            for (int i = 0; i < x; i++)
            {
                int currentMaxHeight = trees[x - 1, i];
                for (int j = y - 2; j >= 0; j--)
                {
                    if (trees[j, i] > currentMaxHeight)
                    {
                        visibleY[j, i] = true;
                    }
                    if (currentMaxHeight < trees[j, i])
                    {
                        currentMaxHeight = trees[j, i];
                    }
                }
            }
        }
        void goFromTop(ref bool[,] visibleY)
        {
            for (int i = 0; i < x; i++)
            {
                int currentMaxHeight = trees[0, i];
                for (int j = 0; j < y - 1; j++)
                {
                    if (j > 0 && visibleY[j, i])
                    {
                        //stop when we get to those marked visible from the bottom
                        break;
                    }
                    if (trees[j, i] > currentMaxHeight)
                    {
                        visibleY[j, i] = true;
                    }
                    if (currentMaxHeight < trees[j, i])
                    {
                        currentMaxHeight = trees[j, i];
                    }
                }
            }
        }
        // check each direction
        goFromLeft(ref visibleX);
        goFromRight(ref visibleX);
        goFromBottom(ref visibleY);
        goFromTop(ref visibleY);

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (visibleX[i, j] || visibleY[i, j])
                {
                    visible[i, j] = true;
                }
            }
        }


        Console.WriteLine("Part a solution: " + SupportRoutines.countVisible(visible));
        Console.WriteLine("===============================");

    }
    public static void Day08bSolution(ref List<string> Data)
    {
        Dictionary<string, Point> Directions = new Dictionary<string, Point> { // This is slower
            {"Left", new Point(-1, 0)},
            {"Right", new Point(1,0)},
            {"Down", new Point(0,-1)},
            {"Up", new Point(0, 1)}
        };
        int ForestHeight = Data.Count;
        int ForestWidth = Data[0].Length;
        int[,] trees = new int[ForestHeight, ForestWidth];
        // populate the forest
        for (int i = 0; i < ForestHeight; i++)
        {
            for (int j = 0; j < ForestWidth; j++)
            {
                trees[i, j] = int.Parse(Data[i][j].ToString());
            }
        }
        // get the score for each position in the forest
        int getMaxScore(int[,] grid)
        {
            int maxScore = 0;
            for (int i = 0; i < ForestHeight; i++)
            {
                for (int j = 0; j < ForestWidth; j++)
                {
                    int currentScore = goLeft(i, j) * goRight(i, j) * goUp(i, j) * goDown(i, j);
                    if (maxScore < currentScore)
                    {
                        maxScore = currentScore;
                    }
                }
            }
            return maxScore;
        }
        int goLeft(int y, int x)
        {
            int visibleScore = 0;
            int height = trees[y, x];
            // using dictionary of direction modifiers, this is way slower
            x--;
            while (x >= 0)
            {
                if (trees[y, x] < height)
                {
                    visibleScore += 1;
                }
                else
                {

                    visibleScore += 1;
                    break;
                }
                x += Directions["Left"].X;
            }
            //for (int j = x - 1; j >= 0; j--) // this is faster than above
            //{
            //    if (trees[y, j] < height)
            //    {
            //        visibleScore += 1;
            //    }
            //    if (trees[y, j] >= height)
            //    {
            //        visibleScore += 1;
            //        break;
            //    }
            //}
            return visibleScore;
        }
        int goRight(int y, int x)
        {
            int visibleScore = 0;
            int height = trees[y, x];
            for (int j = x + 1; j < ForestWidth; j++)
            {

                if (trees[y, j] < height)
                {
                    visibleScore += 1;

                }
                if (trees[y, j] >= height)
                {
                    visibleScore += 1;
                    break;
                }
            }
            return visibleScore;
        }
        int goUp(int y, int x)
        {
            int visibleScore = 0;
            int height = trees[y, x];
            for (int j = y - 1; j >= 0; j--)
            {
                if (trees[j, x] < height)
                {
                    visibleScore += 1;

                }
                if (trees[j, x] >= height)
                {
                    visibleScore += 1;
                    break;
                }
            }
            return visibleScore;
        }
        int goDown(int y, int x)
        {
            int visibleScore = 0;
            int height = trees[y, x];
            for (int j = y + 1; j < ForestHeight; j++)
            {
                if (trees[j, x] < height)
                {
                    visibleScore += 1;

                }
                if (trees[j, x] >= height)
                {
                    visibleScore += 1;
                    break;
                }
            }
            return visibleScore;
        }
        Console.WriteLine("Part b solution: " + getMaxScore(trees));
        Console.WriteLine("===============================");
    }
}