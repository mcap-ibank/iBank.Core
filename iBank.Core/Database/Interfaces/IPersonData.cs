namespace iBank.Core.Database
{
    public interface IPersonData
    {
        string DocumentSerialNumber { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string Patronymic { get; set; }
        string BirthDate { get; set; }
        string BirthPlace { get; set; }
    }
}