using UnityEngine;

public class LootBeamMono : MonoBehaviour
{
    public EquipmentInstance lootEquipment;
    public GameObject previewUIInstance;
    private bool playerInRange = false;

    public void Init(EquipmentInstance equipment)
    {
        lootEquipment = equipment;

        // 你可以在这里做UI展示、特效等
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            UIManager.Instance.ShowLootDetailPanel(lootEquipment, gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // 生成装备预览界面
            previewUIInstance = UIManager.Instance.ShowLootPreview(lootEquipment, transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && previewUIInstance != null)
        {
            playerInRange = true;
            if (previewUIInstance != null)
            {
                Destroy(previewUIInstance);
                previewUIInstance = null;
            }
        }
    }
}