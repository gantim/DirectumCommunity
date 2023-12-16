using DirectumCommunity.Models;

namespace DirectumCommunity.Extensions;

public static class PersonExtension
{
    public static bool IsBirthdayToday(this Person person)
    {
        if (person.DateOfBirth.HasValue)
        {
            DateTimeOffset today = DateTimeOffset.Now;

            if (person.DateOfBirth.Value.Month == today.Month && person.DateOfBirth.Value.Day == today.Day)
            {
                return true;
            }

            return false;
        }

        return false;
    }
    
    public static bool IsSendBirthdayNotification(this Person person)
    {
        if (person.LastBirthdayNotification.HasValue)
        {
            DateTimeOffset today = DateTimeOffset.Now;

            return person.LastBirthdayNotification.Value.Year == today.Year;
        }

        return false;
    }
}