using MidiUWPRouter.ConfigProcessing;
using MidiUWPRouter.Core;
using MidiUWPRouter.FormUtils;
using MidiUWPRouter.MultiForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidiUWPRouter
{
    public partial class frmAddFiltersToAlias : Form
    {
        ClickPosition clickPos = new ClickPosition();


        public frmAddFiltersToAlias()
        {
            InitializeComponent();
        }



        private void Frm_Load(object sender, EventArgs e)
        {
            MFDriver.RegisterModelessForm(this);

        }

        private void Frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Router.StopRouter();
            }
            catch { }

            if (MFDriver.FormCount > 1)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                }
            }
        }


        private void PictClose_DoubleClick(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmDevices_Shown(object sender, EventArgs e)
        {
                 }

        private void FrmDevices_FormClosed(object sender, FormClosedEventArgs e)
        {
            Router.StopRouter();
        }


        #region Menu handling

        private void CmnuDevices_Opening(object sender, CancelEventArgs e)
        {
            mnuExportSelection.Enabled = false;
            mnuExportSelection.Enabled = dgvDeviceList.SelectedRows.Count > 0;
            mnuCopySelectionToClipboard.Enabled = mnuExportSelection.Enabled;
            mnuExportAll.Enabled = (dgvDeviceList.Rows.Count > 0);
            mnuGetInfo.Enabled = dgvDeviceList.SelectedRows?.Count == 1;
            e.Cancel = !mnuExportAll.Enabled;
        }

        private void SaveToCSV(System.Collections.IList rows)
        {
            if (rows != null && rows.Count > 0)
            {
                List<string> lines = new List<string>();

                string headerLine = "";
                foreach (DataGridViewColumn c in dgvDeviceList.Columns)
                {
                    if (headerLine.Length > 0)
                        headerLine += ",";

                    if (c.Name == "in_out")
                    {
                        headerLine += "\"In/Out\"";
                    }
                    else
                    {
                        string text = c.HeaderText;
                        text = text.Replace("\"", "\"\"");
                        headerLine += "\"" + text + "\"";
                    }
                }
                lines.Add(headerLine);

                foreach (DataGridViewRow row in rows)
                {
                    string line = "";
                    string text = "";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (line.Length > 0)
                            line += ",";
                        if (dgvDeviceList.Columns[cell.ColumnIndex].Name == "in_out")
                        {
                            DataGridViewCheckBoxCell cb = cell as DataGridViewCheckBoxCell;
                            if (cb != null)
                            {
                                CheckState cbt = (CheckState)cb.Value;
                                text = cbt == CheckState.Checked ? "In" : "Out";
                            }
                        }
                        else
                        {
                            text = cell.Value.ToString();
                        }
                        text = text.Replace("\"", "\"\"");
                        line += "\"" + text + "\"";
                    }
                    lines.Add(line);
                }

                try
                {
                    DialogResult res = saveFileCSVDialog.ShowDialog(this);
                    Console.WriteLine(res);
                    if (res == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(saveFileCSVDialog.FileName))
                        {
                            foreach (string line in lines)
                            {
                                sw.WriteLine(line);
                            }
                        }
                    }
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.Message);
                }

            }
        }

        private void MnuExportSelected_Click(object sender, EventArgs e)
        {
            if (dgvDeviceList.SelectedRows != null && dgvDeviceList.Rows.Count > 0)
            {
                SaveToCSV(dgvDeviceList.SelectedRows);
            }
        }

        private void MnuExportAll_Click(object sender, EventArgs e)
        {
            SaveToCSV(dgvDeviceList.Rows);
        }

        private void mnuGetInfo_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvDeviceList.CurrentRow;
            if (row != null)
            {
                /*
                if (row.Tag is PedalHardware phwi)
                {
                    string[] version = phwi.Version;
                    FrmMessage.Show(this, $"Firmware version: {version[1]}\r\nCompile date: {version[2]}\r\nType: {phwi.PedalHardwareType}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                */
            }
        }

        private void CopyToClipBoard(System.Collections.IList rows)
        {
            if (rows != null && rows.Count > 0)
            {
                string lines = "";
                foreach (DataGridViewRow row in rows)
                {
                    string line = "";
                    string text = "";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (line.Length > 0)
                            line += ",";
                        if (dgvDeviceList.Columns[cell.ColumnIndex].Name == "in_out")
                        {
                            DataGridViewCheckBoxCell cb = cell as DataGridViewCheckBoxCell;
                            if (cb != null)
                            {
                                CheckState cbt = (CheckState)cb.Value;
                                text = cbt == CheckState.Checked ? "In" : "Out";
                            }
                        }
                        else
                        {
                            text = cell.Value.ToString();
                        }
                        text = text.Replace("\"", "\"\"");
                        line += "\"" + text + "\"";
                    }
                    lines += line + "\r\n";
                    Clipboard.SetText(lines);
                }
            }
        }

        private void MnuCopySelectionToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDeviceList.SelectedRows?.Count > 0)
                CopyToClipBoard(dgvDeviceList.SelectedRows);
        }
        #endregion

        #region Move and Resize
        private void Frm_MouseDown(object sender, MouseEventArgs e)
        {
            clickPos = new ClickPosition(e.Location, MoveStates.MouseDown);
        }

        private void Frm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (e.Y != clickPos.Y || e.X != clickPos.X))
            {
                Point screenPos = PointToScreen(e.Location);
                if (clickPos.MoveState == MoveStates.MouseDown)
                {
                    if (screenPos.Y - Top > 45)
                    {
                        clickPos.MoveState = MoveStates.MouseDownRejected;
                        return;
                    }
                    else
                        clickPos.MoveState = MoveStates.MouseDownAccepted;
                }

                if (clickPos.MoveState == MoveStates.MouseDownRejected)
                    return;

                Point pos = Location;

                pos.Y += (e.Y - clickPos.Y);
                pos.X += (e.X - clickPos.X);
                Location = pos;
            }
        }

        private void Resize_MouseDown(object sender, MouseEventArgs e)
        {
            clickPos = new ClickPosition(e.Location, MoveStates.MouseDown)
            {
                InResizing = false
            };
        }

        private void Resize_MouseMove(object sender, MouseEventArgs e)
        {
            if (!clickPos.InResizing)
            {
                clickPos.InResizing = true;
                if (e.Button == MouseButtons.Left && (e.Y != clickPos.Y || e.X != clickPos.X))
                {
                    Point pos = new Point(this.Width, this.Height);
                    pos.Y += (e.Y - clickPos.Y);
                    pos.X += (e.X - clickPos.X);
                    this.Size = new Size(pos);
                    Application.DoEvents();
                }
                clickPos.InResizing = false;
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        #endregion

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            DnDCell p = e.Data.GetData("MidiUWPRouter.DnDCell", true) as DnDCell;
            if (p != null)
            {
                textBox1.Text = p.row.Cells[1].Value.ToString();
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            
            e.Effect = DragDropEffects.Copy;

            if (e.Data.GetDataPresent("MidiUWPRouter.DnDCell"))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}
