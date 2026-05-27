namespace DesignPatterns.FactoryMethod.PizzaShop
{
    // statt PizzaFactory sollte der Klassenname die Fachlichkeit representieren wie z.B. PizzaShop
    public class PizzaShop
    {
        public Pizza CreateByName(string name) => name switch
        {
            "Margherita" => new Margherita(),
            "Funghi" => new Funghi(),
            "Salami" => new Salami(),
            _ => throw new ArgumentException($"Unknown pizza name: {name}")
        };

        public Pizza CreateMargherita() => new Margherita();

        public Pizza CreateFunghi() => new Funghi();

        public Pizza CreateSalami() => new Salami();
    }
}


