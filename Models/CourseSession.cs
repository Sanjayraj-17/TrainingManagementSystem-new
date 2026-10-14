
using System;

namespace TrainingManagementSystem.Models
{
    public abstract class CourseSession
    {
        public string CourseCode { get; }
        public DateTime StartDate { get; }
        public SessionDuration Duration { get; }

        protected CourseSession(
            string courseCode,
            DateTime startDate,
            SessionDuration duration)
        {
            if (string.IsNullOrWhiteSpace(courseCode))
            {
                throw new ArgumentException(
                    "Course code is required.",
                    nameof(courseCode));
            }

            CourseCode = courseCode;
            StartDate = startDate;
            Duration = duration;
        }

        public abstract decimal CalculateSessionCharge();

        public virtual void DisplayDetails()
        {
            Console.WriteLine($"Course Code: {CourseCode}");
            Console.WriteLine($"Start Date: {StartDate:dd-MM-yyyy}");
            Console.WriteLine($"Duration: {Duration}");
        }
    }
}
