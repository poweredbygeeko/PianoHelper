using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PianoHelper {


    class Note {

        public static Dictionary<string, bool> notes = new Dictionary<string, bool>();

        public static void InitNotes() {
            notes.Add("C", false);
            notes.Add("C#", false);
            notes.Add("D", false);
            notes.Add("D#", false);
            notes.Add("E", false);
            notes.Add("F", false);
            notes.Add("F#", false);
            notes.Add("G", false);
            notes.Add("G#", false);
            notes.Add("A", false);
            notes.Add("A#", false);
            notes.Add("B", false);
        }

        public static string GetNote(byte id) {
            if (id == 48) {
                return "C1";
            } else if (id == 49) {
                return "C#1";
            } else if (id == 50) {
                return "D1";
            } else if (id == 51) {
                return "D#1";
            } else if (id == 52) {
                return "E1";
            } else if (id == 53) {
                return "F1";
            } else if (id == 54) {
                return "F#1";
            } else if (id == 55) {
                return "G1";
            } else if (id == 56) {
                return "G#1";
            } else if (id == 57) {
                return "A1";
            } else if (id == 58) {
                return "A#1";
            } else if (id == 59) {
                return "B1";
            } else if (id == 60) {
                return "C2";
            } else if (id == 61) {
                return "C#2";
            } else if (id == 62) {
                return "D2";
            } else if (id == 63) {
                return "D#2";
            } else if (id == 64) {
                return "E2";
            } else if (id == 65) {
                return "F2";
            } else if (id == 66) {
                return "F#2";
            } else if (id == 67) {
                return "G2";
            } else if (id == 68) {
                return "G#2";
            } else if (id == 69) {
                return "A2";
            } else if (id == 70) {
                return "A#2";
            } else if (id == 71) {
                return "B2";
            } else if (id == 72) {
                return "C3";
            }
            return "";
        }

        public static async void GenerateNote(bool changeColor = true) {

            Random random = new Random();

            string[] notes = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            string note = notes[random.Next(0, notes.Length)];

            if (changeColor) {
                Variables.inputLabels[0].ForeColor = Variables.Colors.green;
                await (Task.Delay(Variables.waitTime));
            }

            HideNote();

            Variables.inputLabels[0].Text = Variables.inputLabels[1].Text = note;
            Variables.inputLabels[0].ForeColor = Variables.Colors.grey;

            if (Settings.isShowNotes) {
                ShowNote();
            }

            Variables.SetLabelLocations();

        }

        public static void SetKeyState(byte id, bool isKeyDown) {

            string note = GetNote(id);

            bool isValid = (Settings.mode == Mode.CHORDS && Chord.GetNotes().Contains(note))
                   || (Settings.mode == Mode.NOTES && Variables.inputLabels[0].Text == note.Substring(0, note.Length - 1));

            Control key = Variables.GetControl(ChordsWindow.ActiveForm, "Key_" + note.Replace("#", "Sharp"));
            Color color = note.Contains("#") ? Color.Black : Color.White;

            if (isKeyDown) {

                if (isValid) {
                    color = note.Contains("#") ? Variables.Colors.darkGreen : Variables.Colors.green;
                } else {
                    color = note.Contains("#") ? Variables.Colors.darkRed : Variables.Colors.red;
                }
            } 

            key.BackColor = color;

        }

        public static void CheckInput() {

            string note = Variables.inputLabels[0].Text;
            if (Note.notes[note]) {
                Note.GenerateNote();
            } else {
                Chord.WrongInput();
            }

        }

        public static void ShowNote() {
            string note = Variables.inputLabels[0].Text;
            for (int i = 1; i <= (note.Equals("C") ? 3 : 2); i++) {
                if (Variables.allNotes.Contains(note + i)) {
                    ShowNote(note + i);
                }
            }
        }

        public static void ShowNote(string note) {
            Control key = Variables.GetControl(ChordsWindow.ActiveForm, "Key_" + note.Replace("#", "Sharp"));
            key.BackColor = note.Contains("#") ? Variables.Colors.darkYellow : Variables.Colors.yellow;
        }

        public static void HideNote() {
            string note = Variables.inputLabels[0].Text;
            for (int i = 1; i <= (note.Equals("C") ? 3 : 2); i++) {
                if (Variables.allNotes.Contains(note + i)) {
                    HideNote(note + i);
                }
            }
        }

        public static void HideNote(string note) {
            Control key = Variables.GetControl(ChordsWindow.ActiveForm, "Key_" + note.Replace("#", "Sharp"));
            key.BackColor = note.Contains("#") ? Color.Black : Color.White;
        }

        public static void ResetKeys() {

            foreach (string note in Variables.allNotes) {
                Control key = Variables.GetControl(ChordsWindow.ActiveForm, "Key_" + note.Replace("#", "Sharp"));
                key.BackColor = note.Contains("#") ? Color.Black : Color.White;
            }

        }

    }

}
