using System;
using System.Collections.Generic;
using UnityEngine;

public class Category
{
    public string name;
    public Sprite icon;
    public AudioClip [] sounds;

    public Category(string name)
    {
        this.name = name;
        //read icon from resources
        icon = Resources.Load<Sprite>($"Icons/{name}");
        if (icon == null)
        {
            Debug.LogError($"Icon for category {name} not found");
        }
        //read sounds from resources
        sounds = Resources.LoadAll<AudioClip>($"CategorySounds/{name}");
    }
}
