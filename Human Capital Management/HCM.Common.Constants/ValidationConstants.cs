namespace HCM.Common.Constants
{
    public class ValidationConstants
    {
        public static class EmployeeConstants
        {
            public const int MinEmployeeAge = 16;
            public const int MaxEmployeeAge = 90;

            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 30;

            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 30;
        }

        public static class PaginationConstants
        {
            public const int DefaultItemsPerPage = 12;
            public const int TasksItemPerPage = 6;
            public const int AuditItemsPerPage = 24;
            public const int AdminEmployeesPerPage = 20;
        }
    }
}