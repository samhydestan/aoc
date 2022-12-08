using static System.IO.File;

var lines = ReadAllLines("./čedomila1/čedomila.txt");
var matrix = lines.Select(line => 
    line.Select(t => 
        new Tree(short.Parse(new ReadOnlySpan<char>(t))))
    .ToArray())
.ToArray();
Console.WriteLine(views(ref matrix));

long views(ref Tree[][] trees){
    long b1g = 0;
    for(int i=0; i<trees.Length; i++){
        for(int j=0; j<trees[0].Length; j++){
            if(view(ref trees, j, i) > b1g){
                b1g = view(ref trees, j, i);
            }
        }
    }
    return b1g;
}

long view(ref Tree[][] trees, int x, int y){
    if(x == 0 || x == trees[0].Length || y == 0 || y == trees.Length){
        return 0;
    }
    long c = 0;
    for(int i=x-1; i>=0; i--){
        c++;
        if(trees[y][i].h >= trees[y][x].h){
            break;
        }
    }
    trees[y][x].left = c;
    c = 0;
    for(int i=x+1; i<trees[0].Length; i++){
        c++;
        if(trees[y][i].h >= trees[y][x].h){
            break;
        }
    }
    trees[y][x].right = c;
    c = 0;
    for(int i=y-1; i>=0; i--){
        c++;
        if(trees[i][x].h >= trees[y][x].h){
            break;
        }
    }
    trees[y][x].up = c;
    c = 0;
    for(int i=y+1; i<trees.Length; i++){
        c++;
        if(trees[i][x].h >= trees[y][x].h){
            break;
        }
    }
    trees[y][x].down = c;
    return trees[y][x].up * trees[y][x].down * trees[y][x].left * trees[y][x].right;
}

class Tree{
    public long up;
    public long down;
    public long left;
    public long right;
    public short h;

    public Tree(short h){
        this.h = h;
        up = 0;
        down = 0;
        left = 0;
        right = 0;
    }
}