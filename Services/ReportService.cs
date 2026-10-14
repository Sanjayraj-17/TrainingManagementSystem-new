
using System;
using System.Collections.Generic;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services
{
    public class ReportService
    {
        private readonly Course[] courses;
        private readonly List<Enrollment> enrollments;
        private readonly List<Payment> payments;

        public ReportService(
            Course[] courses,
            List<Enrollment> enrollments,
            List<Payment> payments)
        {
            this.courses = courses;
            this.enrollments = enrollments;
            this.payments = payments;
        }

        // REPORT A: Course Occupancy
        public void DisplayCourseOccupancyReport()
        {
            Console.WriteLine("\n========== REPORT A: COURSE OCCUPANCY ==========");

            foreach (Course course in courses)
            {
                double occupancyPercentage = 0;

                if (course.TotalSeats > 0)
                {
                    occupancyPercentage =
                        (double)course.EnrolledCount
                        / course.TotalSeats * 100;
                }

                Console.WriteLine($"Course Code: {course.CourseCode}");
                Console.WriteLine($"Course Title: {course.Title}");
                Console.WriteLine($"Total Seats: {course.TotalSeats}");
                Console.WriteLine($"Occupied Seats: {course.EnrolledCount}");
                Console.WriteLine($"Available Seats: {course.AvailableSeats}");
                Console.WriteLine($"Occupancy: {occupancyPercentage:F2}%");
                Console.WriteLine("-----------------------------------------------");
            }
        }

        // REPORT B: Expected Fees
        public void DisplayExpectedFeesReport()
        {
            Console.WriteLine("\n========== REPORT B: EXPECTED FEES ==========");

            decimal expectedFees = 0m;
            int activeEnrollmentCount = 0;

            foreach (Enrollment enrollment in enrollments)
            {
                if (enrollment.Status == EnrollmentStatus.Active)
                {
                    expectedFees += enrollment.FinalFee;
                    activeEnrollmentCount++;
                }
            }

            Console.WriteLine(
                $"Active Enrollments: {activeEnrollmentCount}");

            Console.WriteLine(
                $"Expected Fees: Rs. {expectedFees:F2}");

            Console.WriteLine(
                "Includes active enrollments even when payment is pending.");
        }

        // REPORT C: Payment Summary
        public void DisplayPaymentSummaryReport()
        {
            Console.WriteLine("\n========== REPORT C: PAYMENT SUMMARY ==========");

            int successfulAttempts = 0;
            int failedAttempts = 0;
            decimal totalCollected = 0m;

            foreach (Payment payment in payments)
            {
                if (payment.Status == PaymentStatus.Successful)
                {
                    successfulAttempts++;
                    totalCollected += payment.Amount;
                }
                else if (payment.Status == PaymentStatus.Failed)
                {
                    failedAttempts++;
                }
            }

            Console.WriteLine(
                $"Successful Payment Attempts: {successfulAttempts}");

            Console.WriteLine(
                $"Failed Payment Attempts: {failedAttempts}");

            Console.WriteLine(
                $"Total Collected: Rs. {totalCollected:F2}");
        }

        // REPORT D: Course Enrollment Summary
        public void DisplayCourseEnrollmentSummaryReport()
        {
            Console.WriteLine(
                "\n========== REPORT D: COURSE ENROLLMENT SUMMARY ==========");

            foreach (Course course in courses)
            {
                int activeCount = 0;

                foreach (Enrollment enrollment in enrollments)
                {
                    if (enrollment.Status == EnrollmentStatus.Active &&
                        string.Equals(
                            enrollment.CourseCode,
                            course.CourseCode,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        activeCount++;
                    }
                }

                Console.WriteLine($"Course: {course.Title}");
                Console.WriteLine(
                    $"Active Enrollments: {activeCount}");

                if (course.AvailableSeats <= 0)
                {
                    Console.WriteLine("Status: No remaining seats");
                }
                else
                {
                    Console.WriteLine(
                        $"Remaining Seats: {course.AvailableSeats}");
                }

                Console.WriteLine("-----------------------------------------------");
            }
        }

        // Display all four reports
        public void DisplayAllReports()
        {
            DisplayCourseOccupancyReport();
            DisplayExpectedFeesReport();
            DisplayPaymentSummaryReport();
            DisplayCourseEnrollmentSummaryReport();
        }

        // Keep this method if your Program.cs already calls it.
        public void DisplayDashboardReport()
        {
            DisplayExpectedFeesReport();
            DisplayPaymentSummaryReport();
        }

        // Keep this method if your Program.cs already calls it.
        public void DisplayCourseAvailabilityReport()
        {
            DisplayCourseEnrollmentSummaryReport();
        }
    }
}
