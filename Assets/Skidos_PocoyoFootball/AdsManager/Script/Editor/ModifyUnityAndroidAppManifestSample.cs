#if UNITY_EDITOR
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor.Android;

public class ModifyUnityAndroidAppManifestSample : IPostGenerateGradleAndroidProject
{
    public void OnPostGenerateGradleAndroidProject(string basePath)
    {
        // If needed, add condition checks on whether you need to run the modification routine.
        // For example, specific configuration/app options enabled

        var androidManifest = new AndroidManifest(GetManifestPath(basePath));

        // For API 31+ support (Need explicit exported flag for entries having intent-filter tags)
        androidManifest.SetStartingActivityAttribute("exported", "true");

        // Add your XML manipulation routines
        androidManifest.SetMetaData("/manifest/application", "com.google.android.gms.ads.APPLICATION_ID", Admob.AppId);
        androidManifest.SetRemoveGooglePlayAAID();
        //< uses - permission android: name = "com.google.android.gms.permission.AD_ID" tools: node = "remove" />

        androidManifest.Save();
    }

    public int callbackOrder { get { return 1; } }

    private string _manifestFilePath;

    private string GetManifestPath(string basePath)
    {
        if (string.IsNullOrEmpty(_manifestFilePath))
        {
            var pathBuilder = new StringBuilder(basePath);
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
            _manifestFilePath = pathBuilder.ToString();
        }
        return _manifestFilePath;
    }
}


internal class AndroidXmlDocument : XmlDocument
{
    private string m_Path;
    protected XmlNamespaceManager nsMgr;
    public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
    public readonly string ToolsXmlNamespace = "http://schemas.android.com/tools";

    public AndroidXmlDocument(string path)
    {
        m_Path = path;
        using (var reader = new XmlTextReader(m_Path))
        {
            reader.Read();
            Load(reader);
        }
        nsMgr = new XmlNamespaceManager(NameTable);
        nsMgr.AddNamespace("android", AndroidXmlNamespace);
    }

    public string Save()
    {
        return SaveAs(m_Path);
    }

    public string SaveAs(string path)
    {
        using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
        {
            writer.Formatting = Formatting.Indented;
            Save(writer);
        }
        return path;
    }
}


internal class AndroidManifest : AndroidXmlDocument
{
    private readonly XmlElement ApplicationElement;

    public AndroidManifest(string path) : base(path)
    {
        ApplicationElement = SelectSingleNode("/manifest/application") as XmlElement;
    }

    private XmlAttribute CreateAndroidAttribute(string key, string value)
    {
        XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
        attr.Value = value;
        return attr;
    }

    private XmlAttribute CreateToolsAttribute(string key, string value)
    {
        XmlAttribute attr = CreateAttribute("tools", key, ToolsXmlNamespace);
        attr.Value = value;
        return attr;
    }

    internal XmlNode GetActivityWithLaunchIntent()
    {
        return SelectSingleNode("/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", nsMgr);
    }

    internal void SetApplicationTheme(string appTheme)
    {
        ApplicationElement.Attributes.Append(CreateAndroidAttribute("theme", appTheme));
    }

    internal void SetStartingActivityName(string activityName)
    {
        GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("name", activityName));
    }


    internal void SetHardwareAcceleration()
    {
        GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("hardwareAccelerated", "true"));
    }

    internal void SetMicrophonePermission()
    {
        var manifest = SelectSingleNode("/manifest");
        XmlElement child = CreateElement("uses-permission");
        manifest.AppendChild(child);
        XmlAttribute newAttribute = CreateAndroidAttribute("name", "android.permission.RECORD_AUDIO");
        child.Attributes.Append(newAttribute);
    }

    internal void SetRemoveGooglePlayAAID()
    {
        var manifest = SelectSingleNode("/manifest");
        XmlElement child = CreateElement("uses-permission");
        XmlAttribute newAttribute = CreateAndroidAttribute("name", "com.google.android.gms.permission.AD_ID");
        XmlAttribute newAttribute2 = CreateToolsAttribute("node", "remove");

        child.Attributes.Append(newAttribute);
        child.Attributes.Append(newAttribute2);

        manifest.AppendChild(child);
    }

    internal void SetMetaData(string node, string name, string value)
    {
        var manifest = SelectSingleNode(node);
        XmlElement child = CreateElement("meta-data");
        manifest.AppendChild(child);
        XmlAttribute newAttribute = CreateAndroidAttribute("name", name);
        XmlAttribute newAttribute2 = CreateAndroidAttribute("value", value);

        child.Attributes.Append(newAttribute);
        child.Attributes.Append(newAttribute2);
    }

    internal void SetStartingActivityAttribute(string key, string value)
    {
        XmlNode node = GetActivityWithLaunchIntent();
        if (node != null)
        {
            XmlAttributeCollection attributes = node.Attributes;
            attributes.Append(CreateAndroidAttribute(key, value));
        }
    }
}

#endif