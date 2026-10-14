using System.Collections.Generic;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Services;
using TrainingManagementSystem.Payments;
using TrainingManagementSystem.Utilities;


namespace TrainingManagementSystem
{ 
    internal class Program
    {
        private const int CourseCount = 5;
        private static Course[] courses = new Course[CourseCount]
        {
            new Course("C101", "C# Programming", 5000m, 30, 40, 12),
            new Course("C102", "Java Development", 6500m, 45, 35, 20),
            new Course("C103", "Web Development", 4500m, 30, 30, 10),
            new Course("C104", "Python Programming", 5500m, 35, 25, 8),
            new Course("C105", "Data Analytics", 7000m, 50, 20, 15)
        };
        private static void ConfigureCourses()
        {
            courses[0].StartDate = DateTime.Today.AddDays(20);
            courses[0].IsOnline = true;

            courses[1].StartDate = DateTime.Today.AddDays(10);
            courses[1].IsOnline = false;

            courses[2].StartDate = DateTime.Today.AddDays(30);
            courses[2].IsOnline = true;

            courses[3].StartDate = DateTime.Today.AddDays(7);
            courses[3].IsOnline = false;

            courses[4].StartDate = DateTime.Today.AddDays(21);
            courses[4].IsOnline = true;
        }






        private static Dictionary<string, Learner> learners = new Dictionary<string, Learner>(
        StringComparer.OrdinalIgnoreCase);

        private static List<Enrollment> enrollments = new List<Enrollment>();
        private static EnrollmentService enrollmentService = new EnrollmentService(enrollments, learners, courses);
        private static List<Payment> payments = new List<Payment>();
        private static PaymentService paymentService = new PaymentService(payments, enrollments);
        private static ReportService reportService = new ReportService(courses, enrollments, payments);

        static void Main(string[] args)
        {
            TestSessionDurationValueType();

            TestCourseSummary();
            TestCourseSessions();
            ConfigureCourses();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n===== TRAINING MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Display All Courses");
                Console.WriteLine("2. Search by Course Code");
                Console.WriteLine("3. Search by Course Title");
                Console.WriteLine("4. Sort Courses by Fee");
                Console.WriteLine("5. Display Available Seats");
                Console.WriteLine("6. Register Learner");
                Console.WriteLine("7. Display All Learners");
                Console.WriteLine("8. Search Learner by ID");
                Console.WriteLine("9. Remove Learner");
                Console.WriteLine("10. Create Enrollment");
                Console.WriteLine("11. Display All Enrollments");
                Console.WriteLine("12. Search Enrollment by ID");
                Console.WriteLine("13. Cancel Enrollment");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        CollectionHelper.DisplayItems(courses);

                        break;

                    case "2":
                        {
                            Console.Write("Enter Course Code: ");
                            string searchCode = Console.ReadLine() ?? "";

                            Course? foundCourse = CollectionHelper.FindItem(
                                courses,
                                searchCode,
                                course => course.CourseCode);

                            if (foundCourse != null)
                            {
                                Console.WriteLine($"Found: {foundCourse.Title}");
                            }
                            else
                            {
                                Console.WriteLine("Course not found.");
                            }

                            break;
                        }


                    case "3":
                        SearchByTitle();
                        break;
                    case "4":
                        SortCoursesByFee();
                        break;
                    case "5":
                        DisplayAvailableSeats();
                        break;
                    case "6":
                        RegisterLearner();
                        break;

                    case "7":
                        DisplayAllLearners();
                        break;

                    case "8":
                        SearchLearnerById();
                        break;

                    case "9":
                        RemoveLearner();
                        break;
                    case "10":
                        CreateEnrollment();
                        break;

                    case "11":
                        enrollmentService.DisplayAll();
                        break;

                    case "12":
                        SearchEnrollment();
                        break;

                    case "13":
                        CancelEnrollment();
                        break;
                    case "14":
                        ProcessPaymentMenu();
                        break;

                    case "15":
                        DisplayPaymentHistoryMenu();
                        break;

                    case "16":
                        try
                        {
                            reportService.DisplayAllReports();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                "An unexpected error occurred while generating reports.");
                            Console.WriteLine($"Details: {ex.Message}");
                        }
                        break;


                    case "0":
                        running = false;
                        Console.WriteLine("Thank you for using the system");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;

                }
            }

        }


        static void DisplayAvailableSeats()
        {
            Console.WriteLine("\n===== AVAILABLE SEATS =====");

            foreach (Course course in courses)
            {
                Console.WriteLine(
                    $"{course.CourseCode} - {course.Title}: " +
                    $"{course.AvailableSeats} / {course.TotalSeats} seats available");
            }
        }

        static void DisplayCourses()
        {
            Console.WriteLine("\n{0,-10} {1,-25} {2,12} {3,10} {4,12} {5,12}",
                "Code", "Course Title", "Fee (Rs.)",
                "Duration", "Total Seats", "Enrolled");
            Console.WriteLine(new string('-', 85));

            foreach(Course course in courses)
            {
                Console.WriteLine("{0,-10} {1,-25} {2,12:F2} {3,10} {4,12} {5,12}",
                    course.CourseCode,
                    course.Title,
                    course.Fee,
                    course.DurationDays,
                    course.TotalSeats,
                    course.EnrolledCount);

            }
            

        }
        static void SearchByCode()
        {
            Console.Write("Enter course code: ");
            string code = Console.ReadLine() ?? "";

            bool found = false;

            foreach (Course course in courses)
            {
                if (string.Equals(
                    course.CourseCode,
                    code.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                {
                    DisplayCourseDetails(course);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("Course not found.");
            }
        }
        static void SearchByTitle()
        {
            Console.Write("Enter course code:");
            string title = Console.ReadLine() ?? "";

            bool found = false;
            
            foreach (Course course in courses)
            {
                if (string.Equals(
                    course.Title,
                    title.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                {
                    DisplayCourseDetails(course);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Console.WriteLine("Course not found.");
            }



        }
        static void SortCoursesByFee()
        {
            
            
                Console.WriteLine("1. Ascending");
                Console.WriteLine("2. Descending");
                Console.Write("Choose sorting order: ");

                string order = Console.ReadLine() ?? "";

                if (order != "1" && order != "2")
                {
                    Console.WriteLine("Invalid sorting choice.");
                    return;
                }

                // Copy the array so the original catalogue remains unchanged.
                Course[] sortedCourses = (Course[])courses.Clone();

                // Bubble sort: no LINQ required.
                for (int i = 0; i < sortedCourses.Length - 1; i++)
                {
                    for (int j = 0; j < sortedCourses.Length - 1 - i; j++)
                    {
                        bool shouldSwap;

                        if (order == "1")
                        {
                            shouldSwap =
                                sortedCourses[j].Fee > sortedCourses[j + 1].Fee;
                        }
                        else
                        {
                            shouldSwap =
                                sortedCourses[j].Fee < sortedCourses[j + 1].Fee;
                        }

                        if (shouldSwap)
                        {
                            Course temp = sortedCourses[j];
                            sortedCourses[j] = sortedCourses[j + 1];
                            sortedCourses[j + 1] = temp;
                        }
                    }
                }

                Console.WriteLine("\nCourses sorted by fee:");

                foreach (Course course in sortedCourses)
                {
                    Console.WriteLine(
                        $"{course.CourseCode} | {course.Title} | Rs. {course.Fee:F2}");
                }
            

           


        }

        static void RegisterLearner()
        {
            Console.WriteLine("\n===== LEARNER REGISTRATION =====");

            Console.Write("Enter learner ID: ");
            string learnerId = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(learnerId))
            {
                Console.WriteLine("Invalid input: Learner ID cannot be empty.");
                return;
            }

            if (learners.ContainsKey(learnerId))
            {
                Console.WriteLine("Registration failed: Learner ID already exists.");
                return;
            }

            Console.Write("Enter learner name: ");
            string name = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Invalid input: Learner name cannot be empty.");
                return;
            }

            Console.Write("Enter contact number: ");
            string contactNumber = (Console.ReadLine() ?? "").Trim();

            // Project validation rule:
            // Exactly 10 digits, with no spaces or other characters.
            if (!IsValidContactNumber(contactNumber))
            {
                Console.WriteLine(
                    "Invalid contact number: Enter exactly 10 digits.");
                return;
            }

            Console.Write("Enter email: ");
            string email = (Console.ReadLine() ?? "").Trim();

            if (!IsValidEmail(email))
            {
                Console.WriteLine("Invalid email address.");
                return;
            }

            Learner learner = new Learner(
                learnerId,
                name,
                contactNumber,
                email);

            learners.Add(learnerId, learner);

            Console.WriteLine("Learner registered successfully!");
        }

        static void DisplayAllLearners()
        {
            Console.WriteLine("\n===== REGISTERED LEARNERS =====");

            if (learners.Count == 0)
            {
                Console.WriteLine("No learners registered yet.");
                return;
            }

            Console.WriteLine(
                "{0,-15} {1,-25} {2,-15} {3,-30}",
                "Learner ID", "Name", "Contact", "Email");

            Console.WriteLine(new string('-', 85));

            foreach (Learner learner in learners.Values)
            {
                Console.WriteLine(
                    "{0,-15} {1,-25} {2,-15} {3,-30}",
                    learner.LearnerId,
                    learner.Name,
                    learner.ContactNumber,
                    learner.Email);
            }
        }


        static bool IsValidContactNumber(string contactNumber)
        {
            if (contactNumber.Length != 10)
            {
                return false;
            }

            foreach (char digit in contactNumber)
            {
                if (digit < '0' || digit > '9')
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsValidEmail(string email)
        {
            int atIndex = email.IndexOf('@');
            int dotIndex = email.LastIndexOf('.');

            return atIndex > 0
                && dotIndex > atIndex + 1
                && dotIndex < email.Length - 1
                && !email.Contains(' ');
        }

        static void SearchLearnerById()
        {
            Console.Write("Enter learner ID to search: ");
            string learnerId = (Console.ReadLine() ?? "").Trim();

            if (learners.TryGetValue(learnerId, out Learner? learner))
            {
                Console.WriteLine("\nLearner found!");
                Console.WriteLine($"Learner ID : {learner.LearnerId}");
                Console.WriteLine($"Name       : {learner.Name}");
                Console.WriteLine($"Contact    : {learner.ContactNumber}");
                Console.WriteLine($"Email      : {learner.Email}");
            }
            else
            {
                Console.WriteLine("Learner not found.");
            }
        }

        static void RemoveLearner()
        {
            Console.Write("Enter learner ID to remove: ");
            string learnerId = (Console.ReadLine() ?? "").Trim();

            if (learners.Remove(learnerId))
            {
                Console.WriteLine("Learner removed successfully.");
            }
            else
            {
                Console.WriteLine("Learner not found. Nothing was removed.");
            }
        }

        static void CreateEnrollment()
        {
            Console.Write("Enter learner ID: ");
            string learnerId = (Console.ReadLine() ?? "").Trim();

            Console.Write("Enter course code: ");
            string courseCode = (Console.ReadLine() ?? "").Trim();

            Console.Write("Enter discount amount: ");

            if (!decimal.TryParse(
                Console.ReadLine(),
                out decimal discountAmount))
            {
                Console.WriteLine("Invalid discount amount.");
                return;
            }

            enrollmentService.AddEnrollment(
                learnerId,
                courseCode,
                discountAmount);
        }

        static void SearchEnrollment()
        {
            Console.Write("Enter enrollment ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid enrollment ID.");
                return;
            }

            Enrollment? enrollment = enrollmentService.FindById(id);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }

            Console.WriteLine($"Enrollment ID: {enrollment.EnrollmentId}");
            Console.WriteLine($"Learner ID: {enrollment.LearnerId}");
            Console.WriteLine($"Course Code: {enrollment.CourseCode}");
            Console.WriteLine($"Enrollment Date: {enrollment.EnrollmentDate}");
            Console.WriteLine($"Base Fee: Rs. {enrollment.BaseFee:F2}");
            Console.WriteLine($"Discount: Rs. {enrollment.DiscountAmount:F2}");
            Console.WriteLine($"Final Fee: Rs. {enrollment.FinalFee:F2}");
            Console.WriteLine($"Status: {enrollment.Status}");
        }

        static void CancelEnrollment()
        {
            Console.Write("Enter enrollment ID to cancel: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid enrollment ID.");
                return;
            }

            enrollmentService.CancelEnrollment(id);
        }

        static void TestFeeCalculator()
        {
            FeeCalculator calculator = new FeeCalculator();

            // Example: ₹5,000 base fee, early-bird and group discounts,
            // and an online session.
            var result = calculator.CalculateEnrollmentFee(
                5000m,
                true,
                true,
                3);

            decimal surcharge = 200m;

            FeeSummary summary = new FeeSummary(
                result.BaseFee,
                result.TotalDiscount,
                surcharge,
                result.FinalFee);

            calculator.DisplayFeeBreakdown(in summary);
        }


        static void TestCourseSessions()
        {
            CourseSession online = new OnlineSession(
                "C101",
                DateTime.Today.AddDays(20),
                new SessionDuration(30));

            CourseSession classroom = new ClassroomSession(
                "C102",
                DateTime.Today.AddDays(10),
                new SessionDuration(45));

            Console.WriteLine("\n===== ONLINE SESSION =====");
            online.DisplayDetails();

            Console.WriteLine("\n===== CLASSROOM SESSION =====");
            classroom.DisplayDetails();
        }

        static CourseSummary CreateCourseSummary(Course course)
        {
            return new CourseSummary(
                course.CourseCode,
                course.Title,
                course.Fee,
                course.AvailableSeats);
        }

        static void TestCourseSummary()
        {
            // Create a summary from an existing Course object
            CourseSummary summary1 = CreateCourseSummary(courses[0]);

            // Create another record with exactly the same values
            CourseSummary summary2 = new CourseSummary(
                summary1.CourseCode,
                summary1.Title,
                summary1.Fee,
                summary1.AvailableSeats);

            Console.WriteLine("\n===== COURSE SUMMARY RECORD =====");
            Console.WriteLine(summary1);

            // Records compare values
            Console.WriteLine(
                $"Are summaries equal? {summary1 == summary2}");

            // Create a modified copy using the with expression
            CourseSummary summary3 = summary1 with
            {
                AvailableSeats = summary1.AvailableSeats + 5
            };

            Console.WriteLine("\nOriginal summary:");
            Console.WriteLine(summary1);

            Console.WriteLine("\nModified copy:");
            Console.WriteLine(summary3);

            Console.WriteLine(
                $"Are original and modified equal? {summary1 == summary3}");
        }

        static void TestSessionDurationValueType()
        {
            SessionDuration duration1 = new SessionDuration(30);

            // A value-type assignment copies the value
            SessionDuration duration2 = duration1;

            Console.WriteLine("\n===== SESSION DURATION =====");
            Console.WriteLine($"Duration 1: {duration1}");
            Console.WriteLine($"Duration 2: {duration2}");

            Console.WriteLine(
                $"Same duration value? {duration1.Equals(duration2)}");
        }

        private static void ProcessPaymentMenu()
        {
            Console.Write("Enter Enrollment ID: ");

            if (!int.TryParse(Console.ReadLine(), out int enrollmentId))
            {
                Console.WriteLine("Invalid enrollment ID.");
                return;
            }

            Console.WriteLine("Choose payment method:");
            Console.WriteLine("1. Online Payment");
            Console.WriteLine("2. Classroom Payment");
            Console.Write("Enter choice: ");

            string? choice = Console.ReadLine();

            IPaymentProcessor? processor = choice switch
            {
                "1" => new OnlinePaymentProcessor(),
                "2" => new ClassroomPaymentProcessor(),
                _ => null
            };

            if (processor == null)
            {
                Console.WriteLine("Invalid payment method.");
                return;
            }

            Console.Write("Simulate successful payment? (Y/N): ");
            string? answer = Console.ReadLine();

            bool simulateSuccess =
                string.Equals(answer, "Y",
                    StringComparison.OrdinalIgnoreCase);

            paymentService.MakePayment(
                enrollmentId,
                processor,
                simulateSuccess);
        }

        private static void DisplayPaymentHistoryMenu()
        {
            paymentService.DisplayPaymentHistory();
        }

        










        static void DisplayCourseDetails(Course course)
        {
            Console.WriteLine("\n===== COURSE DETAILS =====");
            Console.WriteLine($"Code           : {course.CourseCode}");
            Console.WriteLine($"Title          : {course.Title}");
            Console.WriteLine($"Fee            : Rs. {course.Fee:F2}");
            Console.WriteLine($"Duration       : {course.DurationDays} days");
            Console.WriteLine($"Total Seats    : {course.TotalSeats}");
            Console.WriteLine($"Enrolled       : {course.EnrolledCount}");
            Console.WriteLine($"Available Seats: {course.AvailableSeats}");
        }

    }
}
  
        





