using static System.IO.File;
using System.Diagnostics;

var stop = new Stopwatch();
stop.Start();
var lines = ReadAllLines("./liljana1/liljana.txt");
var cubes = new HashSet<(int, int, int)>();
int sides = 0;
int maxx = int.MinValue;
int minx = int.MaxValue;
int maxy = int.MinValue;
int miny = int.MaxValue;
int maxz = int.MinValue;
int minz = int.MaxValue;
foreach(var line in lines){
    var cube = line.Split(',').Select(x => int.Parse(x)).ToArray();
    var minus = (cubes.Contains((cube[0], cube[1], cube[2]+1)) ? 2 : 0) +
                (cubes.Contains((cube[0], cube[1], cube[2]-1)) ? 2 : 0) +
                (cubes.Contains((cube[0], cube[1]+1, cube[2])) ? 2 : 0) +
                (cubes.Contains((cube[0], cube[1]-1, cube[2])) ? 2 : 0) +
                (cubes.Contains((cube[0]+1, cube[1], cube[2])) ? 2 : 0) +
                (cubes.Contains((cube[0]-1, cube[1], cube[2])) ? 2 : 0);
    sides += 6 - minus;
    maxx = maxx > cube[0] ? maxx : cube[0];
    minx = minx < cube[0] ? minx : cube[0];
    maxy = maxy > cube[1] ? maxy : cube[1];
    miny = miny < cube[1] ? miny : cube[1];
    maxz = maxz > cube[2] ? maxz : cube[2];
    minz = minz < cube[2] ? minz : cube[2];
    cubes.Add((cube[0], cube[1], cube[2]));
}
var cuboid = new HashSet<(int, int, int)>(cubes);
maxx++;
minx--;
maxy++;
miny--;
maxz++;
minz--;
var q = new HashSet<(int, int, int)>();
q.Add((minx, miny, minz));
while(q.Any()){
    var nq = new HashSet<(int, int, int)>();
    foreach(var (x, y, z) in q){
        if(x > minx && !cuboid.Contains((x-1, y, z))){
            cuboid.Add((x-1, y, z));
            nq.Add((x-1, y, z));
        }
        if(x < maxx && !cuboid.Contains((x+1, y, z))){
            cuboid.Add((x+1, y, z));
            nq.Add((x+1, y, z));
        }
        if(y > miny && !cuboid.Contains((x, y-1, z))){
            cuboid.Add((x, y-1, z));
            nq.Add((x, y-1, z));
        }
        if(y < maxy && !cuboid.Contains((x, y+1, z))){
            cuboid.Add((x, y+1, z));
            nq.Add((x, y+1, z));
        }
        if(z > minz && !cuboid.Contains((x, y, z-1))){
            cuboid.Add((x, y, z-1));
            nq.Add((x, y, z-1));
        }
        if(z < maxz && !cuboid.Contains((x, y, z+1))){
            cuboid.Add((x, y, z+1));
            nq.Add((x, y, z+1));
        }
    }
    q = nq;
}
for(int x=minx; x<=maxx; x++){
    for(int y=miny; y<=maxy; y++){
        for(int z=minz; z<=maxz; z++){
            if(!cuboid.Contains((x, y, z))){
                sides -= (cuboid.Contains((x+1, y, z)) ? 1 : 0) +
                         (cuboid.Contains((x-1, y, z)) ? 1 : 0) +
                         (cuboid.Contains((x, y+1, z)) ? 1 : 0) +
                         (cuboid.Contains((x, y-1, z)) ? 1 : 0) +
                         (cuboid.Contains((x, y, z+1)) ? 1 : 0) +
                         (cuboid.Contains((x, y, z-1)) ? 1 : 0);
            }
        }
    }
}
Console.WriteLine(sides);
stop.Stop();
Console.WriteLine(stop.Elapsed);

/*var ys = cubes.Select(a => a.Item2).Distinct().Order();
var xs = cubes.Select(a => a.Item1).Distinct().Order();
var zs = cubes.Select(a => a.Item3).Distinct().Order();
var schema = new List<string>();
foreach(var y in ys){
    schema.Add($"Y = {y}");
    foreach(var z in zs){
        string line = "";
        foreach(var x in xs){
            if(cubes.Contains((x,y,z))){
                line += "X";
            } else{
                line +="#";
            }
        }
        schema.Add(line);
    }
    schema.Add("");
}
File.WriteAllLinesAsync("./liljana2/map.txt", schema);*/