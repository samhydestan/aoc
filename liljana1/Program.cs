using static System.IO.File;
using System.Diagnostics;

var stop = new Stopwatch();
stop.Start();
var lines = ReadAllLines("./liljana1/liljana.txt");
var cubes = new List<int[]>();
int sides = 0;
foreach(var line in lines){
    var cube = line.Split(',').Select(x => int.Parse(x)).ToArray();
    sides += 6 - 2*cubes.Where(x =>
        x[0] == cube[0] && x[1] == cube[1] && Math.Abs(x[2]-cube[2]) == 1 ||
        x[0] == cube[0] && x[2] == cube[2] && Math.Abs(x[1]-cube[1]) == 1 ||
        x[1] == cube[1] && x[2] == cube[2] && Math.Abs(x[0]-cube[0]) == 1)
        .Count();
    cubes.Add(cube);
}
Console.WriteLine(sides);