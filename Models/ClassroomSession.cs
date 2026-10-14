
using System;

namespace TrainingManagementSystem.Models
{
    public class ClassroomSession : CourseSession
    {
        public ClassroomSession(
            string courseCode,
            DateTime startDate,
            SessionDuration duration)
            : base(courseCode, startDate, duration)
        {
        }

        public override decimal CalculateSessionCharge()
        {
            return 800m;
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine("Session Type: Classroom");
            Console.WriteLine(
                $"Session Charge: Rs. {CalculateSessionCharge():F2}");
        }
    }
}
