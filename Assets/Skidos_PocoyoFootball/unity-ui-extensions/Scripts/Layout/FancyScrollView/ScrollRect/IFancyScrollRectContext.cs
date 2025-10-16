/// Credit setchi (https://github.com/setchi)
/// Sourced from - https://github.com/setchi/FancyScrollView

using System;

namespace UnityEngine.UI.Extensions_Football
{
    /// <summary>
    /// <see cref="FancyScrollRect{TItemData, TContext}"/> のコンテキストインターフェース.
    /// </summary>
    public interface IFancyScrollRectContext
    {
        ScrollDirection ScrollDirection { get; set; }
        Func<(float ScrollSize, float ReuseMargin)> CalculateScrollSize { get; set; }
    }
}