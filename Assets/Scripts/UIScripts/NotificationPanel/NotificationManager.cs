using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager I;

    [Header("UI References")]
    public GameObject notificationPanel; // 信息栏面板
    public GameObject notificationPrefab; // 通知项预制体
    public Transform notificationContainer; // 通知项容器

    [Header("Settings")]
    public float notificationDuration = 5f; // 通知显示时长
    public float fadeDuration = 0.5f; // 淡入淡出时间
    public float moveSpeed = 200f; // 移动速度

    private void Awake()
    {
        // 单例模式
        if (I == null)
            I = this;
        else
            Destroy(gameObject);

        // 初始隐藏信息栏
        notificationPanel.SetActive(false);
    }
    private class NotificationData
    {
        public NotificationItem item;
        public float timer;
        public bool fading;
        public float fadeTimer;
    }

    private List<NotificationData> notificationDatas = new List<NotificationData>();

    void Update()
    {
        for (int i = notificationDatas.Count - 1; i >= 0; i--)
        {
            var data = notificationDatas[i];
            if (!data.fading)
            {
                data.timer += Time.deltaTime;
                float progress = Mathf.Clamp01(1f - data.timer / notificationDuration);
                data.item.SetProgress(progress);
                if (data.timer >= notificationDuration)
                {
                    data.fading = true;
                    data.fadeTimer = 0f;
                }
            }
            else
            {
                data.fadeTimer += Time.deltaTime;
                float t = Mathf.Clamp01(data.fadeTimer / fadeDuration);
                var cg = data.item.GetComponent<CanvasGroup>();
                if (cg == null) cg = data.item.gameObject.AddComponent<CanvasGroup>();
                cg.alpha = 1f - t;

                // 移动动画
                RectTransform rect = data.item.GetComponent<RectTransform>();
                if (rect != null)
                {
                    Vector2 pos = rect.anchoredPosition;
                    pos.x += moveSpeed * Time.deltaTime;
                    rect.anchoredPosition = pos;
                }

                if (t >= 1f)
                {
                    Destroy(data.item.gameObject);
                    notificationDatas.RemoveAt(i);
                }
            }
        }

        // 没有通知时隐藏面板
        if (notificationDatas.Count == 0 && notificationPanel.activeSelf)
            notificationPanel.SetActive(false);
    }

    public void AddNotification(string message)
    {
        GameObject notificationObj = Instantiate(notificationPrefab, notificationContainer);
        NotificationItem notification = notificationObj.GetComponent<NotificationItem>();
        if (notification == null)
            notification = notificationObj.AddComponent<NotificationItem>();
        notification.Setup(" " + message, this);

        notificationDatas.Add(new NotificationData { item = notification, timer = 0f, fading = false, fadeTimer = 0f });

        if (!notificationPanel.activeSelf)
            notificationPanel.SetActive(true);
    }

    public void RemoveNotification(NotificationItem notification)
    {
        for (int i = notificationDatas.Count - 1; i >= 0; i--)
        {
            if (notificationDatas[i].item == notification)
            {
                Destroy(notification.gameObject);
                notificationDatas.RemoveAt(i);
            }
        }
    }

}
