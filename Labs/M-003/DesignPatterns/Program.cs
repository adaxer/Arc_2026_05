using DesignPatterns.Adapter;
using DesignPatterns.BuilderPattern;
using DesignPatterns.Decorator;
using DesignPatterns.FactoryMethod;
using DesignPatterns.FactoryMethod.PizzaShop;
using DesignPatterns.Strategy;

namespace DesignPatterns
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Sonderzeichen darstellen
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //DbClient.FactoryUsageExample();

            Console.WriteLine("\nFactory Pattern:\nStandard-🍕 erzeugen");
            PizzaShopFactorySample();

            Console.WriteLine("\nBuilder Pattern:\tEigene 🍕 zusammenstellen");
            PizzaConfiguratorSample();

            Console.WriteLine("\nDecorator Pattern:\t☕ zusammenstellen");
            CoffeeVendingMachine.CreateCoffee();

            Console.WriteLine("\nAdapter Pattern:\t🥘🍕 bestellen");
            FancyPizzaStore.OrderPizza("NYStylePanPizza");

            Console.WriteLine("\nStrategy Pattern:\t🍕 zustellen mit 🚗, 🚲 oder 🚁");
            var deliveryService = new PizzaExpress();
            deliveryService.Order("Margherita", 9000);

            Console.WriteLine("\nMediator Pattern");
            Mediator.RunChat();
        }

        private static void PizzaShopFactorySample()
        {
            var pizzaShop = new PizzaShop();
            var pizza = pizzaShop.CreateByName("Margherita");
            Console.WriteLine(pizza);
        }

        private static void PizzaConfiguratorSample()
        {            
            var builder = new PizzaConfigurator();
            var pizza = builder
                .AddPepperoni()
                .AddCheese()
                .AddSalami()
                .Build();
            Console.WriteLine("Own Pizza with selected toppings:");
            Console.WriteLine(pizza);
        }
    }
}
