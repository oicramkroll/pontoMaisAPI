using System.Data;

namespace Services
{
    public interface IDatabaseConnection
    {
        IDbConnection CreateConnection();
    }
}
