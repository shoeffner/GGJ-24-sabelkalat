using System.Collections;
using UnityEngine;

public class TransitionManager : Singleton<TransitionManager>
{
    [SerializeField] private Animator sceneTransition;
    [SerializeField] private float transitionTime = 1f;
    
    public void TransitionScenes(System.Action onFinished)
    {
        StartCoroutine(TransitionScenesCoroutine(onFinished)); 
    }

    private IEnumerator TransitionScenesCoroutine(System.Action onFinished)
    {
        sceneTransition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        onFinished?.Invoke();
        sceneTransition.SetTrigger("End");
    }
}
