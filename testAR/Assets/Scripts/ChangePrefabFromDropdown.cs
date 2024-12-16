using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using System.Collections.Generic;

public class ChangePrefabFromDropdown : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public ARRaycastManager raycastManager;
    public TMP_Dropdown prefabDropdown;

    void Start()
    {
        // Получаем список префабов из ObjectManager
        List<GameObject> prefabs = ObjectManager.Instance.prefabs;
        
        // Заполняем список выпадающего меню именами префабов
        List<string> prefabNames = new List<string>();
        foreach (GameObject prefab in prefabs)
        {
            prefabNames.Add(prefab.name);
        }
        prefabDropdown.ClearOptions();
        prefabDropdown.AddOptions(prefabNames);

        
        // Привязываем метод к изменению выбора в выпадающем списке
        prefabDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        

        // Получаем список префабов из ObjectManager
        List<GameObject> prefabs = ObjectManager.Instance.prefabs;

        // Меняем префаб в ARTrackedImageManager
        trackedImageManager.trackedImagePrefab = prefabs[index];
        raycastManager.raycastPrefab = prefabs[index];
        //Debug.Log("Tracked Image Prefab changed to: " + prefabs[index].name);

        // Обновляем уже размещенные объекты
        foreach (var trackedImage in trackedImageManager.trackables)
        {
            UpdateTrackedImage(trackedImage, prefabs[index]);
        }
    }

    void UpdateTrackedImage(ARTrackedImage trackedImage, GameObject newPrefab)
    {
        if (trackedImage.referenceImage != null)
        {
            // Удаляем старый префаб
            foreach (Transform child in trackedImage.transform)
            {
                Destroy(child.gameObject);
            }

            // Спавним новый префаб
            GameObject instantiatedObject = Instantiate(newPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
            instantiatedObject.transform.parent = trackedImage.transform;
        }
    }
}
