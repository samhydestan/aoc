using static System.IO.File;

var lines = ReadAllLines("./albina1/albina.txt");
var pairs = new List<(List<object>, List<object>)>();
int sub = 1;
for(int i=0; i<lines.Length; i+=3){
    var left = read(lines[i], ref sub);
    sub = 1;
    var right = read(lines[i+1], ref sub);
    sub = 1;
    pairs.Add((left, right));
}
int j = 1;
int sum = 0;
foreach(var pair in pairs){
    if(CompareLists(pair.Item1, pair.Item2) >= 0){
        sum += j;
    }
    j++;
}
Console.WriteLine(sum);

int CompareLists(List<object> l, List<object> r){
    int i = 0;
    while(true){
        if(i >= l.Count()){
            if(l.Count() == r.Count()){
                return 0;
            }
            return 1;
        }
        if(i >= r.Count()){
            return -1;
        }
        var lt = l[i].GetType();
        var rt = r[i].GetType();
        if(lt == rt){
            if(lt == typeof(int)){
                var li = (int)l[i];
                var ri = (int)r[i];
                if(li < ri){
                    return 1;
                } else if(li > ri){
                    return -1;
                }
            } else{
                var c = CompareLists((List<object>)l[i], (List<object>)r[i]);
                if(c != 0){
                    return c;
                }
            }
        } else{
            if(lt == typeof(int)){
                l[i] = new List<object>(new object[1]{l[i]});
            } else{
                r[i] = new List<object>(new object[1]{r[i]});
            }
            var c = CompareLists((List<object>)l[i], (List<object>)r[i]);
            if(c != 0){
                return c;
            }
        }
        i++;
    }
}

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