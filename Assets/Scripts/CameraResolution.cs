using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    // Set desired resolution
    public Vector2 targetResolution = new Vector2(1920, 1080);

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();

        // Calculate the aspect ratio of the screen.
        float targetAspect = targetResolution.x / targetResolution.y;
        float screenAspect = (float)Screen.width / Screen.height;

        // Calculate the orthographic size or field of view to maintain the target resolution.
        if (mainCamera.orthographic)
        {
            // For orthographic cameras, adjust the orthographic size.
            float newSize = targetResolution.y / 2f;
            mainCamera.orthographicSize = newSize;
        }
        else
        {
            // For perspective cameras, adjust the field of view.
            float newFieldOfView = mainCamera.fieldOfView * (targetAspect / screenAspect);
            mainCamera.fieldOfView = newFieldOfView;
        }
    }
}
