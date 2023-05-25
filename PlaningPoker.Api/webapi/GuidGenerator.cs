namespace PlaningPoker.Api.Test;

public class GuidGenerator : IGuidGenerator
{
    public Guid Generate()
    {
        return Guid.NewGuid();
    }
}