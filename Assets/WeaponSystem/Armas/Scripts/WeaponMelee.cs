using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : WeaponBase
{
    [SerializeField] Transform meleePoint;

    [SerializeField] float forwardRange = 1f;
    [SerializeField] float horizontalRange = 1f;
    [SerializeField] float verticalRange = 1f;

    [SerializeField] float meleeDamage = 0.25f;
    [SerializeField] float meleeCadence = 1f;
    [SerializeField] AudioSource meleeSound;
    [SerializeField] float meleeAttackSoundStart = 0f;

    protected Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    

    private void Start()
    {
        meleePoint = meleePoint ? meleePoint : transform;
    }

    public override void Swing()
    {
        if (isUsable)
        {
            isUsable = false;
            animator.SetTrigger("Attack");
            meleeSound?.PlayDelayed(meleeAttackSoundStart);
            Invoke(nameof(SwingEnd), 1f / meleeCadence);

            Vector3 halfExtents = new Vector3(horizontalRange / 2f, verticalRange / 2f, forwardRange / 2f);
            Collider[] colliders = Physics.OverlapBox(meleePoint.position, halfExtents, meleePoint.rotation, targetLayers);
            foreach (Collider c in colliders)
            {
                TargetBase targetBase = c.GetComponent<TargetBase>();
                targetBase?.NotifySwing(meleeDamage);
            }
        }
    }

    void SwingEnd()
    {
        isUsable = true;
    }
}
