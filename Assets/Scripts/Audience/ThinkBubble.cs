using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    [SerializeField] private GameObject thinkBubbleHolder;
    private Vector3 thinkBubbleHolderScale;
    private SpriteRenderer categoryIcon;
    private Category nextCategory;
    

    public void SetCategory(Category category)
    {
        nextCategory = category;
        ChangeThinkBubble();
    }

    private void Awake()
    {
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
        LeanTween.scale(thinkBubbleHolder, thinkBubbleHolderScale, 0.5f).setEaseOutBack().setDelay(1f);
    }
}