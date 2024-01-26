using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        RegisterButtonSounds();
    }

    private void RegisterButtonSounds()
    {
        EventTrigger.Entry eventtype = new EventTrigger.Entry();
        eventtype.eventID = EventTriggerType.PointerEnter;
        eventtype.callback.AddListener((eventData) => audioSource.PlayOneShot(hoverSound));

        foreach (Button b in FindObjectsOfType<Button>(true))
        {
            b.onClick.AddListener(() => audioSource.PlayOneShot(clickSound));
            b.gameObject.AddComponent<EventTrigger>();
            b.gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);
        }
    }
}
