using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NUnit.Framework;

public class PanelNavigator : MonoBehaviour
{
    bool isPanelOpened = false;
    [SerializeField] GameObject[] allPanels;
    [SerializeField] StarterAssetsInputs InputManager;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject GenMenu;

    [System.Serializable]
    public struct ButtonPanelPair
    {
        public Button button;
        public GameObject panel;
    }
    public ButtonPanelPair[] buttonPanelPairs;
    public GameObject WeaponButton;
    private Stack<GameObject> history = new Stack<GameObject>();
    private Dictionary<Button, GameObject> buttonToPanel = new Dictionary<Button, GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //绑定快捷键事件
        InputManager.OnPPressed += OpenOrCloseGenMenu;
        InputManager.OnTabPressed += OpenOrClosePanel;
        InputManager.UIOnEscPressed += GoBack;
        //初始化按钮与跳转面板的对应关系
        foreach (var pair in buttonPanelPairs)
        {
            if (pair.button != null && pair.panel != null)
                buttonToPanel[pair.button] = pair.panel;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenOrClosePanel()
    {
        if (isPanelOpened == false && !GenMenu.activeInHierarchy)
        {
            allPanels[0].SetActive(true);
            allPanels[6].SetActive(false);
            ShowPanel(allPanels[1]);
            isPanelOpened = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerInput.SwitchCurrentActionMap("UI");
        }
        else if (isPanelOpened)
        {
            while (history.Count >= 1)
                GoBack();
        }

    }

    public void ShowPanelIfSelected(Button myButton)
    {
        // 只有按钮被选中时才允许跳转
        if (EventSystem.current.currentSelectedGameObject == myButton.gameObject)
        {
            if (buttonToPanel.TryGetValue(myButton, out var panel))
            {
                ShowPanel(panel);
            }
            else
            {
                Debug.LogError("Button-Panel mapping Error!");
            }
        }
    }
    public void ShowPanel(GameObject newPanel)
    {
        if (history.Count > 0)
        {
            var current = history.Peek();
            current.SetActive(false);
        }
        newPanel.SetActive(true);
        history.Push(newPanel);
    }
    public void GoBack()
    {
        var current = history.Pop();
        current.SetActive(false);
        if (history.Count == 0)
        {
            allPanels[0].SetActive(false);
            isPanelOpened = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerInput.SwitchCurrentActionMap("Player");
            allPanels[6].SetActive(true);
            return;
        }
        var prev = history.Peek();
        prev.SetActive(true);
    }
    public void OpenOrCloseGenMenu()
    {
        if (!GenMenu.activeInHierarchy && !isPanelOpened)
        {
            GenMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerInput.SwitchCurrentActionMap("UI");
        }

        else
        {
            GenMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerInput.SwitchCurrentActionMap("Player");
        }
    }

    public void ShowInventoryPanel(Button SelectedBtn)
    {
        var type = SelectedBtn.GetComponent<SlotButtonMono>().slotType;
        var ListMono = allPanels[3].GetComponent<InventoryItemsList>();
        ListMono.Setup(type);
        ShowPanel(allPanels[3]);
    }

}
