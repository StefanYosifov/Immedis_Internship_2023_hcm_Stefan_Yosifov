namespace HCM.Common.Constants
{
    public static class ApplicationAPIConstants
    {
        public const string API_BASE_URL = "https://localhost:7153/";
        public const string MVC_BASE_URL = "https://localhost:7039";

        public static class EmployeeAPI
        {
            public const string Index = "/employee/id";

            public const int ElementsPerPage = 12;
        }

        public static class DepartmentsAPI
        {
            public const string GetAllDepartmentsRoute = "all";
            public const string GetAllPositionsByDepartmentId = "positions/{departmentId}";
            public const string GetAllSenioritiesByPositionId = "seniorities/{positionId}";
        }
    }
}