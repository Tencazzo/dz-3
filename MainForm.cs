using dz3.Data;
using dz3.Logging;
using dz3.Services;
using System;
using System.Windows.Forms;

namespace dz3
{
    public partial class MainForm : Form
    {
        private readonly ILogger logger;
        private readonly TaskManagementService taskService;

        public MainForm(ILogger logger)
        {
            InitializeComponent();
            this.logger = logger;

            this.taskService = new TaskManagementService(
                new SqliteTaskRepository(@"Data Source=TaskManagement.db;", logger),
                logger);

            LoadTasks();
        }

        private void LoadTasks()
        {
            try
            {
                listTasks.Items.Clear();
                var tasks = taskService.GetAllTasks();
                foreach (var task in tasks)
                {
                    listTasks.Items.Add(task.Description);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error loading tasks", ex);
                MessageBox.Show("Error loading tasks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetupEventHandlers()
        {
            btnAdd.Click += BtnAdd_Click;
            btnDelete.Click += BtnDelete_Click;
            btnEdit.Click += BtnEdit_Click;
            btnSave.Click += BtnSave_Click;
            btnClear.Click += BtnClear_Click;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTaskInput.Text))
            {
                taskService.AddNewTask(txtTaskInput.Text);
                LoadTasks();
                txtTaskInput.Clear();
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (listTasks.SelectedIndex >= 0)
            {
                taskService.DeleteTask(listTasks.SelectedIndex + 1);
                LoadTasks();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (listTasks.SelectedIndex >= 0 && !string.IsNullOrWhiteSpace(txtTaskInput.Text))
            {
                taskService.UpdateTask(listTasks.SelectedIndex + 1, txtTaskInput.Text);
                LoadTasks();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tasks saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtTaskInput.Clear();
        }
    }
}