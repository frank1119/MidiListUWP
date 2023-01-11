using MidiUWPRouter.ConfigProcessing;
using MidiUWPRouter.Core;
using MidiUWPRouter.FormUtils;
using MidiUWPRouter.MultiForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MidiUWPRouter
{
    public partial class FrmDevices : Form
    {
        ClickPosition clickPos = new ClickPosition();

        private readonly string arg = "";

        #region Load and Close
        public FrmDevices(string arg)
        {
            this.arg = arg;
            InitializeComponent();
        }

        private void SetupRouter()
        {
            if (arg != "" && !Router.IsRunning)
            {
                Router.RouteInstalledEvent += Router_RouteInstalledEvent;
                Router.RouterSkipEvent += Router_RouteSkipEvent;
                Router.AliasFoundEvent += Router_AliasFoundEvent;
                Router.RouterErrorEvent += Router_RouterErrorEvent;
                Router.MidiInOpenProgress += Router_MidiPortOpenProgress;
                Router.RouteReportMidiInTypeEvent += Router_RouteReportMidiInTypeEvent;
                try
                {
                    Router.StartRouter(arg);
                }
                catch (IOException ioex)
                {
                    listBox1.Items.Add(string.Format("I/O exception: {0}", ioex.Message));

                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    this.Refresh();

                }
                catch (MidiRouterException mrex)
                {
                    listBox1.Items.Add(string.Format("MIDI router exception: {0}", mrex.Message));
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    listBox1.Items.Add(string.Format("Exception: {0}", ex.Message));
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    this.Refresh();
                }
            }
        }

        private void Frm_Load(object sender, EventArgs e)
        {
            MFDriver.RegisterModelessForm(this);

            var sortedMidiIn = ConfigReader.GetSortedList(ConfigReader.GetMidiInDevices(), true);
            var sortedMidiOut = ConfigReader.GetSortedList(ConfigReader.GetMidiOutDevices(), false);

            foreach (var item in sortedMidiIn)
            {
                AddOrUpdate(CheckState.Checked, item.DeviceInformation.Name, item.CookedName, item.DeviceInformation.Properties["System.Devices.DeviceInstanceId"].ToString());
            }

            foreach (var item in sortedMidiOut)
            {
                AddOrUpdate(CheckState.Unchecked, item.DeviceInformation.Name, item.CookedName, item.DeviceInformation.Properties["System.Devices.DeviceInstanceId"].ToString());
            }

            if (dgvDeviceList.Rows.Count > 0)
            {
                dgvDeviceList.ClearSelection();
                dgvDeviceList.Sort(dgvDeviceList.Columns[1], ListSortDirection.Ascending);
                dgvDeviceList.Sort(dgvDeviceList.Columns[0], ListSortDirection.Descending);

                dgvDeviceList.ClearSelection();
                dgvDeviceList.Rows[0].Selected = true;
            }

            frmAddFiltersToAlias frm2 = new frmAddFiltersToAlias();
            frm2.Show();
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
            SetupRouter();
        }

        private void FrmDevices_FormClosed(object sender, FormClosedEventArgs e)
        {
            Router.StopRouter();
        }

        #endregion

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

        #region Database GUI updates

        internal void AddOrUpdate(CheckState in_out, string devName, string cookedName, string id)
        {
            foreach (DataGridViewRow row in dgvDeviceList.Rows)
            {
                if ((string)row.Cells["id"].Value == id)
                {
                    row.Cells["in_out"].Value = in_out;
                    row.Cells["device_name"].Value = devName;
                    row.Cells["cooked_name"].Value = cookedName;
                    row.Cells["id"].Value = id;
                    return;
                }
            }

            object[] oo = new object[] { in_out, devName, cookedName, id };
            int index = dgvDeviceList.Rows.Add(oo);
        }
        /*
        internal void MarkStatus(PedalHardware rpi, CheckState state)
        {
            foreach (DataGridViewRow row in dgvDeviceList.Rows)
            {
                if (row.Tag == rpi)
                {
                    row.Cells["Connected"].Value = state;
                    if (state == CheckState.Unchecked)
                    {
                        row.Tag = null;
                    }
                    dgvDeviceList.CancelEdit();
                    dgvDeviceList.EndEdit();

                }
            }
        }
        */
        #endregion

        #region RouteMessages

        private void Router_RouteReportMidiInTypeEvent(string routeName, string sourceName, string destinationName, bool isBLE)
        {
            if (isBLE)
                listBox1.Items.Add(string.Format("Route {0}. Going to open BLE-Source {1} ", routeName, sourceName));
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            this.Refresh();
        }

        private void Router_MidiPortOpenProgress(int retriesLeft)
        {
            switch (retriesLeft)
            {
                case -1:
                    listBox1.Items.Add(string.Format("Done."));
                    break;
                case -2:
                    listBox1.Items.Add(string.Format("Failed."));
                    break;
            }
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            this.Refresh();
        }


        private void Router_RouterErrorEvent(string configFileName, Exception e, string routeName, string sourceName, string destinationName)
        {
            listBox1.Items.Add(string.Format("Error installing {0}. Source {1} destination {2} {3}", routeName, sourceName, destinationName, e.Message));
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            this.Refresh();
        }

        private void Router_AliasFoundEvent(string type, string deviceName, string aliasName)
        {
            listBox1.Items.Add(string.Format("Alias found {0} {1} -> {2}", type, deviceName, aliasName));
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            this.Refresh();
        }

        private void Router_RouteSkipEvent(string routeName, string sourceName, string destinationName)
        {
            listBox1.Items.Add(string.Format("Skipping {0}. Source {1} destination {2}", routeName, sourceName, destinationName));
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            this.Refresh();
        }

        private void Router_RouteInstalledEvent(string routeName, string sourceName, string destinationName)
        {
            listBox1.Items.Add(string.Format("Installed {0}. Source {1} destination {2}", routeName, sourceName, destinationName));
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            this.Refresh();
        }
        #endregion



        private void dgvDeviceList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // .Right -> Context Menu
            if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex >= 0)
                {
                    DnDCell dnd = new DnDCell();
                    dnd.row = dgvDeviceList.Rows[e.RowIndex];

                    var res = dgvDeviceList.DoDragDrop(dnd, DragDropEffects.Copy);
                }
            }
        }

        // Full Color
        private static Cursor newCursor2 = new Cursor(global::MidiUWPRouter.Properties.Resources.noun_midi_2965_81.GetHicon());

        private void dgvDeviceList_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Cursor.Current = newCursor2;


        }

        private void pictClose_Click(object sender, EventArgs e)
        {

        }
    }

    [Serializable]
    public class DnDCell
    {
        public DataGridViewRow row;
        public DataGridViewCell cell;
    }
}
