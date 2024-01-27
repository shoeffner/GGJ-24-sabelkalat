using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    [SerializeField, Range(1, 5)] private int minNumberOfCategories;
    [SerializeField, Range(5, 10)] private int maxNumberOfCategories;
    [SerializeField, Range(1, 5)] private float minCategoryChangeTime;
    [SerializeField, Range(5, 10)] private float maxCategoryChangeTime;
    [SerializeField] private bool changeCategory = true;
    [SerializeField] ThinkBubble thinkBubble;
    public Category CurrentCategory { get; private set; }

    private List<Category> categories;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
        while (changeCategory)
        {
            CurrentCategory = categories[Random.Range(0, categories.Count)];
            thinkBubble.SetCategory(CurrentCategory);
            if (CurrentCategory.sounds.Length > 0)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(CurrentCategory.sounds[Random.Range(0, CurrentCategory.sounds.Length)]);
            }
            yield return new WaitForSeconds(Random.Range(minCategoryChangeTime, maxCategoryChangeTime));
        }
    }
}