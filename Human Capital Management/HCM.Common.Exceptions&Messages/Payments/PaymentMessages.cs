namespace HCM.Common.Exceptions_Messages.Payments
{
    public static class PaymentMessages
    {

        public const string SuccessfullyChangedSalary = "You have successfully updated the salary";
        public const string IssueUpdatingWithSalary = "There was an issue when trying to update the salary";
        public const string BonusOrDeductionCannotBeBelowOne = "Minimum value is atleast 1";
        public const string ReasonNotFound = "Incorrect reason";
        
        public const string SuccessfullyAddedBonus = "You have successfully added a bonus";
        public const string IssueAddingBonus = "There was an issue when trying to add a bonus";

        public const string SuccessfullyAddedDeduction = "You have successfully deducted this user";
        public const string IssueAddingDeduction = "There was an issue when trying to add deduction to this user";
    }
}