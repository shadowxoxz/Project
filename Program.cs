using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticsAssignment
{
    class Program
    {
        static double[] data = {
            115, 182, 191, 31, 196, 1099, 5, 172, 10,
            179, 83, 21, 20, 21, 186, 177, 195, 193,
            188, 199, 62, 109, 105, 183, 110
        };

        static void Main(string[] args)
        {
            Console.WriteLine("=================================================");
            Console.WriteLine("   Probability & Statistical Distributions");
            Console.WriteLine("   Programming Assignment - Spring 2025/2026");
            Console.WriteLine("=================================================\n");

            Console.WriteLine(">>> PART 1: STATISTICAL MEASURES\n");

            double[] sorted = GetSortedData(data);

            Console.WriteLine("Original Data:");
            Console.WriteLine(string.Join(", ", data));
            Console.WriteLine("\nSorted Data:");
            Console.WriteLine(string.Join(", ", sorted));
            Console.WriteLine($"\nn (count) = {data.Length}\n");
            Console.WriteLine("-------------------------------------------------");

            double mean      = CalculateMean(data);
            double mode      = CalculateMode(data);
            double median    = CalculateMedian(sorted);
            double variance  = CalculateVariance(data, mean);
            double stdDev    = Math.Sqrt(variance);
            double p20       = CalculatePercentile(sorted, 20);
            double p50       = CalculatePercentile(sorted, 50);
            double q1        = CalculatePercentile(sorted, 25);
            double q2        = CalculatePercentile(sorted, 50);
            double q3        = CalculatePercentile(sorted, 75);
            double range     = CalculateRange(sorted);
            double iqr       = q3 - q1;
            double sumOfDevs = CalculateSumOfDeviations(data, mean);

            Console.WriteLine($"(i)    Mean                  = {mean:F4}");
            Console.WriteLine($"(ii)   Mode                  = {mode}");
            Console.WriteLine($"(iii)  Median                = {median}");
            Console.WriteLine($"(iv)   Variance              = {variance:F4}");
            Console.WriteLine($"(v)    P20 (20th Percentile) = {p20}");
            Console.WriteLine($"(vi)   P50 (50th Percentile) = {p50}");
            Console.WriteLine($"(vii)  Third Quartile  (Q3)  = {q3}");
            Console.WriteLine($"(viii) Second Quartile (Q2)  = {q2}");
            Console.WriteLine($"(ix)   Third Quartile  (Q3)  = {q3}");
            Console.WriteLine($"(x)    Range                 = {range}");
            Console.WriteLine($"(xi)   Interquartile Range   = {iqr}");
            Console.WriteLine($"(xii)  Standard Deviation    = {stdDev:F4}");
            Console.WriteLine($"(xiii) Sum of Deviations     = {sumOfDevs:F4}");

            Console.WriteLine("\n-------------------------------------------------");
            Console.WriteLine("\n>>> PART 2: OUTLIER DETECTION\n");
            Console.WriteLine("Method: IQR Fence Rule");
            Console.WriteLine($"  Q1         = {q1}");
            Console.WriteLine($"  Q3         = {q3}");
            Console.WriteLine($"  IQR        = {iqr}");
            Console.WriteLine($"  Lower Fence (Q1 - 1.5*IQR) = {q1 - 1.5 * iqr}");
            Console.WriteLine($"  Upper Fence (Q3 + 1.5*IQR) = {q3 + 1.5 * iqr}\n");

            DetectOutliers(data, q1, q3, iqr);

            Console.WriteLine("\n=================================================");
            Console.WriteLine("                    END OF OUTPUT");
            Console.WriteLine("=================================================");

            Console.ReadKey();
        }

        static double[] GetSortedData(double[] arr)
        {
            double[] copy = (double[])arr.Clone();
            Array.Sort(copy);
            return copy;
        }

        static double CalculateMean(double[] arr)
        {
            double sum = 0;
            foreach (double x in arr)
                sum += x;
            return sum / arr.Length;
        }

        static double CalculateMode(double[] arr)
        {
            Dictionary<double, int> freq = new Dictionary<double, int>();
            foreach (double x in arr)
            {
                if (freq.ContainsKey(x)) freq[x]++;
                else freq[x] = 1;
            }

            int maxFreq = freq.Values.Max();

            List<double> modes = freq
                .Where(kv => kv.Value == maxFreq)
                .Select(kv => kv.Key)
                .OrderBy(v => v)
                .ToList();

            if (maxFreq == 1)
            {
                Console.WriteLine("  Note: No mode (all values appear exactly once).");
                return double.NaN;
            }

            if (modes.Count > 1)
                Console.WriteLine($"  Note: Multiple modes - {string.Join(", ", modes)}");

            return modes[0];
        }

        static double CalculateMedian(double[] sorted)
        {
            int n = sorted.Length;
            if (n % 2 == 1)
                return sorted[n / 2];
            else
                return (sorted[n / 2 - 1] + sorted[n / 2]) / 2.0;
        }

        static double CalculateVariance(double[] arr, double mean)
        {
            double sumSqDiff = 0;
            foreach (double x in arr)
                sumSqDiff += Math.Pow(x - mean, 2);
            return sumSqDiff / arr.Length;
        }

        static double CalculatePercentile(double[] sorted, double p)
        {
            int n = sorted.Length;
            double L = (p / 100.0) * n;

            if (L == Math.Floor(L))
            {
                int idx = (int)L;
                return (sorted[idx - 1] + sorted[idx]) / 2.0;
            }
            else
            {
                int idx = (int)Math.Ceiling(L);
                return sorted[idx - 1];
            }
        }

        static double CalculateRange(double[] sorted)
        {
            return sorted[sorted.Length - 1] - sorted[0];
        }

        static double CalculateSumOfDeviations(double[] arr, double mean)
        {
            double sum = 0;
            foreach (double x in arr)
                sum += (x - mean);
            return sum;
        }

        static void DetectOutliers(double[] arr, double q1, double q3, double iqr)
        {
            double lowerFence = q1 - 1.5 * iqr;
            double upperFence = q3 + 1.5 * iqr;

            Console.WriteLine($"{"Number",-12} {"Status",-15} {"Reason"}");
            Console.WriteLine(new string('-', 60));

            foreach (double x in arr)
            {
                bool isOutlier = x < lowerFence || x > upperFence;
                string status  = isOutlier ? "*** OUTLIER ***" : "Normal";
                string reason  = "";

                if (isOutlier)
                {
                    if (x < lowerFence)
                        reason = $"< Lower Fence ({lowerFence})";
                    else
                        reason = $"> Upper Fence ({upperFence})";
                }

                Console.WriteLine($"{x,-12} {status,-15} {reason}");
            }
        }
    }
}
