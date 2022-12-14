using static System.IO.File;
using System.Linq;

var lines = ReadAllLines("./albina1/albina.txt");
var packs = new List<List<object>>();
int sub = 1;
for(int i=0; i<lines.Length; i++){
    if(lines[i].Length > 0){
        packs.Add(read(lines[i], ref sub));
        sub = 1;
    }
}
var p1 = new List<object>();
p1.Add(new List<object>(new object[1]{2}));
var p2 = new List<object>();
p2.Add(new List<object>(new object[1]{6}));
packs.Add(p1);
packs.Add(p2);
packs.Sort(new PackComparer());
Console.WriteLine(
    (packs.FindIndex(p => p1.SequenceEqual(p)) + 1) *
    (packs.FindIndex(p => p2.SequenceEqual(p)) + 1));

List<object> read(string line, ref int sub){
    var l = new List<object>();
    while(true){
        switch(line[sub]){
            case '[':
                sub++;
                l.Add(read(line, ref sub));
                break;
            case ']':
                sub++;
                return l;
            case ',':
                sub++;
                break;
            default:
                int i = 0;
                while(line[sub] >= '0' && line[sub] <= '9'){
                    i *= 10;
                    i += int.Parse(new ReadOnlySpan<char>(line[sub]));
                    sub++;
                }
                l.Add(i);
                break;
        }
    }
    throw new Exception("uhhh ??");
}

public class PackComparer : Comparer<List<object>>{
    public override int Compare(List<object>? l, List<object>? r)
    {
        if(l == null || r == null){
            throw new ArgumentNullException();
        }
        int i = 0;
        while(true){
            if(i >= l.Count()){
                if(l.Count() == r.Count()){
                    return 0;
                }
                return -1;
            }
            if(i >= r.Count()){
                return 1;
            }
            var lt = l[i].GetType();
            var rt = r[i].GetType();
            if(lt == rt){
                if(lt == typeof(int)){
                    var li = (int)l[i];
                    var ri = (int)r[i];
                    if(li < ri){
                        return -1;
                    } else if(li > ri){
                        return 1;
                    }
                } else{
                    var c = Compare((List<object>)l[i], (List<object>)r[i]);
                    if(c != 0){
                        return c;
                    }
                }
            } else{
                int c;
                if(lt == typeof(int)){
                    c = Compare(new List<object>(new object[1]{l[i]}), (List<object>)r[i]);
                } else{
                    c = Compare((List<object>)l[i], new List<object>(new object[1]{r[i]}));
                }
                if(c != 0){
                    return c;
                }
            }
            i++;
        }
    }
}