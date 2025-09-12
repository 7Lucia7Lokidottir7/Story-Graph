using System.Collections.Generic;
using UnityEngine;

public class HandsContainer : MonoBehaviour
{
    [SerializeField] private HandElement[] _hands;
    public enum HandType { Both, Left, Right };

    [System.Serializable]
    public class HandElement
    {
        public string name;
        public GameObject leftHand;
        public GameObject rightHand;
    }
    public Dictionary<string, HandElement> hands = new Dictionary<string, HandElement>();
    private void Awake()
    {
        for (int i = 0; i < _hands.Length; i++)
        {
            hands.Add(_hands[i].name, _hands[i]);
        }
    }
    public void SetHands(string handName, HandType handType)
    {
        if (hands.TryGetValue(handName, out HandElement handElement))
        {
            switch (handType)
            {
                case HandType.Both:
                    for (int i = 0; i < _hands.Length; i++)
                    {
                        _hands[i].rightHand.SetActive(false);
                        _hands[i].leftHand.SetActive(false);
                    }
                    handElement.rightHand.SetActive(true);
                    handElement.leftHand.SetActive(true);
                    break;
                case HandType.Left:
                    for (int i = 0; i < _hands.Length; i++)
                    {
                        _hands[i].leftHand.SetActive(false);
                    }
                    handElement.leftHand.SetActive(true);
                    break;
                case HandType.Right:
                    for (int i = 0; i < _hands.Length; i++)
                    {
                        _hands[i].rightHand.SetActive(false);
                    }
                    handElement.rightHand.SetActive(true);
                    break;
            }
        }
    }
}
