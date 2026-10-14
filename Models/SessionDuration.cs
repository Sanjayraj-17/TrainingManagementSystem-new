
namespace TrainingManagementSystem.Models
{
    public struct SessionDuration
    {
        public int Days { get; }

        public SessionDuration(int days)
        {
            if (days <= 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    nameof(days),
                    "Duration must be greater than zero.");
            }

            Days = days;
        }

        public override string ToString()
        {
            return $"{Days} days";
        }
    }
}

