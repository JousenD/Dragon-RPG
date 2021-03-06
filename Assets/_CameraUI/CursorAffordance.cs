﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    [RequireComponent(typeof(CameraRaycaster))]
    public class CursorAffordance : MonoBehaviour
    {

        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D targetCursor = null;
        [SerializeField] Texture2D unknownCursor = null;

        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;
        //[SerializeField] int stiffLayerNumber = 10;

        CameraRaycaster cameraRayCaster;


        // Use this for initialization
        void Start()
        {
            cameraRayCaster = GetComponent<CameraRaycaster>();
            cameraRayCaster.notifyLayerChangeObservers += OnLayerChange;

        }

        // Update is called once per frame
        void OnLayerChange(int newLayer)
        {
            //print(cameraRayCaster.layerHit);
            switch (newLayer)
            {
                case walkableLayerNumber:
                    Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                    break;

                case enemyLayerNumber:
                    Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                    break;

                default:
                    Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                    return;
            }

        }
    }
}