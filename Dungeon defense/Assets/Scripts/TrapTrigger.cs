using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] private float _damage = 100f;

    void OnTriggerEnter(Collider other)
    {
        GetComponent<Animation>().Play(); //Should play the animation
        Target target = other.transform.GetComponent<Target>();
        if (target)
            target.TakeDamage(_damage);
    }

    void OnTriggerExit(Collider other)
    {
        GetComponent<Animation>().Stop(); //Should stop the animation
    }
}
