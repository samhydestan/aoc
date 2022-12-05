using static System.IO.File;

string[] food = ReadAllLines("./ernesta/ernesta.txt");
long b1g = 0;
long carry = 0;
foreach(var item in food){
    if(item.Length>0){
        carry += long.Parse(item);
    } else{
        b1g = carry>b1g ? carry : b1g;
        carry = 0;
    }
}
b1g = carry>b1g ? carry : b1g;
Console.WriteLine($"{b1g}");