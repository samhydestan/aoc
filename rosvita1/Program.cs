using static System.IO.File;

var instructions = ReadAllLines("./rosvita1/rosvita.txt");
long cycle = 0;
long register = 1;
long strength = 0;
foreach(var instruction in instructions){
    if(instruction != "noop"){
        tick(ref cycle, ref strength, ref register);
        tick(ref cycle, ref strength, ref register);
        register += long.Parse(instruction.Split(' ')[1]);
    } else{
        tick(ref cycle, ref strength, ref register);
    }
    if(cycle > 220){
        break;
    }
}
Console.WriteLine(strength);

void tick(ref long cycle, ref long strength, ref long register){
    cycle++;
    if((cycle-20)%40 == 0){
        strength += cycle*register;
    }
}