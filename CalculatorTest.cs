using Xunit;
using System;

namespace CalculatorT
{
    public class CalcT
    {
        private CalculatorT calc = new CalculatorT();

        [Fact]
        public void Divide_NormalNumbers_ReturtnsResult()
        {
            int result = calc.Divide(10, 2);
            Assert.Equal(5, result);
        }
        [Fact]
        public void Divide_ByZero_ThrowsException()
        {

        }
    }
}
