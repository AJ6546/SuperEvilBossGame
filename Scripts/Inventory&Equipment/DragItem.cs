using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
     where T : class
{
    Vector3 startPosition;
    Transform originalParent;
    IDragSource<T> source;
    Canvas parentCanvas;
    void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        source = GetComponentInParent<IDragSource<T>>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        originalParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.SetParent(parentCanvas.transform, true);
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.SetParent(originalParent, true);
        IDragDestination<T> container;
        if (!IsPointerOverUIObject())
        {
            container = parentCanvas.GetComponent<IDragDestination<T>>();
        }
        else
        {
            container = GetContainer(eventData);
        }
        if (container != null)
        {
            DropItemIntoContainer(container);
        }
    }
    private void DropItemIntoContainer(IDragDestination<T> destination)
    {
        if (object.ReferenceEquals(destination, source)) return;
        var destinationContainer = destination as IDragContainer<T>;
        var sourceContainer = source as IDragContainer<T>;
        if (destinationContainer == null || sourceContainer == null || destinationContainer.GetItem() == null ||
            object.ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
        {
            AttemptSimpleTransfer(destination);
            return;
        }
        AttemptSwap(destinationContainer, sourceContainer);
    }
    void AttemptSwap(IDragContainer<T> destination, IDragContainer<T> source)
    {
        var removedSourceNumber = source.GetNumber();
        var removedSourceItem = source.GetItem();
        var removedDestinationNumber = destination.GetNumber();
        var removedDestinationItem = destination.GetItem();

        source.RemoveItem(removedSourceNumber);
        destination.RemoveItem(removedDestinationNumber);

        if (removedDestinationNumber > 0)
        {
            source.AddItems(removedDestinationItem, removedDestinationNumber);
        }
        if (removedSourceNumber > 0)
        {
            destination.AddItems(removedSourceItem, removedSourceNumber);
        }
    }


    IDragDestination<T> GetContainer(PointerEventData eventData)
    {
        if (eventData.pointerEnter)
        {
            var container = eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>();
            return container;
        }
        return null;
    }

    bool AttemptSimpleTransfer(IDragDestination<T> destination)
    {
        var draggingItem = source.GetItem();
        var draggingNumber = source.GetNumber();
        var acceptable = destination.MaxAcceptable(draggingItem);
        var toTransfer = Mathf.Min(acceptable, draggingNumber);
        if (toTransfer > 0)
        {
            source.RemoveItem(toTransfer);
            destination.AddItems(draggingItem, toTransfer);
            return false;
        }
        return true;
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
