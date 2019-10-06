using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public float damage = 50f;
    void OnTriggerEnter(Collider other)
    {
      GetComponent<Animation>().Play(); //Should play the animation
        Target target = other.transform.GetComponent<Target>();
        if (target)
        {
            target.TakeDamage(damage);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GetComponent<Animation>().Stop(); //Should play the animation
    }
}
