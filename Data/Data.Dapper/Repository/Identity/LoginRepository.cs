using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Dapper.Repository.Base;
using Data.Entity.Identity;
using Microsoft.Data.SqlClient;

namespace Dapper.Repository.Identity
{
    public class LoginRepository : BaseRepository, IDataRepository<KU_KULLANICI>
    {
        public void Add(KU_KULLANICI entity)
        {
            string query = "INSERT INTO dbo.KK_KULLANICI (AD_SOYAD,E_MAIL,SIFRE,CREDATE) VALUES(@AD_SOYAD,@E_MAIL,@SIFRE,@CREDATE)";

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
                string query = @"SELECT * FROM KK_KULLANICI (NOLOCK) WHERE E_MAIL=@e_mail AND DELETED=0";

                return dbConnection.QueryFirstOrDefault<KU_KULLANICI>(query, new { @email = e_mail });
            }
        }
    }
}

