namespace MidiUWPRouter
{
    partial class frmAddFiltersToAlias
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvDeviceList = new System.Windows.Forms.DataGridView();
            this.in_out = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.device_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cooked_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnuDevices = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuExportSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExportAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGetInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopySelectionToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTitel = new System.Windows.Forms.Label();
            this.saveFileCSVDialog = new System.Windows.Forms.SaveFileDialog();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictClose = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeviceList)).BeginInit();
            this.cmnuDevices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDeviceList
            // 
            this.dgvDeviceList.AccessibleName = "List with MIDI devices";
            this.dgvDeviceList.AllowUserToAddRows = false;
            this.dgvDeviceList.AllowUserToDeleteRows = false;
            this.dgvDeviceList.AllowUserToResizeRows = false;
            this.dgvDeviceList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDeviceList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDeviceList.BackgroundColor = System.Drawing.Color.White;
            this.dgvDeviceList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDeviceList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDeviceList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDeviceList.ColumnHeadersHeight = 25;
            this.dgvDeviceList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDeviceList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.in_out,
            this.device_name,
            this.cooked_name,
            this.id});
            this.dgvDeviceList.ContextMenuStrip = this.cmnuDevices;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(199)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDeviceList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDeviceList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDeviceList.EnableHeadersVisualStyles = false;
            this.dgvDeviceList.GridColor = System.Drawing.Color.Silver;
            this.dgvDeviceList.Location = new System.Drawing.Point(13, 74);
            this.dgvDeviceList.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDeviceList.Name = "dgvDeviceList";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDeviceList.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvDeviceList.RowHeadersVisible = false;
            this.dgvDeviceList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDeviceList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeviceList.Size = new System.Drawing.Size(425, 191);
            this.dgvDeviceList.TabIndex = 0;
            // 
            // in_out
            // 
            this.in_out.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.in_out.HeaderText = "In";
            this.in_out.Name = "in_out";
            this.in_out.ReadOnly = true;
            this.in_out.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.in_out.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.in_out.ToolTipText = "MIDI in or MIDI out";
            this.in_out.Width = 50;
            // 
            // device_name
            // 
            this.device_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.NullValue = null;
            this.device_name.DefaultCellStyle = dataGridViewCellStyle3;
            this.device_name.FillWeight = 20F;
            this.device_name.HeaderText = "MIDI Device Name";
            this.device_name.Name = "device_name";
            this.device_name.ReadOnly = true;
            this.device_name.ToolTipText = "Name of the MIDI Device";
            // 
            // cooked_name
            // 
            this.cooked_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cooked_name.FillWeight = 20F;
            this.cooked_name.HeaderText = "Cooked Name";
            this.cooked_name.Name = "cooked_name";
            this.cooked_name.ReadOnly = true;
            this.cooked_name.ToolTipText = "Cooked Name of the MIDI Device";
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.id.FillWeight = 40F;
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.ToolTipText = "PNP Device ID";
            // 
            // cmnuDevices
            // 
            this.cmnuDevices.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExportSelection,
            this.mnuExportAll,
            this.mnuGetInfo,
            this.mnuCopySelectionToClipboard});
            this.cmnuDevices.Name = "contextMenuStrip1";
            this.cmnuDevices.Size = new System.Drawing.Size(264, 92);
            this.cmnuDevices.Text = "Device Menu";
            this.cmnuDevices.Opening += new System.ComponentModel.CancelEventHandler(this.CmnuDevices_Opening);
            // 
            // mnuExportSelection
            // 
            this.mnuExportSelection.Name = "mnuExportSelection";
            this.mnuExportSelection.Size = new System.Drawing.Size(263, 22);
            this.mnuExportSelection.Text = "Export selection to CSV...";
            this.mnuExportSelection.Click += new System.EventHandler(this.MnuExportSelected_Click);
            // 
            // mnuExportAll
            // 
            this.mnuExportAll.Name = "mnuExportAll";
            this.mnuExportAll.Size = new System.Drawing.Size(263, 22);
            this.mnuExportAll.Text = "Export all to CSV...";
            this.mnuExportAll.Click += new System.EventHandler(this.MnuExportAll_Click);
            // 
            // mnuGetInfo
            // 
            this.mnuGetInfo.Name = "mnuGetInfo";
            this.mnuGetInfo.Size = new System.Drawing.Size(263, 22);
            this.mnuGetInfo.Text = "Get Info...";
            this.mnuGetInfo.Click += new System.EventHandler(this.mnuGetInfo_Click);
            // 
            // mnuCopySelectionToClipboard
            // 
            this.mnuCopySelectionToClipboard.Name = "mnuCopySelectionToClipboard";
            this.mnuCopySelectionToClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mnuCopySelectionToClipboard.Size = new System.Drawing.Size(263, 22);
            this.mnuCopySelectionToClipboard.Text = "Copy selection to Clipboard";
            this.mnuCopySelectionToClipboard.Click += new System.EventHandler(this.MnuCopySelectionToClipboardToolStripMenuItem_Click);
            // 
            // lblTitel
            // 
            this.lblTitel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitel.AutoEllipsis = true;
            this.lblTitel.AutoSize = true;
            this.lblTitel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTitel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitel.ForeColor = System.Drawing.Color.Black;
            this.lblTitel.Location = new System.Drawing.Point(65, 13);
            this.lblTitel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitel.MaximumSize = new System.Drawing.Size(0, 35);
            this.lblTitel.MinimumSize = new System.Drawing.Size(35, 0);
            this.lblTitel.Name = "lblTitel";
            this.lblTitel.Size = new System.Drawing.Size(92, 22);
            this.lblTitel.TabIndex = 22;
            this.lblTitel.Text = "Add Filter";
            this.lblTitel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Frm_MouseDown);
            this.lblTitel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Frm_MouseMove);
            // 
            // saveFileCSVDialog
            // 
            this.saveFileCSVDialog.DefaultExt = "csv";
            this.saveFileCSVDialog.FileName = "unnamed";
            this.saveFileCSVDialog.Filter = "\"CSV file|*.csv|Text file|*.txt";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 18;
            this.listBox1.Location = new System.Drawing.Point(13, 282);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(674, 119);
            this.listBox1.TabIndex = 26;
            // 
            // pictureBox2
            // 
            this.pictureBox2.AccessibleName = "MIDI Devices form Close";
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackgroundImage = global::MidiUWPRouter.Properties.Resources.minimize;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(671, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.TabIndex = 25;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.PictureBox2_Click);
            // 
            // pictClose
            // 
            this.pictClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictClose.Image = global::MidiUWPRouter.Properties.Resources.Guitar_Pedal;
            this.pictClose.Location = new System.Drawing.Point(13, 13);
            this.pictClose.Margin = new System.Windows.Forms.Padding(4);
            this.pictClose.Name = "pictClose";
            this.pictClose.Size = new System.Drawing.Size(35, 35);
            this.pictClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictClose.TabIndex = 23;
            this.pictClose.TabStop = false;
            this.pictClose.DoubleClick += new System.EventHandler(this.PictClose_DoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleName = "MIDI Devices form Resize";
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.PanSE;
            this.pictureBox1.Location = new System.Drawing.Point(685, 393);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(14, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Resize_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Resize_MouseMove);
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(499, 116);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 24);
            this.textBox1.TabIndex = 27;
            this.textBox1.Text = "blah";
            this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
            // 
            // frmAddFiltersToAlias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(700, 413);
            this.ControlBox = false;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lblTitel);
            this.Controls.Add(this.pictClose);
            this.Controls.Add(this.dgvDeviceList);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmAddFiltersToAlias";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmDevices_FormClosed);
            this.Load += new System.EventHandler(this.Frm_Load);
            this.Shown += new System.EventHandler(this.FrmDevices_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Frm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Frm_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeviceList)).EndInit();
            this.cmnuDevices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDeviceList;
        private System.Windows.Forms.ContextMenuStrip cmnuDevices;
        private System.Windows.Forms.ToolStripMenuItem mnuExportSelection;
        private System.Windows.Forms.ToolStripMenuItem mnuExportAll;
        private System.Windows.Forms.Label lblTitel;
        private System.Windows.Forms.PictureBox pictClose;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolStripMenuItem mnuGetInfo;
        private System.Windows.Forms.SaveFileDialog saveFileCSVDialog;
        private System.Windows.Forms.DataGridViewCheckBoxColumn in_out;
        private System.Windows.Forms.DataGridViewTextBoxColumn device_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn cooked_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.ToolStripMenuItem mnuCopySelectionToClipboard;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox1;
    }
}