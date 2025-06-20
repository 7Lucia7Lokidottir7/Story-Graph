using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXInvoker : MonoBehaviour
{
    [SerializeField] private VFXElement[] _visualEffects;
    public Dictionary<string, VisualEffect> visualEffects = new Dictionary<string, VisualEffect>();
    [System.Serializable]
    public class VFXElement
    {
        public VisualEffect visualEffect;
        public string name;
    }
    private void Start()
    {
        for (int i = 0; i < _visualEffects.Length; i++)
        {
            visualEffects.Add(_visualEffects[i].name, _visualEffects[i].visualEffect);
        }
    }
    public void OnInvoke(int value)
    {
        _visualEffects[value].visualEffect.Play();
    }
    public VisualEffect GetParticleSystem(int value)
    {
        return _visualEffects[value].visualEffect;
    }
}
