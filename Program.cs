using System;
using System.Windows.Forms;
using dz3.Data.IMigration;
using dz3.Logging;
using SQLitePCL;

namespace dz3
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Batteries.Init();
            ILogger logger = null;

            try
            {
                logger = new FileLogger();
                logger.LogInfo("=== Starting dz3 application ===");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var migrationManager = new MigrationManager(@"Data Source=TaskManagement.db;", logger);
                migrationManager.ApplyMigrations();

                Application.Run(new MainForm(logger));
            }
            catch (Exception ex)
            {
                logger?.LogError("Critical application error", ex);
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}