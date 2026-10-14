
using System;

namespace TrainingManagementSystem.Models
{
    public class OnlineSession : CourseSession
    {
        public OnlineSession(
            string courseCode,
            DateTime startDate,
            SessionDuration duration)
            : base(courseCode, startDate, duration)
        {
        }

        public override decimal CalculateSessionCharge()
        {
            return 200m;
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine("Session Type: Online");
            Console.WriteLine(
                $"Session Charge: Rs. {CalculateSessionCharge():F2}");
        }
    }
}
