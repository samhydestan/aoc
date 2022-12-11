using static System.IO.File;
using System.Linq.Expressions;

var ap3s= ReadAllLines("./veronika1/veronika.txt");
var m0nkeys = new Ape[(ap3s.Length+1)/7];
for(int i=0; i<ap3s.Length; i+=7){
    var split = ap3s[i+1].Split(':');
    var items = split[1].Split(',').Select(s => long.Parse(s));
    var test = long.Parse(ap3s[i+3].Substring(21));
    var throwTo = (int.Parse(ap3s[i+4].Substring(29)), int.Parse(ap3s[i+5].Substring(30)));
    var old = Expression.Parameter(typeof(long));
    Expression e2;
    if(long.TryParse(ap3s[i+2].Substring(25), out var con)){
        e2 = Expression.Constant(con);
    } else{
        e2 = old;
    }
    BinaryExpression b;
    if(ap3s[i+2][23] == '*'){
        b = Expression.Multiply(old, e2);
    } else{
        b = Expression.Add(old, e2);
    }
    var l = Expression.Lambda(b, true, new []{old});
    m0nkeys[i/7] = new Ape(items.ToList(), test, (Func<long, long>)l.Compile(), throwTo);
}
for(int i=0; i<20; i++){
    foreach(var monkey in m0nkeys){
        monkey.Inspect(ref m0nkeys);
    }
}
m0nkeys = m0nkeys.OrderByDescending(ape => ape.inspections).ToArray();
Console.WriteLine(m0nkeys[0].inspections * m0nkeys[1].inspections);

class Ape{
    List<long> items;
    long test;
    Func<long, long> operation;
    (int, int) throwTo;
    public long inspections;

    public Ape(
        List<long> i,
        long t,
        Func<long, long> o,
        (int, int) th
    ){
        items = i;
        test = t;
        operation = o;
        throwTo = th;
        inspections = 0;
    }

    public void Inspect(ref Ape[] apes){
        inspections += items.Count();
        foreach(var item in items){
            long nitem = operation(item)/3;
            if(nitem%test == 0){
                apes[throwTo.Item1].Catch(nitem);
            } else{
                apes[throwTo.Item2].Catch(nitem);
            }
        }
        items.Clear();
    }

    public void Catch(long l){
        items.Add(l);
    }
}