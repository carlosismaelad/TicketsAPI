namespace TicketsApi.Models.Enums
{
    public enum TicketStatus : int
    {
        New = 0,
        Assigned = 1,
        InProgress = 2,
        OnHold = 3,
        CustomerPending = 4,
        Solved = 5,
        Closed = 6,
    }
}