using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class JigsawPiece : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{

    [Header("Piece Settings")]
    [SerializeField] private int pieceID;

    [SerializeField] private float snapDistance = 50f;

    [SerializeField] private float snapSpeed = 10f;


    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private JigsawManager manager;

    private Vector3 dragOffset;

    private bool locked = false;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }


    private void Start()
    {
        manager = FindFirstObjectByType<JigsawManager>();

        if (manager == null)
        {
            Debug.LogError("No JigsawManager found!");
        }
    }


    public int GetPieceID()
    {
        return pieceID;
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        if (locked)
            return;


        canvasGroup.blocksRaycasts = false;


        Vector3 mouseWorldPosition;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out mouseWorldPosition
        );


        dragOffset =
            rectTransform.position - mouseWorldPosition;


        transform.SetAsLastSibling();
    }



    public void OnDrag(PointerEventData eventData)
    {
        if (locked)
            return;


        Vector3 mouseWorldPosition;


        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out mouseWorldPosition))
        {

            rectTransform.position =
                mouseWorldPosition + dragOffset;

        }
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        if (locked)
            return;


        canvasGroup.blocksRaycasts = true;


        SnapPoint target =
            manager.GetSnapPoint(pieceID);


        if (target == null)
            return;



        float distance =
            Vector3.Distance(
                rectTransform.position,
                target.transform.position
            );



        if (distance <= snapDistance)
        {
            StartCoroutine(SnapIntoPlace(target));
        }
    }



    private IEnumerator SnapIntoPlace(SnapPoint target)
    {
        locked = true;


        Vector3 start =
            rectTransform.position;


        Vector3 end =
            target.transform.position;


        float time = 0;


        while (time < 1)
        {
            time += Time.deltaTime * snapSpeed;


            rectTransform.position =
                Vector3.Lerp(
                    start,
                    end,
                    time
                );


            yield return null;
        }


        rectTransform.position = end;

        // Put completed pieces behind unsolved pieces
        transform.SetSiblingIndex(0);

        manager.PieceCompleted();
    }

}