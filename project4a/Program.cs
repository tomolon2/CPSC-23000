using System;
using System.Text;

class Employee
{
    protected int idNum;
    protected string firstName;
    protected string lastName;

    public Employee(int id = 0, string first = "No Name", string last = "No Name")
    {
        idNum = id;
        firstName = first;
        lastName = last;
    }

    public void SetData(int id, string first, string last)
    {
        idNum = id;
        firstName = first;
        lastName = last;
    }

    public virtual string DisplayData()
    {
        return string.Format("{0,6}{1,12}{2,12}", idNum, firstName, lastName);
    }

    public virtual string Earnings()
    {
        return DisplayData();
    }
}

class SalaryWorker : Employee
{
    private double salary;

    public SalaryWorker(int id = 0, string first = "No Name", string last = "No Name", double sal = 0)
        : base(id, first, last)
    {
        salary = sal;
    }

    public void SetSalary(double sal) => salary = sal;
    public double GetSalary() => salary;

    public override string DisplayData()
    {
        return base.DisplayData() + string.Format("{0,12:F2}", salary);
    }

    public override string Earnings()
    {
        double weekly = salary / 52.0;
        return "Salary Worker " + DisplayData() + string.Format("{0,12:F2}", weekly);
    }
}

class HourlyWorker : Employee
{
    private double hoursWorked;
    private double payRate;

    public HourlyWorker(int id = 0, string first = "No Name", string last = "No Name", double hours = 0, double rate = 0)
        : base(id, first, last)
    {
        hoursWorked = hours;
        payRate = rate;
    }

    public void SetHours(double h) => hoursWorked = h;
    public void SetRate(double r) => payRate = r;
    public double GetHours() => hoursWorked;
    public double GetRate() => payRate;

    public override string DisplayData()
    {
        return base.DisplayData() + string.Format("{0,10}{1,10}", hoursWorked, payRate);
    }

    public override string Earnings()
    {
        double pay;
        if (hoursWorked <= 40)
            pay = hoursWorked * payRate;
        else
            pay = (40 * payRate) + ((hoursWorked - 40) * payRate * 1.5);

        return "Hourly Worker " + DisplayData() + string.Format("{0,12:F2}", pay);
    }
}

class CommissionWorker : Employee
{
    private double salary;
    private double commRate;
    private double sales;

    public CommissionWorker(int id = 0, string first = "No Name", string last = "No Name", double sal = 0, double rate = 0, double s = 0)
        : base(id, first, last)
    {
        salary = sal;
        commRate = rate;
        sales = s;
    }

    public override string DisplayData()
    {
        return base.DisplayData() + string.Format("{0,12}{1,10}{2,12}", salary, commRate, sales);
    }

    public override string Earnings()
    {
        double weekly = (salary / 52.0) + (sales * commRate);
        return "Commission Worker " + DisplayData() + string.Format("{0,12:F2}", weekly);
    }
}

class PieceWorker : Employee
{
    private double wagePerPiece;
    private int quantity;

    public PieceWorker(int id = 0, string first = "No Name", string last = "No Name", double wage = 0, int qty = 0)
        : base(id, first, last)
    {
        wagePerPiece = wage;
        quantity = qty;
    }

    public override string DisplayData()
    {
        return base.DisplayData() + string.Format("{0,10}{1,10}", wagePerPiece, quantity);
    }

    public override string Earnings()
    {
        double pay = wagePerPiece * quantity;
        return "Piece Worker " + DisplayData() + string.Format("{0,12:F2}", pay);
    }
}

class Program
{
    static void Main()
    {
        var e1 = new SalaryWorker(123, "Martha", "Perez", 56785.59);
        var e2 = new HourlyWorker(435, "Joe", "Smith", 42.5, 18.67);
        var e3 = new CommissionWorker(356, "Anthony", "Mendez", 30563.56, 0.003, 57864.53);
        var e4 = new PieceWorker(452, "Jimmy", "James", 0.50, 1201);

        Console.WriteLine(e1.Earnings());
        Console.WriteLine(e2.Earnings());
        Console.WriteLine(e3.Earnings());
        Console.WriteLine(e4.Earnings());
    }
}

