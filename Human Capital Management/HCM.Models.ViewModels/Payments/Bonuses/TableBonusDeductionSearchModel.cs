namespace HCM.Models.ViewModels.Payments.Bonuses
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;

    public class TableBonusDeductionSearchModel
    {
        public string EmployeeId { get; set; } = null!;

        [DateTimeModelBinder]
        public DateTime MonthYearOfSearch { get; set; }
    }
}