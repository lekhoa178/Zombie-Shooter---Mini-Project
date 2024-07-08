using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    [SerializeField] private Page _InitialPage;
    [SerializeField] private GameObject _FirstFocusItem;

    private Stack<Page> _PageStack = new Stack<Page>();
    private List<Page> _PageOverlay = new List<Page>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        PopAllPages();
    }

    private void Start()
    {
        if (_FirstFocusItem != null)
        {
            EventSystem.current.SetSelectedGameObject(_FirstFocusItem);
        }

        if (_InitialPage != null)
        {
            PushPage(_InitialPage);
        }
    }

    public bool IsPageInStack(Page page)
    {
        return _PageStack.Contains(page);
    }

    public bool IsPageOnTopOfStack(Page page)
    {
        return _PageStack.Count > 0 && _PageStack.Peek() == page;
    }

    public void PushPage(Page page, bool isOverlay = false)
    {
        // Open a form multiple time
        if (_PageStack.Count != 0 && page == _PageStack.Peek())
            return;

        page.Enter(true);

        if (_PageStack.Count > 0 && !isOverlay)
        {
            Page currentPage = _PageStack.Peek();

            if (currentPage.ExitOnNewPagePush)
            {
                currentPage.Exit(false, false);
            }
        }

        if (isOverlay)
            _PageOverlay.Add(page);
        else
            _PageStack.Push(page);
    }

    public void PopPage()
    {
        if (_PageStack.Count != 0)
        {
            Page page = _PageStack.Pop();
            if (page == null)
                return;

            page.Exit(true);


            if (_PageStack.Count >= 1)
            {
                Page newCurrentPage = _PageStack.Peek();
                if (newCurrentPage.ExitOnNewPagePush)
                {
                    newCurrentPage.Enter(false);
                }
            }
        }
    }

    public void PopPage(Page page)
    {
        if (_PageOverlay.Count != 0)
        {
            bool removed = _PageOverlay.Remove(page);

            if (removed)
                page.Exit(true);
        }
    }

    public void PopAllPages()
    {
        int length = _PageStack.Count;
        for (int i = 0; i < length; i++)
        {
            PopPage();
        }

        length = _PageOverlay.Count;
        for (int i = 0; i < length; i++)
        {
            PopPage(_PageOverlay[i]);
        }
    }


}