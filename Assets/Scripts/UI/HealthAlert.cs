using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using inonego;
using UnityEngine.Events;

public class HealthAlert : MonoBehaviour
{
    private Health health;

    public List<int> alertThresholds = new List<int>();

    public UnityEvent<int> OnAlert;

    private int index = 0;

    public void Reset()
    {
        index = 0;
    }

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (index >= alertThresholds.Count) return;

        if (alertThresholds[index] <= health.HP)
        {
            index++;

            OnAlert?.Invoke(health.HP);
        }
    }
}
