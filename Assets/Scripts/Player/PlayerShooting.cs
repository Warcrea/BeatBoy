using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;

public enum Weapon { Laser, HomingRocket};

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

    //Misc references
    private GameObject projectilesHolder;

    private Weapon currentWeapon;

    void Start () {
        currentWeapon = (Weapon)0;
        TrackManager trackManager = GameObject.Find("Audio Management").GetComponent<TrackManager>();
        RegisterForOneTrack(null, trackManager.GetCurrentBeatEvent());

        shootableMask = LayerMask.GetMask("Shootable");
        forwardParticles = GetComponent<ParticleSystem>();
        forwardBeamLine = GetComponent<LineRenderer>();
        forwardBeamLine.startWidth = forwardBeamWidth* beamHitboxRatio;
        forwardBeamLine.endWidth = forwardBeamWidth * beamHitboxRatio;

        projectilesHolder = GameObject.Find("Projectiles");
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
	}

    public void SetCurrentWeapon(int weaponId) {
        currentWeapon = (Weapon)weaponId;
        Debug.Log(currentWeapon);   
    }

    public void RegisterForOneTrack(string previousEventId, string eventId) {
        if (previousEventId != null) {
            Koreographer.Instance.UnregisterForEvents(previousEventId, FireEvent);
        }

        Koreographer.Instance.RegisterForEvents(eventId, FireEvent);
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
    }

    void ShootHomingRocket() {
        GameObject rocket = Instantiate(rocketPrefab, transform.position, transform.rotation, projectilesHolder.transform);
        rocket.GetComponent<HomingMissile>().SetDamage(2);
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
        forwardBeamLine.SetPosition(0, transform.position + transform.up);
        forwardBeamLine.SetPosition(1, transform.up * range + transform.position);
        
        RaycastHit2D[] hits = (Physics2D.CircleCastAll(transform.position, adjustedWidth, transform.up, range, shootableMask));

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
