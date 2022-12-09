using static System.IO.File;

var moves = ReadAllLines("./olivija1/olivija.txt");
var treked = new HashSet<(long, long)>();
treked.Add((0, 0));
var rope = new (long, long)[10];
Array.Fill(rope, (0, 0));
long tiles = 1;
foreach(var move in moves){
    var parsed = move.Split(' ');
    switch(parsed[0]){
        case "U":
            for(int i=0; i<int.Parse(parsed[1]); i++){
                for(int j=0; j<9; j++){
                    if(j == 0){
                        rope[j] = (rope[j].Item1, rope[j].Item2+1);
                    }
                    tiles += step(ref rope[j], ref rope[j+1], ref treked, j==8);
                }
            }
            break;
        case "D":
            for(int i=0; i<int.Parse(parsed[1]); i++){
                for(int j=0; j<9; j++){
                    if(j == 0){
                        rope[j] = (rope[j].Item1, rope[j].Item2-1);
                    }
                    tiles += step(ref rope[j], ref rope[j+1], ref treked, j==8);
                }
            }
            break;
        case "R":
            for(int i=0; i<int.Parse(parsed[1]); i++){
                for(int j=0; j<9; j++){ 
                    if(j == 0){
                        rope[j] = (rope[j].Item1+1, rope[j].Item2);
                    }
                    tiles += step(ref rope[j], ref rope[j+1], ref treked, j==8);
                }
            }
            break;
        case "L":
            for(int i=0; i<int.Parse(parsed[1]); i++){
                for(int j=0; j<9; j++){
                    if(j == 0){
                        rope[j] = (rope[j].Item1-1, rope[j].Item2);
                    }
                    tiles += step(ref rope[j], ref rope[j+1], ref treked, j==8);
                }
            }
            break;
    }
}
Console.WriteLine(tiles);

long step(ref (long, long) head, ref (long, long) tail, ref HashSet<(long, long)> treked, bool count){
    long euclidy = head.Item2-tail.Item2;
    long euclidx = head.Item1-tail.Item1;
    if(Math.Abs(euclidx) <= 1 && Math.Abs(euclidy) <= 1){
        return 0;
    }
    if(euclidy > 1){
        if(euclidx > 1){
            tail = (head.Item1-1, head.Item2-1);
        } else if(euclidx < -1){
            tail = (head.Item1+1, head.Item2-1);
        } else{
            tail = (head.Item1, head.Item2-1);
        }
    } else if(euclidy < -1){
        if(euclidx > 1){
            tail = (head.Item1-1, head.Item2+1);
        } else if(euclidx < -1){
            tail = (head.Item1+1, head.Item2+1);
        } else{
            tail = (head.Item1, head.Item2+1);
        }
    } else{
        if(euclidx > 1){
            tail = (head.Item1-1, head.Item2);
        } else if(euclidx < -1){
            tail = (head.Item1+1, head.Item2);
        }
    }
    if(!count){
        return 0;
    }
    var maybe = (tail.Item1, tail.Item2);
    if(!treked.Contains(maybe)){
        treked.Add(maybe);
        return 1;
    }
    return 0;
}