using static System.IO.File;
using System.Diagnostics;

var stop = new Stopwatch();
stop.Start();
var nums = ReadAllLines("./darinka1/darinka.txt");
var ns = new Dictionary<int, Node>();
var final = new List<Node>();
int i = 0;
foreach(var num in nums){
    var x = int.Parse(num);
    var n = new Node(i, x);
    ns[i] = n;
    final.Insert(i, n);
    i++;
}
i = 0;
while(i < nums.Length){
    if(ns[i].value != 0){
        int to;
        var p = ns[i].position;
        var moved = p+ns[i].value;
        if(moved > 0){
            to = moved%(nums.Length-1);
        } else{
            to = nums.Length-1+moved%(nums.Length-1);
        }
        ns[i].position = to;
        final.RemoveAt(p);
        final.Insert(to, ns[i]);
        if(p < to){
            for(int j=p; j<to; j++){
                final[j].position--;
            }
        } else{
            for(int j=p; j>to; j--){
                final[j].position++;
            }
        }
    }
    i++;
}
int zero = final.FindIndex(0, final.Count(), n => n.value == 0);
Console.WriteLine(final[(1000+zero)%nums.Length].value +
    final[(2000+zero)%nums.Length].value +
    final[(3000+zero)%nums.Length].value);
stop.Stop();
Console.WriteLine(stop.Elapsed);

class Node{
    public int position;
    public int value;

    public Node(int p, int v){
        position = p;
        value = v;
    }
}