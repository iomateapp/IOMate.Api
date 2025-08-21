namespace IOMate.Application.Shared.Exceptions
{
    public class BadRequestException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public BadRequestException(string message)
            : base(message)
        {
            Errors = new List<string> { message };
        }

        public BadRequestException(IEnumerable<string> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
