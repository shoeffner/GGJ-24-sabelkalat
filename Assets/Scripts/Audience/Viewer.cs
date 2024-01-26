using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{  
    [SerializeField, Range(1, 5)] private int minNumberOfCategories;
    [SerializeField, Range(5, 10)] private int maxNumberOfCategories;
    [SerializeField, Range(1, 5)] private float minCategoryChangeTime;
    [SerializeField, Range(5, 10)] private float maxCategoryChangeTime;
    [SerializeField] private bool changeCategory = true;
    private Category currentCategory;

    private List<Category> categories;

    private void Start()
    {
        StartCoroutine(ChangeCategory());
    }

    public void SetCategories(List<Category> categories)
    {
        int numberOfCategories = Random.Range(minNumberOfCategories, maxNumberOfCategories);
        for (int i = 0; i < numberOfCategories; i++)
        {
            this.categories.Add(categories[Random.Range(0, categories.Count)]);
        }
        currentCategory = this.categories[Random.Range(0, this.categories.Count)];
    }

    private IEnumerator ChangeCategory()
    {
        while (changeCategory)
        {
            yield return new WaitForSeconds(Random.Range(minCategoryChangeTime, maxCategoryChangeTime));
            currentCategory = categories[Random.Range(0, categories.Count)];
        }
    }
}
