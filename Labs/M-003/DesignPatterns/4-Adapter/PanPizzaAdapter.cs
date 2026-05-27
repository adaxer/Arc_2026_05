using DesignPatterns.FactoryMethod.PizzaShop;

namespace DesignPatterns.Adapter
{
    /// <summary>
    /// Adapter fuer PanPizza welcher die Methoden "weiterleitet",
    /// d.h. die API Aufrufe umschreibt
    /// </summary>
    internal class PanPizzaAdapter : Pizza
    {
        private PanPizza panPizza;

        public PanPizzaAdapter(PanPizza panPizza) : base(panPizza.Name)
        {
            this.panPizza = panPizza;
        }

        public override void Bake()
        {
            panPizza.PutOilInPan();
            panPizza.FryInPan();
        }
    }
}
