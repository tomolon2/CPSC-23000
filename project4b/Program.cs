using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

abstract class Employee
{
	protected int idNum;
	protected string firstName;
	protected string lastName;

	protected Employee(int id, string first, string last)
	{
		idNum = id;
		firstName = first;
		lastName = last;
	}

	public abstract double WeeklyPay();

	public virtual string CommonDisplay()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0,6}{1,12}{2,12}", idNum, firstName, lastName);
	}

	public abstract string ReportLine();
}

class SalaryWorker : Employee
{
	private double salary;

	public SalaryWorker(int id, string first, string last, double sal)
		: base(id, first, last)
	{
		salary = sal;
	}

	public override double WeeklyPay() => salary / 52.0;

	public override string ReportLine()
	{

		string common = CommonDisplay();
		return string.Format(CultureInfo.InvariantCulture, "{0,-15}{1}{2,12:F2}{3,10}{4,12:F2}", "Salary Worker", common, salary, "", WeeklyPay());
	}
}

class HourlyWorker : Employee
{
	private double hoursWorked;
	private double payRate;

	public HourlyWorker(int id, string first, string last, double hours, double rate)
		: base(id, first, last)
	{
		hoursWorked = hours;
		payRate = rate;
	}

	public override double WeeklyPay()
	{
		if (hoursWorked <= 40) return hoursWorked * payRate;
		return (40 * payRate) + ((hoursWorked - 40) * payRate * 1.5);
	}

	public override string ReportLine()
	{
		string common = CommonDisplay();
		return string.Format(CultureInfo.InvariantCulture, "{0,-15}{1}{2,12:F2}{3,10:F2}{4,12:F2}", "Hourly Worker", common, hoursWorked, payRate, WeeklyPay());
	}
}

class CommissionWorker : Employee
{
	private double salary;
	private double commRate;
	private double sales;

	public CommissionWorker(int id, string first, string last, double sal, double rate, double s)
		: base(id, first, last)
	{
		salary = sal;
		commRate = rate;
		sales = s;
	}

	public override double WeeklyPay() => (salary / 52.0) + (sales * commRate);

	public override string ReportLine()
	{
		string common = CommonDisplay();
		return string.Format(CultureInfo.InvariantCulture, "{0,-15}{1}{2,12:F2}{3,10:F3}{4,12:F2}", "Commission Worker", common, salary, commRate, WeeklyPay());
	}
}

class PieceWorker : Employee
{
	private double wagePerPiece;
	private int quantity;

	public PieceWorker(int id, string first, string last, double wage, int qty)
		: base(id, first, last)
	{
		wagePerPiece = wage;
		quantity = qty;
	}

	public override double WeeklyPay() => wagePerPiece * quantity;

	public override string ReportLine()
	{
		string common = CommonDisplay();
		return string.Format(CultureInfo.InvariantCulture, "{0,-15}{1}{2,12:F2}{3,10}{4,12:F2}", "Piece Worker", common, wagePerPiece, quantity, WeeklyPay());
	}
}

class App
{
	static void Main(string[] args)
	{
		string inputFile = "employee.txt";
		string outputFile = "report.txt";

		var employees = new List<Employee>();

		if (!File.Exists(inputFile))
		{
			Console.WriteLine($"Input file '{inputFile}' not found in working directory: {Environment.CurrentDirectory}");
			return;
		}

		var lines = File.ReadAllLines(inputFile);
		int lineNo = 0;
		foreach (var raw in lines)
		{
			lineNo++;
			var line = raw.Trim();
			if (string.IsNullOrWhiteSpace(line)) continue;

			try
			{
			
				var parts = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
	
				var type = parts[0];
				if (type == "S")
				{
				
					int id = int.Parse(parts[1]);
					string first = parts[2];
					string last = parts[3];
					double sal = double.Parse(parts[4], CultureInfo.InvariantCulture);
					employees.Add(new SalaryWorker(id, first, last, sal));
				}
				else if (type == "H")
				{
			
					int id = int.Parse(parts[1]);
					string first = parts[2];
					string last = parts[3];
					double hours = double.Parse(parts[4], CultureInfo.InvariantCulture);
					double rate = double.Parse(parts[5], CultureInfo.InvariantCulture);
					employees.Add(new HourlyWorker(id, first, last, hours, rate));
				}
				else if (type == "C")
				{
		
					int id = int.Parse(parts[1]);
					string first = parts[2];
					string last = parts[3];
					double sal = double.Parse(parts[4], CultureInfo.InvariantCulture);
					double cr = double.Parse(parts[5], CultureInfo.InvariantCulture);
					double sales = double.Parse(parts[6], CultureInfo.InvariantCulture);
					employees.Add(new CommissionWorker(id, first, last, sal, cr, sales));
				}
				else if (type == "P")
				{
					int id = int.Parse(parts[1]);
					string first = parts[2];
					string last = parts[3];
					double wage = double.Parse(parts[4], CultureInfo.InvariantCulture);
					int qty = int.Parse(parts[5]);
					employees.Add(new PieceWorker(id, first, last, wage, qty));
				}
				else
				{
					Console.WriteLine($"Unknown type '{type}' on line {lineNo}, skipping.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error parsing line {lineNo}: {ex.Message}");
			}
		}

		using (var sw = new StreamWriter(outputFile, false))
		{
			sw.WriteLine("Employee Weekly Pay Report");
			sw.WriteLine(new string('=', 80));
			sw.WriteLine();
			sw.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0,-15}{1,6}{2,12}{3,12}{4,12}{5,10}{6,12}", "Type", "ID", "First", "Last", "Fld1", "Fld2", "Weekly Pay"));
			sw.WriteLine(new string('-', 80));

			foreach (var emp in employees)
			{
				sw.WriteLine(emp.ReportLine());
			}

			sw.WriteLine();
			sw.WriteLine($"Generated: {DateTime.Now}");
		}

		Console.WriteLine($"Report written to {outputFile} ({employees.Count} records).");
	}
}
