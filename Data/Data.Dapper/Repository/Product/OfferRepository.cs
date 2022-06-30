using System.Data;
using Dapper;
using Dapper.Repository.Base;
using Data.Entity.Product;

namespace Data.Dapper.Repository.Product;

public class OfferRepository : BaseRepository, IDataRepository<SP_TEKLIF>
{
    public IEnumerable<SP_TEKLIF> GetAll()
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"SELECT * FROM SP_TEKLIF (NOLOCK) ST WHERE ST.DELETED=0";
            return dbConnection.Query<SP_TEKLIF>(query);
        }
    }

    public SP_TEKLIF Get(decimal id)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"SELECT * FROM SP_TEKLIF (NOLOCK) ST WHERE ST.DELETED=0 AND ST.ID=@ID";
            return dbConnection.Query<SP_TEKLIF>(query, new { ID = id }).FirstOrDefault();
        }
    }

    public void Add(SP_TEKLIF entity)
    {
    }

    public void AddOffer(int idKullanici, int idUrun, int fiyat,int idUrunSahibi)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                @"INSERT INTO SP_TEKLIF (ID_URUN,TEKLIF_FIYATI,ID_TEKLIF_DURUM,ID_TEKLIF_VEREN)
                VALUES(@ID_URUN,@TEKLIF_FIYATI,@ID_TEKLIF_DURUM,@ID_TEKLIF_VEREN)";
            dbConnection.Execute(query,
                new { ID_URUN = idUrun, TEKLIF_FIYATI = fiyat, ID_TEKLIF_DURUM = 1, ID_TEKLIF_VEREN = idKullanici });
        }
    }

    public void Update(SP_TEKLIF entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(SP_TEKLIF entity)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"UPDATE SP_TEKLIF SET DELETED=1 WHERE ID=@ID";
            dbConnection.Execute(query, new { ID = entity.ID_TEKLIF });
        }
    }

    public void OrderOffer(SP_TEKLIF entity)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                @"INSERT INTO SP_TEKLIF (ID_URUN,TEKLIF_FIYATI,ID_TEKLIF_DURUM,ID_URUN_SAHIBI,ID_TEKLIF_VEREN,CREDATE)";
            dbConnection.Execute(query, entity);
        }
    }

    public IEnumerable<SP_TEKLIF> GetLastOfferByIdProduct(int idUrun)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"SELECT TOP 1 * FROM SP_TEKLIF (NOLOCK) ST WHERE ST.DELETED=0 AND ST.ID_URUN=@ID_URUN";
            return dbConnection.Query<SP_TEKLIF>(query, new { ID_URUN = idUrun });
        }
    }

    public IEnumerable<SP_TEKLIF> GetOfferOfUser(int idKullanici)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                @"SELECT * FROM SP_TEKLIF (NOLOCK) ST WHERE ST.DELETED=0 AND ST.ID_TEKLIF_VEREN=@ID_TEKLIF_VEREN";
            return dbConnection.Query<SP_TEKLIF>(query, new { ID_TEKLIF_VEREN = idKullanici });
        }
    }

    public void AcceptOffer(int idOffer, int idKullanici)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                @"UPDATE SP_TEKLIF SET ID_TEKLIF_DURUM=2,ID_TEKLIF_VEREN=@idkullanici WHERE ID_TEKLIF=@idOffer";
            dbConnection.Execute(query, new { idkullanici = idKullanici, idOffer = idOffer });
        }
    }

    public void BuyProduct(int idKullanici, int idUrun)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"UPDATE UR_URUN SET IS_SOLD=1,ID_KULLANICI=@idKullanici WHERE ID_URUN=@idUrun";
            dbConnection.Execute(query, new { idKullanici = idKullanici, idUrun = idUrun });
        }
    }

    public void RejectOffer(int idOffer)
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query =
                @"UPDATE SP_TEKLIF SET ID_TEKLIF_DURUM=3 WHERE ID_TEKLIF=@idOffer";
            dbConnection.Execute(query, new { idOffer = idOffer });
        }
    }

    public IEnumerable<SP_TEKLIF_DURUM> GetOfferStatus()
    {
        using (IDbConnection dbConnection = _connection)
        {
            string query = @"SELECT * FROM SP_TEKLIF_DURUM (NOLOCK)";
            return dbConnection.Query<SP_TEKLIF_DURUM>(query);
        }
    }
}