using System;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Canvas overlayCanvas;
    [SerializeField] PlayerInput playerInput;
    public static UIManager Instance { get; private set; }
    [SerializeField] StarterAssetsInputs InputManager;
    [SerializeField] Sprite SemiAutoIcon;
    [SerializeField] Sprite FullAutoIcon;

    [SerializeField] Image ShotModeIcon;
    [SerializeField] TMP_Text CurrentAmmo;
    [SerializeField] TMP_Text AmmoLeft;
    [SerializeField] private GameObject hitNumberPrefab; // 拖拽你的HitNumberText预制体
    [SerializeField] private RectTransform hitNumberRoot; // 拖拽Canvas下用于飘字的父节点
    [SerializeField] private GameObject lootPreviewPanelPrefab; // 拖拽你的Panel预制体
    [SerializeField] private GameObject lootDetailPanel;
    public Slider ReloadStatusBar;



    // public RenderTexture myBlurResult;

    public float blurSize = 5.0f;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 保证只有一个实例
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // InputManager.OnVPressed += ChangeShotModeUI;
        InputManager.UIOnEscPressed += CloseLootDetailPanel;
    }

    public void ChangeShotModeUI(FireMode mode)
    {
        if (mode is FireMode.SemiAuto) ShotModeIcon.sprite = SemiAutoIcon;
    }

    public void UpdateAmmoPanel(int Cur, int Left, FireMode mode)
    {
        UpdateCurrentAmmo(Cur);
        UpdateLeftAmmo(Left);
        ChangeShotModeUI(mode);
    }
    public void UpdateCurrentAmmo(int Cur)
    {
        CurrentAmmo.text = Cur.ToString();
    }
    public void UpdateLeftAmmo(int Left)
    {
        AmmoLeft.text = Left.ToString();
    }
    public void UpdateReloadStatusBar(float value)
    {
        ReloadStatusBar.value = value;
    }

    public void ShowHitNumber(int value)
    {
        var go = Instantiate(hitNumberPrefab, hitNumberRoot);
        var text = go.GetComponent<TMPro.TMP_Text>();
        text.text = value.ToString();
        go.transform.localPosition = new Vector3(300, -100, 0); // 右下偏移，可根据UI分辨率调整
        StartCoroutine(HitNumberAnim(go));
    }

    private System.Collections.IEnumerator HitNumberAnim(GameObject go)
    {
        float duration = 2f;
        float elapsed = 0f;
        var rect = go.GetComponent<RectTransform>();
        var cg = go.GetComponent<CanvasGroup>();
        Vector3 startPos = rect.localPosition;
        Vector3 endPos = startPos + new Vector3(0, -200, 0); // 向下移动80像素

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rect.localPosition = Vector3.Lerp(startPos, endPos, t);
            cg.alpha = 1 - t;
            yield return null;
        }
        Destroy(go);
    }

    public GameObject ShowLootPreview(EquipmentInstance equip, Vector3 worldPos)
    {
        var panel = Instantiate(lootPreviewPanelPrefab, overlayCanvas.transform);
        // 设置信息
        panel.GetComponent<lootPreviewMono>().Setup(equip);

        // 可选：将Panel定位到屏幕指定位置（如光柱上方/屏幕中央等）
        var rect = panel.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        rect.position = screenPos + new Vector2(0, -300);

        return panel;
    }

    public void ShowLootDetailPanel(EquipmentInstance lootEquipment, GameObject lootBeam)
    {
        lootDetailPanel.GetComponent<LootDetailMono>().Setup(lootEquipment, lootBeam);
        lootDetailPanel.SetActive(true);
        playerInput.SwitchCurrentActionMap("UI");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CloseLootDetailPanel()
    {
        lootDetailPanel.SetActive(false);
        playerInput.SwitchCurrentActionMap("Player");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
