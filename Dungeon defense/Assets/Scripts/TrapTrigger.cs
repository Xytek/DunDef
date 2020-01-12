using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] private Trap _trap = default;
    private Animation _anim;

    private void Start()
    {
        _anim = GetComponent<Animation>();
    }
    void OnTriggerEnter(Collider other)
    {
            _anim.Play(); //Should play the animation
            Enemy target = other.transform.GetComponent<Enemy>();
            if (target)
                target.TakeDamage(_trap.Damage);
            if (_trap.Type == TrapType.Weapon)
                Destroy(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (_trap.name == "Cutter")
            _anim.Play("Anim_TrapCutter_Stop");
        if (_trap.name == "Needle")
            _anim.Play("Anim_TrapNeedle_Hide");
        //_anim.
    }


}
