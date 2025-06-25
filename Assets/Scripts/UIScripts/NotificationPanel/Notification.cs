using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;


public class NotificationItem : MonoBehaviour
{
    private TMP_Text messageTMP_Text;
    private NotificationManager manager;
    private Slider TimeCounter;

    // 设置通知内容
    public void Setup(string message, NotificationManager manager)
    {
        this.manager = manager;

        // 查找TMP_Text组件
        messageTMP_Text = GetComponentInChildren<TMP_Text>();
        TimeCounter = GetComponentInChildren<Slider>();
        if (messageTMP_Text == null)
        {
            Debug.LogError("Notification prefab missing TMP_Text component!");
            return;
        }

        // 设置文本内容
        messageTMP_Text.text = message;

    }
    public void SetProgress(float progress)
    {
        if (TimeCounter == null)
            TimeCounter = GetComponentInChildren<Slider>();
        if (TimeCounter != null)
            TimeCounter.value = progress;
    }

    // 手动移除通知
    public void Remove()
    {
        if (manager != null)
            manager.RemoveNotification(this);
    }
}