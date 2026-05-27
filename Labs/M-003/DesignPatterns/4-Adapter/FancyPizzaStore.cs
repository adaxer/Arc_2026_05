using DesignPatterns.FactoryMethod.PizzaShop;

namespace DesignPatterns.Adapter
{
    public class FancyPizzaStore
    {
        public static Pizza OrderPizza(string name)
        {
            var pizza = CreatePizza(name);
            pizza.Prepare();
            pizza.Bake();
            pizza.Cut();
            pizza.Box();
            return pizza;
        }

        private static Pizza CreatePizza(string name)
        {
            switch (name)
            {
                case "Margherita":
                    return new Margherita();

                case "Funghi":
                    return new Funghi();

                case "NYStylePanPizza":
                    // Problem: Geht nicht weil PanPizza keine "Pizza" ist
                    //return new PanPizza();

                    // Loesung: Adapter fuer PanPizza
                    return new PanPizzaAdapter(new PanPizza());

                default:
                    throw new InvalidOperationException("Unknown Pizza");
            }
        }
    }
}
