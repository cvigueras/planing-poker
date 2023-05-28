namespace webapi.Helpers;

public class GuidGenerator : IGuidGenerator
{
    public Guid Generate()
    {
        return Guid.NewGuid();
    }
}