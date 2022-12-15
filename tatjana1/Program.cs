using static System.IO.File;
using System.Collections;

var positions = ReadAllLines("./tatjana1/tatjana.txt");
var line = new ArrayList();
var beacons = new HashSet<int>();
var y = 2000000;
int used = 0;
foreach(var position in positions){
    var items = position.Split(':');
    var left = items[0].Split(',');
    var right = items[1].Split(',');
    var sensor = (int.Parse(left[0].Split('=')[1]), int.Parse(left[1].Split('=')[1]));
    var beacon = (int.Parse(right[0].Split('=')[1]), int.Parse(right[1].Split('=')[1]));
    if(beacon.Item2 == y){
        beacons.Add(beacon.Item1);
    }
    var distance = Math.Abs(sensor.Item1 - beacon.Item1) + Math.Abs(sensor.Item2 - beacon.Item2);
    var toline = Math.Abs(sensor.Item2 - y);
    var diff = distance-toline;
    if(diff >= 0){
        var start = sensor.Item1-diff;
        var end = sensor.Item1+diff;
        if(line.Count == 0){
            line.Add((start, end));
            used += end-start+1-Overlaying(start, end);
            continue;
        }
        if(end < (((int, int))line[0]!).Item1){
            line.Insert(0, (start, end));
            used += end-start+1-Overlaying(start, end);
            continue;
        }
        if(start > (((int, int))line[line.Count-1]!).Item2){
            line.Insert(line.Count, (start, end));
            used += end-start+1-Overlaying(start, end);
            continue;
        }
        var overlaps = line.ToArray().Where(o => {
            var range = ((int, int))o!;
            return end >= range.Item1 || start <= range.Item2;
        }).Select((o, i) => (((int, int))o!, i));
        foreach(var overlap in overlaps.Reverse()){
            var overlaying = Overlaying(overlap.Item1.Item1, overlap.Item1.Item2);
            used -= overlap.Item1.Item2-overlap.Item1.Item1+1-overlaying;
            line.RemoveAt(overlap.Item2);
        }
        var index = overlaps.First().Item2;
        var first = overlaps.First().Item1;
        var final = overlaps.Last().Item1;
        start = first.Item1 > start ? start : first.Item1;
        end = final.Item2 > end ? final.Item2 : end;
        line.Insert(index, (start, end));
        used += end-start+1-Overlaying(start, end);
    }
}
Console.WriteLine(used);

int Overlaying(int s, int e){
    return beacons!.Where(i => i >= s && i <= e).Count();
}