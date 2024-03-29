﻿using System.Collections.Generic;
using UnityEngine;

public class CategoryReader : MonoBehaviour
{
    public List<Category> categories;

    [SerializeField] private string setupFileName = "categoriesSetup.txt";
    [SerializeField] private string punchlineFileName = "categoriesPunchline.txt";

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (Application.isPlaying)
        {
            return;
        }
        categories = new List<Category>();
        LoadCategories(setupFileName, true);
        LoadCategories(punchlineFileName, false);
    }
#endif

    private void LoadCategories(string fileName, bool isSetup)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogError("File name is empty");
            return;
        }

        var path = $"Assets/Resources/{fileName}";

        //read text file, each line is a category
        var lines = System.IO.File.ReadAllLines(path);
        foreach (var line in lines)
        {
            categories.Add(new Category(line, isSetup));
        }
    }


    public Category GetCategoryByName(string name)
    {
        foreach (var category in categories)
        {
            if (category.name == name)
            {
                return category;
            }
        }
        Debug.LogError($"Could not find category for {name}!");
        return categories[0];
    }
}