namespace ArtGallery.Application.Common.Helpers;

public class AgeHelper
{
    public static int GetAge(DateTime birthDate)
    {
        DateTime x = DateTime.Now; 
        int age = x.Year - birthDate.Year;

        if (x.Month < birthDate.Month || (x.Month == birthDate.Month && x.Day < birthDate.Day))
            age--;

        return age;
    }
}
