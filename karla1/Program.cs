using static System.IO.File;

var rounds = ReadAllLines("./karla1/karla.txt");
long sum = 0;
foreach(var round in rounds){
    var re = round[0];
    var sh = (short)round[2];
    var tard = (char)(sh-23);
    sh -= 87;
    sum += sh;
    if(re == tard){
        sum += 3;
        continue;
    }
    if(tard == 'A' && re == 'C' || tard == 'B' && re == 'A' || tard == 'C' && re == 'B'){
        sum += 6;
    }
}
Console.WriteLine($"{sum}");