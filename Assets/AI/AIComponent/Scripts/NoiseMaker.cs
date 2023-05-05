using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{

    [SerializeField] float noiseRadius=1f;
    [SerializeField] bool onlyWhenMoving=false;
    [SerializeField] bool silent=false;
    [SerializeField] float noiseFrequency=5f;


    public void Start()
    {
        oldPosition = transform.position;
    }
    public interface INoiseLitener
    {
        void onHeard(NoiseMaker noiseMaker);
    }

    public void MakeNoise()
    {
        InternalMakeNoise();
    }

    Vector3 oldPosition;
    float timeSinceLastNoise;
    private void Update()
    {
        if (!silent)
        {
            timeSinceLastNoise += Time.deltaTime;
            if (timeSinceLastNoise > 81f / noiseFrequency)
            {
                timeSinceLastNoise -= (1f / noiseFrequency);

                bool makeSoundAllTheTIme = !onlyWhenMoving;
                if (makeSoundAllTheTIme || oldPosition != transform.position)
                {
                    InternalMakeNoise();
                }
            }
        }
        oldPosition = transform.position;
    }

    void InternalMakeNoise()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, noiseRadius);

        foreach(Collider c in colliders)
        {
            INoiseLitener listener = c.GetComponent<INoiseLitener>();
            listener?.onHeard(this);
        }
    }
}
