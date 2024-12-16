using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance { get; private set; }

    public List<GameObject> prefabs; // Список префабов

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем объект при смене сцен
        }
        else
        {
            Destroy(gameObject);
        }
    }
}