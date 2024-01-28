using System.Collections.Generic;
using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    [SerializeField] private GameObject thinkBubbleHolder;
    [SerializeField] private List<AudioClip> closeBubbleSounds;
    private Vector3 thinkBubbleHolderScale;
    private SpriteRenderer spriteRenderer;
    private Category nextCategory;
    private AudioSource audioSource;
    private float soundVolumeDiff = 0.3f;
    private int soundIndex = 0;

    public void SetCategory(Category category)
    {
        nextCategory = category;
        ChangeThinkBubble();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        thinkBubbleHolderScale = thinkBubbleHolder.transform.localScale;
        thinkBubbleHolder.transform.localScale = Vector3.zero;
    }

    public void ChangeThinkBubble()
    {
        if (soundIndex != 0) { PlayCloseBubbleSound(); }
        soundIndex++;
        LeanTween.scale(thinkBubbleHolder, Vector3.zero, 0.5f).setEaseInBack().setOnComplete(() =>
        {
            spriteRenderer.sprite = nextCategory.icon;
            ShowCategeory();
        });
    }
    public void ShowCategeory()
    {
        var delay = 1f;
        PlayCategorySound(delay);
        LeanTween.scale(thinkBubbleHolder, thinkBubbleHolderScale, 0.5f).setEaseOutBack().setDelay(delay);
    }

    private void PlayCloseBubbleSound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(closeBubbleSounds[Random.Range(0, closeBubbleSounds.Count)], audioSource.volume - soundVolumeDiff);
    }

    public void PlayCategorySound(float delay)
    {
        if (nextCategory.sounds.Length > 0)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.clip = nextCategory.sounds[Random.Range(0, nextCategory.sounds.Length)];
            audioSource.PlayDelayed(delay);
        }
    }
}