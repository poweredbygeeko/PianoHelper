using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PianoHelper {


    public enum Mode {
        NOTES, CHORDS
    };

    public class Variables {

        public const int waitTime = 1500;

        public static int inputNotesAmount = 0;

        public static readonly string[] allNotes = { "C1", "C#1", "D1", "D#1", "E1", "F1", "F#1", "G1", "G#1", "A1", "A#1", "B1", "C2", "C#2", "D2", "D#2", "E2", "F2", "F#2", "G2", "G#2", "A2", "A#2", "B2" };

        public static Control[] inputLabels = new Control[2];

        public static void Init(Form form) {
            inputLabels = GetNoteLabels(form);
            Note.InitNotes();
            Chord.InitChords();
        }

        public static Control GetControl(Form form, string name) {
            if (form != null) {
                foreach (Control control in form.Controls.Find(name, true)) {
                    if (control.Name.Equals(name)) {
                        return control;
                    }
                }
            }
            return null;
        }
             
        public static void SetLabelLocations() {

            Point[] labelLocations = GetLabelLocations();

            Variables.inputLabels[0].Location = labelLocations[0];
            if (Settings.mode == Mode.CHORDS) {
                Variables.inputLabels[1].Location = labelLocations[1];
            }

        }

        private static Control[] GetNoteLabels(Form form) {

            Control[] controls = new Control[2];

            foreach (Control control in form.Controls.Find("NoteLabel", true)) {
                if (control.Name.Equals("NoteLabel")) {
                    controls[0] = control;
                    break;
                }
            }

            foreach (Control control in form.Controls.Find("NoteLabelCaption", true)) {
                if (control.Name.Equals("NoteLabelCaption")) {
                    controls[1] = control;
                    break;
                }
            }

            return controls;
        }

        private static Point[] GetLabelLocations() {

            Point centre = GetCentreLocation();

            if (Settings.mode == Mode.CHORDS) {

                int height = Variables.inputLabels[0].Height + Variables.inputLabels[1].Height;

                int x1 = centre.X - Variables.inputLabels[0].Width / 2;
                int y1 = centre.Y - height / 2 - 6;

                int x2 = centre.X - Variables.inputLabels[1].Width / 2;
                int y2 = y1 + Variables.inputLabels[0].Height;

                Point location1 = new Point(x1, y1);
                Point location2 = new Point(x2, y2);

                return new Point[] { location1, location2 };

            }

            return new Point[] { new Point(centre.X - Variables.inputLabels[0].Width / 2, centre.Y - Variables.inputLabels[0].Height / 2) };
        }

        private static Point GetCentreLocation() {

            Size windowSize = ChordsWindow.ActiveForm.ClientSize;

            Control modeControl = ChordsWindow.ActiveForm.Controls.Find("modeRadio", true)[0];
            Control showNotesCheckbox = ChordsWindow.ActiveForm.Controls.Find("showNotesCheckbox", true)[0];

            int top = modeControl.Location.Y + modeControl.Height;
            int bottom = showNotesCheckbox.Location.Y;

            int x = windowSize.Width / 2;
            int y = (bottom - top) / 2 + top;

            return new Point(x, y);
        }

        public static class Colors {

            public static Color green = Color.FromArgb(198, 255, 26);
            public static Color darkGreen = Color.FromArgb(153, 204, 0);

            public static Color red = Color.FromArgb(204, 0, 0);
            public static Color darkRed = Color.FromArgb(153, 0, 0);

            public static Color yellow = Color.FromArgb(255, 210, 40);
            public static Color darkYellow = Color.FromArgb(170, 130, 20);

            public static Color grey = Color.FromArgb(50, 50, 50);

        }

    }

    public class Settings {

        public static Mode mode;

        public static bool isShowNotes = false;

    }

}
