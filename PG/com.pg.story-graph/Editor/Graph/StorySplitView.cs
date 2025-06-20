using UnityEngine;

using UnityEngine.UIElements;

//For Unity 6 and later
#if UNITY_6000_0_OR_NEWER
[UxmlElement]
#endif
public sealed partial class StorySplitView : TwoPaneSplitView
{
    //For Unity 2022 LTS and older
#if !UNITY_6000_0_OR_NEWER
    public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> { }
#endif
    //public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> { }
}
