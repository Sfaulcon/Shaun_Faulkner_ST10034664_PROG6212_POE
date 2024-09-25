using Azure.Data.Tables;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Services
{
    public class TableStorageService
    {
        private readonly string _tableConnectionString;
        private readonly string _tableName;

        public TableStorageService(string tableConnectionString, string tableName)
        {
            _tableConnectionString = tableConnectionString;
            _tableName = tableName;
        }

        public async Task SaveClaimAsync(string name, string surname, string email, int hoursWorked, string documentUrl)
        {
            var client = new TableClient(_tableConnectionString, _tableName);
            await client.CreateIfNotExistsAsync();

            var claimEntity = new TableEntity(Guid.NewGuid().ToString(), email)
            {
                { "Name", name },
                { "Surname", surname},
                { "Email", email },
                { "HoursWorked", hoursWorked },
                { "DocumentUrl", documentUrl },
                { "Status", "Pending" }
            };

            await client.AddEntityAsync(claimEntity);
        }
    }
}
