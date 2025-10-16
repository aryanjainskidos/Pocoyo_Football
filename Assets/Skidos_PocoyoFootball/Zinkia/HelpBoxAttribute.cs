using UnityEngine;
public class HelpBoxAttribute : PropertyAttribute
{
    public readonly string helpMessage;
    public readonly float helpHeight;

    public HelpBoxAttribute(string helpMessage, float height = 60f)
    {
        this.helpMessage = helpMessage;
        this.helpHeight = height;
    }
}
