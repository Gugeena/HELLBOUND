using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverShitScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Glow;
    public bool isHovering = false;
    public bool isReplay, isVisualizer;
    public bool isEntered;

    private void OnDisable()
    {
        if(Glow != null) Glow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        Glow.SetActive(true);
        if (isReplay)
        {
            if (SaveSystem.Load().information.turnedon == 0) Glow.SetActive(true);
            else if (SaveSystem.Load().information.turnedon == 1) Glow.SetActive(false);
        }
        else if(isVisualizer)
        {
            if (SaveSystem.Load().information.visualizer == 0) Glow.SetActive(true);
            else if (SaveSystem.Load().information.visualizer == 1) Glow.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if(isReplay)
        {
            if (SaveSystem.Load().information.turnedon == 1) Glow.SetActive(true);
            else if (SaveSystem.Load().information.turnedon == 0) Glow.SetActive(false);
        }
        else if (isVisualizer)
        {
            if (SaveSystem.Load().information.visualizer == 1) Glow.SetActive(true);
            else if (SaveSystem.Load().information.visualizer == 0) Glow.SetActive(false);
        }
        else Glow.SetActive(false);
    }

    public void OnButtonClick()
    {
        StartCoroutine(buttonclicker());
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
