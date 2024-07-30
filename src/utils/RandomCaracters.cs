namespace utils;
public class RandomCaracters
{
    public static string GenerateRandomString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public static int GenerateRandomInteger()
    {
        Random random = new Random();
        int randomNumber = random.Next(0, 101);
        return randomNumber;
    }
}