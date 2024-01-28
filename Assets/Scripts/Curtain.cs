using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : MonoBehaviour
{

    [NotNull]
    public GameObject left;
    [NotNull]
    public GameObject right;

    public Transform closedTransformLeft;
    public Transform closedTransformRight;
    public Transform openTransformLeft;
    public Transform openTransformRight;

    public float moveDuration = 1;

    public bool openOnStart = true;
    public float delayBeforeStartOpen = 1;

    void Start()
    {
        /*openTransformLeft = left.transform.position;
        openTransformRight = right.transform.position;*/
        if (openOnStart)
        {
            Invoke("Open", delayBeforeStartOpen);
        }
    }

    public void Open()
    {
        Debug.Log($"Open to {openTransformLeft.position}");
        LeanTween.move(left, openTransformLeft.position, moveDuration).setEaseInQuad();
        LeanTween.move(right, openTransformRight.position, moveDuration).setEaseInQuad();
    }

    public void Close()
    {
        Debug.Log($"Close to {closedTransformLeft.position}");
        LeanTween.move(left, closedTransformLeft.position, moveDuration).setEaseOutQuad();
        LeanTween.move(right, closedTransformRight.position, moveDuration).setEaseOutQuad();
    }


}
