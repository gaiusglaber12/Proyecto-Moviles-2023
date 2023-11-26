using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageView : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Transform center = null;
    [SerializeField] private GameObject water = null;
    [SerializeField] private List<GameObject> cageGOS = null;
    [SerializeField] private float minForce = 0.0f;
    [SerializeField] private float maxForce = 0.0f;
    [SerializeField] private GameObject particleSystemPrefab = null;
    [SerializeField] private GameObject[] textParticleSystemPrefabs = null;
    #endregion

    #region PRIATE_FIELDS
    private List<GameObject> currentAnimals = null;
    private bool onCollisioned = false;
    #endregion

    #region PROPERTIES
    [NonSerialized] public int CantAnimals = 0;
    #endregion

    #region ACTIONS
    private Action<int> onBallHit = null;
    #endregion

    #region PUBLIC_METHODS
    public void Init(int cantAnimals, GameObject animal, bool isAquatic, Action<int> onBallHit)
    {
        currentAnimals = new List<GameObject>();
        this.onBallHit = onBallHit;
        CantAnimals = cantAnimals;

        for (int i = 0; i < cantAnimals; i++)
        {
            GameObject go = Instantiate(animal, center.position, Quaternion.identity, transform);
            currentAnimals.Add(go);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallEntity>() != null)
        {
            Destroy(collision.gameObject);
            Handheld.Vibrate();
            var particleSystemGO = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
            Destroy(particleSystemGO, 3);
            var textParticleSystemGO = Instantiate(textParticleSystemPrefabs[UnityEngine.Random.Range(0, textParticleSystemPrefabs.Length)], transform.position, Quaternion.identity);
            Destroy(textParticleSystemGO, 3);
            for (int i = 0; i < cageGOS.Count; i++)
            {
                var rb = cageGOS[i].AddComponent<Rigidbody>();
                rb.AddForce(new Vector3(UnityEngine.Random.Range(minForce, maxForce), UnityEngine.Random.Range(minForce, maxForce), UnityEngine.Random.Range(minForce, maxForce)));
                Destroy(cageGOS[i], 3);
            }
            cageGOS.Clear();
            for (int i = 0; i < currentAnimals.Count; i++)
            {
                var rb = currentAnimals[i].AddComponent<Rigidbody>();
                rb.AddForce(new Vector3(UnityEngine.Random.Range(minForce, maxForce), UnityEngine.Random.Range(minForce, maxForce), UnityEngine.Random.Range(minForce, maxForce)));
                Destroy(currentAnimals[i], 3);
            }
            if (!onCollisioned)
            {
                onCollisioned = true;
                onBallHit.Invoke(currentAnimals.Count);
            }
            var getrb = gameObject.GetComponent<Rigidbody>();
            if (getrb == null)
            {
                getrb = gameObject.AddComponent<Rigidbody>();
            }
            if (getrb != null)
            {
                getrb.AddForce(new Vector3(UnityEngine.Random.Range(minForce, maxForce), UnityEngine.Random.Range(minForce, maxForce), UnityEngine.Random.Range(minForce, maxForce)));
            }
            Destroy(gameObject, 3);
            currentAnimals.Clear();
        }
    }
    #endregion
}
