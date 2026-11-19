using Smogview.UI;
using UnityEngine;

[System.Serializable]
public class UnlockLens : Interaction
{
    private LensIndicatorUI _indicatorUI;

    [SerializeField] private LensManager.lens lens;
    [SerializeField] private bool unlock = true;
    
    public void Awake()
    {
        _indicatorUI = FindAnyObjectByType<LensIndicatorUI>();
    }
    
    public override void Behaviour()
    {
        _indicatorUI.ActivateIcon(lens, unlock);
    }

}
