using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance =>
        _instance ? _instance : (_instance = FindObjectOfType<T>() ?? Instantiate(_instance));

    private static T _instance;

    private void OnDestroy()
    {
        _instance = null;
    }
}