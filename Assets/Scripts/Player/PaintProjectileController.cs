using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintProjectileController : MonoBehaviour
{
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private GameObject paintBullet;
    [SerializeField] private float bulletSpd = 10000f;
    [SerializeField] private float firerate = 1f;
    [SerializeField] private bool isReloading;


    // Start is called before the first frame update
    void Start()
    {
        isReloading = false;
    }


    public void Fire()
    {
        if (isReloading) return;
        isReloading = true;
        var bullet = Instantiate(paintBullet, muzzlePoint.position, muzzlePoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = muzzlePoint.forward * bulletSpd;
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(firerate);
        isReloading = false;
    }
}
