namespace MusicVideoStoreCourseProject
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage reportTabPage;
        private System.Windows.Forms.SplitContainer reportSplitContainer;
        private System.Windows.Forms.Panel reportPanel;
        private System.Windows.Forms.Label roleLabel;
        private System.Windows.Forms.Button catalogButton;
        private System.Windows.Forms.Button groupButton;
        private System.Windows.Forms.Label havingLabel;
        private System.Windows.Forms.TextBox havingCountField;
        private System.Windows.Forms.Button havingButton;
        private System.Windows.Forms.Button profitButton;
        private System.Windows.Forms.Button ordersButton;
        private System.Windows.Forms.Button excelButton;
        private System.Windows.Forms.Button wordButton;
        private System.Windows.Forms.DataGridView reportGrid;

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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.reportTabPage = new System.Windows.Forms.TabPage();
            this.reportSplitContainer = new System.Windows.Forms.SplitContainer();
            this.reportPanel = new System.Windows.Forms.Panel();
            this.roleLabel = new System.Windows.Forms.Label();
            this.catalogButton = new System.Windows.Forms.Button();
            this.groupButton = new System.Windows.Forms.Button();
            this.havingLabel = new System.Windows.Forms.Label();
            this.havingCountField = new System.Windows.Forms.TextBox();
            this.havingButton = new System.Windows.Forms.Button();
            this.profitButton = new System.Windows.Forms.Button();
            this.ordersButton = new System.Windows.Forms.Button();
            this.excelButton = new System.Windows.Forms.Button();
            this.wordButton = new System.Windows.Forms.Button();
            this.reportGrid = new System.Windows.Forms.DataGridView();
            this.reportTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reportSplitContainer)).BeginInit();
            this.reportSplitContainer.Panel1.SuspendLayout();
            this.reportSplitContainer.Panel2.SuspendLayout();
            this.reportSplitContainer.SuspendLayout();
            this.reportPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reportGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1164, 661);
            this.tabControl.TabIndex = 0;
            // 
            // reportTabPage
            // 
            this.reportTabPage.Controls.Add(this.reportSplitContainer);
            this.reportTabPage.Location = new System.Drawing.Point(4, 24);
            this.reportTabPage.Name = "reportTabPage";
            this.reportTabPage.Padding = new System.Windows.Forms.Padding(0);
            this.reportTabPage.Size = new System.Drawing.Size(1156, 633);
            this.reportTabPage.TabIndex = 0;
            this.reportTabPage.Text = "Отчеты";
            this.reportTabPage.UseVisualStyleBackColor = true;
            // 
            // reportSplitContainer
            // 
            this.reportSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.reportSplitContainer.IsSplitterFixed = true;
            this.reportSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.reportSplitContainer.Name = "reportSplitContainer";
            // 
            // reportSplitContainer.Panel1
            // 
            this.reportSplitContainer.Panel1.Controls.Add(this.reportPanel);
            this.reportSplitContainer.Panel1MinSize = 230;
            // 
            // reportSplitContainer.Panel2
            // 
            this.reportSplitContainer.Panel2.Controls.Add(this.reportGrid);
            this.reportSplitContainer.Panel2MinSize = 400;
            this.reportSplitContainer.Size = new System.Drawing.Size(1156, 633);
            this.reportSplitContainer.SplitterDistance = 230;
            this.reportSplitContainer.SplitterWidth = 3;
            this.reportSplitContainer.TabIndex = 0;
            // 
            // reportPanel
            // 
            this.reportPanel.AutoScroll = true;
            this.reportPanel.Controls.Add(this.roleLabel);
            this.reportPanel.Controls.Add(this.catalogButton);
            this.reportPanel.Controls.Add(this.groupButton);
            this.reportPanel.Controls.Add(this.havingLabel);
            this.reportPanel.Controls.Add(this.havingCountField);
            this.reportPanel.Controls.Add(this.havingButton);
            this.reportPanel.Controls.Add(this.profitButton);
            this.reportPanel.Controls.Add(this.ordersButton);
            this.reportPanel.Controls.Add(this.excelButton);
            this.reportPanel.Controls.Add(this.wordButton);
            this.reportPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportPanel.Location = new System.Drawing.Point(0, 0);
            this.reportPanel.Name = "reportPanel";
            this.reportPanel.Size = new System.Drawing.Size(230, 633);
            this.reportPanel.TabIndex = 0;
            // 
            // roleLabel
            // 
            this.roleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.roleLabel.Location = new System.Drawing.Point(10, 8);
            this.roleLabel.Name = "roleLabel";
            this.roleLabel.Size = new System.Drawing.Size(190, 20);
            this.roleLabel.TabIndex = 0;
            this.roleLabel.Text = "Роль:";
            // 
            // catalogButton
            // 
            this.catalogButton.Location = new System.Drawing.Point(10, 35);
            this.catalogButton.Name = "catalogButton";
            this.catalogButton.Size = new System.Drawing.Size(185, 30);
            this.catalogButton.TabIndex = 1;
            this.catalogButton.Text = "Каталог с JOIN";
            this.catalogButton.UseVisualStyleBackColor = true;
            this.catalogButton.Click += new System.EventHandler(this.CatalogButton_Click);
            // 
            // groupButton
            // 
            this.groupButton.Location = new System.Drawing.Point(10, 75);
            this.groupButton.Name = "groupButton";
            this.groupButton.Size = new System.Drawing.Size(185, 30);
            this.groupButton.TabIndex = 2;
            this.groupButton.Text = "GROUP BY";
            this.groupButton.UseVisualStyleBackColor = true;
            this.groupButton.Click += new System.EventHandler(this.GroupButton_Click);
            // 
            // havingLabel
            // 
            this.havingLabel.Location = new System.Drawing.Point(10, 115);
            this.havingLabel.Name = "havingLabel";
            this.havingLabel.Size = new System.Drawing.Size(120, 20);
            this.havingLabel.TabIndex = 3;
            this.havingLabel.Text = "Мин. поджанров";
            // 
            // havingCountField
            // 
            this.havingCountField.Location = new System.Drawing.Point(135, 112);
            this.havingCountField.Name = "havingCountField";
            this.havingCountField.Size = new System.Drawing.Size(60, 23);
            this.havingCountField.TabIndex = 4;
            this.havingCountField.Text = "2";
            // 
            // havingButton
            // 
            this.havingButton.Location = new System.Drawing.Point(10, 155);
            this.havingButton.Name = "havingButton";
            this.havingButton.Size = new System.Drawing.Size(185, 30);
            this.havingButton.TabIndex = 5;
            this.havingButton.Text = "HAVING";
            this.havingButton.UseVisualStyleBackColor = true;
            this.havingButton.Click += new System.EventHandler(this.HavingButton_Click);
            // 
            // profitButton
            // 
            this.profitButton.Location = new System.Drawing.Point(10, 195);
            this.profitButton.Name = "profitButton";
            this.profitButton.Size = new System.Drawing.Size(185, 30);
            this.profitButton.TabIndex = 6;
            this.profitButton.Text = "Отчет по ценам";
            this.profitButton.UseVisualStyleBackColor = true;
            this.profitButton.Click += new System.EventHandler(this.ProfitButton_Click);
            // 
            // ordersButton
            // 
            this.ordersButton.Location = new System.Drawing.Point(10, 235);
            this.ordersButton.Name = "ordersButton";
            this.ordersButton.Size = new System.Drawing.Size(185, 30);
            this.ordersButton.TabIndex = 7;
            this.ordersButton.Text = "Сводка магазинов";
            this.ordersButton.UseVisualStyleBackColor = true;
            this.ordersButton.Click += new System.EventHandler(this.OrdersButton_Click);
            // 
            // excelButton
            // 
            this.excelButton.Location = new System.Drawing.Point(10, 295);
            this.excelButton.Name = "excelButton";
            this.excelButton.Size = new System.Drawing.Size(185, 30);
            this.excelButton.TabIndex = 8;
            this.excelButton.Text = "Экспорт в Excel";
            this.excelButton.UseVisualStyleBackColor = true;
            this.excelButton.Click += new System.EventHandler(this.ExcelButton_Click);
            // 
            // wordButton
            // 
            this.wordButton.Location = new System.Drawing.Point(10, 335);
            this.wordButton.Name = "wordButton";
            this.wordButton.Size = new System.Drawing.Size(185, 30);
            this.wordButton.TabIndex = 9;
            this.wordButton.Text = "Экспорт в Word";
            this.wordButton.UseVisualStyleBackColor = true;
            this.wordButton.Click += new System.EventHandler(this.WordButton_Click);
            // 
            // reportGrid
            // 
            this.reportGrid.AllowUserToAddRows = false;
            this.reportGrid.AllowUserToDeleteRows = false;
            this.reportGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.reportGrid.BackgroundColor = System.Drawing.Color.White;
            this.reportGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.reportGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportGrid.Location = new System.Drawing.Point(0, 0);
            this.reportGrid.MultiSelect = false;
            this.reportGrid.Name = "reportGrid";
            this.reportGrid.ReadOnly = true;
            this.reportGrid.RowHeadersWidth = 28;
            this.reportGrid.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.reportGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.reportGrid.Size = new System.Drawing.Size(923, 633);
            this.reportGrid.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 661);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Информационная система МУЗЫКАЛЬНЫЙ (ВИДЕО-) МАГАЗИН";
            this.reportTabPage.ResumeLayout(false);
            this.reportSplitContainer.Panel1.ResumeLayout(false);
            this.reportSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.reportSplitContainer)).EndInit();
            this.reportSplitContainer.ResumeLayout(false);
            this.reportPanel.ResumeLayout(false);
            this.reportPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reportGrid)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
