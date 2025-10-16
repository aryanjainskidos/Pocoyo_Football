/// Credit Titinious (https://github.com/Titinious)
/// Sourced from - https://github.com/Titinious/CurlyUI
#if UNITY_EDITOR
using UnityEditor;

namespace UnityEngine.UI.Extensions
{
    [CustomEditor(typeof(CUIText))]
    public class CUITextEditor : CUIGraphicEditor { }
}

#endif