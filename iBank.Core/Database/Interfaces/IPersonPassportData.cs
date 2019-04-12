namespace iBank.Database
{
    public interface IPersonPassportData
    {
        string PassportIssue { get; set; }
        string PassportIssueDate { get; set; }
        string PassportDivisionCode { get; set; }
        string Address { get; set; }
        string PhoneHome { get; set; }
        string PhoneMobile { get; set; }
    }
}