namespace dz3
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtTaskInput = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.listTasks = new System.Windows.Forms.ListView();
            this.columnId = new System.Windows.Forms.ColumnHeader();
            this.columnDescription = new System.Windows.Forms.ColumnHeader();
            this.columnCreated = new System.Windows.Forms.ColumnHeader();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(155, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Управление задачами";

            // txtTaskInput
            this.txtTaskInput.Location = new System.Drawing.Point(12, 50);
            this.txtTaskInput.Name = "txtTaskInput";
            this.txtTaskInput.Size = new System.Drawing.Size(400, 20);
            this.txtTaskInput.TabIndex = 1;
            this.txtTaskInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTaskInput_KeyDown);

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(418, 48);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // listTasks
            this.listTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.columnId,
                this.columnDescription,
                this.columnCreated});
            this.listTasks.FullRowSelect = true;
            this.listTasks.GridLines = true;
            this.listTasks.Location = new System.Drawing.Point(12, 85);
            this.listTasks.MultiSelect = false;
            this.listTasks.Name = "listTasks";
            this.listTasks.Size = new System.Drawing.Size(560, 300);
            this.listTasks.TabIndex = 3;
            this.listTasks.UseCompatibleStateImageBehavior = false;
            this.listTasks.View = System.Windows.Forms.View.Details;

            // columnId
            this.columnId.Text = "ID";
            this.columnId.Width = 50;

            // columnDescription
            this.columnDescription.Text = "Описание";
            this.columnDescription.Width = 350;

            // columnCreated
            this.columnCreated.Text = "Создано";
            this.columnCreated.Width = 150;

            // btnEdit
            this.btnEdit.Location = new System.Drawing.Point(12, 400);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Изменить";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(100, 400);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.listTasks);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtTaskInput);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Управление задачами";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtTaskInput;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView listTasks;
        private System.Windows.Forms.ColumnHeader columnId;
        private System.Windows.Forms.ColumnHeader columnDescription;
        private System.Windows.Forms.ColumnHeader columnCreated;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblTitle;
    }
}