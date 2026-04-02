public abstract class Home : Building
{
    private int bedrooms;

    public int Bedrooms
    {
        get { return bedrooms; }
        set { bedrooms = value; }
    }

    public Home(int squareFootage, int bedrooms)
        : base(squareFootage)
    {
        Bedrooms = bedrooms;
    }
}