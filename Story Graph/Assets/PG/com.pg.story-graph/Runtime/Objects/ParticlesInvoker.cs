using System.Collections.Generic;
using UnityEngine;

public class ParticlesInvoker : MonoBehaviour
{
    [SerializeField] private ParticleElement[] _particles;
    public Dictionary<string, ParticleSystem> particles = new Dictionary<string, ParticleSystem>();
    [System.Serializable]
    public class ParticleElement
    {
        public ParticleSystem particleSystem;
        public string name;
    }
    private void Start()
    {
        for (int i = 0; i < _particles.Length; i++)
        {
            particles.Add(_particles[i].name, _particles[i].particleSystem);
        }
    }
    public void OnInvoke(int value)
    {
        _particles[value].particleSystem.Play();
    }
    public ParticleSystem GetParticleSystem(int value)
    {
        return _particles[value].particleSystem;
    }
}
