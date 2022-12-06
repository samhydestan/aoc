﻿using static System.IO.File;

var txt = ReadAllText("./elvira1/elvira.txt");
int p = 0;
var q = new Queue<char>(15);
while(p<txt.Length){
    q.Enqueue(txt[p]);
    if(q.Count == 15){
        q.Dequeue();
        if(q.Distinct().Count() == q.Count){
            break;
        }
    }
    p++;
}
Console.WriteLine(p+1);