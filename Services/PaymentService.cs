
using System;
using System.Collections.Generic;
using System.Linq;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Payments;

namespace TrainingManagementSystem.Services
{
    public class PaymentService
    {
        private readonly List<Payment> payments;
        private readonly List<Enrollment> enrollments;
        private int nextPaymentId;

        public PaymentService(
            List<Payment> payments,
            List<Enrollment> enrollments)
        {
            this.payments = payments;
            this.enrollments = enrollments;

            nextPaymentId = payments.Count > 0
                ? payments.Max(p => p.PaymentId) + 1
                : 1;
        }

        public bool MakePayment(
            int enrollmentId,
            IPaymentProcessor processor,
            bool simulateSuccess)
        {
            Enrollment? enrollment = enrollments.Find(
                e => e.EnrollmentId == enrollmentId);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return false;
            }

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                Console.WriteLine(
                    "Cannot pay for a cancelled enrollment.");
                return false;
            }

            if (enrollment.Status == EnrollmentStatus.Completed)
            {
                Console.WriteLine(
                    "Cannot pay for a completed enrollment.");
                return false;
            }

            if (enrollment.PaymentStatus == PaymentStatus.Successful)
            {
                Console.WriteLine(
                    "Payment has already been completed.");
                return false;
            }

            Payment payment = new Payment(
                nextPaymentId,
                enrollment.EnrollmentId,
                enrollment.FinalFee);

            bool success = processor.ProcessPayment(
                payment,
                simulateSuccess);

            // Record both successful and failed attempts.
            payments.Add(payment);
            nextPaymentId++;

            if (success)
            {
                enrollment.PaymentStatus = PaymentStatus.Successful;

                Console.WriteLine(
                    $"Payment ID: {payment.PaymentId}");
                Console.WriteLine(
                    $"Amount paid: ₹{payment.Amount:F2}");
                Console.WriteLine(
                    $"Payment status: {payment.Status}");
            }
            else
            {
                Console.WriteLine(
                    "Payment attempt recorded as failed.");
            }

            return success;
        }

        public void DisplayPaymentHistory()
        {
            if (payments.Count == 0)
            {
                Console.WriteLine("No payment records found.");
                return;
            }

            Console.WriteLine("\n========== PAYMENT HISTORY ==========");

            foreach (Payment payment in payments)
            {
                Console.WriteLine(
                    $"Payment ID: {payment.PaymentId}");
                Console.WriteLine(
                    $"Enrollment ID: {payment.EnrollmentId}");
                Console.WriteLine(
                    $"Amount: ₹{payment.Amount:F2}");
                Console.WriteLine(
                    $"Date: {payment.PaymentDate}");
                Console.WriteLine(
                    $"Status: {payment.Status}");
                Console.WriteLine("-------------------------------------");
            }
        }
    }
}
