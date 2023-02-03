using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle; // 시야각
    [SerializeField] private float viewDistance; // 시야거리
    [SerializeField] private LayerMask targetMask; // ex 플레이어

    private Animal animal;

    private void Awake()
    {
        animal = GetComponent<Animal>();
    }

    private void Update()
    {
        View();
    }

    private Vector3 GetBoundary(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()
    {
        Vector3 center = transform.position + transform.up;
        Vector3 _leftBoundary = GetBoundary(-viewAngle * 0.5f);
        Vector3 _rightBoundary = GetBoundary(viewAngle * 0.5f);
        
        Debug.DrawRay(center, _leftBoundary, Color.red);
        Debug.DrawRay(center, _rightBoundary, Color.red);

        Collider[] _targets = Physics.OverlapSphere(center, viewDistance, targetMask);

        for (int i = 0; i < _targets.Length; i++)
        {
            Transform _targetTransform = _targets[i].transform;
            if (_targetTransform.CompareTag("Player"))
            {
                ReactToPlayer(_targetTransform);
            }
        }
    }

    private void ReactToPlayer(Transform _targetTransform)
    {
        Vector3 center = transform.position + transform.up;
        Vector3 _direction = (_targetTransform.position - transform.position).normalized;
        float _angle = Vector3.Angle(_direction, transform.forward);
        if (_angle > viewAngle * 0.5f) return;
        
        RaycastHit _hit;
        if (Physics.Raycast(center, _direction, out _hit, viewDistance))
        {
            if (animal is FrendlyAnimal fAnimal)
            {
                fAnimal = (FrendlyAnimal)animal;
                fAnimal.Run(_hit.transform.position);
            }
            Debug.DrawRay(center, _direction, Color.blue);
        }
    }
}
