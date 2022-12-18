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
var minutes = 26;
var max = 0;
var nextmax = -1;
var accumulator = 0;
int take = 1;
List<Path> paths;
while(nextmax != max){
    paths = new List<Path>();
    paths.Add(new Path(minutes, minutes, graph.Keys.Where(x => x != "AA"), graph["AA"], graph["AA"]));
    while(paths.Any()){
        var npaths = new List<Path>();
        foreach(var path in paths){
            var news = path.step(graph, take);
            accumulator = news.Item2 > accumulator ? news.Item2 : accumulator;
            npaths.AddRange(news.Item1);
        }
        paths = npaths;
    }
    if(take == 1){
        max = accumulator;
    } else{
        max = nextmax;
        nextmax = accumulator;
    }
    accumulator = 0;
    take++;
}
Console.WriteLine(max);
stop.Stop();
Console.WriteLine(stop.Elapsed);

class Path{
    List<string> choices;
    int minutes;
    int elephantminutes;
    int flow;
    Node? node;
    Node? elephant;

    public Path(int m, int me, IEnumerable<string> c, Node? s, Node? e, int f = 0){
        flow = f;
        minutes = m;
        elephantminutes = me;
        choices = new List<string>(c);
        elephant = e;
        node = s;
    }

    public (List<Path>, int) step(Dictionary<string, Node> graph, int take){
        if(!choices.Any()){
            return (new List<Path>(), 0);
        }
        int max = 0;
        List<Path> future = new List<Path>();
        if(node != null && elephant != null){
            var pairs = from human in choices
                        from elek in choices
                        where human != elek
                        select (
                            human,
                            elek,
                            minutes - node.paths[human] - 1,
                            elephantminutes - elephant.paths[elek] - 1,
                            (minutes - node.paths[human] - 1) * graph[human].rate +
                            (elephantminutes - elephant.paths[elek] - 1) * graph[elek].rate);
            pairs = pairs.OrderByDescending(x => x.Item5);
            pairs = pairs.Take(pairs.Count() < take ? pairs.Count() : take);
            foreach(var pair in pairs){
                var newflow = flow + pair.Item5;
                if(newflow > max){
                    max = newflow;
                }
                var p = new Path(
                    pair.Item3,
                    pair.Item4,
                    choices.Where(x => x != pair.Item1 && x != pair.Item2),
                    pair.Item3 > 0 ? graph[pair.Item1] : null,
                    pair.Item4 > 0 ? graph[pair.Item2] : null,
                    newflow
                );
                future.Add(p);
            }
        } else if(node != null){
            var next = choices.Select(x =>{
                var time = minutes - node.paths[x] - 1;
                var gain = graph[x].rate * time;
                return (x, time, gain);
            });
            var best = next.MaxBy(x => x.Item3).Item3;
            var bests = next.Where(x => x.Item3 == best);
            foreach(var b in bests){
                var newflow = flow + b.Item3;
                if(newflow > max){
                    max = newflow;
                }
                var p = new Path(
                    b.Item2,
                    elephantminutes,
                    choices.Where(x => x != b.Item1),
                    graph[b.Item1],
                    null,
                    newflow
                );
                future.Add(p);
            }
        } else if(elephant != null){
            var next = choices.Select(x =>{
                var time = minutes - elephant.paths[x] - 1;
                var gain = graph[x].rate * time;
                return (x, time, gain);
            });
            var best = next.MaxBy(x => x.Item3).Item3;
            var bests = next.Where(x => x.Item3 == best);
            foreach(var b in bests){
                var newflow = flow + b.Item3;
                if(newflow > max){
                    max = newflow;
                }
                var p = new Path(
                    minutes,
                    b.Item2,
                    choices.Where(x => x != b.Item1),
                    null,
                    graph[b.Item1],
                    newflow
                );
                future.Add(p);
            }
        }
        future = future.Where(x => x.node != null || x.elephant != null).ToList();
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