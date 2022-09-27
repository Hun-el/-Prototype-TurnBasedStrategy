using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform bulletHitVfxTransform;
    [SerializeField] private TrailRenderer trailRenderer;
    private Vector3 targetPosition;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(targetPosition, transform.position);

        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(targetPosition, transform.position);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            trailRenderer.transform.SetParent(null);
            transform.position = targetPosition;

            Destroy(this.gameObject);

            Instantiate(bulletHitVfxTransform,targetPosition,Quaternion.identity);
        }
    }
}
