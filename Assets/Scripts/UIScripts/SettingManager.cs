using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq; // For Linq methods like FirstOrDefault

public class TabViewControl : MonoBehaviour
{
    public UIDocument uiDocument; // 拖拽你的UI Document到这里
    public string defaultTabName = "tab-button-1"; // 默认选中的标签按钮的名称

    private VisualElement m_Root;
    private List<Button> m_TabButtons;
    private List<VisualElement> m_TabContentPanes;

    private Button m_ActiveButton;
    private VisualElement m_ActivePane;

    // USS 类名
    private const string ActiveTabClassName = "tab-button--active";

    void OnEnable()
    {
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not assigned to TabViewControl.");
            return;
        }
        m_Root = uiDocument.rootVisualElement;
        if (m_Root == null)
        {
            Debug.LogError("RootVisualElement not found in UIDocument.");
            return;
        }

        // 1. 获取所有标签按钮和内容面板
        // 使用 UQuery 查找所有类名为 "tab-button" 的按钮
        m_TabButtons = m_Root.Query<Button>(className: "tab-button").ToList();
        // 使用 UQuery 查找所有类名为 "tab-content-pane" 的内容面板
        m_TabContentPanes = m_Root.Query<VisualElement>(className: "tab-content-pane").ToList();

        if (m_TabButtons.Count == 0 || m_TabContentPanes.Count == 0)
        {
            Debug.LogWarning("No tab buttons or content panes found. Check UXML class names.");
            return;
        }

        if (m_TabButtons.Count != m_TabContentPanes.Count)
        {
            Debug.LogWarning("The number of tab buttons does not match the number of content panes.");
            // 你可能需要更复杂的逻辑来匹配它们，例如基于名称约定
        }

        // 2. 为每个标签按钮注册点击事件
        for (int i = 0; i < m_TabButtons.Count; i++)
        {
            Button button = m_TabButtons[i];
            // 使用 VisualElementUserData 来存储与按钮关联的内容面板的索引或名称
            // 这里简单地使用索引，假设按钮和面板的顺序是一致的
            button.userData = i; // 将索引存入userData
            button.RegisterCallback<ClickEvent>(OnTabButtonClicked);
        }

        // 3. 设置默认选中的标签页
        Button defaultButton = m_TabButtons.FirstOrDefault(b => b.name == defaultTabName);
        if (defaultButton != null)
        {
            SwitchToTab(defaultButton);
        }
        else if (m_TabButtons.Count > 0)
        {
            SwitchToTab(m_TabButtons[0]); // 如果默认名称找不到，则选中第一个
        }
    }

    void OnDisable()
    {
        if (m_TabButtons != null)
        {
            foreach (var button in m_TabButtons)
            {
                button.UnregisterCallback<ClickEvent>(OnTabButtonClicked);
            }
        }
    }

    private void OnTabButtonClicked(ClickEvent evt)
    {
        Button clickedButton = evt.currentTarget as Button;
        if (clickedButton != null && clickedButton != m_ActiveButton)
        {
            SwitchToTab(clickedButton);
        }
    }

    private void SwitchToTab(Button targetButton)
    {
        if (targetButton == null || targetButton.userData == null) return;

        int targetIndex = (int)targetButton.userData;

        // 检查索引是否有效
        if (targetIndex < 0 || targetIndex >= m_TabContentPanes.Count)
        {
            Debug.LogError($"Invalid target index {targetIndex} for tab content panes.");
            return;
        }

        VisualElement targetPane = m_TabContentPanes[targetIndex];

        // 1. 停用当前活动的标签和面板 (如果存在)
        if (m_ActiveButton != null)
        {
            m_ActiveButton.RemoveFromClassList(ActiveTabClassName);
        }
        if (m_ActivePane != null)
        {
            m_ActivePane.style.display = DisplayStyle.None;
        }

        // 2. 激活新的标签和面板
        targetButton.AddToClassList(ActiveTabClassName);
        targetPane.style.display = DisplayStyle.Flex; // 或者 DisplayStyle.Block，取决于你的布局

        // 3. 更新当前活动的引用
        m_ActiveButton = targetButton;
        m_ActivePane = targetPane;
    }
}