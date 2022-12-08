using static System.IO.File;

var lines = ReadAllLines("./čedomila1/čedomila.txt");
var matrix = lines.Select(line => 
    line.Select(t => 
        (short.Parse(new ReadOnlySpan<char>(t)), false))
    .ToArray())
.ToArray();
long trees = 0;
foreach(var row in matrix){
    short max = -1;
    for(int i=0; i<row.Length; i++){
        if(row[i].Item1 > max){
            max = row[i].Item1;
            trees++;
            row[i].Item2 = true;
        }
    }
    short mirror = -1;
    for(int i=row.Length-1; i>=0; i--){
        if(row[i].Item1 == max){
            if(!row[i].Item2){
                row[i].Item2 = true;
                trees++;
            }
            break;
        }
        if(row[i].Item1 > mirror){
            mirror = row[i].Item1;
            trees++;
            row[i].Item2 = true;
        }
    }
}
for(int i=1; i<matrix[0].Length-1; i++){
    short max = -1;
    for(int j=0; j<matrix.Length; j++){
        if(matrix[j][i].Item1 > max){
            max = matrix[j][i].Item1;
            if(!matrix[j][i].Item2){
                trees++;
                matrix[j][i].Item2 = true;
            }
        }
    }
    short mirror = -1;
    for(int j=matrix.Length-1; j>=0; j--){
        if(matrix[j][i].Item1 == max){
            if(!matrix[j][i].Item2){
                trees++;
            }
            break;
        }
        if(matrix[j][i].Item1 > mirror){
            mirror = matrix[j][i].Item1;
            if(!matrix[j][i].Item2){
                matrix[j][i].Item2 = true;
                trees++;
            }
        }
    }
}
Console.WriteLine(trees);