using static System.IO.File;
using System.Linq.Expressions;
using System.Numerics;

var ap3s= ReadAllLines("./veronika1/veronika.txt");
var m0nkeys = new Ape[(ap3s.Length+1)/7];
var modulos = new long[(ap3s.Length+1)/7];
for(int i=0; i<ap3s.Length; i+=7){
    var split = ap3s[i+1].Split(':');
    var items = split[1].Split(',').Select(s => long.Parse(s));
    modulos[i/7] = long.Parse(ap3s[i+3].Substring(21));
    var throwTo = (int.Parse(ap3s[i+4].Substring(29)), int.Parse(ap3s[i+5].Substring(30)));
    m0nkeys[i/7] = new Ape(items.ToList(), modulos[i/7], throwTo);
}
var fat = Expression.Constant(modulos.Aggregate((a, b) => a*b));
for(int i=0; i<ap3s.Length; i+=7){
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
    var m = Expression.Modulo(b, fat);
    m0nkeys[i/7].Operation(Expression.Lambda(m, true, new []{old}).Compile());
}
for(int i=0; i<10000; i++){
    foreach(var monkey in m0nkeys){
        monkey.Inspect(ref m0nkeys);
    }
}
Console.WriteLine(m0nkeys.Select(ape => ape.inspections).OrderDescending().Take(2).Aggregate(BigInteger.Multiply));

class Ape{
    List<long> items;
    Func<long, long>? operation;
    long test;
    (int, int) throwTo;
    public BigInteger inspections;

    public Ape(
        List<long> i,
        long t,
        (int, int) th
    ){
        items = i;
        test = t;
        throwTo = th;
        inspections = BigInteger.Zero;
    }

    public void Operation(Delegate o){
        operation = (Func<long, long>)o;
    }

    public void Inspect(ref Ape[] apes){
        inspections = BigInteger.Add(inspections, items.Count());
        foreach(var item in items){
            long nitem = operation!(item);
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