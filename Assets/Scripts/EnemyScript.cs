using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyScript : MonoBehaviour
{
    private float hp = 100;
    
    [SerializeField] private float deltaX;

    public void DamageEnemy(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
