using static System.IO.File;
using System.Diagnostics;
using System.Numerics;

var stop = new Stopwatch();
stop.Start();
BigInteger res = BigInteger.Zero;
Dictionary<long, long> aftercache = new Dictionary<long, long>();
long until = 1000000000000;
var peakcache = new List<long>();
var stopcache = new List<long>();
var jetcache = new List<int>();
var jets = ReadAllLines("./nevenka2/nevenka.txt")[0];
bool cycled = false;
long stopped = 0;
long dstopped = 0;
long dpeak = 0;
long peak = 0;
long floor = 0;
var peaks = new long[7];
Array.Fill(peaks, -1);
var plane = new Dictionary<long, bool[]>();
for(long i=0; i<4; i++){
    plane[i] = new bool[7];
}
long planey = 3;
short x = 2;
long y = 3;
short shape = 0;
int jet = 0;
bool done = false;
while(true){
    switch(shape){
        case 0:
            done = minus(
                jets[jet],
                ref x,
                ref y,
                plane,
                ref peak,
                ref floor,
                ref peaks,
                stopped
            );
            break;
        case 1:
            done = plus(
                jets[jet],
                ref x,
                ref y,
                plane,
                ref peak,
                ref floor,
                ref peaks,
                stopped
            );
            break;
        case 2:
            done = j(
                jets[jet],
                ref x,
                ref y,
                plane,
                ref peak,
                ref floor,
                ref peaks,
                stopped
            );
            break;
        case 3:
            done = stick(
                jets[jet],
                ref x,
                ref y,
                plane,
                ref peak,
                ref floor,
                ref peaks,
                stopped
            );
            break;
        case 4:
            done = box(
                jets[jet],
                ref x,
                ref y,
                plane,
                ref peak,
                ref floor,
                ref peaks,
                stopped
            );
            break;
    }
    if(jet == 0 && stopped > 0){
        cycled = true;
    }
    if(done){
        done = false;
        for(long i=planey-peak; i<=7; i++){
            plane[peak+i] = new bool[7];
            planey++;
        }
        y = peak+4;
        x = 2;
        if(cycled && shape == 0){
            var s = skip(until, aftercache, peakcache, stopcache, jetcache, jet, peak-dpeak, stopped-dstopped);
            if(!BigInteger.Equals(s, res)){
                res = s;
                break;
            }
            dstopped = stopped;
            dpeak = peak;
        }
        shape = (short)((shape+1)%5);
        cycled = false;
        aftercache[stopped] = peak;
        stopped++;
    }
    jet = ++jet%jets.Length;
}
Console.WriteLine(res);
stop!.Stop();
Console.WriteLine(stop!.Elapsed);

bool minus(
    char jet,
    ref short x,
    ref long y,
    Dictionary<long, bool[]> plane,
    ref long peak,
    ref long floor,
    ref long[] peaks,
    long stopped
){
    if(jet == '>'){
        if(x < 3 && !plane[y][x+4]){
            x++;
        }
    } else{
        if(x > 0 && !plane[y][x-1]){
            x--;
        }
    }
    if(y > floor && !plane[y-1][x] && !plane[y-1][x+1] && !plane[y-1][x+2] && !plane[y-1][x+3]){
        y--;
        return false;
    } else
    {
        plane[y][x] = true;
        plane[y][x + 1] = true;
        plane[y][x + 2] = true;
        plane[y][x + 3] = true;
        peak = peak > y ? peak : y;
        for (int i = x; i <= x + 3; i++)
        {
            if (peaks[i] < y)
            {
                peaks[i] = y;
            }
        }
        floor = clean(plane, floor, peaks);
        return true;
    }
}

bool plus(
    char jet,
    ref short x,
    ref long y,
    Dictionary<long, bool[]> plane,
    ref long peak,
    ref long floor,
    ref long[] peaks,
    long stopped
){
    if(jet == '>'){
        if(x < 4 && !plane[y+1][x+3] && !plane[y][x+2]){
            x++;
        }
    } else{
        if(x > 0 && !plane[y+1][x-1] && !plane[y][x]){
            x--;
        }
    }
    if(y > floor && !plane[y][x] && !plane[y-1][x+1] && !plane[y][x+2]){
        y--;
        return false;
    } else{
        plane[y+1][x] = true;
        plane[y+1][x+2] = true;
        plane[y+2][x+1] = true;
        peak = peak > y+2 ? peak : y+2;
        if(peaks[x] < y+1){
            peaks[x] = y+1;
        }
        if(peaks[x+2] < y+1){
            peaks[x+2] = y+1;
        }
        if(peaks[x+1] < y+2){
            peaks[x+1] = y+2;
        }
        floor = clean(plane, floor, peaks);
        return true;
    }
}

bool j(
    char jet,
    ref short x,
    ref long y,
    Dictionary<long, bool[]> plane,
    ref long peak,
    ref long floor,
    ref long[] peaks,
    long stopped
){
    if(jet == '>'){
        if(x < 4 && !plane[y][x+3] && !plane[y+1][x+3] && !plane[y+2][x+3]){
            x++;
        }
    } else{
        if(x > 0 && !plane[y][x-1]){
            x--;
        }
    }
    if(y > floor && !plane[y-1][x] && !plane[y-1][x+1] && !plane[y-1][x+2]){
        y--;
        return false;
    } else{
        plane[y][x] = true;
        plane[y][x+1] = true;
        plane[y][x+2] = true;
        plane[y+1][x+2] = true;
        plane[y+2][x+2] = true;
        peak = peak > y+2 ? peak : y+2;
        if(peaks[x] < y){
            peaks[x] = y;
        }
        if(peaks[x+1] < y){
            peaks[x+1] = y;
        }
        if(peaks[x+2] < y+2){
            peaks[x+2] = y+2;
        }
        floor = clean(plane, floor, peaks);
        return true;
    }
}

bool stick(
    char jet,
    ref short x,
    ref long y,
    Dictionary<long, bool[]> plane,
    ref long peak,
    ref long floor,
    ref long[] peaks,
    long stopped
){
    if(jet == '>'){
        if(x < 6 && !plane[y][x+1] && !plane[y+1][x+1] && !plane[y+2][x+1] && !plane[y+3][x+1]){
            x++;
        }
    } else{
        if(x > 0 && !plane[y][x-1] && !plane[y+1][x-1] && !plane[y+2][x-1] && !plane[y+3][x-1]){
            x--;
        }
    }
    if(y > floor && !plane[y-1][x]){
        y--;
        return false;
    } else{
        plane[y][x] = true;
        plane[y+1][x] = true;
        plane[y+2][x] = true;
        plane[y+3][x] = true;
        peak = peak > y+3 ? peak : y+3;
        if(peaks[x] < y+3){
            peaks[x] = y+3;
        }
        floor = clean(plane, floor, peaks);
        return true;
    }
}

bool box(
    char jet,
    ref short x,
    ref long y,
    Dictionary<long, bool[]> plane,
    ref long peak,
    ref long floor,
    ref long[] peaks,
    long stopped
){
    if(jet == '>'){
        if(x < 5 && !plane[y][x+2] && !plane[y+1][x+2]){
            x++;
        }
    } else{
        if(x > 0 && !plane[y][x-1] && !plane[y+1][x-1]){
            x--;
        }
    }
    if(y > floor && !plane[y-1][x] && !plane[y-1][x+1]){
        y--;
        return false;
    } else{
        plane[y][x] = true;
        plane[y][x+1] = true;
        plane[y+1][x] = true;
        plane[y+1][x+1] = true;
        peak = peak > y+1 ? peak : y+1;
        if(peaks[x] < y+1){
            peaks[x] = y+1;
        }
        if(peaks[x+1] < y+1){
            peaks[x+1] = y+1;
        }
        floor = clean(plane, floor, peaks);
        return true;
    }
}

long clean(Dictionary<long, bool[]> plane, long floor, long[] peaks)
{
    var nfloor = peaks.Min();
    if(nfloor > -1){
        for (long i = nfloor; i >= floor; i--)
        {
            plane.Remove(i);
        }
        floor = nfloor + 1;
    }
    return floor;
}

BigInteger skip (
    long until,
    Dictionary<long, long> aftercache,
    List<long> peakcache,
    List<long> stopcache,
    List<int> jetcache,
    int jet,
    long peakgain,
    long stopgain
){
    jetcache.Add(jet);
    peakcache.Add(peakgain);
    stopcache.Add(stopgain);
    if(jetcache.Count()%2 == 0){
        var a = jetcache.Take(jetcache.Count()/2);
        var b = jetcache.Skip(jetcache.Count()/2);
        if(a.SequenceEqual(b)){
            var before = peakcache.Take(peakcache.Count()/2).Sum();
            var beforestops = stopcache.Take(stopcache.Count()/2).Sum();
            var cycle = peakcache.Skip(peakcache.Count()/2).Sum();
            var cyclestops = stopcache.Skip(stopcache.Count()/2).Sum();
            var cycles = (until - beforestops)/cyclestops;
            var after = (until - beforestops)%cyclestops;
            var end = new BigInteger(aftercache[beforestops+after] - aftercache[beforestops]);
            var mid = BigInteger.Multiply(new BigInteger(cycle), new BigInteger(cycles));
            return BigInteger.Add(BigInteger.Add(new BigInteger(before), mid), end);
        }
    }
    return BigInteger.Zero;
}