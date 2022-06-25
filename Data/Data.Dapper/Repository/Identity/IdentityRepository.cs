using System.Data;
using Dapper;
using Dapper.Repository.Base;
using Data.Entity.Identity;


namespace Data.Dapper.Repository.Identity
{
    public class IdentityRepository : BaseRepository, IDataRepository<KU_KULLANICI>
    {
        public void Add(KU_KULLANICI entity)
        {
            string query =
                "INSERT INTO dbo.KU_KULLANICI (AD_SOYAD,E_MAIL,SIFRE,CREDATE) VALUES(@AD_SOYAD,@E_MAIL,@SIFRE,@CREDATE)";

            var lastId = _connection.ExecuteScalar<int>(query, entity);
            entity.ID_KULLANICI = lastId;
        }

        public void Delete(KU_KULLANICI entity)
        {
            throw new NotImplementedException();
        }

        public KU_KULLANICI Get(decimal id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KU_KULLANICI> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(KU_KULLANICI entity)
        {
            throw new NotImplementedException();
        }

        public KU_KULLANICI HaveAccountWithThatEmail(string e_mail)
        {
            using (IDbConnection dbConnection = _connection)
            {
                string query = @"SELECT * FROM KU_KULLANICI (NOLOCK) WHERE E_MAIL=@e_mail AND DELETED=0";

                return dbConnection.QueryFirstOrDefault<KU_KULLANICI>(query, new { @email = e_mail });
            }
        }

        public KU_KULLANICI UserLogin(string e_mail, string sifre)
        {
            using (IDbConnection dbConnection = _connection)
            {
                string query =
                    "SELECT * FROM KU_KULLANICI (NOLOCK) WHERE E_MAIL=@e_mail AND SIFRE=@sifre AND DELETED=0";

                return dbConnection.QueryFirstOrDefault<KU_KULLANICI>(query, new { @e_mail = e_mail, @sifre = sifre });
            }
        }
    }
}