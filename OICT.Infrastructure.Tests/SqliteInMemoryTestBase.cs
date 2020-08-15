using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OICT.Infrastructure.Common;
using System.Data.Common;
using System.Threading.Tasks;

namespace OICT.Infrastructure.Tests
{
    /// <summary>
    /// EF Core in-memory provider often behaves differently than classic SQL databases
    /// https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory
    /// Sqlite in memory is used for testing to more closely match common relational database behaviors. 
    /// </summary>
    public abstract class SqliteInMemoryTestBase
    {
        private readonly string _connectionString = new SqliteConnectionStringBuilder { DataSource = ":memory:" }.ToString();
        private SqliteConnection _dbConnection;
        protected IUnitOfWork UnitOfWork { get; private set; }

        [SetUp]
        public void Setup()
        {
            _dbConnection = new SqliteConnection(_connectionString);
            RecreateUnitOfWork();
            UnitOfWork.Context.Database.OpenConnection();
            UnitOfWork.Context.Database.EnsureCreated();
        }

        private void RecreateUnitOfWork()
        {
            UnitOfWork = new UnitOfWork(new OICTApiContext(GetDatabaseOptions(_dbConnection)));
        }

        protected async Task SaveAndRecreateUnitOfWork()
        {
            await UnitOfWork.CommitAsync();
            DisposeUnitOfWork();
            RecreateUnitOfWork();
        }

        private void DisposeUnitOfWork()
        {
            UnitOfWork?.Dispose();
        }

        [TearDown]
        public void Teardown()
        {
            DisposeUnitOfWork();
            _dbConnection?.Dispose();
            _dbConnection = null;
        }

        private static DbContextOptions<OICTApiContext> GetDatabaseOptions(DbConnection connection)
        {
            return new DbContextOptionsBuilder<OICTApiContext>()
                .UseSqlite(connection)
                .Options;
        }
    }
}
