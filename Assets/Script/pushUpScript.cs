using UnityEngine;

public class pushUpScript : MonoBehaviour
{
    public int pushUp = 0;

    private void OnDisable()
    {
        pushUp = 0;
    }
}
