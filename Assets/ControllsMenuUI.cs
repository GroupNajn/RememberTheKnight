using UnityEngine;

public class ControllsMenuUI : AutoSelectFirstButtonOnEnable
{
    UIManager uiManager;
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
    }

}
