using System.Data.SqlClient;

namespace TalentAtmDAL
{
    public class TalentAtmDBContext : IDisposable
    {


        private readonly string _connString;

        private bool _disposed;

        private SqlConnection _dbConnection = null;






        public TalentAtmDBContext() : this(@"Data Source= ;Initial Catalog=PTalentAtmDB;Integrated Security=True; MultipleActiveResultSets=true; Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {

        }

        public TalentAtmDBContext(string connString)
        {
            _connString = connString;

        }


        public async Task<SqlConnection> OpenConnection()
        {
            _dbConnection = new SqlConnection(_connString);

            await _dbConnection.OpenAsync();


            return _dbConnection;
        }



        protected virtual void Dispose(bool disposing)
        {

            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbConnection.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
