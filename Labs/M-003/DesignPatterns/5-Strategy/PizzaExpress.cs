using DesignPatterns.Adapter;
using DesignPatterns.FactoryMethod.PizzaShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Strategy
{
    internal class PizzaExpress
    {
        // Statt einer konkreten Implementierung nutzen wir den abstrakten Typ IVehicle
        public IVehicle DeliveryStrategy { get; private set; }

        public void Order(string name, int distanceInMeters)
        {
            var pizza = FancyPizzaStore.OrderPizza(name);

            SelectDeliveryStrategy(distanceInMeters);

            Deliver(pizza);
        }

        public void SelectDeliveryStrategy(int distanceInMeters)
        {
            if (distanceInMeters < 1000)
            {
                DeliveryStrategy = new Bike();
            }
            else if (distanceInMeters < 5000)
            {
                DeliveryStrategy = new Car("Fiat Punto");
            }
            else 
            {
                DeliveryStrategy = new Drone();
            }
        }

        public void Deliver(Pizza pizza)
        {
            Console.WriteLine($"{pizza} mit {DeliveryStrategy.Name} an Kunden ausliefern.");
        }
    }
}
