using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIRaycastTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                Debug.Log("UI Hit: " + result.gameObject.name);
            }
        }
    }
}
