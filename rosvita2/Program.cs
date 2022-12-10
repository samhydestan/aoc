using static System.IO.File;

var instructions = ReadAllLines("./rosvita1/rosvita.txt");
long cycle = 0;
long register = 1;
int vga = 0;
var screen = new List<char>();
foreach(var instruction in instructions){
    if(instruction != "noop"){
        tick(ref cycle, ref register, ref vga, ref screen);
        tick(ref cycle, ref register, ref vga, ref screen);
        register += long.Parse(instruction.Split(' ')[1]);
    } else{
        tick(ref cycle, ref register, ref vga, ref screen);
    }
}
for(int i=0; i<screen.Count(); i++){
    if(i>0 && i%40 == 0)
        Console.WriteLine();
    Console.Write(screen[i]);
}
Console.WriteLine();

void tick(ref long cycle, ref long register, ref int vga, ref List<char> screen){
    var p = vga%40;
    if(p-1 <= register && register <= p+1){
        screen.Add('#');
    } else{
        screen.Add('.');
    }
    vga++;
    cycle++;
}