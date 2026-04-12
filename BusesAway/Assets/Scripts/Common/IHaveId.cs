using System.Linq;

public interface IHaveId
{
    public string id => IdGenerator.Generate().Take(1).First();
}