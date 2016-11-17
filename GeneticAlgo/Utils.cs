using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace GeneticAlgo
{
    public static class Utils
    {
        private static CultureInfo invCI = CultureInfo.InvariantCulture;

        private static CultureInfo ruCI = new CultureInfo("ru-RU");

        public static void LoadFromFile(this List<string> list, string filename)
        {
            if (!File.Exists(filename))
            {
                list.Clear();
                return;
            }
            StreamReader reader = new StreamReader(filename);
            string s = reader.ReadToEnd();
            string[] lines = s.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            list.AddRange(lines);
            reader.Close();
        }

        public static string ArrayOfArraysToString(this int[][] array)
        {
            var sb = new StringBuilder();
            foreach (var line in array)
            {
                foreach (var column in line)
                    sb.AppendFormat("{0}\t", column);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static void SaveToFile(this List<string> list, string filename)
        {
            StreamWriter writer = new StreamWriter(filename, false);
            foreach (string s in list)
            {
                writer.WriteLine(s);
            }
            writer.Close();
        }

        public static void SaveToFile(this List<BestIndividuum> list, string filename)
        {
            StreamWriter writer = new StreamWriter(filename, false);
            foreach (var item in list)
            {
                writer.WriteLine(item.ToString());
            }
            writer.Close();
        }

        public static void FillWithEmptyValues(this List<List<string>> list, int size1, int size2)
        {
            for (int i = 0; i < size1; i++)
            {
                list.Add(new List<string>(new string[size2]));
            }
        }

        public static void FillWithEmptyValues(this List<List<long>> list, int size1, int size2)
        {
            for (int i = 0; i < size1; i++)
            {
                list.Add(new List<long>(new long[size2]));
            }
        }

        public static void AddEmptyValues(this List<float> list, int size)
        {
            for (int i = 0; i < size; i++)
            {
                list.Add(0);
            }
        }

        public static void AddEmptyValues(this List<double> list, int size)
        {
            for (int i = 0; i < size; i++)
            {
                list.Add(0);
            }
        }

        public static void AddEmptyValues(this List<int> list, int size)
        {
            for (int i = 0; i < size; i++)
            {
                list.Add(0);
            }
        }

        public static void AddEmptyValues(this List<List<long>> list, int size1, int size2)
        {
            int delta2 = size1 - list.Count;
            foreach (List<long> l in list)
            {
                int delta1 = size2 - l.Count;
                for (int i = 0; i < delta1; i++)
                {
                    l.Add(0);
                }
            }
            for (int i = 0; i < delta2; i++)
            {
                list.Add(new List<long>(new long[size2]));
            }
        }     
        
        public static string DecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public static bool TryParse(string s, out double val)
        {
            s = s.Replace(".", DecimalSeparator).Replace(",", DecimalSeparator);
            return double.TryParse(s, out val);
        }

        public static bool TryParse(string s, out int val)
        {
            s = s.Replace(".", DecimalSeparator).Replace(",", DecimalSeparator);
            return int.TryParse(s, out val);
        }

        public static bool TryParse(string s, out long val)
        {
            s = s.Replace(".", DecimalSeparator).Replace(",", DecimalSeparator);
            return long.TryParse(s, out val);
        }

        public static bool ParseFloatDualCulture(string s, out float f)
        {
            if (!float.TryParse(s, NumberStyles.Any, invCI, out f))
            {
               if (!float.TryParse(s, NumberStyles.Float, ruCI, out f))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ParseLongDualCulture(string s, double step, out long v)////////////
        {
            double variable;

            if (!double.TryParse(s, NumberStyles.Float, ruCI, out variable))
            {
                if (!double.TryParse(s, NumberStyles.Float, invCI, out variable))
                {
                    v = (long)Math.Round(variable / step, 0);
                    return false;
                }
            }
            v = (long)Math.Round(variable / step, 0);
            return true;
         }

        public static int GetDecimalDigitsCount(decimal x)
        {
            var result = 0;
            var val = x;
            var reminder = x;

            while (reminder > 0)
            {
                reminder = val - decimal.Truncate(val);
                if (reminder > 0)
                    result++;
                val *= 10;
            }

            return result;
        }

        public static Dictionary<T, T> Reverse<T>(this Dictionary<T, T> source)
        {
            var result = new Dictionary<T, T>();

            foreach (var pair in source)
                result[pair.Value] = pair.Key;

            return result;
        }
    }
}
