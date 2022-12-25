using static System.IO.File;
using System.Diagnostics;

var stop = new Stopwatch();
stop.Start();
var prints = ReadAllLines("./julijana1/julijana.txt");
var blueprints = new Blueprint[prints.Length];
for(int i=0; i<prints.Length; i++){
    var print = prints[i];
    var costs = print.Split(':')[1].Split('.');
    var ore = int.Parse(costs[0].Split(' ')[5]);
    var clay = int.Parse(costs[1].Split(' ')[5]);
    var o = costs[2].Split(' ');
    var obsidianore = int.Parse(o[5]);
    var obsidianclay = int.Parse(o[8]);
    var g = costs[3].Split(' ');
    var geodeore = int.Parse(g[5]);
    var geodeobsidian = int.Parse(g[8]);
    blueprints[i] = new Blueprint(ore, clay, obsidianore, obsidianclay, geodeore, geodeobsidian);
}
int sum = 0;
for(int i=1; i<=blueprints.Length; i++){
    var m = simulate(blueprints[i-1]);
    sum += i*m;
}
Console.WriteLine(sum);
stop.Stop();
Console.WriteLine(stop.Elapsed);

int simulate(Blueprint b){
    int max = int.MinValue;
    int minute = 24;
    var q = new List<(Bot, Resources)>();
    q.Add((Bot.None, new Resources()));
    while(minute > 0){
        var nq = new List<(Bot, Resources)>();
        foreach(var path in q){
            var bot = path.Item1;
            var resources = path.Item2;
            resources.ore += resources.orebot;
            resources.clay += resources.claybot;
            resources.obsidian += resources.obsidianbot;
            resources.geode += resources.geodebot;
            if(resources.geode > max){
                max = resources.geode;
            }
            switch(bot){
                case Bot.Ore:
                    resources.ore -= b.ore;
                    resources.orebot++;
                    break;
                case Bot.Clay:
                    resources.ore -= b.clay;
                    resources.claybot++;
                    break;
                case Bot.Obsidian:
                    resources.ore -= b.obsidianore;
                    resources.clay -= b.obsidianclay;
                    break;
                case Bot.Geode:
                    resources.ore -= b.geodeore;
                    resources.obsidian -= b.geodeobsidian;
                    break;
                case Bot.None:
                    break;
            }
            if(resources.ore >= b.ore){
                nq.Add((Bot.Ore, new Resources(resources)));
            }
            if(resources.ore >= b.clay){
                nq.Add((Bot.Clay, new Resources(resources)));
            }
            if(resources.ore >= b.obsidianore && resources.clay >= b.obsidianclay){
                nq.Add((Bot.Obsidian, new Resources(resources)));
            }
            if(resources.ore >= b.geodeore && resources.obsidian >= b.geodeobsidian){
                nq.Add((Bot.Geode, new Resources(resources)));
            }
            nq.Add((Bot.None, new Resources(resources)));
        }
        q = nq;
        minute--;
    }
    return max;
}

enum Bot{
    None,
    Ore,
    Clay,
    Obsidian,
    Geode
}

class Resources{
    public int ore;
    public int clay;
    public int obsidian;
    public int geode;
    public int orebot;
    public int claybot;
    public int obsidianbot;
    public int geodebot;

    public Resources(
        int o = 0, int c = 0, int ob = 0, int g = 0,
        int or = 1, int cr = 0, int obr = 0, int gr = 0
    ){
        ore = o;
        clay = c;
        obsidian = ob;
        geode = g;
        orebot = or;
        claybot = cr;
        obsidianbot = obr;
        geodebot = gr;
    }

    public Resources(Resources r){
        ore = r.ore;
        clay = r.clay;
        obsidian = r.obsidian;
        geode = r.geode;
        orebot = r.orebot;
        claybot = r.claybot;
        obsidianbot = r.obsidianbot;
        geodebot = r.geodebot;
    }
}

class Blueprint{
    public int ore;
    public int clay;
    public int obsidianore;
    public int obsidianclay;
    public int geodeore;
    public int geodeobsidian;

    public Blueprint(int o, int c, int oo, int oc, int go, int gob){
        ore = o;
        clay = c;
        obsidianore = oo;
        obsidianclay = oc;
        geodeore = go;
        geodeobsidian = gob;
    }
}