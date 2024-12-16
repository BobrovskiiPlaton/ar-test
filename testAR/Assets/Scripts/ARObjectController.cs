using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

public class ARObjectController : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    private GameObject placedObject;
    private Renderer objectRenderer;

    void Start()
    {
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager не инициализирован!");
            return;
        }

        // Подписка на изменения значений ползунков
        redSlider.onValueChanged.AddListener(UpdateColor);
        greenSlider.onValueChanged.AddListener(UpdateColor);
        blueSlider.onValueChanged.AddListener(UpdateColor);
    }

    void Update()
    {
        if (Input.touchCount > 0 && raycastManager.raycastPrefab != null)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                PlaceOrMoveObject(touch.position);
            }
        }
    }

    void PlaceOrMoveObject(Vector2 position)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(position, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            Pose hitPose = hits[0].pose;

            if (placedObject == null)
            {
                placedObject = Instantiate(raycastManager.raycastPrefab, hitPose.position, hitPose.rotation);
                InitializeComponents();
                placedObject.SetActive(true);
            }
            else
            {
                placedObject.transform.position = hitPose.position;
            }
        }
    }

    void InitializeComponents()
    {
        if (placedObject != null)
        {
            objectRenderer = placedObject.GetComponent<Renderer>();

            if (objectRenderer != null)
            {
                Color initialColor = objectRenderer.material.color;
                redSlider.value = initialColor.r * 255;
                greenSlider.value = initialColor.g * 255;
                blueSlider.value = initialColor.b * 255;
            }
            UpdateColor(0); // Инициализация начального цвета
        }
    }

    void UpdateColor(float value)
    {
        if (objectRenderer != null)
        {
            float red = redSlider.value / 255f;
            float green = greenSlider.value / 255f;
            float blue = blueSlider.value / 255f;

            Color newColor = new Color(red, green, blue);
            objectRenderer.material.color = newColor;

            Debug.Log("Цвет объекта изменен на: " + newColor);
        }
    }
}
