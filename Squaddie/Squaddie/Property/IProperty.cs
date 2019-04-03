namespace Squaddie
{
    public interface IProperty
    {
        string Type { get; }
        string Name { get; }
        dynamic Value { get; set; }
    }
}
