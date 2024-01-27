using System.Collections.Generic;
using UnityEngine;

public class Audience : MonoBehaviour
{
    [SerializeField] private List<GameObject> viewerObjects;
    [SerializeField] private List<Viewer> viewerTypePrefabs;
    [SerializeField] CategoryReader categoryReader;

    private List<Viewer> audience;

    private void Awake()
    {
        if(viewerTypePrefabs.Count == 0 || viewerObjects.Count < viewerTypePrefabs.Count)
        {
            Debug.LogError("Check viewers in audience object");
            return;
        }
        audience = new List<Viewer>();
        // create viewers evenly per type
        int numberPerViewerType = viewerObjects.Count/ viewerTypePrefabs.Count;
        Debug.Log("number per viewer type: " + numberPerViewerType);
        int viewerTypeIndex = 0;
        for (int i = 0; i < viewerObjects.Count; i++)
        {
            if(i != 0 && i % numberPerViewerType == 0)
            {
                viewerTypeIndex++;
                Debug.Log("current viewer: " + i + " change viewer type to " + viewerTypeIndex);
            }
            Viewer viewer = Instantiate(viewerTypePrefabs[viewerTypeIndex], viewerObjects[i].transform);
            viewer.SetCategories(categoryReader.categories);
            audience.Add(viewer);
        }
    }

    public void AddViewer(Viewer viewer)
    {
        audience.Add(viewer);
    }


}
