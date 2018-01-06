using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;

public enum Weapon { HomingRocket, Laser, Bullet };

public class PlayerShooting : MonoBehaviour {

    //Laser
    float effectTimer;
    RaycastHit2D forwardHit;
    LineRenderer forwardBeamLine;
    ParticleSystem forwardParticles;
    float forwardBeamWidth = 0.5f;
    float beamHitboxRatio = 6.25f;  // 1/The amount of the laser texture that is actually deadly
    float effectsDisplayTime = 0.05f;
    public float range = 100f;
    int shootableMask;
    public int laserDamage;

    //Rockets
    public GameObject rocketPrefab;
    public int rocketDamage;

    //Bullets
    public GameObject bulletPrefab;
    public int bulletDamage;

    //Misc references
    private GameObject projectilesHolder;
    private Transform shootDirection;

    private Weapon currentWeapon;

    void Start () {
        currentWeapon = (Weapon)1;
        TrackManager trackManager = GameObject.Find("Audio Management").GetComponent<TrackManager>();
        Koreographer.Instance.RegisterForEvents("Beat", FireEvent);

        shootableMask = LayerMask.GetMask("Shootable");
        forwardParticles = GetComponent<ParticleSystem>();
        forwardBeamLine = GetComponent<LineRenderer>();
        forwardBeamLine.startWidth = forwardBeamWidth* beamHitboxRatio;
        forwardBeamLine.endWidth = forwardBeamWidth * beamHitboxRatio;

        projectilesHolder = GameObject.Find("Projectiles");
        shootDirection = gameObject.transform.GetChild(0);
    }
	
	// Update is called once per frame
	void Update () {
        effectTimer += Time.deltaTime;

		if (Input.GetButtonDown("Fire1")) {
            //ShootForward();
        }

        if (effectTimer >= effectsDisplayTime) {
            DisableEffects();
        }

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
        shootDirection.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetCurrentWeapon(int weaponId) {
        currentWeapon = (Weapon)weaponId;
        Debug.Log(currentWeapon);   
    }

    public void DisableEffects() {
        forwardBeamLine.enabled = false;
    }

    void FireEvent(KoreographyEvent koreoEvent) {
        int payLoad = 1;
        if (koreoEvent.Payload is IntPayload)
            FireCurrentWeapon(((IntPayload)koreoEvent.Payload).IntVal);
    }

    void FireCurrentWeapon(int power) {
        if (currentWeapon == Weapon.Laser) {
            ShootForwardLaser(power);
        }
        else if (currentWeapon == Weapon.HomingRocket) {
            ShootHomingRocket();
        }
        else if (currentWeapon == Weapon.Bullet) {
            ShootIonBullet();
        }
    }

    void ShootHomingRocket() {
        GameObject rocket = Instantiate(rocketPrefab, transform.position, transform.rotation, projectilesHolder.transform);
        rocket.GetComponent<HomingMissile>().SetDamage(4);
    }

    void ShootIonBullet() {
        forwardParticles.Stop();
        forwardParticles.Play();
        GameObject bullet = Instantiate(bulletPrefab, transform.position + shootDirection.up, shootDirection.rotation, projectilesHolder.transform);
        bullet.GetComponent<Bullet>().SetDamage(2);
    }

    void ShootForwardLaser(int power) {
        forwardParticles.Stop();
        forwardParticles.Play();
        effectTimer = 0f;

        float adjustedWidth = power * 2 * forwardBeamWidth;
        forwardBeamLine.startWidth = adjustedWidth * beamHitboxRatio;
        forwardBeamLine.endWidth = adjustedWidth * beamHitboxRatio;

        forwardBeamLine.enabled = true;
        forwardBeamLine.positionCount = 2;
        forwardBeamLine.SetPosition(0, transform.position + shootDirection.up);
        forwardBeamLine.SetPosition(1, shootDirection.up * range + transform.position);
        
        RaycastHit2D[] hits = (Physics2D.CircleCastAll(transform.position, adjustedWidth, shootDirection.up, range, shootableMask));

        foreach(RaycastHit2D hit in hits) {
            if (hit.collider != null) {
                Debug.Log(hit.collider);
                GameObject hitTarget = hit.rigidbody.gameObject;
                Destroyable targetDestroyable = hitTarget.GetComponent<Destroyable>();
                if (targetDestroyable != null) {
                    targetDestroyable.TakeDamage(laserDamage);
                }
            }
        }

    }
}
