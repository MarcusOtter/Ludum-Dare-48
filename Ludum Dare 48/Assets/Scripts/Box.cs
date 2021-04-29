using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IDamageable
{

    public void TakeDamage(float incomingDamage)
    {
        gameObject.SetActive(false);

        if (transform.parent == null) { return; }

        var activeSiblings = 0;
        foreach (Transform sibling in transform.parent)
        {
            if (sibling.gameObject.activeSelf) { activeSiblings++; }
        }

        if (activeSiblings == 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
