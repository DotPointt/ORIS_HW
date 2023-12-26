using System.Reflection;
using System;
using MyTemplate;


class Program
{
    static void Main()
    {

        var stud = new Student(){
            name = "Venera",
            address = "Kadirovo"
        };
        
        System.Console.WriteLine(Mapper.Method2(stud));
        Mapper.Method3(new Student());

    }




}