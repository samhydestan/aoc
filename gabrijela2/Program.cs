using static System.IO.File;

var lines = ReadAllLines("./gabrijela1/gabrijela.txt");
var cave = new Dictionary<int, HashSet<int>>();
int abyss = 0;
foreach(var line in lines){
    var points = line.Split(" -> ");
    for(int i=0; i<points.Length-1; i++){
        var s1 = points[i].Split(',');
        var s2 = points[i+1].Split(',');
        var p1 = (int.Parse(s1[0]), int.Parse(s1[1]));
        var p2 = (int.Parse(s2[0]), int.Parse(s2[1]));
        if(p1.Item1 == p2.Item1){
            var min = Math.Min(p1.Item2, p2.Item2);
            if(min > abyss){
                abyss = min;
            }
            var max = Math.Max(p1.Item2, p2.Item2);
            if(cave.TryGetValue(p1.Item1, out var set)){
                for(int j=min; j<=max; j++){
                    set.Add(j);
                }
            } else{
                var hs = new HashSet<int>();
                for(int j=min; j<=max; j++){
                    hs.Add(j);
                }
                cave[p1.Item1] = hs;
            }
        } else{
            if(p1.Item2 > abyss){
                abyss = p1.Item2;
            }
            var min = Math.Min(p1.Item1, p2.Item1);
            var max = Math.Max(p1.Item1, p2.Item1);
            for(int j=min; j<=max; j++){
                if(cave.TryGetValue(j, out var set)){
                    set.Add(p1.Item2);
                } else{
                    var hs = new HashSet<int>();
                    hs.Add(p1.Item2);
                    cave[j] = hs;
                }
            }
        }
    }
}
foreach(var set in cave.Values){
    set.Add(abyss+2);
}
int grains = 0;
var p = (500, 0);
while(true){
    if(!cave.TryGetValue(p.Item1, out var set)){
        set = new HashSet<int>();
        set.Add(abyss+2);
        cave[p.Item1] = set;
    }
    if(!set.Contains(p.Item2+1)){
        p = (p.Item1, p.Item2+1);
    } else{
        if(!cave.TryGetValue(p.Item1-1, out var lset)){
            lset = new HashSet<int>();
            lset.Add(abyss+2);
            cave[p.Item1-1] = lset;
        }
        if(!lset.Contains(p.Item2+1)){
            p = (p.Item1-1, p.Item2+1);
        } else{
            if(!cave.TryGetValue(p.Item1+1, out var rset)){
                rset = new HashSet<int>();
                rset.Add(abyss+2);
                cave[p.Item1+1] = rset;
            }
            if(!rset.Contains(p.Item2+1)){
                p = (p.Item1+1, p.Item2+1);
            } else{
                if(p.Item1 == 500 && p.Item2 == 0){
                    grains++;
                    break;
                }
                set.Add(p.Item2);
                grains++;
                p = (500, 0);
            }
        }
    }
}
Console.WriteLine(grains);