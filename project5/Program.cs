using System;

class Program
{
    static void Main(string[] args)
    {
        TestWarehouseAndOffice();
        TestApartment();
        TestCondo();
        TestHouse();
    }

    static void TestWarehouseAndOffice()
    {
        var w = new Warehouse();
        Console.WriteLine(w.ToString());

        var o = new Office(50000, 10, 4, 3, "city", 200, 3);
        Console.WriteLine("Office cost: $" + o.CalculateCost().ToString("N2"));

        var h = new House(2500, 4);
        Console.WriteLine("House cost: $" + h.CalculateCost().ToString("N2"));
    }

    static void TestApartment()
    {
        Console.WriteLine("\n--- Apartment Tests ---");

        int[] sqft = { 1200, 2301, 2699, 3600, 4300 };
        int[] beds = { 1, 2, 3, 3, 4 };

        for (int i = 0; i < sqft.Length; i++)
        {
            var a = new Apartment(sqft[i], beds[i]);
            Console.WriteLine($"Test {i + 1}: {a.CalculateCost():N2}");
        }
    }

    static void TestCondo()
    {
        Console.WriteLine("\n--- Condo Tests ---");

        int[] sqft = { 2899, 3801, 2500, 5500, 2499 };
        int[] beds = { 3, 3, 1, 4, 0 };

        for (int i = 0; i < sqft.Length; i++)
        {
            var c = new Condo(sqft[i], beds[i]);
            Console.WriteLine($"Test {i + 1}: {c.CalculateCost():N2}");
        }
    }

    static void TestHouse()
    {
        Console.WriteLine("\n--- House Tests ---");

        int[] sqft = { 1000, 1500, 1840, 2000, 1760 };
        int[] beds = { 1, 2, 2, 3, 2 };

        for (int i = 0; i < sqft.Length; i++)
        {
            var h = new House(sqft[i], beds[i]);
            Console.WriteLine($"Test {i + 1}: {h.CalculateCost():N2}");
        }
    }
}
