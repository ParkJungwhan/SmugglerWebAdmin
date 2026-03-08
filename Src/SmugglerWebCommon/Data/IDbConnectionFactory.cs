using System.Data;

namespace SmugglerWebCommon.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
