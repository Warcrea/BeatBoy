using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private bool isExploding = false;
    private bool isGrowing = true;

    public float speed;
    private int damage;
    private float timer;
    public float explosionGrowDuration;
    public float explosionShrinkDuration;
    public Vector3 growScale;

    public void SetDamage(int damage) {
        this.damage = damage;
    }

    void Update() {
        if (!isExploding)
            transform.Translate(transform.up * (speed * Time.deltaTime), Space.World);
        else {
            if (isGrowing) {
                transform.localScale = Vector3.Lerp(new Vector3(1,1,1), growScale, timer / explosionGrowDuration);
                if (timer >= explosionGrowDuration) {
                    isGrowing = false;
                    timer = 0;
                }
            }
            else {
                transform.localScale = Vector3.Lerp(growScale, new Vector3(0,0,0), timer / explosionShrinkDuration);
                if (timer >= explosionShrinkDuration) {
                    Destroy(gameObject);
                }
            }
            timer += Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<Destroyable>().TakeDamage(damage);
            if (!isExploding) {
                isExploding = true;
                GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    IEnumerator Lerp(Vector3 startScale, Vector3 endScale, int timeScale) {
        float progress = 0;

        while (progress <= 1) {
            transform.localScale = Vector3.Lerp(startScale, endScale, progress);
            progress += Time.deltaTime * timeScale;
            yield return null;
        }
    }

}
