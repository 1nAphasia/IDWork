using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatusBar : MonoBehaviour
{
    public EnemyMono target;
    public TMP_Text Level;
    public Image EnemyTypeIcon;
    public Slider ArmorSlider;
    public Slider healthSlider;
    public Vector3 offset = new Vector3(0, 1f, 0); // 血条在敌人头顶偏移
    void Start()
    {
        Level.text = target.level.ToString();
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        // 跟随敌人
        Vector3 worldPos = target.transform.position + offset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        transform.position = screenPos;

        // 更新血量
        ArmorSlider.value = (float)target.Armor / target.MaxArmor;
        if (ArmorSlider.value == 0) ArmorSlider.gameObject.SetActive(false);

        healthSlider.value = (float)target.Health / target.MaxHealth;
    }
}