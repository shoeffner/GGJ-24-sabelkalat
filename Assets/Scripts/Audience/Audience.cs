using System.Collections.Generic;
using UnityEngine;

public class Audience : MonoBehaviour
{
    [SerializeField] private List<GameObject> viewerObjects;
    [SerializeField] CategoryReader categoryReader;

    private List<Viewer> audience;
    private List<Viewer> viewerTypePrefabs;

    public void SetNewViewers(List<Viewer> viewerTypePrefabs)
    {
        this.viewerTypePrefabs = viewerTypePrefabs;
        SetupAudience();
    }

    private void SetupAudience()
    {
        if (viewerTypePrefabs.Count == 0 || viewerObjects.Count < viewerTypePrefabs.Count)
        {
            Debug.LogError("Check viewers in audience object");
            return;
        }
        audience = new List<Viewer>();
        // create viewers evenly per type
        int numberPerViewerType = viewerObjects.Count / viewerTypePrefabs.Count;
        Debug.Log(numberPerViewerType);
        int viewerTypeIndex = 0;
        for (int i = 0; i < viewerObjects.Count; i++)
        {
            if (i != 0 && i % numberPerViewerType == 0)
            {
                viewerTypeIndex++;
            }
            Viewer viewer = Instantiate(viewerTypePrefabs[Mathf.Min(viewerTypePrefabs.Count -1 ,viewerTypeIndex)], viewerObjects[i].transform);
            viewer.SetCategories(categoryReader.categories);
            audience.Add(viewer);
        }
    }

    public List<Category> GetCurrentAudienceCategories()
    {
        List<Category> categories = new List<Category>();
        foreach (var viewer in audience)
        {
            categories.Add(viewer.CurrentCategory);
        }
        return categories;
    }

}