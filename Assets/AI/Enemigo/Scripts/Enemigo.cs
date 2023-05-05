using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavigateToTransform))]
[RequireComponent(typeof(NavigateRute))]
[RequireComponent(typeof(NavigateToPosition))]
public class Enemigo : MonoBehaviour, TargetWithLifeThatNotifies.IDeathNotifiable, NoiseMaker.INoiseLitener
{
    public enum BehaviourType
    {
        Cautious,
        Valiant,
        Sneaky,
        Guardian,
    };
    [SerializeField] BehaviourType behaviourType = BehaviourType.Valiant;

    NavigateToTransform navigateToTransform;
    NavigateRute navigateRoute;
    NavigateToPosition navigateToPosition;

    WeaponBase currentWeapon;
    [SerializeField] float attacksPerSecond = 0.5f;
    [SerializeField] float timeToForgetNoiseMaker = 1f;

    [Header("Cover Parameters")]
    [SerializeField] float fearDistance = 15f;
    [SerializeField] float coverSearchRadius = 20f;
    [SerializeField] LayerMask coverSearchLayerMask;
    [SerializeField] LayerMask occludingLayerMask = Physics.DefaultRaycastLayers;

    Transform currentTarget = null;
    NoiseMaker lastHeardNoiseMaker = null;


    Sight sight;

    enum State
    {
        Patrol,
        Idle,
        Seek,
        Attack,
        TakeCover,
        Die,
        CheckLastPosition,
    }
    [SerializeField] State state = State.Seek;
    [SerializeField] float checkPositionThreshold = 1.5f;

    private void Awake()
    {
        navigateToTransform = GetComponent<NavigateToTransform>();
        navigateRoute = GetComponent<NavigateRute>();
        navigateToPosition = GetComponent<NavigateToPosition>();
        currentWeapon = GetComponentInChildren<WeaponBase>();
        sight = GetComponent<Sight>();
        timeCovering = maxTimeCovering;
    }

    void Start()
    {
        navigateToTransform.enabled = false;
        navigateRoute.enabled = false;
        navigateToPosition.enabled = false;

        state = State.Idle;
        if (behaviourType == BehaviourType.Guardian)
        { GetComponent<NavMeshAgent>().speed = 0f; }
        else if (navigateRoute.GetRoute() != null)
        { state = State.Patrol; }


    }

    float timeForNextAttack = 0f;
    float timeLeftToForgetNoiseMaker;
    private Vector3 lastNoticedPosition;
    bool locatedFirstTarget = false;

    private void Update()
    {
        UpdateNoiseMaker();
        UpdateCurrentTarget();

        /// IMPLEMENTAR: Patrol e Idle
        switch (state)
        {
            case State.Idle:
                if (currentTarget != null)
                { state = State.Seek; }
                else
                { GoTo(transform.position); }
                break;

            case State.Patrol:
                if (currentTarget != null)
                { state = State.Seek; }
                else
                { Patrol(); }
                break;

            case State.Seek:
                if (currentTarget == null)
                {
                    navigateRoute.enabled = false;
                    state = State.CheckLastPosition;
                }
                else
                {
                    GoTo(currentTarget);
                    if (IsInAttackRange())
                    {
                        state = State.Attack;
                        timeForNextAttack = 1f / attacksPerSecond;
                        navigateToTransform.transformGoTo = null;
                    }
                }
                break;

            case State.Attack:
                UpdateAttack();
                break;

            case State.TakeCover:
                UpdateTakeCover();
                break;

            case State.CheckLastPosition:
                if (currentTarget != null)
                {
                    state = State.Seek;
                }
                else
                {
                    GoTo(lastNoticedPosition);
                    if (Vector3.Distance(navigateToPosition.position, transform.position) < checkPositionThreshold)
                    {
                        if (navigateRoute.GetRoute() != null)
                        {
                            Patrol();
                            state = State.Patrol;
                        }
                        else
                        { state = State.Idle; }
                    }
                }
                break;

            case State.Die:
                break;
        }
    }

    void UpdateNoiseMaker()
    {
        timeLeftToForgetNoiseMaker -= Time.deltaTime;
        if (timeLeftToForgetNoiseMaker <= 0f) { lastHeardNoiseMaker = null; }
    }

    void UpdateCurrentTarget()
    {
        currentTarget = null;
        if (sight.colliderInSight.Count > 0)
        { currentTarget = sight.colliderInSight[0].transform; }
        else
        {
            // Simple
            //if (behaviourType == BehaviourType.Sneaky)
            //{
            //    if (locatedFirstTarget)
            //    {
            //        if (lastHeardNoiseMaker != null) { currentTarget = lastHeardNoiseMaker.transform; }
            //    }
            //}
            //else
            //{
            //    if (lastHeardNoiseMaker != null) { currentTarget = lastHeardNoiseMaker.transform; }
            //}

            // Complex, but it's the same
            if ((behaviourType != BehaviourType.Sneaky) || locatedFirstTarget)
            {
                if (lastHeardNoiseMaker != null) { currentTarget = lastHeardNoiseMaker.transform; }
            }
        }

        // Simple
        //if (currentTarget != null)
        //    { locatedFirstTarget = true; }

        // Complex, but it's the same
        locatedFirstTarget |= currentTarget != null;

        if (currentTarget != null) { lastNoticedPosition = currentTarget.position; }
        //Debug.Log(currentTarget);
    }

    bool currentSideStepDirection = false;
    bool oldIsInMinRange = false;
    void UpdateAttack()
    {
        if (currentTarget == null)
        { state = State.CheckLastPosition; }
        else
        {
            bool advanceWhileAttacking = behaviourType == BehaviourType.Valiant;
            bool isInMinRange = Vector3.Distance(currentTarget.position, transform.position) < currentWeapon.GetMinRange();

            if (advanceWhileAttacking)
            {
                if (!isInMinRange)
                { GoTo(currentTarget); }
                else
                {
                    if (oldIsInMinRange != isInMinRange)
                    {
                        currentSideStepDirection = Random.Range(0f, 100f) < 50f;
                    }

                    SideStep(currentSideStepDirection);
                }
            }
            else if ((behaviourType == BehaviourType.Cautious) && (Vector3.Distance(currentTarget.position, transform.position) < fearDistance))
            {
                selectedCover = FindBestCover();
                if (selectedCover)
                { state = State.TakeCover; }
                else
                { GoTo(transform.position); }
            }
            else
            { GoTo(transform.position); }


            // Aiming / Shooting
            LookAt(currentTarget);

            // TODO: chequear el tipo de arma, y 
            // utilizar las llamadas correctas
            // para disparar
            currentWeapon.Shot();

            if (currentWeapon.NeedsReload()) { currentWeapon.Reload(); }

            if (!IsInAttackRange())
            {
                state = State.Seek;
                GoTo(currentTarget);
            }

            oldIsInMinRange = isInMinRange;
        }
    }

    Transform selectedCover;
    [SerializeField] float maxTimeCovering=2f;
    float timeCovering;
    [SerializeField] float thresholdCover = 1.5f;
    void UpdateTakeCover()
    {
        if (Vector3.Distance(selectedCover.position, transform.position) > thresholdCover)
        {
            // Yendo a cubrirse
            GoTo(selectedCover);
        }
        else
        {
            // Estamos a cubierto
            if (currentTarget != null)
            {
                selectedCover = FindBestCover();
                if (!selectedCover)
                { state = State.Attack; }
            }
            else
            {
                timeCovering -= Time.deltaTime;
                if (timeCovering < 0f)
                {
                    timeCovering = maxTimeCovering;
                    state = State.CheckLastPosition; 
                }
            }
        }
    }


    void GoTo(Vector3 position)
    {
        navigateRoute.enabled = false;
        navigateToTransform.enabled = false;
        navigateToPosition.enabled = true;
        navigateToPosition.position = position;
    }

    void Patrol()
    {
        navigateRoute.enabled = true;
        navigateToTransform.enabled = false;
        navigateToPosition.enabled = false;
    }

    void GoTo(Transform targetTransform)
    {
        navigateRoute.enabled = false;
        navigateToTransform.enabled = true;
        navigateToTransform.transformGoTo = targetTransform;
        navigateToPosition.enabled = false;
    }

    void LookAt(Transform lookTarget)
    {
        Vector3 positionOnSameHeight = lookTarget.position;
        positionOnSameHeight.y = transform.position.y;
        transform.LookAt(positionOnSameHeight);
    }

    void SideStep(bool toRight)
    {
        Vector3 destination = transform.position + (toRight ? transform.right : -transform.right);
        GoTo(destination);
    }

    bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) < currentWeapon.GetMaxRange();
    }

    void TargetWithLifeThatNotifies.IDeathNotifiable.NotifyDeath()
    {
        if (state != State.Die)
        {

            state = State.Die;

            navigateToTransform.transformGoTo = null;

            Collider collider = GetComponent<Collider>();
            if (collider) { collider.enabled = false; }

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb) { rb.isKinematic = true; }

            // TODO: Replantear si es aquí el mejor
            // lugar para desactivar todos estos componentes
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("IsDead", true);

            Instantiate(currentWeapon.GetAmmoPrefab(), transform.position, transform.rotation);
            
            Destroy(gameObject, 3f);
        }
    }

    void NoiseMaker.INoiseLitener.onHeard(NoiseMaker noiseMaker)
    {
        //Debug.Log(noiseMaker);
        lastHeardNoiseMaker = noiseMaker;
        timeLeftToForgetNoiseMaker = timeToForgetNoiseMaker;
    }

    Transform FindBestCover()
    {
        Collider[] potentialCovers = Physics.OverlapSphere(transform.position, coverSearchRadius, coverSearchLayerMask, QueryTriggerInteraction.Ignore);

        // TODO: discard covers that are closer to
        //    the currentTarget than this entity
        // TODO: sort potential covers
        foreach (Collider c in potentialCovers)
        {
            RaycastHit hit;
            Vector3 direction =  currentTarget.position- c.transform.position;
            if (Physics.Raycast(c.transform.position, direction, out hit, direction.magnitude, occludingLayerMask, QueryTriggerInteraction.Ignore))
            {

                //if (LayerMask.LayerToName(hit.transform.gameObject.layer)!="Enemigo" && LayerMask.LayerToName(hit.transform.gameObject.layer) != "Player" && LayerMask.LayerToName(hit.transform.gameObject.layer) != "CoverPoints")
                //{
                    Debug.DrawRay(hit.point, Vector3.up * 5f, Color.green);
                    Debug.DrawLine(c.transform.position, hit.point, Color.green, 5f);
                    return c.transform;
                //}
            }
        }

        return null;
    }

    public void SetBehaviour(BehaviourType newBehaviour)
    {
        behaviourType = newBehaviour;
    }
}
