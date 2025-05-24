using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UIDocument HUDDocument;
    public UIDocument SettingsDocument;
    public UIDocument EscDocument;

    private VisualElement hudRoot;
    private VisualElement settingsRoot;
    private VisualElement escRoot;
    void Start()
    {
        hudRoot = HUDDocument.rootVisualElement;
        settingsRoot = SettingsDocument.rootVisualElement;
        escRoot = EscDocument.rootVisualElement;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
