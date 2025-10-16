/// Credit setchi (https://github.com/setchi)
/// Sourced from - https://github.com/setchi/FancyScrollView

using System;

using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UI.Extensions;

    /// <summary>
    /// <see cref="FancyScrollRect{TItemData, TContext}"/> のコンテキスト基底クラス.
    /// </summary>
    public class FancyScrollRectContext : IFancyScrollRectContext
    {
        ScrollDirection IFancyScrollRectContext.ScrollDirection { get; set; }
        Func<(float ScrollSize, float ReuseMargin)> IFancyScrollRectContext.CalculateScrollSize { get; set; }
    }
