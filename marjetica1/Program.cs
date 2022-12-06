using static System.IO.File;
using System.Text.RegularExpressions;

var full = ReadAllText("./marjetica1/marjetica.txt");
var p = @"([0-9]+) \r$";
var m = Regex.Match(full, p, RegexOptions.Multiline).Value;
m = m.Substring(0, m.Length-2);
var l = long.Parse(m);
var stacks = new Stack<char>[l];
for(long i=0; i<l; i++){
    stacks[i] = new Stack<char>();
}
var lines = full.Split("\r\n");
long line = 0;
while(true){
    if(lines[line].Length == 0){
        break;
    }
    var carts = lines[line].Split(']');
    long offset = 0;
    for(int i=0; i<carts.Length-1; i++){
        offset += (carts[i].Length-2)/4;
        stacks[offset].Push(carts[i][carts[i].Length-1]);
        offset++;
    }
    line++;
}
for(long i=0; i<l; i++){
    stacks[i] = new Stack<char>(stacks[i]);
}
var p2 = @"move ([0-9]+) from ([0-9]+) to ([0-9]+)";
var m2 = Regex.Matches(full, p2, RegexOptions.Multiline);
foreach(Match match in m2){
    var q = long.Parse(match.Groups[1].Value);
    var f = long.Parse(match.Groups[2].Value)-1;
    var t = long.Parse(match.Groups[3].Value)-1;
    for(int i=0; i<q; i++){
        stacks[t].Push(stacks[f].Pop());
    }
}
foreach(var s in stacks){
    Console.Write(s.Peek());
}
Console.WriteLine();