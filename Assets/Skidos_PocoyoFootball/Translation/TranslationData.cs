using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Globalization;
#endif

[CreateAssetMenu(fileName = "language", menuName = "TranslationData", order = 1)]
public class TranslationData : ScriptableObject {
	public int nextStringId = 0;

	[System.Serializable]
	public struct TranslationEntry {
		public string grougId;
		public string stringId;
		public string translation;
	};

	[System.Serializable]
	public struct Language {
		public string languageId;
		public List<TranslationEntry> translations;
	};

	public Language OriginalText = new Language();
	public List<Language> Languages = new List<Language>();

#if UNITY_EDITOR
    [ContextMenu("Parse Qt Linguist to CVS")]
    public void ParseQtFileToCVS()
    {
        char[] separator = new char[] { '\n' };

        string path = EditorUtility.OpenFolderPanel("Qt Translation Folder", "", "");
		if (!string.IsNullOrEmpty(path))
		{
			string[] TsFiles = Directory.GetFiles(path, "*.ts");

            List<string> CVSLines = new List<string>();

			foreach(string filePath in TsFiles)
            {				
				if (!string.IsNullOrEmpty(filePath))
				{                    
                    string tsname = Path.GetFileNameWithoutExtension(filePath);
                    string language = tsname.Replace("tranlations_", "");

                    if (CVSLines.Count < 1)
                        CVSLines.Add(language);
                    else
                        CVSLines[0] = CVSLines[0] + "; " + language;

					string ts = File.ReadAllText(filePath);
                    ts = ts.Replace("&apos;", "'");
                    string[] lines = ts.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                    int translationCounter = 1;
                    for(int i = 0; i < lines.Length; i++)
                    {
                        string trimLine = lines[i].Trim();
                        if(trimLine.StartsWith("<translation>"))
                        {
                            string translation = trimLine.Replace("<translation>", "").Replace("</translation>", "");
                            if (CVSLines.Count < 1 + translationCounter)
                                CVSLines.Add(translation);
                            else
                                CVSLines[translationCounter] = CVSLines[translationCounter] + "; " + translation;
                            translationCounter++;
                        }
                    }
				}
            }

            string outText = string.Join("\n", CVSLines);
            File.WriteAllText(Application.dataPath + "/out.csv", outText);
		}
    }
#endif
}

/*
 <?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE TS>
<TS version="2.1" language="de_DE">
<context>
    <name>Freemium</name>
    <message>
        <location filename="../qml/Freemium.qml" line="109"/>
        <source>Do you want to unlock Premium Content?</source>
        <translation>Möchtest du die Premium-Inhalte freischalten?</translation>
    </message>
    <message>
        <location filename="../qml/Freemium.qml" line="113"/>
        <source>20 extra figures!</source>
        <translation>20 Extra-Figuren!</translation>
    </message>
    <message>
        <location filename="../qml/Freemium.qml" line="115"/>
        <source>Without ads!</source>
        <translation>Ohne Werbung!</translation>
    </message>
</context>
<context>
    <name>MainMenu</name>
    <message>
        <location filename="../qml/MainMenu.qml" line="173"/>
        <source>My World</source>
        <translation>Meine Welt</translation>
    </message>
    <message>
        <location filename="../qml/MainMenu.qml" line="233"/>
        <source>Create Shapes</source>
        <translation>Erstell Formen</translation>
    </message>
    <message>
        <location filename="../qml/MainMenu.qml" line="371"/>
        <source>Welcome to Pocoyo Shapes! Follow me and I&apos;ll show you</source>
        <translation>Willkommen zu Pocoyo Shapes! Folge mir und ich zeige dir, wie es geht</translation>
    </message>
    <message>
        <location filename="../qml/MainMenu.qml" line="385"/>
        <source>First we&apos;re going to create your first figure.</source>
        <translation>Zuerst werden wir deine erste Figur erstellen.</translation>
    </message>
    <message>
        <location filename="../qml/MainMenu.qml" line="401"/>
        <source>Now let&apos;s decorate the geometric world.</source>
        <translation>Dekorieren wir nun die Welt der Geometrie.</translation>
    </message>
</context>
<context>
    <name>MarketPopUp</name>
    <message>
        <location filename="../qml/common/MarketPopUp.qml" line="140"/>
        <source>BUY</source>
        <translation>Kaufen</translation>
    </message>
    <message>
        <location filename="../qml/common/MarketPopUp.qml" line="160"/>
        <source>LATER</source>
        <translation>Später</translation>
    </message>
</context>
<context>
    <name>MarketResultPopUp</name>
    <message>
        <location filename="../qml/common/MarketResultPopUp.qml" line="47"/>
        <source>Purchase completed successfully.
Enjoy the new content.</source>
        <translation>Der Kauf war erfolgreich.
Viel Spaß mit dem neuen Inhalt.</translation>
    </message>
    <message>
        <location filename="../qml/common/MarketResultPopUp.qml" line="47"/>
        <source>There has been an error in the purchase process</source>
        <translation>Beim Kaufprozess ist ein Fehler aufgetreten</translation>
    </message>
</context>
<context>
    <name>OptionsPopUp</name>
    <message>
        <location filename="../qml/common/OptionsPopUp.qml" line="38"/>
        <source>Terms and Conditions</source>
        <translation>Allgemeine Geschäftsbedingungen</translation>
    </message>
    <message>
        <location filename="../qml/common/OptionsPopUp.qml" line="64"/>
        <source>Privacy Policy</source>
        <translation>Datenschutzpolitik</translation>
    </message>
    <message>
        <location filename="../qml/common/OptionsPopUp.qml" line="90"/>
        <source>More Applications</source>
        <translation>Weitere Anwendungen</translation>
    </message>
    <message>
        <location filename="../qml/common/OptionsPopUp.qml" line="119"/>
        <source>Tutorial</source>
        <translation>Lernprogramm</translation>
    </message>
    <message>
        <location filename="../qml/common/OptionsPopUp.qml" line="212"/>
        <source>CLOSE</source>
        <translation>Schließen</translation>
    </message>
</context>
<context>
    <name>ShapeCollection</name>
    <message>
        <location filename="../qml/ShapeCollection.qml" line="247"/>
        <source>Get all the figures for your world and eliminate advertising.</source>
        <translation>Erhalte alle Figuren für deine Welt und eliminier die Werbung.</translation>
    </message>
    <message>
        <location filename="../qml/ShapeCollection.qml" line="401"/>
        <source>Choose the figure you want to create. For example, this!</source>
        <translation>Wähl die Figur aus, die du erstellen möchtest. Zum Beispiel so!</translation>
    </message>
    <message>
        <location filename="../qml/ShapeCollection.qml" line="421"/>
        <source>Click here to return to the Main Screen.</source>
        <translation>Hier klicken, um zum Startbildschirm zurückzukehren.</translation>
    </message>
</context>
<context>
    <name>ShapeModel</name>
    <message>
        <location filename="../qml/ShapeModel.qml" line="687"/>
        <source>Pine</source>
        <translation>Kiefer</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="688"/>
        <source>Loula</source>
        <translation>Lula</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="689"/>
        <source>Duck</source>
        <translation>Ente</translation>
    </message>
    <message>
        <source>Nina</source>
        <translation type="vanished">Mädchen</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="690"/>
        <source>Whale</source>
        <translation>Wal</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="691"/>
        <source>Pig</source>
        <translation>Schwein</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="692"/>
        <source>Fred</source>
        <translation>Krake</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="693"/>
        <location filename="../qml/ShapeModel.qml" line="698"/>
        <location filename="../qml/ShapeModel.qml" line="718"/>
        <source>Tree</source>
        <translation>Baum</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="694"/>
        <source>Train</source>
        <translation>Zug</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="695"/>
        <source>Balloon</source>
        <translation>Luftballon</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="696"/>
        <source>Pato</source>
        <translation>Pato</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="697"/>
        <source>Palm Tree</source>
        <translation>Palme</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="699"/>
        <source>Baby Bird</source>
        <translation>Babypieps</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="700"/>
        <location filename="../qml/ShapeModel.qml" line="713"/>
        <location filename="../qml/ShapeModel.qml" line="723"/>
        <source>House</source>
        <translation>Haus</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="701"/>
        <source>Lion</source>
        <translation>Löwe</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="702"/>
        <source>Caterpillar</source>
        <translation>Valentina</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="703"/>
        <source>Sleepy Bird</source>
        <translation>Schlummerpieps</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="704"/>
        <source>Elly</source>
        <translation>Elly</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="705"/>
        <source>Pocoyo</source>
        <translation>Pocoyo</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="706"/>
        <source>Cat</source>
        <translation>Katze</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="707"/>
        <source>Penguin</source>
        <translation>Pinguin</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="708"/>
        <source>Panda Bear</source>
        <translation>Pandabär</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="709"/>
        <source>Cow</source>
        <translation>Kuh</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="710"/>
        <source>Toucan</source>
        <translation>Tukan</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="711"/>
        <source>Rock</source>
        <translation>Stein</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="712"/>
        <source>Wind Mill</source>
        <translation>Windmühle</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="714"/>
        <source>Green Alien</source>
        <translation>grüner Außerirdischer</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="715"/>
        <source>Monkey</source>
        <translation>Affe</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="716"/>
        <source>Dog</source>
        <translation>Hund</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="717"/>
        <source>Camp Fire</source>
        <translation>Lagerfeuer</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="719"/>
        <source>Street Lamp</source>
        <translation>Lampe</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="720"/>
        <source>Vamoosh</source>
        <translation>Vamoosh</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="721"/>
        <source>Well</source>
        <translation>Loch</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="722"/>
        <source>Giraffe</source>
        <translation>Giraffe</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="724"/>
        <source>Chicken</source>
        <translation>Huhn</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="725"/>
        <source>Orange Alien</source>
        <translation>orangefarbener Außerirdischer</translation>
    </message>
    <message>
        <location filename="../qml/ShapeModel.qml" line="726"/>
        <source>Plane</source>
        <translation>Flugzeug</translation>
    </message>
</context>
<context>
    <name>ShapeWorld</name>
    <message>
        <location filename="../qml/ShapeWorld.qml" line="76"/>
        <source>Drag the figure you created up to decorate your world.</source>
        <translation>Zieh die Figur nach oben, die du zur Dekoration deiner Welt erstellt hast.</translation>
    </message>
    <message>
        <location filename="../qml/ShapeWorld.qml" line="96"/>
        <source>Erase any figure from your world by dragging it into the trash.</source>
        <translation>Lösch die Figuren, indem du sie zum Papierkorb ziehst.</translation>
    </message>
    <message>
        <location filename="../qml/ShapeWorld.qml" line="206"/>
        <source>You haven´t made this figure yet.
Would you like to make it now?</source>
        <translation>Du hast diese Figur noch nicht erstellt.
Möchtest du sie jetzt machen?</translation>
    </message>
</context>
<context>
    <name>ShapesPage</name>
    <message>
        <location filename="../qml/ShapesPage.qml" line="45"/>
        <source>The image is incomplete. Drag the geometric shapes to the right place.</source>
        <translation>Das Bild ist unvollständig. Zieh die geometrischen Formen an ihren Platz.</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="91"/>
        <source>You can also select the shape and then where you want to put it.</source>
        <translation>Du kannst auch eine Form auswählen und später den Ort, an dem du sie platzieren möchtest.</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="107"/>
        <source>Click here</source>
        <translation>Hier klicken</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="932"/>
        <source>UNKNOW</source>
        <translation></translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="933"/>
        <source>TRIANGLE</source>
        <translation>Dreieck</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="934"/>
        <source>SQUARE</source>
        <translation>Quadrat</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="935"/>
        <source>RECTANGLE</source>
        <translation>Rechteck</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="936"/>
        <source>ROMBUS</source>
        <translation>Raute</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="937"/>
        <source>PENTAGON</source>
        <translation>Fünfeck</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="938"/>
        <source>HEXAGON</source>
        <translation>Sechseck</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="939"/>
        <source>CIRCLE</source>
        <translation>Kreis</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="943"/>
        <source>1 side</source>
        <translation>1 Seite</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="944"/>
        <source>2 sides</source>
        <translation>2 Seiten</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="945"/>
        <source>3 sides</source>
        <translation>3 Seiten</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="946"/>
        <source>4 sides</source>
        <translation>4 Seiten</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="947"/>
        <source>5 sides</source>
        <translation>5 Seiten</translation>
    </message>
    <message>
        <location filename="../qml/ShapesPage.qml" line="948"/>
        <source>6 sides</source>
        <translation>6 Seiten</translation>
    </message>
</context>
</TS>

 */
