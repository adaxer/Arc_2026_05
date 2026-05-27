namespace DesignPatterns.FactoryMethod.PizzaShop
{
    public class Margherita : Pizza
    {
        public Margherita() : base("Margherita")
        {
            Toppings.Add(PizzaToppings.TomatoSauce);
            Toppings.Add(PizzaToppings.Cheese);
        }
    }
}
