using static System.IO.File;

var rounds = ReadAllLines("./karla1/karla.txt");
long sum = 0;
foreach(var round in rounds){
    var re = round[0];
    var exp = round[2];
    char tard = default;
    switch(exp){
        case 'X':
            switch(re){
                case 'A':
                    tard = 'C';
                    break;
                case 'B':
                    tard = 'A';
                    break;
                case 'C':
                    tard = 'B';
                    break;
            }
            break;
        case 'Y':
            tard = re;
            sum += 3;
            break;
        case 'Z':
            switch(re){
                case 'A':
                    tard = 'B';
                    break;
                case 'B':
                    tard = 'C';
                    break;
                case 'C':
                    tard = 'A';
                    break;
            }
            sum += 6;
            break;
    }
    sum += ((short)tard)-64;
}
Console.WriteLine($"{sum}");