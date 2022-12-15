using static System.IO.File;

var positions = ReadAllLines("./tatjana1/tatjana.txt");
var lines = new Dictionary<int, List<(int, int)>>();
var min = 0;
var max = 4000000;
var dones = new bool[max+1];
foreach(var position in positions){
    var items = position.Split(':');
    var left = items[0].Split(',');
    var right = items[1].Split(',');
    var sensor = (int.Parse(left[0].Split('=')[1]), int.Parse(left[1].Split('=')[1]));
    var beacon = (int.Parse(right[0].Split('=')[1]), int.Parse(right[1].Split('=')[1]));
    var distance = Math.Abs(sensor.Item1 - beacon.Item1) + Math.Abs(sensor.Item2 - beacon.Item2);
    int y0 = sensor.Item2 - distance;
    y0 = y0 < min ? min : y0;
    int y1 = sensor.Item2 + distance;
    y1 = y1 > max ? max : y1;
    for(int j=y0; j<=y1; j++){
        if(dones[j]){
            continue;
        }
        if(!lines.TryGetValue(j, out var ranges)){
            lines[j] = new List<(int, int)>();
            ranges = lines[j];
        }
        var toline = Math.Abs(sensor.Item2 - j);
        var diff = distance-toline;
        var start = sensor.Item1-diff;
        start = start < min ? min : start;
        var end = sensor.Item1+diff;
        end = end > max ? max : end;
        if(ranges.Count == 0){
            ranges.Add((start, end));
            continue;
        }
        if(end < ranges.First().Item1){
            ranges.Insert(0, (start, end));
            continue;
        }
        if(start > ranges.Last().Item2){
            ranges.Insert(ranges.Count, (start, end));
            continue;
        }
        var overlaps = ranges.Select((r, i) => (r, i))
        .Where(r => {
            var range = r.Item1;
            return start >= range.Item1 && start <= range.Item2 ||
                    end >= range.Item1 && end <= range.Item2 ||
                    start < range.Item1 && end > range.Item2 ||
                    end == range.Item1-1 ||
                    start == range.Item2+1;
        });
        if(overlaps.Count() == 0){
            var middle = ranges.Select((r, i) => (r, i))
            .Where(r => {
                var range = r.Item1;
                return range.Item2 < start;
            })
            .Select(x => x.Item2)
            .First();
            ranges.Insert(middle+1, (start, end));
            continue;
        } else{
            var index = overlaps.First().Item2;
            var first = overlaps.First().Item1;
            var final = overlaps.Last().Item1;
            foreach(var overlap in overlaps.Reverse()){
                ranges.RemoveAt(overlap.Item2);
            }
            start = first.Item1 > start ? start : first.Item1;
            end = final.Item2 > end ? final.Item2 : end;
            ranges.Insert(index, (start, end));
            bool done = false;
            if(ranges.Count == 1){
                var range = ranges.First();
                if(range.Item1 == min && range.Item2 == max){
                    done = true;
                }
            }
            if(done){
                lines.Remove(j);
            }
            dones[j] = done;
        }
    }
}
var space = lines.First();
var spacey = space.Key;
var spacex = space.Value.First().Item2+1;
Console.WriteLine(spacex + ' ' + spacey);