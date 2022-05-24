using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Sanford.Multimedia.Midi;

namespace PianoHelper {

    public partial class ScalesWindow : Form {
        public ScalesWindow() {
            InitializeComponent();
        }

        private void ScalesWindow_Load(object sender, EventArgs e) {

            scaleCombo.SelectedItem = scaleCombo.Items[0];
            noteCombo.SelectedItem = noteCombo.Items[0];

            InputDevice device = new InputDevice(0);
            device.MessageReceived += Device_MessageReceived;
            device.StartRecording();

        }

        private void Device_MessageReceived(IMidiMessage message) {
            Byte[] bytes = message.GetBytes();

            bool isKeyDown = bytes[0] == 144;

            if(isKeyDown) {
                Scales.CheckInput(bytes[1]);
            }

        }

        private void scaleCombo_SelectedIndexChanged_1(object sender, EventArgs e) {
            Scales.setScale(scaleCombo.SelectedItem.ToString());
            Scales.ResetKeys();
        }

        private void noteCombo_SelectedIndexChanged(object sender, EventArgs e) {

            Scales.activeNote = noteCombo.SelectedItem.ToString();
            Scales.ResetKeys();

        }

        private void showNotesCheckbox_CheckedChanged(object sender, EventArgs e) {
            if(showNotesCheckbox.Checked) {
                Scales.isShowNotes = true;
                Scales.ShowNotes();
            } else {
                Scales.isShowNotes = false;
                Scales.ResetKeys();
            }
        }

        private void randomButton_Click(object sender, EventArgs e) {
            Random random = new Random();
            if (randomTypeCheckbox.Checked) {
                scaleCombo.SelectedItem = scaleCombo.Items[random.Next(0, scaleCombo.Items.Count)];
            }
            noteCombo.SelectedItem = noteCombo.Items[random.Next(0, noteCombo.Items.Count)];
        }

        private void notesChordsToolStripMenuItem_Click(object sender, EventArgs e) {
            ChordsWindow.ActiveForm.Show();
            ScalesWindow.ActiveForm.Hide();
        }

        private void scalesToolStripMenuItem_Click(object sender, EventArgs e) {
            ScalesWindow.ActiveForm.Show();
            ChordsWindow.ActiveForm.Hide();
        }
    }
}
