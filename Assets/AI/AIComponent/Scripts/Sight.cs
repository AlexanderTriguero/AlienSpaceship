using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sight : MonoBehaviour
{
    [SerializeField] float sightDistance=10f;
    [SerializeField] float sightWidth=10f;
    [SerializeField] float sightHeight=5f;
    [SerializeField] LayerMask targetLayerMask=Physics.DefaultRaycastLayers;
    [SerializeField] LayerMask ocludingLayerMask = Physics.DefaultRaycastLayers;

    [SerializeField] string[] targetTags= { "Player" };

    public List<Collider> colliderInSight;


    // Update is called once per frame
    void Update()
    {
       Collider[] collidersInBox=
            Physics.OverlapBox(
                transform.position + (transform.forward*sightDistance/2f),
                new Vector3(sightWidth/2f,sightHeight/2f, sightDistance/2f),
                transform.rotation,
                targetLayerMask,
                QueryTriggerInteraction.Ignore);


        colliderInSight.Clear();
        foreach (Collider c in collidersInBox)
        {
            if (targetTags.Contains(c.tag)) { 
                Vector3 direction = c.transform.position - transform.position;
                if (!Physics.Raycast(transform.position, direction, direction.magnitude, ocludingLayerMask, QueryTriggerInteraction.Ignore))
                {
                    colliderInSight.Add(c);
                }
            }
        }
    }
}
