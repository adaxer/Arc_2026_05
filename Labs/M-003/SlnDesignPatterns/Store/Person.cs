namespace PaymentService.Store;

public class Person
{
    public int GetCash()
    {
        return Random.Shared.Next(100, 1000);
    }
}
