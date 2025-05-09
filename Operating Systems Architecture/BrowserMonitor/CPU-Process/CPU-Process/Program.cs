// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System;

int iterations = int.Parse(args[0]);

Stopwatch sw = new Stopwatch();
sw.Start();
double NewtonRaphson(double x0)
{
    double x = x0;
    for (int i = 0; i < 20; i++)
    {
        x = x - (Math.Pow(x, 3) - 2) / (3 * Math.Pow(x, 2));
    }
    return x;
}

double result = 0;
for (int i = 0; i < iterations; i++)
{
    result += NewtonRaphson(i % 10 + 1);


}
sw.Stop();
Console.WriteLine("Running intensive calculations...");
Console.WriteLine("Time for " + iterations + " iterations: " + sw.ElapsedMilliseconds + "ms");
