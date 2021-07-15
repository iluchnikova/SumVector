using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace SumVector
{
    class Program
    {
        public class SumAndTime
        {
            private int sum;
            private long time;

            public SumAndTime(int sum, long time)
            {
                this.sum = sum;
                this.time = time;
            }

            public int Sum { get { return sum; } set { sum = value; } }
            public long Time { get { return time; } set { time = value; } }
        }
        public class ParallelProgramming
        {
            private List<int> vector;

            public List<int> Vector
            {
                set
                {
                    try
                    {
                        if (vector.Count > 0)
                        {
                            vector = value;
                        }
                        else
                        {
                            Console.WriteLine("Вектор должен содержать хотя бы одно значение");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                get
                {
                    return vector;
                }
            }

            public ParallelProgramming()
            {

            }
            public ParallelProgramming(List<int> vector)
            {
                this.vector = vector;
            }

            public List<int> RandomVector(int count, int minValue, int maxValue)//count- количество элементов в векторе
            {
                List<int> r = new List<int>();
                Random rand = new Random();

                for (int i = 0; i < count; i++)
                {
                    r.Add(rand.Next(minValue, maxValue));
                }

                return r;
            }

            public List<int> RandomVector(int count)
            {
                return RandomVector(count, 1, 100);
            }

            public SumAndTime SumVector(List<int> vector)
            {
                var timer = Stopwatch.StartNew();
                int sum = 0;
                for (int i = 0; i < vector.Count; i++)
                {
                    sum += vector[i];
                }
                timer.Stop();
                return new SumAndTime(sum, timer.ElapsedTicks);
            }

            public SumAndTime SimpleSum(List<int> vector)
            {
                var timer = Stopwatch.StartNew();

                int sum = vector.Sum();

                timer.Stop();
                return new SumAndTime(sum, timer.ElapsedTicks);
            }

            public SumAndTime CascadeSchema(List<int> vector)
            {
                var timer = Stopwatch.StartNew();
                List<int> sum_vector = new List<int>();

                int n = vector.Count;

                while (n > 1)
                {
                    for (int i = 0; i < n - 1; i += 2)
                    {
                        sum_vector.Add(vector[i] + vector[i + 1]);
                    }

                    if (n % 2 != 0)
                    {
                        sum_vector.Add(vector[n - 1]);
                    }

                    n = sum_vector.Count;
                    vector.Clear();
                    for (int i = 0; i < sum_vector.Count; i++)
                    {
                        vector.Add(sum_vector[i]);
                    }
                    sum_vector.Clear();
                }
                timer.Stop();
                return new SumAndTime(vector[0], timer.ElapsedTicks);
            }

            public SumAndTime ModifiedCascadeSchema(List<int> vector)
            {
                var timer = Stopwatch.StartNew();
                List<int> sum_vector = new List<int>();

                int n = vector.Count;


                int k = 0;
                int z = (int)(n / Math.Log(n, 2.0));

                for (int j = 0; j < n / z; j++)
                {
                    int sum = 0;
                    for (int i = k; i < z; i++)
                    {
                        sum += vector[i];
                        k++;
                    }
                    sum_vector.Add(sum);
                }

                if (n % z != 0)
                {
                    for (int i = 0; i < n % z; i++)
                    {
                        sum_vector.Add(vector[k]);
                    }
                }

                n = sum_vector.Count;
                vector.Clear();
                for (int i = 0; i < sum_vector.Count; i++)
                {
                    vector.Add(sum_vector[i]);
                }
                sum_vector.Clear();

                while (n > 1)
                {
                    for (int i = 0; i < n - 1; i += 2)
                    {
                        sum_vector.Add(vector[i] + vector[i + 1]);
                    }

                    if (n % 2 != 0)
                    {
                        sum_vector.Add(vector[n - 1]);
                    }

                    n = sum_vector.Count;
                    vector.Clear();
                    for (int i = 0; i < sum_vector.Count; i++)
                    {
                        vector.Add(sum_vector[i]);
                    }
                    sum_vector.Clear();
                }

                timer.Stop();
                return new SumAndTime(vector[0], timer.ElapsedTicks);
            }

            public SumAndTime ModifiedCascadeSchema1(List<int> vector)
            {
                var timer = Stopwatch.StartNew();
                List<int> sum_vector = new List<int>();

                int n = vector.Count;

                while (n > 1)
                {
                    int k = 0;
                    int z = (int)(n / Math.Log(n, 2.0));

                    for (int j = 0; j < n / z; j++)
                    {
                        int sum = 0;
                        for (int i = k; i < z; i++)
                        {
                            sum += vector[i];
                            k++;
                        }
                        sum_vector.Add(sum);
                    }

                    if (n % z != 0)
                    {
                        for (int i = 0; i < n % z; i++)
                        {
                            sum_vector.Add(vector[k]);
                        }
                    }

                    n = sum_vector.Count;
                    vector.Clear();
                    for (int i = 0; i < sum_vector.Count; i++)
                    {
                        vector.Add(sum_vector[i]);
                    }
                    sum_vector.Clear();
                }
                timer.Stop();
                return new SumAndTime(vector[0], timer.ElapsedTicks);
            }
        }


        static void Main(string[] args)
        {
            ParallelProgramming p = new ParallelProgramming();
            List<int> vector = new List<int>();

            for (int i = 1; i <= 10000000; i *= 10)
            {
                vector.Clear();
                vector = p.RandomVector(i);

                Console.WriteLine();
                Console.WriteLine("Количество элементов в массиве: " + i);
                Console.WriteLine("Встроенное суммирование: Сумма: {0}, Время: {1}", p.SimpleSum(vector).Sum, p.SimpleSum(vector).Time);
                Console.WriteLine("Поэлементное суммирование: Сумма: {0}, Время: {1}", p.SumVector(vector).Sum, p.SumVector(vector).Time);
                Console.WriteLine("Каскадная схема: Сумма: {0}, Время: {1}", p.CascadeSchema(vector).Sum, p.CascadeSchema(vector).Time);
                Console.WriteLine("Модифицированная каскадная схема: Сумма: {0}, Время: {1}", p.ModifiedCascadeSchema(vector).Sum, p.ModifiedCascadeSchema(vector).Time);
                Console.WriteLine("Модифицированная каскадная схема1: Сумма: {0}, Время: {1}", p.ModifiedCascadeSchema1(vector).Sum, p.ModifiedCascadeSchema1(vector).Time);

            }
        }
    }
}

