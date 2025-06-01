using System;
using System.Windows.Forms;
using dz3.Models;
using dz3.Services;

namespace dz3
{
    public partial class MainForm : Form
    {
        private readonly TaskService taskService;

        public MainForm(TaskService taskService)
        {
            InitializeComponent();
            this.taskService = taskService;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadTasks();
        }

        private void LoadTasks()
        {
            try
            {
                var tasks = taskService.GetAllTasks();

                listTasks.Items.Clear();

                foreach (var task in tasks)
                {
                    var item = new ListViewItem(task.Id.ToString());
                    item.SubItems.Add(task.Description);
                    item.SubItems.Add(task.CreatedAt.ToString("dd.MM.yyyy HH:mm"));
                    item.Tag = task;
                    listTasks.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки задач: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddTask();
        }

        private void txtTaskInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddTask();
            }
        }

        private void AddTask()
        {
            var description = txtTaskInput.Text.Trim();

            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Введите описание задачи", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var success = taskService.AddTask(description);

                if (success)
                {
                    txtTaskInput.Clear();
                    LoadTasks();
                }
                else
                {
                    MessageBox.Show("Не удалось добавить задачу", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления задачи: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listTasks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите задачу для изменения", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = listTasks.SelectedItems[0];
            var task = (TaskEntity)selectedItem.Tag;

            var inputForm = new InputForm("Изменить задачу", "Новое описание:", task.Description);

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var success = taskService.UpdateTask(task.Id, inputForm.InputText);

                    if (success)
                    {
                        LoadTasks();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось изменить задачу", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка изменения задачи: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listTasks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите задачу для удаления", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = listTasks.SelectedItems[0];
            var task = (TaskEntity)selectedItem.Tag;

            var result = MessageBox.Show($"Удалить задачу '{task.Description}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var success = taskService.DeleteTask(task.Id);

                    if (success)
                    {
                        LoadTasks();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить задачу", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления задачи: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}