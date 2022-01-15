using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace Word_Counter
{    
    class Program
    {
        static void Main(string[] args)
        {
            string text;
            string path = @"ViM1.txt";
            string result = @"result.txt";
            using (StreamReader sr = new StreamReader(path))
            {
                text = sr.ReadToEnd();
            }
            using (StreamWriter sw = new StreamWriter(result, false, System.Text.Encoding.Default))
            {
                sw.WriteLine("Итого:\n");
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Assembly assembly = Assembly.Load(File.ReadAllBytes("Transfer.dll"));
            Type type = assembly.GetType("Transfer.TransferClass");
            MethodInfo method = type.GetMethod("MakeDictionaryParallel", BindingFlags.Instance | BindingFlags.NonPublic);
            object instance = Activator.CreateInstance(type);
            Dictionary<string, int> wordlist = (Dictionary<string, int>)method.Invoke(instance, new object[] { text });

            stopWatch.Stop();
            Console.WriteLine("Время записи в словарь: " + stopWatch.Elapsed);

            Console.WriteLine("Идёт запись...");
            foreach (var pair in wordlist.OrderByDescending(pair => pair.Value))
            {
                using (StreamWriter sw = new StreamWriter(result, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine("{0} - {1}", pair.Key, pair.Value);
                }
            }            
        }
    }
}
