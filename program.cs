using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    static void RunLoad(int seconds)
    {
        Stopwatch sw = Stopwatch.StartNew();
        long sum = 0;

        while (sw.Elapsed.TotalSeconds < seconds)
        {
            for (int i = 0; i < 1000000; i++)
            {
                sum += (long)i * i;
            }
        }

        Console.WriteLine($"Поток {Environment.CurrentManagedThreadId} завершил работу, сумма: {sum}");
    }

    static void Main()
    {
        Console.WriteLine($"Логических процессоров: {Environment.ProcessorCount}");
        Console.WriteLine($"Архитектура процесса: {RuntimeInformation.ProcessArchitecture}");
        Console.WriteLine($"64-битный процесс: {Environment.Is64BitProcess}");

        Console.WriteLine("\nНажми Enter для запуска теста в 1 поток...");
        Console.ReadLine();

        Console.WriteLine("Старт (1 поток)...");
        RunLoad(5);

        int[] counts = { 1, 2, 4, 8, 16 };

        foreach (int threadCount in counts)
        {
            Console.WriteLine($"\nНажми Enter для теста на {threadCount} потоках...");
            Console.ReadLine();

            Console.WriteLine($"Запуск потоков: {threadCount}");
            Thread[] pool = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                pool[i] = new Thread(() => RunLoad(5));
                pool[i].Start();
            }

            for (int i = 0; i < threadCount; i++)
            {
                pool[i].Join();
            }

            Console.WriteLine($"Тест на {threadCount} потоках закончен.");
        }

        Console.WriteLine("\nГотово.");
    }
}
