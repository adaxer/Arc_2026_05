namespace DesignPatterns.FactoryMethod.PizzaShop
{
    public class Funghi : Pizza
    {
        public Funghi() : base("Funghi")
        {
            Toppings.Add(PizzaToppings.TomatoSauce);
            Toppings.Add(PizzaToppings.Cheese);
            Toppings.Add(PizzaToppings.Mushroom);
        }
    }
}
