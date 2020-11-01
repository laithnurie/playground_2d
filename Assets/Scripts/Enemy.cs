using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void Damage()
    {
        // Do nothing

    }

    // will be called from animation event
    void Death()
    {
        Destroy(gameObject);
    }

    public virtual int GetPoints()
    {
        return 0;
    }

}
