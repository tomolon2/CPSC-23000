public abstract class Business : Building
{
    public int Offices { get; set; }
    public int Restrooms { get; set; }
    public int Floors { get; set; }
    public string Location { get; set; } = "";   // fixes null warning
    public int ParkingSpaces { get; set; }

    public Business(int squareFootage, int offices, int restrooms,
                    int floors, string location, int parkingSpaces)
        : base(squareFootage)
    {
        Offices = offices;
        Restrooms = restrooms;
        Floors = floors;
        Location = location;
        ParkingSpaces = parkingSpaces;
    }
}