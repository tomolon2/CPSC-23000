public class Office : Business
{
    public int Elevators { get; set; }

    public Office(int sf, int offices, int restrooms, int floors,
                  string location, int parkingSpaces, int elevators)
        : base(sf, offices, restrooms, floors, location, parkingSpaces)
    {
        Elevators = elevators;
    }

    public override double CalculateCost()
    {
        double cost = SquareFootage * 160;
        cost += Offices * 15000;
        cost += Restrooms * 10000;
        cost += Elevators * 50000;
        return cost;
    }
}