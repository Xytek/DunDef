using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    private void Update()
    {
       if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
