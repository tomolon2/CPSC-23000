using System;

public abstract class Building
{
    private int squareFootage;

    public int SquareFootage
    {
        get { return squareFootage; }
        set { squareFootage = value; }
    }

    public Building(int squareFootage)
    {
        SquareFootage = squareFootage;
    }

    public abstract double CalculateCost();

    public override string ToString()
    {
        return "The cost of the ";
    }
}