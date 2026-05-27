namespace DesignPatterns.Adapter
{
    public class PanPizza
    {
        public string Name => "NY Style Pfannen-Pizza";

        public void PutOilInPan()
        {
            Console.WriteLine("Oel in Pfanne gießen.");
        }

        public void FryInPan()
        {
            Console.WriteLine("NY Style Pizza wird in Pfanne zubereitet.");
        }
    }
}
