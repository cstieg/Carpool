namespace Carpool.Domain.Models
{
    public enum WeekDay
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64,
        WeekDays = Monday | Tuesday | Wednesday | Thursday | Friday,
        WeekEnds = Saturday | Sunday,
        EveryDay = WeekDays | WeekEnds
    }
}
