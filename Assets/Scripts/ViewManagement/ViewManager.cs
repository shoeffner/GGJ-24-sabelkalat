using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewManager : Singleton<ViewManager>
{
    [SerializeField] private View _startingView;
    [SerializeField] private Camera _camera;

    //all views
    [SerializeField] private View[] _views;

    private View _currentView;

    //history of opend views to go back
    private readonly Stack<View> _history = new Stack<View>();

    //get specific view
    public T GetView<T>() where T : View
    {
        for (int i = 0; i < _views.Length; i++)
        {
            if (_views[i] is T tView)
            {
                return tView;
            }
        }

        return null;
    }

    public void Show<T>(bool remember = true) where T : View
    {
        for (int i = 0; i < _views.Length; i++)
        {
            if (_views[i] is T tView)
            {
                RememberAndShow(remember, tView);
                break;
            }
        }
    }

    public void Show(View view, bool remember = true)
    {
        RememberAndShow(remember, view);
    }

    //removes last view from Stack and shows it
    public void ShowLast()
    {
        if (_history.Count != 0)
        {
            Show(_history.Pop(), false);
        }
    }

    private void Start()
    {
        //clear duplicates
        _views = _views.Distinct().ToArray<View>();

        for (int i = 0; i < _views.Length; i++)
        {
            _views[i].Initialize();

            _views[i].Hide();
        }

        if (_startingView != null)
        {
            Show(_startingView, true);
        }
    }

    private void RememberAndShow<T>(bool remember, T view) where T : View
    {
        if (_currentView != null)
        {
            if (remember)
            {
                _history.Push(_currentView);
            }

            _currentView.Hide();
        }

        view.Show();

        _currentView = view;
    }

    public void SetStartingView(View view)
    {
        _startingView = view;
    }

    public void SetCameraState(bool state)
    {
        _camera.gameObject.SetActive(state);
    }

}
