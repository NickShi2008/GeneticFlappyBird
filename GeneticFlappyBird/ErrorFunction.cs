using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticFlappyBird
{
    public class ErrorFunction
    {
        Func<double, double, double> function;
        Func<double, double, double> derivative;
        public ErrorFunction(Func<double, double, double> function, Func<double, double, double> derivative)
        {
            this.function = function;
            this.derivative = derivative;
        }
        public double Function(double outputs, double targets) => function(outputs, targets);
        public double Derivative(double outputs, double targets) => derivative(outputs, targets);
    }
}
