namespace HCM.Models.ViewModels.Statistics
{
    using Employees;

    using Tasks;

    public class HomePageStatisticsModel
    {

        public HomePageStatisticsModel()
        {
            this.EmployeeBirthdays = new HashSet<EmployeeBirthdayModel>();
            this.UpcomingTasks = new HashSet<TaskModel>();
            this.BestPerformingEmployees = new HashSet<EmployeeBonusModel>();
            this.WorstPerformingEmployees = new HashSet<EmployeeDeductionModel>();
            this.BusiestEmployees = new HashSet<BusiestEmployeeTaskModel>();
        }

        public int NumberOfEmployees { get; set; }

        public int NumberOfDepartments { get; set; }

        public int NumberOfNewEmployeesThisMonth { get; set; }

        public ICollection<EmployeeBirthdayModel> EmployeeBirthdays { get; set; }

        public ICollection<TaskModel> UpcomingTasks { get; set; }

        public ICollection<TaskModel> TaskIssuedByMe { get; set; }

        public ICollection<EmployeeBonusModel> BestPerformingEmployees { get; set; }

        public ICollection<EmployeeDeductionModel> WorstPerformingEmployees { get; set; }

        public ICollection<BusiestEmployeeTaskModel> BusiestEmployees { get; set; }

    }
}
