using static System.IO.File;
using System.Collections.Generic;
using System.Linq;

var sacks = ReadAllLines("./brigita1/brigita.txt");
long sum = 0;
long i = 0;
while(i<sacks.Length){
    var fs = new HashSet<char>(sacks[i]);
    i++;
    var ss = new HashSet<char>(sacks[i]);
    i++;
    var ts = new HashSet<char>(sacks[i]);
    i++;
    fs.IntersectWith(ss);
    fs.IntersectWith(ts);
    var impostor = fs.First();
    if(char.IsUpper(impostor)){
        sum += 26;
    }
    sum += (short)char.ToLower(impostor) - 96;
}
Console.WriteLine($"{sum}");