using System.Data;
using Dapper;
using Dapper.Repository.Base;
using Data.Entity.Product;

namespace Data.Dapper.Repository.Product;

public class ProductRepository : BaseRepository, IDataRepository<UR_URUN>
{
    public IEnumerable<UR_URUN> GetAll()
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                @"SELECT * FROM UR_URUN (NOLOCK) UU WHERE UU.DELETED = 0";
            return dbConnection.Query<UR_URUN>(query).ToList();
        }
    }

    public UR_URUN Get(decimal id)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"SELECT * FROM UR_URUN (NOLOCK) UU WHERE  UU.ID_URUN=@id AND UU.DELETED=0";
            return dbConnection.QueryFirstOrDefault<UR_URUN>(query, new { id });
        }
    }

    public void Add(UR_URUN entity)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                @"INSERT INTO UR_URUN (URUN_ADI,URUN_ACIKLAMA,ID_KATEGORI,ID_RENK,ID_MARKA,ID_KULLANIM_DURUMU,FIYAT,IS_OFFERABLE,IS_SOLD,CREDATE,ID_KULLANICI)
            VALUES(@URUN_ADI,@URUN_ACIKLAMA,@ID_KATEGORI,@ID_RENK,@ID_MARKA,@ID_KULLANIM_DURUMU,@FIYAT,@IS_OFFERABLE,@IS_SOLD,@CREDATE,@ID_KULLANICI)";
            dbConnection.Execute(query, entity);
        }
    }

    public void Update(UR_URUN entity)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                @"UPDATE UR_URUN SET URUN_ADI=@URUN_ADI,URUN_ACIKLAMA=@URUN_ACIKLAMA,ID_KATEGORI=@ID_KATEGORI,
                ID_KULLANIM_DURUMU=@ID_KULLANIM_DURUMU,ID_KULLANIM_DURUMU=@ID_KULLANIM_DURUMU,ID_MARKA=@ID_MARKA,ID_RENK=@ID_RENK,IS_SOLD=@IS_SOLD,IS_OFFERABLE=@IS_OFFERABLE
                WHERE ID_URUN=@ID_URUN";
            dbConnection.Execute(query, entity);
        }
    }

    public void Delete(UR_URUN entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ProductResponse> GetProductByCategoryName(string categoryName)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                string.Format(
                    @"SELECT UU.ID_URUN,UU.URUN_ADI,UU.URUN_ACIKLAMA,UU.FIYAT,UU.IS_SOLD,UU.IS_OFFERABLE,UK.KATEGORI_ADI,UR.RENK_ADI,UM.MARKA_ADI,UKD.KULLANIM_DURUM_ADI FROM UR_URUN (NOLOCK) UU
                    INNER JOIN UR_KATEGORI (NOLOCK) UK ON UK.ID_KATEGORI=UU.ID_KATEGORI
                    INNER JOIN UR_RENK (NOLOCK) UR ON UR.ID_RENK = UU.ID_RENK
                    INNER JOIN UR_MARKA (NOLOCK) UM ON UM.ID_MARKA=UU.ID_MARKA
                    INNER JOIN UR_KULLANIM_DURUM (NOLOCK) UKD ON UKD.ID_KULLANIM_DURUM = UU.ID_KULLANIM_DURUMU
                    WHERE UU.DELETED = 0 AND UK.KATEGORI_ADI=@categoryName");
            return dbConnection.Query<ProductResponse>(query, new { @categoryName = categoryName });
        }
    }

    public IEnumerable<ProductResponse> GetAllProducts()
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                @"SELECT UU.ID_URUN,UU.URUN_ADI,UU.URUN_ACIKLAMA,UU.FIYAT,UU.IS_SOLD,UU.IS_OFFERABLE,UK.KATEGORI_ADI,UR.RENK_ADI,UM.MARKA_ADI,UKD.KULLANIM_DURUM_ADI,UU.ID_KULLANICI AS ID_URUN_SAHIBI FROM UR_URUN (NOLOCK) UU
                    INNER JOIN UR_KATEGORI (NOLOCK) UK ON UK.ID_KATEGORI=UU.ID_KATEGORI
                    INNER JOIN UR_RENK (NOLOCK) UR ON UR.ID_RENK = UU.ID_RENK
                    INNER JOIN UR_MARKA (NOLOCK) UM ON UM.ID_MARKA=UU.ID_MARKA
                    INNER JOIN UR_KULLANIM_DURUM (NOLOCK) UKD ON UKD.ID_KULLANIM_DURUM = UU.ID_KULLANIM_DURUMU
                    WHERE UU.DELETED = 0 AND UU.IS_SOLD=0
                    ORDER BY UU.ID_URUN DESC";
            return dbConnection.Query<ProductResponse>(query);
        }
    }

    public IEnumerable<UR_RENK> GetAllColors()
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"SELECT * FROM UR_RENK (NOLOCK) UR WHERE UR.DELETED = 0";
            return dbConnection.Query<UR_RENK>(query).ToList();
        }
    }

    public IEnumerable<UR_MARKA> GetAllBrands()
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"SELECT * FROM UR_MARKA (NOLOCK) UM WHERE UM.DELETED = 0";
            return dbConnection.Query<UR_MARKA>(query).ToList();
        }
    }

    public IEnumerable<UR_KULLANIM_DURUM> GetAllUsageStatuses()
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"SELECT * FROM UR_KULLANIM_DURUM (NOLOCK) UKD WHERE UKD.DELETED = 0";
            return dbConnection.Query<UR_KULLANIM_DURUM>(query).ToList();
        }
    }

    public IEnumerable<ProductResponse> GetUserProduct(int idKullanici)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                @"SELECT UU.ID_URUN,UU.URUN_ADI,UU.URUN_ACIKLAMA,UU.FIYAT,UU.IS_SOLD,UU.IS_OFFERABLE,UK.KATEGORI_ADI,UR.RENK_ADI,UM.MARKA_ADI,UKD.KULLANIM_DURUM_ADI FROM UR_URUN (NOLOCK) UU
                    INNER JOIN UR_KATEGORI (NOLOCK) UK ON UK.ID_KATEGORI=UU.ID_KATEGORI
                    INNER JOIN UR_RENK (NOLOCK) UR ON UR.ID_RENK = UU.ID_RENK
                    INNER JOIN UR_MARKA (NOLOCK) UM ON UM.ID_MARKA=UU.ID_MARKA
                    INNER JOIN UR_KULLANIM_DURUM (NOLOCK) UKD ON UKD.ID_KULLANIM_DURUM = UU.ID_KULLANIM_DURUMU
                    WHERE UU.DELETED = 0 AND UU.ID_KULLANICI=@idKullanici";
            return dbConnection.Query<ProductResponse>(query, new { @idKullanici = idKullanici }).ToList();
        }
    }
}