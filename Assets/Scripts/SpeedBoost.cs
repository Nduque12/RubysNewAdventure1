using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour

{

    public GameObject pickupEffect;

    public float multiplier = 1.4f;
    public float duration = 4f;

    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("rubyController"))
        {
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider2D rubyController)
    {
        Instantiate(pickupEffect, transform.position, transform.rotation);

        RubyController stats = rubyController.GetComponent<RubyController>();
        stats.speed *= multiplier;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(duration);

        stats.speed /= multiplier;

        Destroy(gameObject);
    }
}
