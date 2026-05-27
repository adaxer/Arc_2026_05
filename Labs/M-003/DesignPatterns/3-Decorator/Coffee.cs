using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Decorator
{
    /// <summary>
    /// Basis-Komponente
    /// </summary>
    public abstract class Coffee
    {
        public abstract string Description { get; }

        public abstract double GetCost();
    }

    /// <summary>
    /// Konkrete Komponente
    /// </summary>
    public class SimpleCoffee : Coffee
    {
        public override string Description => "Einfacher Kaffee";

        public double Price => 1.99;

        public override double GetCost() => Price;
    }
}
