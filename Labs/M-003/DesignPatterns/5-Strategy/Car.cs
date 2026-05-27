namespace DesignPatterns.Strategy
{
    public class Car : IVehicle
    {
        public string Name { get; }

        public Car(string name)
        {
            Name = name;
        }
    }
}
