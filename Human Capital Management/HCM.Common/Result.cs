namespace HCM.Common
{
    using Exceptions_Messages;

    public class Result
    {

        private Result(bool isSuccess, Error error)
        {
            this.IsSuccess = isSuccess;
            this.Error = error;
        }

        private Result(bool isSuccess, Error error, string message)
        {
            this.IsSuccess = isSuccess;
            this.Error = error;
            this.Message = message;
        }

        public bool IsSuccess { get;  init; }

        public bool IsFailure => !IsSuccess;

        public string Message { get; init; } //Probably needs to be generic

        public Error Error { get; init; }

        public static Result Success(string message) => new Result(true, Error.None, message);

        public static Result Failure(Error error) => new Result(false, error);

    }
}
