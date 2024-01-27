using System.Collections.Generic;
using UnityEngine;

public class CategoryReader : MonoBehaviour
{
    public List<Category> categories;

    [SerializeField] private string fileName = "categories.txt";

    public void OnValidate()
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
            categories.Add(new Category(line));
        }
    }
}