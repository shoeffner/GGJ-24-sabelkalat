using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    [SerializeField] private GameObject thinkBubbleHolder;
    private Vector3 thinkBubbleHolderScale;
    private SpriteRenderer categoryIcon;
    private Category nextCategory;
    private AudioSource audioSource;

    public void SetCategory(Category category)
    {
        nextCategory = category;
        ChangeThinkBubble();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        categoryIcon = GetComponent<SpriteRenderer>();
        thinkBubbleHolderScale = thinkBubbleHolder.transform.localScale;
        thinkBubbleHolder.transform.localScale = Vector3.zero;
    }

    public void ChangeThinkBubble()
    {
        LeanTween.scale(thinkBubbleHolder, Vector3.zero, 0.5f).setEaseInBack().setOnComplete(() =>
        {
            categoryIcon.sprite = nextCategory.icon;
            ShowCategeory();
        });
    }
    public void ShowCategeory()
    {
        var delay = 1f;
        PlaySound(delay);
        LeanTween.scale(thinkBubbleHolder, thinkBubbleHolderScale, 0.5f).setEaseOutBack().setDelay(delay);
    }

    public void PlaySound(float delay)
    {
        if (nextCategory.sounds.Length > 0)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.clip = nextCategory.sounds[Random.Range(0, nextCategory.sounds.Length)];
            audioSource.PlayDelayed(delay);
        }
    }
}