using static System.IO.File;
using System.Diagnostics;

var stop = new Stopwatch();
stop.Start();
var lines = ReadAllLines("./liljana1/liljana.txt");
var cubes = new HashSet<(int, int, int)>();
int sides = 0;
foreach(var line in lines){
    var cube = line.Split(',').Select(x => int.Parse(x)).ToArray();
    var minus = (cubes.Contains((cube[0], cube[1], cube[2]+1)) ? 2 : 0) +
                (cubes.Contains((cube[0], cube[1], cube[2]-1)) ? 2 : 0) +
                (cubes.Contains((cube[0], cube[1]+1, cube[2])) ? 2 : 0) +
                (cubes.Contains((cube[0], cube[1]-1, cube[2])) ? 2 : 0) +
                (cubes.Contains((cube[0]+1, cube[1], cube[2])) ? 2 : 0) +
                (cubes.Contains((cube[0]-1, cube[1], cube[2])) ? 2 : 0);
    sides += 6 - minus;
    cubes.Add((cube[0], cube[1], cube[2]));
}
/*foreach(var cube in cubes){
    if(cubes.Any(x => x[0] == cube[0] && x[1] == cube[1]+1 && x[2] == cube[2]+1) &&
        ){
        sides -= 6;
    }
}*/
Console.WriteLine(sides);