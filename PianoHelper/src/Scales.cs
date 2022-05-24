using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace PianoHelper {

    public static class Scales {

        enum Step {
            WHOLE, HALF
        }

        private static readonly Step[] MAJOR_SCALE = new Step[] { Step.WHOLE, Step.WHOLE, Step.HALF, Step.WHOLE, Step.WHOLE, Step.WHOLE, Step.HALF };
        private static readonly Step[] MINOR_SCALE = new Step[] { Step.WHOLE, Step.HALF, Step.WHOLE, Step.WHOLE, Step.HALF, Step.WHOLE, Step.WHOLE };

        private static readonly Step[][] SCALES = { MAJOR_SCALE, MINOR_SCALE };

        public static int currentIndex = 0;

        public static string activeNote;

        public static bool isShowNotes = false;

        private static bool resetKeysOnFail = false;

        private static Step[] activeScale;

        public static void setScale(string scale) {
            if (scale == "Major") {
                activeScale = MAJOR_SCALE;
            } else if (scale == "Minor") {
                activeScale = MINOR_SCALE;
            }
        }

        public static async void CheckInput(byte id) {

            string note = Note.GetNote(id);
            string[] notes = GetNotes(activeNote, activeScale);

            Control key = Variables.GetControl(ScalesWindow.ActiveForm, "Key_" + note.Replace("#", "Sharp"));

            if (note == notes[currentIndex]) {
                key.BackColor = note.Contains("#") ? Variables.Colors.darkGreen : Variables.Colors.green;
                if (currentIndex == 7) {
                    currentIndex = 0;
                    await (Task.Delay(Variables.waitTime));
                    ResetKeys();
                } else {
                    currentIndex++;
                }
            } else {

                Color previousColor = key.BackColor;

                key.BackColor = note.Contains("#") ? Variables.Colors.darkRed : Variables.Colors.red;
                await (Task.Delay(Variables.waitTime));

                if (resetKeysOnFail) {
                    ResetKeys();
                    currentIndex = 0;
                } else {
                    Color resetColor;
                    if (isShowNotes && notes.Contains(note)) {
                        resetColor = note.Contains("#") ? Variables.Colors.darkYellow : Variables.Colors.yellow;
                    } else {
                        resetColor = note.Contains("#") ? Color.Black : Color.White;
                    }
                    key.BackColor = GetIndex(note, notes) >= currentIndex ? resetColor : previousColor;
                }
            }
        }

        private static int GetIndex(string note, string[] notes) {
            int index = 0;
            foreach (string n in notes) {
                if (n == note) {
                    break;
                }
                index++;
            }
            return index;
        }

        public static void ShowNotes() {

            string[] notes = GetNotes(activeNote, activeScale);

            for (int i = currentIndex; i < notes.Length; i++) {

                string note = notes[i];

                Control key = Variables.GetControl(ScalesWindow.ActiveForm, "Key_" + note.Replace("#", "Sharp"));
                key.BackColor = note.Contains("#") ? Variables.Colors.darkYellow : Variables.Colors.yellow;
            }

        }

        public static void ResetKeys() {

            string[] scaleNotes = GetNotes(activeNote, activeScale);

            foreach (string note in Variables.allNotes) {

                Control key = Variables.GetControl(ScalesWindow.ActiveForm, "Key_" + note.Replace("#", "Sharp"));

                Color resetColor = note.Contains("#") ? Color.Black : Color.White;
                if (isShowNotes && scaleNotes.Contains(note)) {
                    resetColor = note.Contains("#") ? Variables.Colors.darkYellow : Variables.Colors.yellow;
                }

                if (key != null) {
                    if (key.BackColor != resetColor) {
                        key.BackColor = resetColor;
                    }
                }
            }

        }

        private static string[] GetNotes(string note, Step[] scale) {

            if (note != null && scale != null) {

                string[] notes = new string[8];

                int index = 0;
                foreach (string n in Variables.allNotes) {
                    if (n.IndexOf(note) == 0) {
                        break;
                    }
                    index++;
                }

                notes[0] = Variables.allNotes[index];

                for (int i = 0; i < scale.Length; i++) {
                    Step step = scale[i];
                    if (step == Step.WHOLE) {
                        index += 2;
                    } else if (step == Step.HALF) {
                        index++;
                    }
                    notes[i + 1] = Variables.allNotes[index];
                }

                return notes;

            }

            return new string[] { };

        }


    }



}
