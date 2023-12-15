using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.IO;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;
using Microsoft.AspNetCore.Authorization;

namespace VetServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BackupsController : ControllerBase
{
    private readonly ILogger<BackupsController> _logger;
    private readonly string _connectionString;

    public BackupsController(ILogger<BackupsController> logger)
    {
        _logger = logger;
        _connectionString = "Server=Anesia;Database=VetCare_db;Trusted_Connection=True;TrustServerCertificate=True;";
    }

    // POST: /api/Backup/create-backup
    [HttpPost("create-backup")]
    public IActionResult CreateBackup()
    {
        try
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string databaseName = sqlConnection.Database;

                //string customBackupDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
                //string backupPath = Path.Combine(customBackupDirectory, $"{databaseName}_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

                string customBackupDirectory = "B:\\";
                string backupPath = Path.Combine(customBackupDirectory, $"{databaseName}_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

                //string backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{databaseName}_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

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

    // GET: /api/Backup/get-backups
    [HttpGet("get-backups")]
    public IActionResult GetBackups()
    {
        try
        {
            // Находим все файлы бекапов в директории
            var backupFiles = Directory.GetFiles("B:\\", "*.bak")
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

    // POST: /api/Backup/restore-backup
    [HttpPost("restore-backup/{backupFileName}")]
    public IActionResult RestoreBackup(string backupFileName)
    {
        try
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                // Полный путь к выбранному файлу бекапа
                string backupFilePath = Path.Combine("B:\\", backupFileName);

                if (!System.IO.File.Exists(backupFilePath))
                {
                    return NotFound("Backup file not found");
                }

                // Construct the SQL query for restoration
                string restoreQuery = $"USE master; RESTORE DATABASE VetCare_db FROM DISK = '{backupFilePath}' WITH REPLACE;";

                // Execute the restore query
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
