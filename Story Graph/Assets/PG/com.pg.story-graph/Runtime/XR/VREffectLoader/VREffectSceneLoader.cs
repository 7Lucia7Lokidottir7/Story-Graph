using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class VREffectSceneLoader : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _duration = 0.25f;
    public float duration => _duration;
    public static VREffectSceneLoader instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += StartLoad;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= StartLoad;
    }
    public void StartUnload()
    {
        if (Camera.main != null)
        {
            _renderer.transform.position = Camera.main.transform.position;
        }
        StartCoroutine(CrossFadeAlpha(1f, 0f));
    }
    public void StartLoad()
    {
        if (Camera.main != null)
        {
            _renderer.transform.position = Camera.main.transform.position;
        }
        StartCoroutine(CrossFadeAlpha(0f, 1f));
    }
    public void StartLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (Camera.main != null)
        {
            _renderer.transform.position = Camera.main.transform.position;
        }
        StartCoroutine(CrossFadeAlpha(0f, 1f));
    }
    IEnumerator CrossFadeAlpha(float alpha)
    {
        Color color = _renderer.material.color;
        for (float i = 0; i < _duration; i += Time.deltaTime)
        {
            _renderer.material.color = Color.Lerp(color, new Color(color.r, color.g, color.b, alpha), i / _duration);
            yield return null;
        }
        _renderer.material.color = new Color(color.r, color.g, color.b, alpha);
    }
    IEnumerator CrossFadeAlpha(float alpha, float from)
    {
        Color color = _renderer.material.color;
        for (float i = 0; i < _duration; i += Time.deltaTime)
        {
            _renderer.material.color = Color.Lerp(new Color(color.r, color.g, color.b, from),new Color(color.r, color.g, color.b, alpha), i / _duration);
            yield return null;
        }
        _renderer.material.color = new Color(color.r, color.g, color.b, alpha);
    }

}
