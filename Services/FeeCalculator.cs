
using System;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services
{
    public class FeeCalculator
    {
        private const decimal OnlineSurcharge = 200m;
        private const decimal ClassroomSurcharge = 800m;

        // Overload 1: Calculate fee without discounts
        public (decimal BaseFee, decimal TotalDiscount, decimal FinalFee)
            CalculateFee(decimal baseFee, bool isOnline)
        {
            ValidateFee(baseFee);

            decimal surcharge = GetSurcharge(isOnline);
            decimal finalFee = baseFee + surcharge;

            return (baseFee, 0m, finalFee);
        }

        // Overload 2: Calculate fee with discount information
        public (decimal BaseFee, decimal TotalDiscount, decimal FinalFee)
            CalculateFee(
                decimal baseFee,
                decimal earlyBirdDiscount,
                decimal groupDiscount,
                bool isOnline)
        {
            ValidateFee(baseFee);

            if (earlyBirdDiscount < 0 || groupDiscount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "Discount amounts cannot be negative.");
            }

            decimal totalDiscount =
                earlyBirdDiscount + groupDiscount;

            if (totalDiscount > baseFee)
            {
                throw new ArgumentException(
                    "Total discount cannot exceed the base fee.");
            }

            decimal surcharge = GetSurcharge(isOnline);
            decimal finalFee = baseFee - totalDiscount + surcharge;

            return (baseFee, totalDiscount, finalFee);
        }

        // Apply the project business rules
        public (decimal BaseFee, decimal TotalDiscount, decimal FinalFee)
            CalculateEnrollmentFee(
                decimal baseFee,
                bool isOnline,
                bool qualifiesForEarlyBird,
                int eligibleLearners)
        {
            ValidateFee(baseFee);

            if (eligibleLearners < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(eligibleLearners),
                    "Eligible learner count cannot be negative.");
            }

            decimal earlyBirdDiscount =
                qualifiesForEarlyBird ? baseFee * 0.10m : 0m;

            decimal groupDiscount =
                eligibleLearners >= 3 ? baseFee * 0.05m : 0m;

            return CalculateFee(
                baseFee,
                earlyBirdDiscount,
                groupDiscount,
                isOnline);
        }

        private static decimal GetSurcharge(bool isOnline)
        {
            return isOnline
                ? OnlineSurcharge
                : ClassroomSurcharge;
        }

        private static void ValidateFee(decimal baseFee)
        {
            if (baseFee < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(baseFee),
                    "Base fee cannot be negative.");
            }
        }

        // Display a read-only fee summary using 'in'
        public void DisplayFeeBreakdown(in FeeSummary fee)
        {
            Console.WriteLine("\n------ FEE BREAKDOWN ------");
            Console.WriteLine($"Base Fee       : Rs. {fee.BaseFee:F2}");
            Console.WriteLine($"Total Discount : Rs. {fee.TotalDiscount:F2}");
            Console.WriteLine($"Surcharge      : Rs. {fee.Surcharge:F2}");
            Console.WriteLine($"Final Fee      : Rs. {fee.FinalFee:F2}");
        }
    }
}
