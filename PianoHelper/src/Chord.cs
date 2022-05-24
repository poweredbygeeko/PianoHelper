using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoHelper {
    public class Chord {

        public string shortName;
        public string longName;

        public string[] notes;

        public static Chord activeChord;

        private static Chord[] chords_triads_major = new Chord[12];
        private static Chord[] chords_triads_minor = new Chord[12];

        private static Chord[][] chords = new Chord[][] { chords_triads_minor, chords_triads_major };

        public Chord(string shortName, string longName, string[] notes) {
            this.shortName = shortName;
            this.longName = longName;
            this.notes = notes;
        }

        public static void InitChords() {

            chords_triads_major[0] = new Chord("Cmaj", "C Major", new string[] { "C", "E", "G" });
            chords_triads_major[1] = new Chord("C#maj", "C# Major", new string[] { "C#", "F", "G#" });
            chords_triads_major[2] = new Chord("Dmaj", "D Major", new string[] { "D", "F#", "A" });
            chords_triads_major[3] = new Chord("E♭maj", "E♭ Major", new string[] { "D#", "G", "A#" });
            chords_triads_major[4] = new Chord("Emaj", "E Major", new string[] { "E", "G#", "B" });
            chords_triads_major[5] = new Chord("Fmaj", "F Major", new string[] { "F", "A", "C" });
            chords_triads_major[6] = new Chord("F#maj", "F# Major", new string[] { "F#", "A#", "C#" });
            chords_triads_major[7] = new Chord("Gmaj", "G Major", new string[] { "G", "B", "D" });
            chords_triads_major[8] = new Chord("A♭maj", "A♭ Major", new string[] { "G#", "C", "D#" });
            chords_triads_major[9] = new Chord("Amaj", "A Major", new string[] { "A", "C#", "E" });
            chords_triads_major[10] = new Chord("B♭maj", "B♭ Major", new string[] { "A#", "D", "F" });
            chords_triads_major[11] = new Chord("Bmaj", "B Major", new string[] { "B, D#", "F#" });

            chords_triads_minor[0] = new Chord("cmin", "c Minor", new string[] { "C", "D#", "G" });
            chords_triads_minor[1] = new Chord("c#min", "c# Minor", new string[] { "C#", "E", "G#" });
            chords_triads_minor[2] = new Chord("dmin", "d Minor", new string[] { "D", "F", "A" });
            chords_triads_minor[3] = new Chord("e♭min", "e♭ Minor", new string[] { "D#", "F#", "A#" });
            chords_triads_minor[4] = new Chord("emin", "e Minor", new string[] { "E", "G", "B" });
            chords_triads_minor[5] = new Chord("fmin", "f Minor", new string[] { "F", "G#", "C" });
            chords_triads_minor[6] = new Chord("f#min", "f# Minor", new string[] { "F#", "A", "C#" });
            chords_triads_minor[7] = new Chord("gmin", "g Minor", new string[] { "G", "A#", "D" });
            chords_triads_minor[8] = new Chord("a♭min", "a♭ Minor", new string[] { "G#", "B", "D#" });
            chords_triads_minor[9] = new Chord("amin", "a Minor", new string[] { "A", "C", "E" });
            chords_triads_minor[10] = new Chord("b♭min", "b♭ Minor", new string[] { "A#", "C#", "F" });
            chords_triads_minor[11] = new Chord("bmin", "b Minor", new string[] { "B", "D", "F#" });

        }

        public static async void GenerateChord(bool changeColor = true) {

            Random random = new Random();

            Chord[] chords = Chord.chords[random.Next(0, Chord.chords.Length)];
            Chord chord = activeChord = chords[random.Next(0, chords.Length)];

            if (changeColor) {
                Variables.inputLabels[0].ForeColor = Variables.Colors.green;
                await (Task.Delay(Variables.waitTime));
            }

            HideNotes();

            Variables.inputLabels[0].Text = chord.shortName;
            Variables.inputLabels[1].Text = chord.longName;
            Variables.inputLabels[0].ForeColor = Variables.Colors.grey;
            Variables.SetLabelLocations();

            if(Settings.isShowNotes) {
                ShowNotes();
            }


        }

        public static void CheckInput() {

            if (Variables.inputNotesAmount == activeChord.notes.Length) {

                bool isInputCorrect = true;
                foreach (string note in activeChord.notes) {
                    if (!Note.notes[note]) {
                        isInputCorrect = false;
                    }
                }
                if (!isInputCorrect) {
                    WrongInput();
                } else {
                    GenerateChord();
                }
            }
        }

        public static async void WrongInput() {

            Variables.inputLabels[0].ForeColor = Variables.Colors.red;
            await (Task.Delay(1500));
            Variables.inputLabels[0].ForeColor = Variables.Colors.grey;

        }

        public static string[] GetNotes() {

            if (activeChord != null) {

                string[] notes = new string[activeChord.notes.Length];

                int chordIndex = activeChord.notes.Length - 1;

                for (int i = Variables.allNotes.Length - 1; i >= 0; i--) { 

                    String note = Variables.allNotes[i];

                    if (note.Substring(0, note.Length - 1).Equals(activeChord.notes[chordIndex])) {
                        notes[notes.Length - 1 - chordIndex] = note;
                        chordIndex--;
                        if (chordIndex < 0) {
                            break;
                        }
                    }
                }

                return notes;
            }

            return new string[] { };

        }

        public static void ShowNotes() {
            foreach(string note in GetNotes()) {
                Note.ShowNote(note);
            }
        }

        public static void HideNotes() {
            foreach(string note in GetNotes()) {
                Note.HideNote(note);
            }
        }

    }
}
