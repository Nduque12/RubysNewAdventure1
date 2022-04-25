using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour

{

    public GameObject pickupEffect;

    public float multiplier3 = 1.75f;
    public float duration = 4f;

    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("rubyController"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider2D rubyController)
    {
        Instantiate(pickupEffect, transform.position, transform.rotation);

        RubyController stats = rubyController.GetComponent<RubyController>();
        stats.timeInvincible *= multiplier3;

        Destroy(gameObject);
    }

}



