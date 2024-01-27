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
        CleanUpPreviousRound();
        audience = new List<Viewer>();
        // create viewers evenly per type
        int numberPerViewerType = viewerObjects.Count / viewerTypePrefabs.Count;
        int viewerTypeIndex = 0;
        for (int i = 0; i < viewerObjects.Count; i++)
        {
            if (i != 0 && i % numberPerViewerType == 0)
            {
                viewerTypeIndex++;
            }
            Viewer viewer = Instantiate(viewerTypePrefabs[Mathf.Min(viewerTypePrefabs.Count - 1, viewerTypeIndex)], viewerObjects[i].transform);
            viewer.SetCategories(categoryReader.categories);
            audience.Add(viewer);
        }
    }

    public void CleanUpPreviousRound()
    {
        if (audience != null)
        {
            foreach (var viewerObject in viewerObjects)
            {
                Viewer previousViewer = viewerObject.GetComponentInChildren<Viewer>();
                if (previousViewer != null)
                {
                    LeanTween.scale(previousViewer.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
                    {
                        Destroy(previousViewer.gameObject);
                    });
                }
            }
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

    public List<Viewer> GetViewers()
    {
        return audience;
    }

}