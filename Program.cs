using System;
using System.IO;
using System.Windows.Forms;
using dz3.Container;
using dz3.Data;
using dz3.Logging;
using dz3.Services;

namespace dz3
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                var container = new SimpleContainer();

                var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dz3");
                Directory.CreateDirectory(appDataPath);

                var dbPath = Path.Combine(appDataPath, "tasks.db");
                var connectionString = $"Data Source={dbPath};Version=3;";

                var logger = new FileLogger();
                var taskRepository = new SqliteTaskRepository(connectionString, logger);

                // Инициализируем базу синхронно при запуске
                taskRepository.InitializeDatabaseAsync().GetAwaiter().GetResult();

                var taskService = new TaskService(taskRepository, logger);

                container.Register<ILogger>(logger);
                container.Register<ITaskRepository>(taskRepository);
                container.Register<TaskService>(taskService);

                var mainForm = new MainForm(taskService);
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка запуска: {ex.Message}\n\nПроверьте установку System.Data.SQLite",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}