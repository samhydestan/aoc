using static System.IO.File;

var lines = ReadAllLines("./zvonimira1/zvonimira.txt");
Dir? dir = null;
Dir root = new Dir();
foreach(var line in lines){
    if(line == "$ ls"){
        continue;
    }
    if(line.StartsWith("$ cd")){
        var path = line.Substring(5);
        switch(path){
            case "/":
                dir = root;
                break;
            case "..":
                dir = dir!.Parent();
                break;
            default:
                dir!.TryAdd(path, new Dir(dir));
                dir = (Dir)dir[path];
                break;
        }
    } else if(line.StartsWith("dir ")){
        var path = line.Substring(4);
        dir!.TryAdd(path, new Dir(dir));
    } else{
        var s = line.Split(' ');
        dir!.TryAdd(s[1], new File(long.Parse(s[0])));
    }
}
Console.WriteLine(root.Scan());

interface IEntry{
    public long Size();
}

class File : IEntry{
    private readonly long _size;

    public File(long size){
        _size = size;
    }

    public long Size() => _size;
}

class Dir : Dictionary<string, IEntry>, IEntry{
    private readonly Dir? _parent;
    private long _size = -1;

    public Dir(Dir? parent = null){
        _parent = parent;
    }

    public long Size(){
        if(_size < 0){
            _size = 0;
            foreach(var entry in this.Values){
                _size += entry.Size();
            }
        }
        return _size;
    }

    public Dir? Parent() => _parent;

    public long Scan(){
        long sum = 0;
        foreach(var entry in this.Values){
            if(entry is Dir d){
                sum += d.Scan();
            }
        }
        if(this.Size()<=100000){
            sum += this.Size();
        }
        return sum;
    }
}