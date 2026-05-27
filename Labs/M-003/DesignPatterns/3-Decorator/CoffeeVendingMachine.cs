namespace DesignPatterns.Decorator
{
    public class CoffeeVendingMachine
    {
        public static void CreateCoffee()
        {
            var coffee = new SimpleCoffee();
            Console.WriteLine($"{coffee.Description} kostet: {coffee.GetCost():C} Euro");

            var coffeeWithMilk = new MilkDecorator(coffee);
            Console.WriteLine($"{coffeeWithMilk.Description} kostet: {coffeeWithMilk.GetCost():C} Euro");

            var coffeeWithMilkAndSugar = new SugarDecorator(coffeeWithMilk);
            Console.WriteLine($"{coffeeWithMilkAndSugar.Description} kostet: {coffeeWithMilkAndSugar.GetCost():C} Euro");
        }
    }
}
