using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionLife : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            if (hit.CompareTag("Player"))
            {
                health.Heal(50);
                Destroy(gameObject);
            }
        }

    }
}
