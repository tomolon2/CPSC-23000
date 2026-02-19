using System;
using System.IO;
using System.Linq;

namespace Noise
{
    class NoiseStatsApp
    {
        static void Main(string[] args)
        {
            const double GrandMean = 57.3;

            string filePath = "trend.txt";
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {Path.GetFullPath(filePath)}");
                return;
            }      
            string[] months = new string[12];
            double[,] data = new double[12, 5];
            var lines = File.ReadAllLines(filePath)
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith("TREND", StringComparison.OrdinalIgnoreCase) && !l.StartsWith("-"))
                .ToArray();

            int row = 0;
            foreach (var raw in lines)
            {
                if (row >= 12) break;
                var parts = raw.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;
                months[row] = parts[0].ToUpperInvariant();
                for (int c = 1; c < parts.Length && c <= 5; c++)
                {
                    if (double.TryParse(parts[c], out double v))
                        data[row, c - 1] = v;
                }
                row++;
            }

           
            double[] monthlyAvg = new double[12];
            double[] monthlyDev = new double[12];
            for (int r = 0; r < 12; r++)
            {
                double sum = 0;
                int count = 0;
                for (int c = 0; c < 5; c++) { sum += data[r, c]; count++; }
                monthlyAvg[r] = Math.Round(sum / count, 1);
                monthlyDev[r] = Math.Round(Math.Pow(GrandMean - monthlyAvg[r], 2), 2);
            }

         
            double[] quarterAvg = new double[4];
            double[] quarterDev = new double[4];
            for (int q = 0; q < 4; q++)
            {
                int start = q * 3;
                double s = monthlyAvg[start] + monthlyAvg[start + 1] + monthlyAvg[start + 2];
                quarterAvg[q] = Math.Round(s / 3.0, 2);
                quarterDev[q] = Math.Round(Math.Pow(GrandMean - quarterAvg[q], 2), 2);
            }

            double[] yearlyAvg = new double[5];
            double[] yearlyDev = new double[5];
            for (int c = 0; c < 5; c++)
            {
                double s = 0;
                for (int r = 0; r < 12; r++) s += data[r, c];
                yearlyAvg[c] = Math.Round(s / 12.0, 2);
                yearlyDev[c] = Math.Round(Math.Pow(GrandMean - yearlyAvg[c], 2), 2);
            }

       
            Console.WriteLine("TREND-SEASONAL-NOISE ANALYSIS\n");
            Console.WriteLine("Month 2021 2022 2023 2024 M:Average M:Devi Q:Devi");

            for (int r = 0; r < 12; r++)
            {
                Console.Write(months[r].PadRight(4));
                for (int c = 0; c < 5; c++) Console.Write(" " + ((int)data[r, c]).ToString().PadLeft(3));

                Console.Write(" " + monthlyAvg[r].ToString("0.0").PadLeft(6));
                Console.Write(" " + monthlyDev[r].ToString("0.00").PadLeft(8));
                if ((r + 1) % 3 == 0)
                {
                    int q = (r) / 3;
                    Console.Write(" " + quarterDev[q].ToString("0.00").PadLeft(7));
                }

                Console.WriteLine();
            }

            Console.WriteLine(new string('-', 59));
            Console.WriteLine("\nYearly");
            Console.Write("Average ");
            for (int c = 0; c < 5; c++) Console.Write(yearlyAvg[c].ToString("0.00") + " ");
            Console.WriteLine("\nYearly");
            Console.Write("Deviation ");
            for (int c = 0; c < 5; c++) Console.Write(yearlyDev[c].ToString("0.00") + " ");
            Console.WriteLine();
        }
    }
}

