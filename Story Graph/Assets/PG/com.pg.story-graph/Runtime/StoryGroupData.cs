using System.Collections.Generic;
using UnityEngine;
namespace PG.StorySystem
{
    using System;

    [Serializable]
    public class StoryGroupData
    {
        public string guid;
        public string title;
        public Rect position;        // рамка группы
        public List<string> nodeGuids = new();   // какие ноды внутри
        public string guidGroupNode;
    }
}
