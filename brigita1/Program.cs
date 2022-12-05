using static System.IO.File;
using System.Collections.Generic;
using System.Linq;

var sacks = ReadAllLines("./brigita1/brigita.txt");
long sum = 0;
foreach(var sack in sacks){
    var fh = new HashSet<char>();
    var sh = new HashSet<char>();
    for(int i=0; i<sack.Length/2; i++){
        fh.Add(sack[i]);
    }
    for(int i=sack.Length/2; i<sack.Length; i++){
        sh.Add(sack[i]);
    }
    fh.IntersectWith(sh);
    var impostor = fh.First();
    if(char.IsUpper(impostor)){
        sum += 26;
    }
    sum += (short)char.ToLower(impostor) - 96;
}
Console.WriteLine($"{sum}");