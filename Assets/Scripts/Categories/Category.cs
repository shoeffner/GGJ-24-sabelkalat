using System;
using UnityEngine;

public class Category
{
    public string name;
    public Sprite icon;

    public Category(string name)
    {
        //read icon from resources
        icon = Resources.Load<Sprite>($"Icons/{name}");
        if (icon == null)
        {
            Debug.LogError($"Icon for category {name} not found");
        }
    }
}
