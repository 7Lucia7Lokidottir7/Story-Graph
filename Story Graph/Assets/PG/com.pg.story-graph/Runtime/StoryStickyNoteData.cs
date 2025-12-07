using UnityEngine;
namespace PG.StorySystem
{
    using System;
    public enum StoryStickyNoteTheme
    {
        Classic = 0, // как StickyNoteTheme.Classic
        Black = 1, // как StickyNoteTheme.Black
    }

    public enum StoryStickyNoteFontSize
    {
        Small = 0,
        Medium = 1,
        Large = 2,
        Huge = 3
    }

    [Serializable]
    public class StoryStickyNoteData
    {
        public string guid;
        public string title;
        public string description;
        public Rect position;        // рамка группы
        public string guidGroupNode;


        public StoryStickyNoteTheme theme;
        public StoryStickyNoteFontSize fontSize;
    }
}
