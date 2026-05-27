namespace DesignPatterns.Strategy
{
    public class Drone : IVehicle
    {
        public string Name { get; }

        public Drone()
        {
            Name = "DJI Mavic Air 2";
        }
    }
}
