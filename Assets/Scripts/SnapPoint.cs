using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    [SerializeField] private int snapID;

    public int SnapID => snapID;

    public Vector2 Position
    {
        get
        {
            RectTransform rect = GetComponent<RectTransform>();

            return rect.anchoredPosition;
        }
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        gameObject.name = $"SnapPoint {snapID}";
    }
#endif
}