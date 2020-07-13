using System;
using Xunit;
using Shared.Contract.Utils;

namespace SagaTest.Tests
{
    public class StringExternsionTest
    {
        [Fact]
        public void KebabCaseMassageTest()
        {
            string message = "WithdrawCustomerNameMessage";
            var kebabCase =  message.PascalToKebabCaseMessage();
            Assert.Equal("withdraw-customer-name",kebabCase);
        }
    }
}
