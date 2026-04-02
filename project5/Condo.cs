public class Condo : Home
{
    public Condo(int sf, int bedrooms) : base(sf, bedrooms) { }

    public override double CalculateCost()
    {
        return SquareFootage * 135 + Bedrooms * 12000;
    }
}