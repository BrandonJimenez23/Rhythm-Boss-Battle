using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageAsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Color defaultColor = Color.white;
    public Color hoverColor = new Color32(255, 237, 0, 255);  // FFED00
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cambiar el color de la imagen cuando el ratón esté encima
        image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Cambiar el color de la imagen de vuelta al color por defecto cuando el ratón ya no esté encima
        image.color = defaultColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Cambiar el color de la imagen cuando se selecciona con las flechas del teclado
        image.color = hoverColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Cambiar el color de la imagen de vuelta al color por defecto cuando ya no está seleccionada
        image.color = defaultColor;
    }
}

