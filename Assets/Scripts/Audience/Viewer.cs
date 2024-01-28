using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    public bool ChangesCategory { get; set; } = true;
    [SerializeField, Range(1, 14)] private int minNumberOfCategories;
    [SerializeField, Range(1, 14)] private int maxNumberOfCategories;
    [SerializeField, Range(1, 60)] private float minCategoryChangeTime;
    [SerializeField, Range(1, 60)] private float maxCategoryChangeTime;
    [SerializeField] ThinkBubble thinkBubble;
    public Category CurrentCategory { get; private set; }

    private List<Category> categories;

    private void Start()
    {
        StartCoroutine(ChangeCategory());
    }

    public void SetCategories(List<Category> categories)
    {
        this.categories = new();
        int numberOfCategories = Random.Range(minNumberOfCategories, maxNumberOfCategories);
        for (int i = 0; i < numberOfCategories; i++)
        {
            this.categories.Add(categories[Random.Range(0, categories.Count)]);
        }
        CurrentCategory = this.categories[Random.Range(0, this.categories.Count)];
    }

    private IEnumerator ChangeCategory()
    {
        while (ChangesCategory)
        {
            CurrentCategory = categories[Random.Range(0, categories.Count)];
            thinkBubble.SetCategory(CurrentCategory);
            yield return new WaitForSeconds(Random.Range(minCategoryChangeTime, maxCategoryChangeTime));
        }
    }
}
