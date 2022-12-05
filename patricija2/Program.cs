using static System.IO.File;

var pairs = ReadAllLines("./patricija1/patricija.txt");
long c = 0;
foreach(var pair in pairs){
    var elves = pair.Split(',');
    var f = elves[0].Split('-');
    var s = elves[1].Split('-');
    var limits = new long[4]{
        long.Parse(f[0]),
        long.Parse(f[1]),
        long.Parse(s[0]),
        long.Parse(s[1])
    };
    if(!(limits[1]<limits[2] || limits[3]<limits[0])){
        c++;
    }
}
Console.WriteLine($"{c}");