using System.Collections.Generic;
using UnityEngine;

public class CategoryReader : MonoBehaviour
{
    public List<Category> categories;

    [SerializeField] private string setupFileName = "categoriesSetup.txt";
    [SerializeField] private string punchlineFileName = "categoriesPunchline.txt";

    public void OnValidate()
    {
        LoadCategories(setupFileName, true);
        LoadCategories(punchlineFileName, false);
    }

    private void LoadCategories(string fileName, bool isSetup)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogError("File name is empty");
            return;
        }

        categories = new List<Category>();
        var path = $"Assets/Resources/{fileName}";

        //read text file, each line is a category
        var lines = System.IO.File.ReadAllLines(path);
        foreach (var line in lines)
        {
            categories.Add(new Category(line, isSetup));
        }
    }
}