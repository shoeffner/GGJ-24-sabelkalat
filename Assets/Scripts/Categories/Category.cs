using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Category
{
    public string name;
    public bool isSetup;
    public Sprite icon;
    public AudioClip[] sounds;

    public Category(string name, bool isSetup)
    {
        this.name = name;
        this.isSetup = isSetup;
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
