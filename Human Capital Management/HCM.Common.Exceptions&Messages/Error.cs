namespace HCM.Common.Exceptions_Messages
{
    public record Error(string ErrorType, string ErrorMessage)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
    }
}
