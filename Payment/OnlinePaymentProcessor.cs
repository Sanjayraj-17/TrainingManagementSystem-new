
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Payments
{
    public class OnlinePaymentProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(Payment payment, bool simulateSuccess)
        {
            if (simulateSuccess)
            {
                payment.Status = PaymentStatus.Successful;
                Console.WriteLine("Online payment successful.");
                return true;
            }

            payment.Status = PaymentStatus.Failed;
            Console.WriteLine("Online payment failed.");
            return false;
        }
    }
}
