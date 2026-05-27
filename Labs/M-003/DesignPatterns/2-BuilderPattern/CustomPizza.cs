using DesignPatterns.FactoryMethod.PizzaShop;

namespace DesignPatterns.BuilderPattern
{
    public class CustomPizza : Pizza
    {
        public CustomPizza(IEnumerable<PizzaToppings> toppings) : base("My Custom Pizza")
        {
            if (!toppings.Any())
            {
                throw new ArgumentException("Custom pizza must have at least one topping.");
            }

            Toppings.AddRange(toppings);
        }
    }
}
