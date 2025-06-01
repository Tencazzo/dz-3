namespace dz3.Data.IMigration
{
    public interface IMigration
    {
        string Version { get; }
        string Description { get; }
        string Sql { get; }
    }
}