using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class lensSwitching : MonoBehaviour
{

    [SerializeField] private TagHandle defaultLayer;
    [SerializeField] private TagHandle physicalLayer;
    [SerializeField] private TagHandle socialLensLayer;

    public int state =0;

    enum lens {
    Ldefault,
    Lphysical,
    Lsocial
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lens currentlens = lens.Ldefault;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        { 
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        }



    }

    public void swtiching(InputAction.CallbackContext context)
    {
        string num = context.control.name;
        Debug.Log(num);
       // int sate =  
        
    }














}
