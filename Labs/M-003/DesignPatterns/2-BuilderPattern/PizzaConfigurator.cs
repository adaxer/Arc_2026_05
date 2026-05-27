using DesignPatterns.FactoryMethod.PizzaShop;

namespace DesignPatterns.BuilderPattern
{
    /// <summary>
    /// Koennte auch PizzaBuilder heissen
    /// </summary>
    public class PizzaConfigurator
    {
        private List<PizzaToppings> toppings = new();

        public PizzaConfigurator()
        {

        }

        public PizzaConfigurator AddCheese()
        {
            toppings.Add(PizzaToppings.Cheese);

            // Ausschlaggebende beim Builder Pattern ist,
            // dass die Instanz von sich selber zurueckgegeben wird.
            // Das ermoeglicht sog. Chaining.
            return this;
        }

        public PizzaConfigurator AddPepperoni()
        {
            toppings.Add(PizzaToppings.Pepperoni);
            return this;
        }

        public PizzaConfigurator AddSalami()
        {
            toppings.Add(PizzaToppings.Salami);
            return this;
        }

        public Pizza Build()
        {
            var pizza = new CustomPizza(toppings);
            return pizza;
        }
    }
}
