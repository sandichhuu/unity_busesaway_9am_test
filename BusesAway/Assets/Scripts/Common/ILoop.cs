public interface ILoop : IHaveId
{
    void Invoke(float dt);
}

public interface IFixedLoop : IHaveId
{
    void Invoke(float fdt);
}