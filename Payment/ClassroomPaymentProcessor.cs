
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Payments
{
    public class ClassroomPaymentProcessor : IPaymentProcessor
    {
        // Explicit interface implementation
        bool IPaymentProcessor.ProcessPayment(
            Payment payment,
            bool simulateSuccess)
        {
            if (simulateSuccess)
            {
                payment.Status = PaymentStatus.Successful;
                Console.WriteLine("Classroom payment successful.");
                return true;
            }

            payment.Status = PaymentStatus.Failed;
            Console.WriteLine("Classroom payment failed.");
            return false;
        }
    }
}
