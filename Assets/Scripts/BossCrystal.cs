﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrystal : MonoBehaviour
{
    public Boss boss;
    public BossOrb orbPrefab;
    public float orbSpeed = 3f;
    public float orbDelay = 0;
    public float orbInterval = 2f;
    public float orbSpawnOffset = 1f;

    private Player player;
    private bool isCrystalActive;
    private BoxCollider2D boxCollider2D;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isCrystalActive = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        BossOrb orb;
        if (col.gameObject.TryGetComponent<BossOrb>(out orb))
        {
            if (orb.IsReflected())
            {
                boss.BreakCrystal();
                StartCoroutine(Explode());
            }
        }
    }

    public void ActivateCrystal()
    {
        StartCoroutine(ShootOrbs());
    }

    private IEnumerator ShootOrbs()
    {
        yield return new WaitForSeconds(orbDelay);
        while (isCrystalActive)
        {
            Vector3 dirToPlayer = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.left, dirToPlayer);
            Vector3 offset = dirToPlayer * orbSpawnOffset;
            BossOrb orb = Instantiate(orbPrefab, transform.position + offset, rotation);
            orb.GetComponent<Rigidbody2D>().velocity = dirToPlayer * orbSpeed;
            yield return new WaitForSeconds(orbInterval);
        }
    }

    private IEnumerator Explode()
    {
        boxCollider2D.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
