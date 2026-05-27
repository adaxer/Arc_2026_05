
namespace DesignPatterns.Base
{
    public abstract class Pizza
    {
        private readonly string name;

        public List<PizzaToppings> Toppings { get; } = new List<PizzaToppings>();

        public string Description => $"{Name} mit {string.Join(", ", Toppings)}";

        public decimal Price { get; protected set; } = 5;

        protected Pizza(string name)
        {
            this.name = name;
        }

        // Abgeleitete Klassen koennen virtuelle Methode ueberschreiben,
        // d. h. das Verhalten der Standardimplementierung kann veraendert werden
        public virtual void Bake()
        {
            Console.WriteLine($"{name} in Steinofen backen...");
        }

        public virtual decimal GetCost() => Price;

        public override string ToString() => Description;

        internal void Prepare()
        {
            Console.WriteLine($"{name} vorbereiten...");
        }

        internal void Cut()
        {
            Console.WriteLine($"{name} schneiden...");
        }

        internal void Box()
        {
            Console.WriteLine($"{name} verpacken...");
        }
    }
}
