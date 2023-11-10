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

        public const string SuccessfullyAddedPayroll = "You have successfully added {0} number of payrolls";
        public const string IssueAddingPayroll = "There was an issue with creating payrolls, please try again later";

        public const string SuccessfullyCompletedPayroll = "You have successfully completed # {0} payroll";
        public const string InvalidPayroll = "Could not find the payroll";

        public const string SuccessfullyRemovedPayroll = "You have successfully removed # {0} payroll";

        public const string IssueCompletingPayroll =
            "There was an issue with completing the payroll, please try again later";

        public const string IssueRemovingPayroll =
            "There was an issue with removing the payroll,please try again later";

        public const string RemovingCompletedPayroll = "You cannot remove payroll that has already been completed";
    }
}