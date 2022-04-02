namespace Calendar.AgendaScheduler.Domain.Interfaces
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }

        public static ValidationResult Succeed()
        {
            return new ValidationResult { IsValid = true };
        }

        public static ValidationResult Failed(string message)
        {
            return new ValidationResult
            {
                IsValid = false,
                Message = message
            };
        }
    }
}