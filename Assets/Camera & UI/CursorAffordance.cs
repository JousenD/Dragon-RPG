using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Texture2D unknownCursor = null;

    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

    CameraRaycaster cameraRayCaster;
    

	// Use this for initialization
	void Start () {
        cameraRayCaster = GetComponent<CameraRaycaster>();
        cameraRayCaster.onLayerChange += OnLayerChange;

    }
	
	// Update is called once per frame
	void OnLayerChange(Layer newLayer) {
        //print(cameraRayCaster.layerHit);
        switch (newLayer)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;

            case Layer.Enemy:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;

            case Layer.RaycastEndStop:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                break;

            default:
                Debug.LogError("Don't know which cursor to show");
                return;
        }

	}
}
