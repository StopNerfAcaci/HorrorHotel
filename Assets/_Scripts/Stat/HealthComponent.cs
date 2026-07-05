using System;
using UnityEngine;
using Utils.Helpers;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float MaxValue = 100f;
    private float value;
    public float Value => value;
    public Action<float> OnValueChanged = delegate { };

    public void Init()
    {
        value = MaxValue;
    }
    public void TakeDamage(float damage)
    {
        value -= damage;
        OnValueChanged?.Invoke(damage);
    }

    private void Update()
    {
    }

    public void SetMaxValue(float configBaseHp)
    {
        MaxValue = configBaseHp;
    }
}
