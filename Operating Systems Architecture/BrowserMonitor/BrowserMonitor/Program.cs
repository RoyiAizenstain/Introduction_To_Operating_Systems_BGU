using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

class Program
{
    static readonly string[] Browsers = { "chrome", "msedge", "firefox", "opera" };
    static Dictionary<string, HashSet<int>> previousState = new Dictionary<string, HashSet<int>>();

    static void Main()
    {
        // אתחול של המצב הקודם (ריק בתחילה)
        foreach (var browser in Browsers)
        {
            previousState[browser] = new HashSet<int>();
        }

        while (true)
        {
            foreach (var browser in Browsers)
            {
                HashSet<int> currentPids = new HashSet<int>();
                Process[] processes = Process.GetProcessesByName(browser);

                foreach (var p in processes)
                {
                    currentPids.Add(p.Id);
                    if (!previousState[browser].Contains(p.Id))
                    {
                        Console.WriteLine($"[+] {browser} (PID: {p.Id}) has started at {DateTime.Now:HH:mm:ss}");
                    }
                }

                // בדיקה אם תהליכים שהיו – כבר לא קיימים
                foreach (var oldPid in previousState[browser])
                {
                    if (!currentPids.Contains(oldPid))
                    {
                        Console.WriteLine($"[-] {browser} (PID: {oldPid}) has closed at {DateTime.Now:HH:mm:ss}");
                    }
                }

                // עדכון המצב לקראת הסיבוב הבא
                previousState[browser] = currentPids;
            }

            Thread.Sleep(1000); // המתנה של שנייה
        }
    }
}