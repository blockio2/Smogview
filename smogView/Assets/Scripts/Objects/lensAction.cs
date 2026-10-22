using UnityEngine;
public class lensAction : MonoBehaviour
{
    [SerializeField] private string lensOfGameObject;

    Collider2D C;
    SpriteRenderer SR;

    void Start()
    {
        lensOfGameObject = gameObject.tag;

        SR = gameObject.GetComponent<SpriteRenderer>();
        C = gameObject.GetComponent<Collider2D>();
    }

   
    public void StateChange(string lens)
    {
        if(lens == lensOfGameObject)
        {
            SR.enabled = true;
            C.enabled = true;
        }
        else
        {
            SR.enabled = false;
            C.enabled = false;
        }
    }


}
