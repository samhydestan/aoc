using static System.IO.File;

string[] food = ReadAllLines("./ernesta1/ernesta.txt");
long[] b1g = new long[3];
long carry = 0;
foreach(var item in food){
    if(item.Length>0){
        carry += long.Parse(item);
    } else{
        replace(b1g, carry);
        carry = 0;
    }
}
replace(b1g, carry);
Console.WriteLine($"{b1g[0]+b1g[1]+b1g[2]}");

void replace(long[] b1g, long carry){
    for(int i=2; i>0; i--){
        if(carry>=b1g[i]){
            for(int j=0; j<=i-1; j++){
                b1g[j] = b1g[j+1];
            }
            b1g[i] = carry;
            break;
        }
    }
}