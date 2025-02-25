using UnityEngine;
using UnityEngine.EventSystems;

public class BlockInput : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        eventData.Use(); 
    }
}
