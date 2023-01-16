namespace ARStarter
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.LDDevicesList = new System.Windows.Forms.ListView();
            this.TextToFindBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.StopButton = new System.Windows.Forms.PictureBox();
            this.StopAllButton = new System.Windows.Forms.PictureBox();
            this.RefreshButton = new System.Windows.Forms.PictureBox();
            this.StartButton = new System.Windows.Forms.PictureBox();
            this.PauseButton = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.DeleteFromQueueButton = new System.Windows.Forms.PictureBox();
            this.DisableAllCheckboxesButton = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.KillADB = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopAllButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RefreshButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PauseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeleteFromQueueButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisableAllCheckboxesButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // LDDevicesList
            // 
            this.LDDevicesList.AutoArrange = false;
            this.LDDevicesList.FullRowSelect = true;
            this.LDDevicesList.HideSelection = false;
            this.LDDevicesList.Location = new System.Drawing.Point(13, 121);
            this.LDDevicesList.Name = "LDDevicesList";
            this.LDDevicesList.ShowGroups = false;
            this.LDDevicesList.Size = new System.Drawing.Size(648, 408);
            this.LDDevicesList.TabIndex = 2;
            this.LDDevicesList.UseCompatibleStateImageBehavior = false;
            this.LDDevicesList.View = System.Windows.Forms.View.Details;
            // 
            // TextToFindBox
            // 
            this.TextToFindBox.Location = new System.Drawing.Point(699, 22);
            this.TextToFindBox.Name = "TextToFindBox";
            this.TextToFindBox.Size = new System.Drawing.Size(182, 20);
            this.TextToFindBox.TabIndex = 6;
            this.TextToFindBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextToFindBox_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ARStarter.Properties.Resources.find;
            this.pictureBox1.InitialImage = global::ARStarter.Properties.Resources.find;
            this.pictureBox1.Location = new System.Drawing.Point(887, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.findItemInLDList_Click);
            // 
            // StopButton
            // 
            this.StopButton.Enabled = false;
            this.StopButton.Image = global::ARStarter.Properties.Resources.StopButton;
            this.StopButton.InitialImage = global::ARStarter.Properties.Resources.StopButton;
            this.StopButton.Location = new System.Drawing.Point(206, 66);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(53, 49);
            this.StopButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.StopButton.TabIndex = 14;
            this.StopButton.TabStop = false;
            this.StopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // StopAllButton
            // 
            this.StopAllButton.Image = global::ARStarter.Properties.Resources.StopAllButton;
            this.StopAllButton.InitialImage = global::ARStarter.Properties.Resources.StopAllButton;
            this.StopAllButton.Location = new System.Drawing.Point(72, 17);
            this.StopAllButton.Name = "StopAllButton";
            this.StopAllButton.Size = new System.Drawing.Size(53, 49);
            this.StopAllButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.StopAllButton.TabIndex = 13;
            this.StopAllButton.TabStop = false;
            this.StopAllButton.Click += new System.EventHandler(this.stopAllButton_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Image = global::ARStarter.Properties.Resources.RefreshButton;
            this.RefreshButton.InitialImage = global::ARStarter.Properties.Resources.RefreshButton;
            this.RefreshButton.Location = new System.Drawing.Point(628, 87);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(33, 28);
            this.RefreshButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RefreshButton.TabIndex = 12;
            this.RefreshButton.TabStop = false;
            this.RefreshButton.Click += new System.EventHandler(this.updateLDList_Click);
            // 
            // StartButton
            // 
            this.StartButton.Image = global::ARStarter.Properties.Resources.PlayButton;
            this.StartButton.InitialImage = global::ARStarter.Properties.Resources.PlayButton;
            this.StartButton.Location = new System.Drawing.Point(13, 17);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(53, 49);
            this.StartButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.StartButton.TabIndex = 11;
            this.StartButton.TabStop = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Enabled = false;
            this.PauseButton.Image = global::ARStarter.Properties.Resources.PauseButton;
            this.PauseButton.InitialImage = global::ARStarter.Properties.Resources.PauseButton;
            this.PauseButton.Location = new System.Drawing.Point(147, 66);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(53, 49);
            this.PauseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PauseButton.TabIndex = 10;
            this.PauseButton.TabStop = false;
            this.PauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ARStarter.Properties.Resources.Logo;
            this.pictureBox2.Location = new System.Drawing.Point(628, 305);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(332, 297);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 16;
            this.pictureBox2.TabStop = false;
            // 
            // DeleteFromQueueButton
            // 
            this.DeleteFromQueueButton.Enabled = false;
            this.DeleteFromQueueButton.Image = global::ARStarter.Properties.Resources.DeleteFromQueueButton;
            this.DeleteFromQueueButton.Location = new System.Drawing.Point(670, 120);
            this.DeleteFromQueueButton.Name = "DeleteFromQueueButton";
            this.DeleteFromQueueButton.Size = new System.Drawing.Size(34, 29);
            this.DeleteFromQueueButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DeleteFromQueueButton.TabIndex = 17;
            this.DeleteFromQueueButton.TabStop = false;
            this.DeleteFromQueueButton.Visible = false;
            this.DeleteFromQueueButton.Click += new System.EventHandler(this.deleteFromWorkListButton_Click);
            // 
            // DisableAllCheckboxesButton
            // 
            this.DisableAllCheckboxesButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DisableAllCheckboxesButton.Image = global::ARStarter.Properties.Resources.DisableAllCheckBoxes;
            this.DisableAllCheckboxesButton.Location = new System.Drawing.Point(13, 77);
            this.DisableAllCheckboxesButton.Name = "DisableAllCheckboxesButton";
            this.DisableAllCheckboxesButton.Size = new System.Drawing.Size(40, 38);
            this.DisableAllCheckboxesButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DisableAllCheckboxesButton.TabIndex = 18;
            this.DisableAllCheckboxesButton.TabStop = false;
            this.DisableAllCheckboxesButton.Click += new System.EventHandler(this.DisableAllCheckboxesButton_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::ARStarter.Properties.Resources.WindowToAddChecksOnLDDevices;
            this.pictureBox3.Location = new System.Drawing.Point(661, 17);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 36);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 19;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Print", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(296, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 112);
            this.label1.TabIndex = 20;
            this.label1.Text = "Starter";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(667, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = "text";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(707, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 17);
            this.label3.TabIndex = 22;
            this.label3.Text = "Добавлено аккаунтов:";
            this.label3.Visible = false;
            // 
            // KillADB
            // 
            this.KillADB.BackColor = System.Drawing.Color.Red;
            this.KillADB.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.KillADB.Location = new System.Drawing.Point(699, 273);
            this.KillADB.Name = "KillADB";
            this.KillADB.Size = new System.Drawing.Size(201, 69);
            this.KillADB.TabIndex = 23;
            this.KillADB.Text = "УБИТЬ ADB";
            this.KillADB.UseVisualStyleBackColor = false;
            this.KillADB.Click += new System.EventHandler(this.KillADB_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 541);
            this.Controls.Add(this.KillADB);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.DisableAllCheckboxesButton);
            this.Controls.Add(this.DeleteFromQueueButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StopAllButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.TextToFindBox);
            this.Controls.Add(this.LDDevicesList);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(947, 580);
            this.MinimumSize = new System.Drawing.Size(947, 580);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ARStarter";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextToFindBox_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopAllButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RefreshButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PauseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeleteFromQueueButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisableAllCheckboxesButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ListView LDDevicesList;
        private System.Windows.Forms.TextBox TextToFindBox;
        private System.Windows.Forms.PictureBox PauseButton;
        private System.Windows.Forms.PictureBox StartButton;
        private System.Windows.Forms.PictureBox RefreshButton;
        private System.Windows.Forms.PictureBox StopAllButton;
        private System.Windows.Forms.PictureBox StopButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox DeleteFromQueueButton;
        private System.Windows.Forms.PictureBox DisableAllCheckboxesButton;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button KillADB;
    }
}

