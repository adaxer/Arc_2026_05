namespace DesignPatterns.Decorator
{
    /// <summary>
    /// Basis-Dekorator
    /// </summary>
    public abstract class CoffeeDecorator : Coffee
    {
        protected Coffee _coffee;

        protected CoffeeDecorator(Coffee coffee)
        {
            _coffee = coffee;
        }

        public override string Description => _coffee.Description;
    }

    /// <summary>
    /// Konkreter Dekorator für Milch
    /// </summary>
    public class MilkDecorator : CoffeeDecorator
    {
        public MilkDecorator(Coffee coffee) : base(coffee) { }


        public override string Description => _coffee.Description + ", mit Milch";

        public override double GetCost() => _coffee.GetCost() + 0.5;
    }

    /// <summary>
    /// Konkreter Dekorator für Zucker
    /// </summary>
    public class SugarDecorator : CoffeeDecorator
    {
        public SugarDecorator(Coffee coffee) : base(coffee) { }

        public override string Description => _coffee.Description + ", mit Zucker";

        public override double GetCost() => _coffee.GetCost() + 0.2;

    }
}
