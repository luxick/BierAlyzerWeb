namespace BierAlyzer.Contracts.Model
{
    public enum UserType
    {
        User = 10,
        Admin = 100
    }

    public enum EventType
    {
        Public = 10,
        Private = 20,
        Hidden = 30,
    }

    public enum EventStatus
    {
        NotYet = 10,
        Open = 20,
        Closed = 30
    }
}
