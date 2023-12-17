using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace VetServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackupsController : ControllerBase
    {
        private readonly ILogger<BackupsController> _logger;
        private readonly string _connectionString;
        private readonly string _backupDirectory = "B:\\";

        public BackupsController(ILogger<BackupsController> logger)
        {
            _logger = logger;
            _connectionString = "Server=Anesia;Database=VetCare_db;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        private SqlConnection CreateAndOpenConnection()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        private string GetBackupFilePath(string databaseName)
        {
            return Path.Combine(_backupDirectory, $"{databaseName}_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");
        }

        [HttpPost("create-backup")]
        public IActionResult CreateBackup()
        {
            try
            {
                using (var sqlConnection = CreateAndOpenConnection())
                {
                    string databaseName = sqlConnection.Database;
                    string backupPath = GetBackupFilePath(databaseName);

                    using (var sqlCommand = new SqlCommand($"BACKUP DATABASE [{databaseName}] TO DISK='{backupPath}' WITH FORMAT", sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    return Ok($"Backup created successfully at {backupPath}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a backup");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get-backups")]
        public IActionResult GetBackups()
        {
            try
            {
                var backupFiles = Directory.GetFiles(_backupDirectory, "*.bak")
                    .Select(Path.GetFileName)
                    .ToList();

                return Ok(backupFiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching backup files");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("restore-backup/{backupFileName}")]
        public IActionResult RestoreBackup(string backupFileName)
        {
            try
            {
                using (var sqlConnection = CreateAndOpenConnection())
                {
                    string backupFilePath = Path.Combine(_backupDirectory, backupFileName);

                    if (!System.IO.File.Exists(backupFilePath))
                    {
                        return NotFound("Backup file not found");
                    }

                    string restoreQuery = $"USE master; RESTORE DATABASE VetCare_db FROM DISK = '{backupFilePath}' WITH REPLACE;";

                    using (SqlCommand sqlCommand = new SqlCommand(restoreQuery, sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    return Ok($"Database restored successfully from {backupFilePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while restoring the database");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
