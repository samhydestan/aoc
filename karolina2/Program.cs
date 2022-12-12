using static System.IO.File;

var chars = ReadAllText("./karolina1/karolina.txt");
(int, int) place = default;
var split = chars.Split("\r\n");
var map = new short[split.Length, split[0].Length];
for(int i=0; i<split.Length; i++){
    for(int j=0; j<split[i].Length; j++){
        if(split[i][j] == 'S'){
            map[i, j] = (short)'a';
        } else if(split[i][j] == 'E'){
            map[i, j] = (short)'z';
            place = (i, j);
        } else{
            map[i, j] = (short)split[i][j];
        }
    }
}
var q = new List<Path>();
var lava = new HashSet<(int, int)>();
lava.Add(place);
q.Add(new Path(place));
Action walking = delegate{
    while(q.Count() > 0){
        var nq = new List<Path>();
        foreach(var p in q){
            foreach(var d in (Direction[])Enum.GetValues(typeof(Direction))){
                if(p.Viable(d, ref map, ref place, ref lava)){
                    var movedp = p.Move(d, ref lava);
                    if(map[movedp.tile.Item1, movedp.tile.Item2] == (short)'a'){
                        Console.WriteLine(p.steps+1);
                        return;
                    }
                    nq.Add(movedp);
                }
            }
        }
        q = nq;
    }
};
walking();

class Path{
    public (int, int) tile;
    public int steps;
    public Path((int, int) t, int s = 0){
        tile = t;
        steps = s;
    }

    public Path Move(Direction d, ref HashSet<(int, int)> lava){
        Path? np = default;
        switch(d){
            case Direction.Up:
                np = new Path((tile.Item1-1, tile.Item2), steps+1);
                break;
            case Direction.Down:
                np = new Path((tile.Item1+1, tile.Item2), steps+1);
                break;
            case Direction.Left:
                np = new Path((tile.Item1, tile.Item2-1), steps+1);
                break;
            case Direction.Right:
                np = new Path((tile.Item1, tile.Item2+1), steps+1);
                break;
        }
        lava.Add(np!.tile);
        return np!;
    }

    public bool Viable(Direction d, ref short[,] map, ref (int, int) place, ref HashSet<(int, int)> lava){
        (int, int) step = default;
        switch(d){
            case Direction.Up:
                step = (tile.Item1-1, tile.Item2);
                break;
            case Direction.Down:
                step = (tile.Item1+1, tile.Item2);
                break;
            case Direction.Left:
                step = (tile.Item1, tile.Item2-1);
                break;
            case Direction.Right:
                step = (tile.Item1, tile.Item2+1);
                break;
        }
        if(!(
            lava.Contains(step) ||
            step.Item1 < 0 || step.Item2 < 0 ||
            step.Item1 >= map.GetLength(0) || step.Item2 >= map.GetLength(1) ||
            step.Item1 == place.Item1 && step.Item2 == place.Item2
        )){
            var diff = map[tile.Item1, tile.Item2] - map[step.Item1, step.Item2];
            return diff < 2 ;
        } else{
            return false;
        }
    }
}

enum Direction{
    Up,
    Down,
    Left,
    Right
};