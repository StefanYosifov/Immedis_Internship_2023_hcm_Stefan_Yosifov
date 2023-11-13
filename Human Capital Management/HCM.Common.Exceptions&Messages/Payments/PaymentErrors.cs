namespace HCM.Common.Exceptions_Messages.Payments
{
    public static class PaymentErrors
    {

        public static readonly Error InvalidDepartmentId = new("Invalid.Department", "This department is incorrect!");

        public static readonly Error IncorrectCredentials = new("Incorrect.Password", "Incorrect credentials");

        public static readonly Error NotAuthorized =
            new("Invalid.Roles", "You must be admin or HR in order to complete the payroll");

    }
}
