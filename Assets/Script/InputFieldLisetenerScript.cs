using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldLisetenerScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Image sp;

    public void OnSelect(BaseEventData eventData)
    {
        sp.color = Color.red;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        sp.color = new Color(126f/255f, 0, 0);
    }
}
