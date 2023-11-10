namespace HCM.Models.ViewModels.Payments.Payroll
{
    public class PayrollEmployeeDetails
    {

        public int PayrollId { get; set; }

        public string EmployeeId { get; set; }
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public decimal Received { get; set; }


        public bool IsPaid { get; set; }


    }
}
