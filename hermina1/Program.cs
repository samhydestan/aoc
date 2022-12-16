using static System.IO.File;
using System.Diagnostics;

var stop = new Stopwatch();
stop.Start();
var lines = ReadAllLines("./hermina1/hermina.txt");
var graph = new Dictionary<string, Node>();
foreach(var line in lines){
    var split = line.Split(' ');
    var name = split[1];
    var rate = int.Parse(split[4].Substring(5, split[4].Length-5-1));
    var adj = split.Reverse().Take(split.Count()-9).Select(x => x.EndsWith(',') ?
                                                                x.Substring(0, x.Length-1) :
                                                                x);
    graph[name] = new Node(adj, rate, name);
}
var others = graph.Keys.ToList().Where(x => x != "AA");
foreach(var name in others){
    if(name == "AA"){
        continue;
    }
    var node = graph[name];
    if(node.rate == 0){
        foreach(var step in node.paths.Keys){
            var stepnode = graph[step];
            var distance = stepnode.paths[name];
            stepnode.paths.Remove(name);
            foreach(var other in node.paths.Keys){
                if(other == step){
                    continue;
                }
                stepnode.paths.TryAdd(other, node.paths[other]+distance);
            }
        }
        graph.Remove(name);
    }
}
foreach(var name in graph.Keys){
    var node = graph[name];
    var far = graph.Keys.Except(node.paths.Keys).Where(x => x != name);
    foreach(var other in far){
        node.paths.Add(other, int.MaxValue);
    }
}
foreach(var middle in graph.Keys){
    foreach(var start in graph.Keys){
        foreach(var end in graph.Keys){
            if(start == middle || start == end || middle == end){
                continue;
            }
            int stoe = graph[start].paths[end];
            int stom = graph[start].paths[middle];
            int mtoe = graph[middle].paths[end];
            if(stom < int.MaxValue && mtoe < int.MaxValue && stoe > stom + mtoe){
                graph[start].paths[end] = stom + mtoe;
            }
        }
    }
}
var minutes = 30;
var max = 0;
var paths = new List<Path>();
paths.Add(new Path(minutes, graph.Keys.Where(x => x != "AA"), graph["AA"]));
while(paths.Any()){
    var npaths = new List<Path>();
    foreach(var path in paths){
        var news = path.step(graph);
        max = news.Item2 > max ? news.Item2 : max;
        npaths.AddRange(news.Item1);
    }
    paths = npaths;
}
Console.WriteLine(max);
stop.Stop();
Console.WriteLine(stop.Elapsed);

class Path{
    List<string> choices;
    int minutes;
    int flow;
    Node node;

    public Path(int m, IEnumerable<string> c, Node s, int f = 0){
        flow = f;
        minutes = m;
        choices = new List<string>(c);
        node = s;
    }

    public (List<Path>, int) step(Dictionary<string, Node> graph){
        List<Path> future = new List<Path>();
        int max = 0;
        foreach(var next in choices){
            if(minutes - node.paths[next] - 1 < 0){
                continue;
            }
            var nextnode = graph[next];
            var nextminutes = minutes - node.paths[next] - 1;
            var nextflow = flow + nextminutes * nextnode.rate;
            var nextchoices = choices.Where(c => c != next);
            Path p = new Path(
                nextminutes,
                nextchoices,
                nextnode,
                nextflow
            );
            max = nextflow > max ? nextflow : max;
            future.Add(p);
        }
        return (future, max);
    }
}

class Node{
    public Dictionary<string, int> paths;
    public int rate;
    public string name;

    public Node(IEnumerable<string> r, int ra, string n){
        paths = new Dictionary<string, int>(r.Select(x => new KeyValuePair<string, int>(x, 1)));
        rate = ra;
        name = n;
    }
}