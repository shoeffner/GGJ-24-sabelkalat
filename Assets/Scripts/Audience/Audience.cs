using System.Collections.Generic;
using UnityEngine;

public class Audience : MonoBehaviour
{
    [SerializeField] private List<Viewer> viewerTypePrefabs;
    [SerializeField] CategoryReader categoryReader;
    [SerializeField] private int numberOfViewers;

    private List<Viewer> audience;

    private void Awake()
    {
        audience = new List<Viewer>();
        if (viewerTypePrefabs.Count == 0) return;
        // create viewers evenly per type
        int numberPerViewerType = numberOfViewers/viewerTypePrefabs.Count;
        int viewerTypeIndex = 0;
        for (int i = 0; i < numberOfViewers; i++)
        {
            if(i != 0 && i % numberPerViewerType == 0)
            {
                viewerTypeIndex++;
            }
            Viewer viewer = Instantiate(viewerTypePrefabs[viewerTypeIndex], transform);
            viewer.SetCategories(categoryReader.categories);
            audience.Add(viewer);
        }
    }

    public void AddViewer(Viewer viewer)
    {
        audience.Add(viewer);
    }


}
