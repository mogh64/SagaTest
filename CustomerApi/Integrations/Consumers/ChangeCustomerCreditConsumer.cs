using CustomerApi.Infrastructure;
using MassTransit;
using Shared.Contract.Messages;
using System.Threading.Tasks;

namespace CustomerApi.Integrations.Consumers
{
    public class ChangeCustomerCreditConsumer : IConsumer<ChangeCustomerCreditMessage>
    {
        private readonly ICustomerRepository customerRepository;

        public ChangeCustomerCreditConsumer(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        public async Task Consume(ConsumeContext<ChangeCustomerCreditMessage> context)
        {
            if (context.Message.CostomerId < 1)
            {
                await context.RespondAsync<InputValueRejectedResponse>(new
                {
                    Reason = "CustomerId is Invalid!"
                });
                return;
            }
            var customer = await customerRepository.Get(context.Message.CostomerId);
            customer.Credit += context.Message.Credit;
            await customerRepository.Update(customer);
            await context.RespondAsync<ChangeCustomerCreditResponse>(new { TotalCredit = customer.Credit });
        }
    }
}
