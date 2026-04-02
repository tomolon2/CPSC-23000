public class Apartment : Home
{
    public Apartment(int sf, int bedrooms) : base(sf, bedrooms) { }

    public override double CalculateCost()
    {
        return SquareFootage * 120 + Bedrooms * 10000;
    }
}