using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using Sanford.Multimedia.Midi;


namespace PianoHelper {

    public partial class ChordsWindow : Form {
        public ChordsWindow() {
            InitializeComponent();
            Variables.Init(this);
        }

        private void ChordsWindow_Load(object sender, EventArgs e) {
            InputDevice device = new InputDevice(0);
            device.MessageReceived += ProcessInput;
            device.StartRecording();
        }

        private void ProcessInput(IMidiMessage message) {

            Byte[] bytes = message.GetBytes();

            bool isKeyDown = bytes[0] == 144;

           
            String note = Note.GetNote(bytes[1]);
            note = note.Substring(0, note.Length - 1);

            Note.notes[note] = isKeyDown;

            Note.SetKeyState(bytes[1], isKeyDown);

            if (isKeyDown) {
                Variables.inputNotesAmount++;
                if (Settings.mode == Mode.NOTES) {
                    Note.CheckInput();
                } else if (Settings.mode == Mode.CHORDS) {
                    Chord.CheckInput();
                }
            } else {
                Variables.inputNotesAmount--;
            }

        }

        private void modeRadio_notes_CheckedChanged(object sender, EventArgs e) {
            if (modeRadio_notes.Checked) {
                Note.ResetKeys();
                NoteLabelCaption.Visible = false;
                Settings.mode = Mode.NOTES;
                Note.GenerateNote(false);
            }
        }

        private void modeRadio_chords_CheckedChanged(object sender, EventArgs e) {
            if (modeRadio_chords.Checked) {
                Note.ResetKeys();
                NoteLabelCaption.Visible = true;
                Settings.mode = Mode.CHORDS;
                Chord.GenerateChord(false);
            }
        }

        private void showNotesCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (showNotesCheckbox.Checked) {
                Settings.isShowNotes = true;
                if (Settings.mode == Mode.NOTES) {
                    Note.ShowNote();
                } else if (Settings.mode == Mode.CHORDS) {
                    Chord.ShowNotes();
                }
            } else {
                Settings.isShowNotes = false;
                if (Settings.mode == Mode.NOTES) {
                    Note.HideNote();
                } else if (Settings.mode == Mode.CHORDS) {
                    Chord.HideNotes();
                }
            }
        }

        private void scalesToolStripMenuItem_Click(object sender, EventArgs e) {
            ScalesWindow.ActiveForm.Show();
            ChordsWindow.ActiveForm.Hide();
        }

        private void notesChordsToolStripMenuItem_Click(object sender, EventArgs e) {
            ChordsWindow.ActiveForm.Show();
            ScalesWindow.ActiveForm.Hide();
        }
    }

}
