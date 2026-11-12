using UnityEngine;

public class TempDoorVisual : MonoBehaviour
{
    [SerializeField] GameObject doorHitBox;
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr =  GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(doorHitBox.activeSelf == false)
        {
            sr.enabled = false;
        }
        else
        {
            sr.enabled = true;
        }

    }
}
