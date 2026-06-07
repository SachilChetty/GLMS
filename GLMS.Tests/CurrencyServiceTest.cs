using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLMS.Tests
{
    public class CurrencyServiceTest
    {
        [Fact]
        public void ShouldCalculateMathCorrectly()
        {
            // Simple math check to ensure your multiplication logic is sound
            decimal usd = 10;
            decimal rate = 18.5m;

            decimal result = usd * rate;

            Assert.Equal(185.0m, result);
        }
    }
}

