using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Oop2App
{
	
	abstract class Employee
	{
		public int Id { get; }
		public string First { get; }
		public string Last { get; }

		protected Employee(int id, string first, string last)
		{
			Id = id;
			First = first;
			Last = last;
		}
		public abstract double WeeklyPay();

		public abstract string EmpType { get; }
	}

	class SalaryWorker : Employee
	{
		private double salary;
		public SalaryWorker(int id, string first, string last, double sal) : base(id, first, last) => salary = sal;
		public override double WeeklyPay() => salary / 52.0;
		public double YearlySalary => salary;
		public override string EmpType => "Salary";
	}

	class HourlyWorker : Employee
	{
		private double hours;
		private double rate;
		public HourlyWorker(int id, string first, string last, double hoursWorked, double payRate) : base(id, first, last)
		{
			hours = hoursWorked;
			rate = payRate;
		}
		public override double WeeklyPay()
		{
			if (hours <= 40) return hours * rate;
			return (40 * rate) + ((hours - 40) * rate * 1.5);
		}
		public double HoursWorked => hours;
		public double WageRate => rate;
		public override string EmpType => "Hourly";
	}

	class CommissionWorker : Employee
	{
		private double salary;
		private double commRate;
		private double sales;
		public CommissionWorker(int id, string first, string last, double sal, double rate, double s) : base(id, first, last)
		{
			salary = sal;
			commRate = rate;
			sales = s;
		}
		public override double WeeklyPay() => (salary / 52.0) + (sales * commRate);
		public double YearlySalary => salary;
		public double CommissionRate => commRate;
		public double Sales => sales;
		public override string EmpType => "Commission";
	}

	class PieceWorker : Employee
	{
		private double wagePerPiece;
		private int quantity;
		public PieceWorker(int id, string first, string last, double wage, int qty) : base(id, first, last)
		{
			wagePerPiece = wage;
			quantity = qty;
		}
		public override double WeeklyPay() => wagePerPiece * quantity;
		public double WagePerPiece => wagePerPiece;
		public int Quantity => quantity;
		public override string EmpType => "Piece";
	}

	class Program
	{
		static void Main(string[] args)
		{
			string input = "employee.txt";
			string output = "report.txt";

			if (!File.Exists(input))
			{
				Console.WriteLine($"Input file '{input}' not found in working directory: {Environment.CurrentDirectory}");
				return;
			}

			
			var employees = new List<Employee>();
			var lines = File.ReadAllLines(input);
			int lineNo = 0;
			foreach (var raw in lines)
			{
				lineNo++;
				var line = raw.Trim();
				if (string.IsNullOrWhiteSpace(line)) continue;
				var parts = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
				try
				{
					var type = parts[0];
					int id = int.Parse(parts[1]);
					string first = parts[2];
					string last = parts[3];
					switch (type)
					{
						case "S":
							employees.Add(new SalaryWorker(id, first, last, double.Parse(parts[4], CultureInfo.InvariantCulture)));
							break;
						case "H":
							employees.Add(new HourlyWorker(id, first, last, double.Parse(parts[4], CultureInfo.InvariantCulture), double.Parse(parts[5], CultureInfo.InvariantCulture)));
							break;
						case "C":
							employees.Add(new CommissionWorker(id, first, last, double.Parse(parts[4], CultureInfo.InvariantCulture), double.Parse(parts[5], CultureInfo.InvariantCulture), double.Parse(parts[6], CultureInfo.InvariantCulture)));
							break;
						case "P":
							employees.Add(new PieceWorker(id, first, last, double.Parse(parts[4], CultureInfo.InvariantCulture), int.Parse(parts[5])));
							break;
						default:
							Console.WriteLine($"Unknown type '{type}' on line {lineNo}");
							break;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error parsing line {lineNo}: {ex.Message}");
				}
			}

			
			using (var sw = new StreamWriter(output, false))
			{
				sw.WriteLine("Employee Weekly Pay Report");
				sw.WriteLine(new string('=', 80));
				sw.WriteLine();
				sw.WriteLine("Employee Type, Record Definition");
				sw.WriteLine();

				
				sw.WriteLine("Hourly");
				sw.WriteLine("Emp Type, Emp Num, First Name, Last Name, Hours Worked, Wage Rate");
				foreach (var e in employees)
				{
					if (e is HourlyWorker hw)
						sw.WriteLine($"Hourly,{hw.Id},{hw.First},{hw.Last},{hw.HoursWorked:F2},{hw.WageRate:F2}");
				}

				sw.WriteLine();
				
				sw.WriteLine("Salary");
				sw.WriteLine("Emp Type, Emp Num, First Name, Last Name, Yearly Salary");
				foreach (var e in employees)
				{
					if (e is SalaryWorker swk)
						sw.WriteLine($"Salary,{swk.Id},{swk.First},{swk.Last},{swk.YearlySalary:F2}");
				}

				sw.WriteLine();
				
				sw.WriteLine("Commission");
				sw.WriteLine("Emp Type, Emp Num, First Name, Last Name, Yearly Salary, Commission Rate, Sales");
				foreach (var e in employees)
				{
					if (e is CommissionWorker cw)
						sw.WriteLine($"Commission,{cw.Id},{cw.First},{cw.Last},{cw.YearlySalary:F2},{cw.CommissionRate:F3},{cw.Sales:F2}");
				}

				sw.WriteLine();
				
				sw.WriteLine("Piece");
				sw.WriteLine("Emp Type, Emp Num, First Name, Last Name, Wage per Piece, Number of Pieces");
				foreach (var e in employees)
				{
					if (e is PieceWorker pw)
						sw.WriteLine($"Piece,{pw.Id},{pw.First},{pw.Last},{pw.WagePerPiece:F2},{pw.Quantity}");
				}

				sw.WriteLine();
			}

			Console.WriteLine($"Report written to {output} ({employees.Count} records).");
		}
	}
}
