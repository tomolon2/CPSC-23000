using System;

public class Warehouse : Business
{
    public int TruckBays { get; set; }

    public Warehouse() : base(
        100000,   
        5,        
        2,       
        1,        
        "suburbs",
        50)       
    {
        TruckBays = 2;
    }

    public override double CalculateCost()
    {
        double cost = 875000; 

       
        if (SquareFootage > 100000)
        {
            int extraUnits = (int)Math.Ceiling((SquareFootage - 100000) / 100.0);
            cost += extraUnits * 1250;
        }

       
        cost += (Offices - 1) * 12000;

        
        cost += Restrooms * 8500;

       
        cost += (ParkingSpaces / 50) * 3000;

        
        cost += TruckBays * 11125;

       
        if (Location == "suburbs")
            cost *= 1.30;
        else if (Location == "superboonies")
            cost *= 0.90;

        return cost;
    }

    public override string ToString()
    {
        return base.ToString() + "warehouse is $" + CalculateCost().ToString("N2");
    }
}