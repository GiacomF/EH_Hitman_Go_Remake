using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color hoverColor;
    public TMP_Text theText;
    private Color defaultColor;

    private void Start()
    {
        defaultColor = theText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        theText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = defaultColor;
    }

    private void OnDisable()
    {
        theText.color = defaultColor;
    }
}