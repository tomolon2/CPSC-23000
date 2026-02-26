using System;
using System.IO;
using System.Collections.Generic;

namespace DataApp
{
    class Program
    {
        struct Employee
        {
            public int Id;
            public string Name;
            public DateTime HireDate;
            public double Years;
            public double Salary;
            public string Insurance;
            public int Deductible;
            public int Coverage;

            public Employee(int id, string name, DateTime hire, double years, double salary, string ins, int ded, int cov)
            {
                Id = id; Name = name; HireDate = hire; Years = years;
                Salary = salary; Insurance = ins; Deductible = ded; Coverage = cov;
            }
        }

        static void Main(string[] args)
        {
            var list = LoadEmployees("payroll.txt");
            if (list.Count == 0) return;

            int choice = 0;
            while (choice != 10)
            {
                Console.WriteLine("\n1.Delete First 2.Sum Salary 3.Largest Years 4.Sort Name 5.Sort Salary 6.Print Report 7.Delete By Key 8.Add Record 9.Display All 10.Quit");
                choice = int.Parse(Console.ReadLine() ?? "10");

                if (choice == 1) DeleteFirst(list);
                else if (choice == 2) Console.WriteLine("Total Salary: " + SumSalary(list));
                else if (choice == 3) PrintLargestYears(list);
                else if (choice == 4) { list.Sort((a, b) => a.Name.CompareTo(b.Name)); Console.WriteLine("Sorted by Name"); }
                else if (choice == 5) { list.Sort((a, b) => b.Salary.CompareTo(a.Salary)); Console.WriteLine("Sorted by Salary"); }
                else if (choice == 6) PrintReport(list, "report.txt");
                else if (choice == 7) DeleteUsingKey(list);
                else if (choice == 8) AddRecord(list);
                else if (choice == 9) DisplayAll(list);
            }
        }

        static List<Employee> LoadEmployees(string file)
        {
            var list = new List<Employee>();
            if (!File.Exists(file)) { Console.WriteLine("File not found"); return list; }

            foreach (var line in File.ReadAllLines(file))
            {
                var p = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries); // split on any whitespace
                if (p.Length < 8) continue;

                if (!int.TryParse(p[0], out int id)) continue;
                string name = p[1];
                if (!DateTime.TryParse(p[2], out DateTime hire)) continue;
                if (!double.TryParse(p[3], out double years)) continue;
                if (!double.TryParse(p[4], out double salary)) continue;
                string ins = p[5];
                if (!int.TryParse(p[6], out int ded)) continue;
                if (!int.TryParse(p[7], out int cov)) continue;

                list.Add(new Employee(id, name, hire, years, salary, ins, ded, cov));
            }

            Console.WriteLine($"Loaded {list.Count} employees");
            return list;
        }

        static void DeleteFirst(List<Employee> list) { if (list.Count > 0) { list.RemoveAt(0); Console.WriteLine("First record deleted"); } }
        static double SumSalary(List<Employee> list) { double total = 0; foreach (var e in list) total += e.Salary; return total; }
        static void PrintLargestYears(List<Employee> list)
        {
            if (list.Count == 0) return;
            var e = list[0]; foreach (var emp in list) if (emp.Years > e.Years) e = emp;
            Console.WriteLine($"{e.Id,-4}{e.Name,-10}{e.HireDate.ToShortDateString(),-12}{e.Years,-6}{e.Salary}");
        }

        static void PrintReport(List<Employee> list, string file)
        {
            using var w = new StreamWriter(file);
            w.WriteLine("EMPLOYEE REPORT".PadLeft(30));
            w.WriteLine("ID  Name     HireDate    Years  Salary  Insurance  Ded  Cov");
            foreach (var e in list) w.WriteLine($"{e.Id,-4}{e.Name,-10}{e.HireDate.ToShortDateString(),-12}{e.Years,-6}{e.Salary,-8}{e.Insurance,-10}{e.Deductible,-6}{e.Coverage}");
            Console.WriteLine("Report generated");
        }

        static int FindRecordUsingKey(List<Employee> list, int key) { for (int i = 0; i < list.Count; i++) if (list[i].Id == key) return i; return -1; }
        static void DeleteRecordAtLocation(List<Employee> list, int loc) { list.RemoveAt(loc); }

        static void DeleteUsingKey(List<Employee> list)
        {
            Console.Write("Enter ID: "); 
            if (!int.TryParse(Console.ReadLine(), out int key)) return;
            int loc = FindRecordUsingKey(list, key);
            if (loc == -1) { Console.WriteLine("Not found"); return; }
            Console.Write("Confirm delete (y/n): "); if ((Console.ReadLine() ?? "").ToLower() == "y") { DeleteRecordAtLocation(list, loc); Console.WriteLine("Record deleted"); }
        }

        static void AddRecord(List<Employee> list)
        {
            Console.Write("Enter ID: "); if (!int.TryParse(Console.ReadLine(), out int id)) return;
            if (FindRecordUsingKey(list, id) != -1) { Console.WriteLine("ID in use"); return; }

            Console.Write("Name: "); string name = Console.ReadLine() ?? "";
            Console.Write("Hire Date: "); if (!DateTime.TryParse(Console.ReadLine(), out DateTime hire)) return;
            Console.Write("Years: "); if (!double.TryParse(Console.ReadLine(), out double years)) return;
            Console.Write("Salary: "); if (!double.TryParse(Console.ReadLine(), out double salary)) return;
            Console.Write("Insurance: "); string ins = Console.ReadLine() ?? "";
            Console.Write("Deductible: "); if (!int.TryParse(Console.ReadLine(), out int ded)) return;
            Console.Write("Coverage: "); if (!int.TryParse(Console.ReadLine(), out int cov)) return;

            Console.Write("Confirm add (y/n): "); if ((Console.ReadLine() ?? "").ToLower() == "y") { list.Add(new Employee(id, name, hire, years, salary, ins, ded, cov)); Console.WriteLine("Record added"); }
        }

        static void DisplayAll(List<Employee> list)
        {
            Console.WriteLine("ID  Name     HireDate    Years  Salary  Insurance  Ded  Cov");
            foreach (var e in list) Console.WriteLine($"{e.Id,-4}{e.Name,-10}{e.HireDate.ToShortDateString(),-12}{e.Years,-6}{e.Salary,-8}{e.Insurance,-10}{e.Deductible,-6}{e.Coverage}");
        }
    }
}