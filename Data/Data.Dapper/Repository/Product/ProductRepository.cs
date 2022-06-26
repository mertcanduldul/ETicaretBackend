using System.Data;
using Dapper;
using Dapper.Repository.Base;
using Data.Entity.Product;

namespace Data.Dapper.Repository.Product;

public class ProductRepository : BaseRepository, IDataRepository<UR_KATEGORI>
{
    public IEnumerable<UR_KATEGORI> GetAll()
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"SELECT * FROM UR_KATEGORI NOLOCK";

            return dbConnection.Query<UR_KATEGORI>(query).ToList();
        }
    }

    public UR_KATEGORI Get(decimal id)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"SELECT * FROM UR_KATEGORI NOLOCK WHERE ID_KATEGORI = @id";
            return dbConnection.QueryFirstOrDefault<UR_KATEGORI>(query, new { id });
        }
    }

    public void Add(UR_KATEGORI entity)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"INSERT INTO UR_KATEGORI (KATEGORI_ADI,CREDATE) VALUES (@KATEGORI_ADI,@CREDATE)";
            dbConnection.Execute(query, entity);
        }
    }

    public void Update(UR_KATEGORI entity)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"UPDATE UR_KATEGORI SET KATEGORI_ADI=@KATEGORI_ADI WHERE ID_KATEGORI=@ID_KATEGORI";
            dbConnection.Execute(query, entity);
        }
    }

    public void Delete(UR_KATEGORI entity)
    {
        throw new NotImplementedException();
    }

    public UR_KATEGORI GetCategoryByCategoryName(string categoryName)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                string.Format(@"SELECT * FROM UR_KATEGORI NOLOCK WHERE KATEGORI_ADI=@categoryName");
            return dbConnection.QueryFirstOrDefault<UR_KATEGORI>(query, new { @categoryName = categoryName });
        }
    }
}