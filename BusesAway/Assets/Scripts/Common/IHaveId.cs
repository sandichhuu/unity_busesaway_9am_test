using System.Linq;
using System.Runtime.CompilerServices;

public interface IHaveId
{
}

public static class IHaveIdExtension
{
    private static readonly ConditionalWeakTable<IHaveId, string> Storage = new();

    public static string GetUUID(this IHaveId haveId)
    {
        return Storage.GetValue(haveId, _ => IdGenerator.Generate().First());
    }
}