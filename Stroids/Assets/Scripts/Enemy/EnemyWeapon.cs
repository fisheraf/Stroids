using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] float attackRange = 10f;
    [SerializeField] float reloadTime = 3f;
    [SerializeField] GameObject ammo = null;

    [SerializeField] bool doubleShot = true;
    [SerializeField] bool missileShot = false;

    public Vector3 heading;
    public float bulletHeading;
    public float lastTimeShot;
    public bool attacking = false;

    [SerializeField] AudioSource audioSourceShoot = null;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttackRange();
    }

    private void CheckAttackRange()
    {
        heading = player.transform.position - transform.position;
        bulletHeading = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg - 90f;

        if (heading.sqrMagnitude < attackRange * attackRange && !attacking)
        {
            if (doubleShot) { StartCoroutine(DoubleShoot(reloadTime)); }
            if (missileShot) { StartCoroutine(ShootMissile(reloadTime)); }
            attacking = true;
        }
    }

    private IEnumerator DoubleShoot(float reloadTime)
    {
        Shoot();
        audioSourceShoot.PlayOneShot(audioSourceShoot.clip);
        yield return new WaitForSeconds(.05f);
        Shoot();
        yield return new WaitForSeconds(reloadTime);
        attacking = false;
        yield break;
    }

    void Shoot()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Enemy Bullet");
        if (bullet != null)
        {            
            bullet.SetActive(true);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.AngleAxis(bulletHeading, Vector3.forward);//heading.normalized);
        }
    }

    private IEnumerator ShootMissile(float reloadTime)
    {

        Instantiate(ammo);
        yield return new WaitForSeconds(reloadTime);
        attacking = false;
        yield break;
    }
}
