namespace HCM.Common.Exceptions_Messages.Departments
{
    public static class DepartmentMessages
    {
        public static class Department
        {
            public const string NotFound = "Could not find the department";
            public const string EmployeeNotFound = "Employee cannot be found in the department";

            public const string DepartmentIsFull =
                "Department is full. Consider increasing the max capacity or remove some people";

            public const string SuccessfullyAddedAnEmployee = "You have successfully added {0} to the department";

            public const string CannotHaveLessCapacityThanEmployees =
                "You cannot change the capacity below the employee count, consider removing some employees first";
        }

        public static class Position
        {
            public const string NotFound = "Could not find the position";
        }

        public static class Seniority
        {
            public const string NotFound = "Could not find the seniority";
        }

        public static class Success
        {
            public const string SuccessfullyRemovedEmployeeFromDepartment =
                "You have successfully removed the employee from the department";

            public const string SuccessfullyEditedDepartmentData = "You have successfully eddited departments data";
        }
    }
}