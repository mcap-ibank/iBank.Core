namespace iBank.Core.Database
{
    public interface IPersonDocumentData
    {
        string DocumentIssue { get; set; }
        string DocumentIssueDate { get; set; }
        string DocumentDivisionCode { get; set; }
        string Address { get; set; }
        string PhoneHome { get; set; }
        string PhoneMobile { get; set; }
    }
}