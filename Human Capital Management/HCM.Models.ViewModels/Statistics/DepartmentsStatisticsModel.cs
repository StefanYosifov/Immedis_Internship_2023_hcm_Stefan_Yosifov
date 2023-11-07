namespace HCM.Models.ViewModels.Statistics
{
    using Employees;

    using Tasks;

    public class DepartmentsStatisticsModel
    {
        public DepartmentsStatisticsModel()
        {

        }

        public int TotalDepartmentCount { get; set; }

        public int MostPopulatedDepartmentCount { get; set; }

        public int MostPopulatedDepartmentName { get; set; }

        public int LeastPopulatedDepartmentCount { get; set; }

        public int LeastPopulatedDepartmentName { get; set; }


    }
}