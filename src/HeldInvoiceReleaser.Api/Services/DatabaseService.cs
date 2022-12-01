using System.Data;
using CSharpFunctionalExtensions;
using Dapper;
using HeldInvoiceReleaser.Api.Models.Commands;
using HeldInvoiceReleaser.Models;
using Microsoft.Data.SqlClient;

namespace HeldInvoiceReleaser.Api.Services
{
    public interface IDatabaseService
    {
        Task<Result<IEnumerable<HeldInvoice>>> GetAllHeldInvoicesByLocation(string location);
        Task<Result> ReleaseHeldInvoicesByLocationAndPicketTicket(ReleaseHeldInvoiceCommand command);
    }

    public class SqlServerDatabaseService : IDatabaseService
    {
        public static readonly string ConnectionStringName = "SqlServer";
        private const string GetInvoices = "GetHeldInvoicesByLocation";
        private const string ReleaseInvoice = "ReleaseHeldInvoiceByPickTicket";
        private readonly IConfiguration _configuration;

        public SqlServerDatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Result<IEnumerable<HeldInvoice>>> GetAllHeldInvoicesByLocation(string location)
        {
            Result<IEnumerable<HeldInvoice>> output;
            try
            {
                await using var connection = new SqlConnection(GetConnectionString());
                connection.Open();

                IEnumerable<HeldInvoice> invoices = await connection.QueryAsync<HeldInvoice>(GetInvoices,
                    new { LocationId = location },
                    commandType: CommandType.StoredProcedure);

                output = Result.Success(invoices);
            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<HeldInvoice>>(ex.Message);
            }

            return output;
        }

        public async Task<Result> ReleaseHeldInvoicesByLocationAndPicketTicket(ReleaseHeldInvoiceCommand command)
        {
            Result output;
            try
            {
                await using var connection = new SqlConnection(GetConnectionString());
                connection.Open();

                await connection.ExecuteAsync(ReleaseInvoice,
                    new { PickTicket = command.PickTicket, LocationId = command.LocationId },
                    commandType: CommandType.StoredProcedure);

                output = Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            return output;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString(ConnectionStringName);
        }
    }
}