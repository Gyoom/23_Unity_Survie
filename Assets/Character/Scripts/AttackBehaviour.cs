using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    // Player
    [Header("References")]
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Equipement equipementSystem;

    [SerializeField]
    private InteractBehaviour interactBehaviour;

    [SerializeField]
    private UIManager uIManager;

    [Header("Stats")]

    [SerializeField]
    private float attackRange;
    [SerializeField]
    private LayerMask layerMask;

    private bool isAttacking = false;

    [SerializeField]
    private Vector3 attackOffset;

    void Update()
    {
        // Debug.DrawRay(transform.position + attackOffset, transform.forward * attackRange, Color.red);

        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            isAttacking = true;
            SendAttack();
            playerAnimator.SetTrigger("Attack");
        }

    }

    void SendAttack () 
    {
        Debug.Log("Attack Sent");
        RaycastHit hit;
        if (Physics.Raycast(transform.position + attackOffset, transform.forward, out hit, attackRange, layerMask))
        {
            Debug.Log("test");
            if (hit.transform.CompareTag("AI"))
            {
                EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
                enemy.TakeDamage(Random.Range(equipementSystem.equipedWeaponItem.minDamagePoints, equipementSystem.equipedWeaponItem.maxDamagePoints));
            }
        }
    }
    // call from attack animation
    public void AttackFinished() 
    {
        isAttacking = false;
    }

    bool CanAttack()
    {
        return equipementSystem.equipedWeaponItem != null && 
                !isAttacking &&
                !uIManager.atleastOneOpenPanel &&
                !interactBehaviour.isBusy;

    }
}
