
using System;
using System.Collections.Generic;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services
{
    public class EnrollmentService
    {
        private readonly List<Enrollment> enrollments;
        private readonly Dictionary<string, Learner> learners;
        private readonly Course[] courses;
        private readonly FeeCalculator feeCalculator = new FeeCalculator();

        private int nextEnrollmentId = 1;

        public EnrollmentService(
            List<Enrollment> enrollments,
            Dictionary<string, Learner> learners,
            Course[] courses)
        {
            this.enrollments = enrollments;
            this.learners = learners;
            this.courses = courses;

            foreach (Enrollment item in enrollments)
            {
                if (item.EnrollmentId >= nextEnrollmentId)
                {
                    nextEnrollmentId = item.EnrollmentId + 1;
                }
            }
        }

        public bool AddEnrollment(
            string learnerId,
            string courseCode,
            decimal discountAmount)
        {
            learnerId = learnerId.Trim();
            courseCode = courseCode.Trim();

            // 1. Validate learner
            if (learnerId.Length == 0 ||
                !learners.ContainsKey(learnerId))
            {
                Console.WriteLine("Learner not found.");
                return false;
            }

            // 2. Find course
            Course? selectedCourse = null;

            foreach (Course course in courses)
            {
                if (string.Equals(
                    course.CourseCode,
                    courseCode,
                    StringComparison.OrdinalIgnoreCase))
                {
                    selectedCourse = course;
                    break;
                }
            }

            if (selectedCourse == null)
            {
                Console.WriteLine("Course not found.");
                return false;
            }

            // 3. Reject active duplicate enrollment
            foreach (Enrollment item in enrollments)
            {
                if (item.Status == EnrollmentStatus.Active &&
                    string.Equals(
                        item.LearnerId,
                        learnerId,
                        StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(
                        item.CourseCode,
                        courseCode,
                        StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(
                        "Learner is already actively enrolled in this course.");
                    return false;
                }
            }

            // 4. Check seats
            if (selectedCourse.AvailableSeats <= 0)
            {
                Console.WriteLine("No seats available.");
                return false;
            }

            // 5. Validate manually entered discount
            if (discountAmount < 0 ||
                discountAmount > selectedCourse.Fee)
            {
                Console.WriteLine("Invalid discount amount.");
                return false;
            }

            // 6. Calculate automatic discounts
            bool qualifiesForEarlyBird =
                (selectedCourse.StartDate.Date -
                 DateTime.Today).Days >= 14;

            // Group discount applies only when at least
            // three eligible learners enroll together.
            // This single-enrollment method cannot verify a group,
            // so it does not apply a group discount automatically.
            int eligibleLearners = 1;

            var automaticFee = feeCalculator.CalculateEnrollmentFee(
                selectedCourse.Fee,
                selectedCourse.IsOnline,
                qualifiesForEarlyBird,
                eligibleLearners);

            // Combine the user-entered discount with the early-bird
            // discount, but do not allow the total to exceed base fee.
            decimal totalDiscount =
                automaticFee.TotalDiscount + discountAmount;

            if (totalDiscount > selectedCourse.Fee)
            {
                Console.WriteLine(
                    "Combined discounts cannot exceed the base fee.");
                return false;
            }

            decimal surcharge = selectedCourse.IsOnline ? 200m : 800m;

            // 7. Create enrollment
            Enrollment enrollment = new Enrollment(
                nextEnrollmentId,
                learnerId,
                selectedCourse.CourseCode,
                DateTime.Now,
                selectedCourse.Fee,
                totalDiscount,
                surcharge);

            // 8. Save record and update seat count only after validation
            enrollments.Add(enrollment);
            selectedCourse.EnrolledCount++;
            nextEnrollmentId++;

            // 9. Display confirmation and fee breakdown
            Console.WriteLine("\nEnrollment successful!");
            Console.WriteLine("----------------------");
            Console.WriteLine($"Enrollment ID : {enrollment.EnrollmentId}");
            Console.WriteLine($"Learner ID    : {enrollment.LearnerId}");
            Console.WriteLine($"Course Code   : {enrollment.CourseCode}");
            Console.WriteLine($"Base Fee      : Rs. {enrollment.BaseFee:F2}");
            Console.WriteLine($"Total Discount: Rs. {enrollment.DiscountAmount:F2}");
            Console.WriteLine($"Surcharge     : Rs. {enrollment.Surcharge:F2}");
            Console.WriteLine($"Final Fee     : Rs. {enrollment.FinalFee:F2}");
            Console.WriteLine($"Status        : {enrollment.Status}");
            Console.WriteLine(
                $"Available Seats: {selectedCourse.AvailableSeats}");

            return true;
        }

        public void DisplayAll()
        {
            if (enrollments.Count == 0)
            {
                Console.WriteLine("No enrollment records found.");
                return;
            }

            for (int i = 0; i < enrollments.Count; i++)
            {
                Enrollment item = enrollments[i];

                Console.WriteLine(
                    $"ID: {item.EnrollmentId} | " +
                    $"Learner: {item.LearnerId} | " +
                    $"Course: {item.CourseCode} | " +
                    $"Fee: Rs. {item.FinalFee:F2} | " +
                    $"Status: {item.Status}");
            }
        }

        public Enrollment? FindById(int enrollmentId)
        {
            foreach (Enrollment item in enrollments)
            {
                if (item.EnrollmentId == enrollmentId)
                {
                    return item;
                }
            }

            return null;
        }

        public bool CancelEnrollment(int enrollmentId)
        {
            Enrollment? enrollment = FindById(enrollmentId);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return false;
            }

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                Console.WriteLine("This enrollment is already cancelled.");
                return false;
            }

            if (enrollment.Status == EnrollmentStatus.Completed)
            {
                Console.WriteLine("Completed enrollment cannot be cancelled.");
                return false;
            }

            Course? selectedCourse = null;

            foreach (Course course in courses)
            {
                if (string.Equals(
                    course.CourseCode,
                    enrollment.CourseCode,
                    StringComparison.OrdinalIgnoreCase))
                {
                    selectedCourse = course;
                    break;
                }
            }

            if (selectedCourse == null)
            {
                Console.WriteLine("Associated course not found.");
                return false;
            }

            enrollment.Status = EnrollmentStatus.Cancelled;
            selectedCourse.EnrolledCount--;

            Console.WriteLine("Enrollment cancelled successfully!");
            Console.WriteLine(
                "Enrollment history has been retained.");

            return true;
        }
    }
}

