using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "SongData", order = 3)]
public class SongData: ScriptableObject
{
    [System.Serializable]
    public struct NoteCommand
    {
        public double timeStamp;
        public bool isOn;
        public int keyNote;
    };

    public List<NoteCommand> KeyCommands;

#if UNITY_EDITOR
    private double BPM = 120;

    public double ppqToMs(long p_timestamp)
    {
        double msPerBeat = (60.0 / BPM) * 1000.0;
        double msPerPPQ = msPerBeat / 960.0;
        return msPerPPQ * p_timestamp;
    }

    [ContextMenu("ParseFile")]
    public void ParseFile()
    {
        string fileName = UnityEditor.EditorUtility.OpenFilePanel("Select File", "", "mtx");
        if( !string.IsNullOrEmpty(fileName) )
        {
            string fileTxt = File.ReadAllText(fileName);
            KeyCommands = new List<NoteCommand>();
            Parse(fileTxt);
        }
    }

    public void Parse(string song)
    {
        List<string> Notes = new List<string>() { "c6", "c#6", "d6", "d#6", "e6", "f6", "f#6", "g6", "g#6", "a6", "a#6", "b6", "c7" };

        string[] lines = song.Split(new char[] { '\n' });

        foreach(string line in lines)
        {
            if(line.StartsWith("MFile") )
            {
                string[] components = line.Split(new char[] { ' ' });
                //File header:        Mfile <format> <ntrks> <division>
                // <division> Indica el número de intervalos de tiempo contenidos en un cuarto de nota. Especifica así el tempo de la reproducción. bpm
                BPM = double.Parse(components[3]);
            }            
            else if (line.Contains("On"))
            {
                string[] components = line.Split(new char[] { ' ' });
                long time = long.Parse(components[0]); //El campo delta-time es de longitud variable y se expresa una determinada fracción del compás, tal como se indica en la cabecera.
                double timestamp = ppqToMs(time);
                //int time = int.Parse(components[1]); //On
                //int time = int.Parse(components[2]); //ch=
                string n = components[3].Substring(2); //n=<note><octave>
                string v = components[4].Substring(2); //v=
                int vol = int.Parse(v);

                NoteCommand newCmd = new NoteCommand();
                newCmd.isOn = (vol > 0);
                newCmd.timeStamp = timestamp;
                newCmd.keyNote = Notes.FindIndex(0, x => { return (x == n); });
                KeyCommands.Add(newCmd);
            }
            else if (line.Contains("Off"))
            {
                string[] components = line.Split(new char[] { ' ' });
                long time = long.Parse(components[0]);
                double timestamp = ppqToMs(time);
                //int time = int.Parse(components[1]); //Off
                //int time = int.Parse(components[2]); //ch=
                string n = components[3].Substring(2); //n=<note><octave>
                //string v = components[4].Substring(2); //v=

                NoteCommand newCmd = new NoteCommand();
                newCmd.isOn = false;
                newCmd.timeStamp = timestamp;
                newCmd.keyNote = Notes.FindIndex(0, x => { return (x == n); });
                KeyCommands.Add(newCmd);
            }
        }
    }
#endif
}
