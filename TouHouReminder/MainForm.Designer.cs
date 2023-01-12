namespace TouHouReminder
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_ShowTimetable = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_AutoRun = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_AutoExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeView = new System.Windows.Forms.TreeView();
            this.ContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.ContextMenuStrip;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "TouHou Reminder";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseClick);
            // 
            // ContextMenuStrip
            // 
            this.ContextMenuStrip.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_ShowTimetable,
            this.ToolStripMenuItem_AutoRun,
            this.ToolStripMenuItem_AutoExit,
            this.ToolStripMenuItem_Exit});
            this.ContextMenuStrip.Name = "ContextMenuStrip";
            this.ContextMenuStrip.Size = new System.Drawing.Size(231, 114);
            // 
            // ToolStripMenuItem_ShowTimetable
            // 
            this.ToolStripMenuItem_ShowTimetable.Name = "ToolStripMenuItem_ShowTimetable";
            this.ToolStripMenuItem_ShowTimetable.Size = new System.Drawing.Size(230, 22);
            this.ToolStripMenuItem_ShowTimetable.Text = "Show the timetable";
            this.ToolStripMenuItem_ShowTimetable.Click += new System.EventHandler(this.ToolStripMenuItem_ShowTimetable_Click);
            // 
            // ToolStripMenuItem_AutoRun
            // 
            this.ToolStripMenuItem_AutoRun.Name = "ToolStripMenuItem_AutoRun";
            this.ToolStripMenuItem_AutoRun.Size = new System.Drawing.Size(230, 22);
            this.ToolStripMenuItem_AutoRun.Text = "Start automatically at boot";
            this.ToolStripMenuItem_AutoRun.Click += new System.EventHandler(this.ToolStripMenuItem_AutoRun_Click);
            // 
            // ToolStripMenuItem_AutoExit
            // 
            this.ToolStripMenuItem_AutoExit.Name = "ToolStripMenuItem_AutoExit";
            this.ToolStripMenuItem_AutoExit.Size = new System.Drawing.Size(230, 22);
            this.ToolStripMenuItem_AutoExit.Text = "Exit automatically at boot";
            this.ToolStripMenuItem_AutoExit.Click += new System.EventHandler(this.ToolStripMenuItem_AutoExit_Click);
            // 
            // ToolStripMenuItem_Exit
            // 
            this.ToolStripMenuItem_Exit.Name = "ToolStripMenuItem_Exit";
            this.ToolStripMenuItem_Exit.Size = new System.Drawing.Size(230, 22);
            this.ToolStripMenuItem_Exit.Text = "Exit";
            this.ToolStripMenuItem_Exit.Click += new System.EventHandler(this.ToolStripMenuItem_Exit_Click);
            // 
            // TreeView
            // 
            this.TreeView.Location = new System.Drawing.Point(12, 12);
            this.TreeView.Name = "TreeView";
            this.TreeView.Size = new System.Drawing.Size(560, 387);
            this.TreeView.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.TreeView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "TouHou Reminder";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NotifyIcon NotifyIcon;
        private new ContextMenuStrip ContextMenuStrip;
        private ToolStripMenuItem ToolStripMenuItem_ShowTimetable;
        private ToolStripMenuItem ToolStripMenuItem_AutoRun;
        private ToolStripMenuItem ToolStripMenuItem_AutoExit;
        private ToolStripMenuItem ToolStripMenuItem_Exit;
        private TreeView TreeView;
    }
}