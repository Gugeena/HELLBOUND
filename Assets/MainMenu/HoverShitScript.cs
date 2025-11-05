using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverShitScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Glow;
    public bool isHovering = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        Glow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        Glow.SetActive(false);
    }

    public void OnButtonClick()
    {
        StartCoroutine(buttonclicker());
        if (!isHovering)
            Glow.SetActive(false);
    }

    public IEnumerator buttonclicker()
    {
        //Glow.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        //EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(0.8f);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
