public class House : Home
{
    public House(int sf, int bedrooms) : base(sf, bedrooms) { }

    public override double CalculateCost()
    {
        return SquareFootage * 150 + Bedrooms * 15000;
    }
}