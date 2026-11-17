using Unity.Collections.Tests.CoreCLR.TestJobs;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject _interactableNPC;
    [SerializeField] private GameObject _defaultNPC;
    private LensManager _lensSwitching;

    private void Awake()
    {
        _lensSwitching = FindAnyObjectByType<LensManager>();
    }

    public void Update()
    {
        if(_lensSwitching.CurrentLens == LensManager.lens.Lsocial)
        {
            _defaultNPC.SetActive(false);
        }
        else
        {
            _defaultNPC.SetActive(true);
        }
    }

}
