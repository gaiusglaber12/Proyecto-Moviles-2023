using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageView : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Transform center = null;
    [SerializeField] private GameObject water = null;
    #endregion

    #region PRIATE_FIELDS
    private List<GameObject> currentAnimals = null;
    #endregion

    #region PROPERTIES
    [NonSerialized] public int CantAnimals = 0;
    #endregion

    #region PUBLIC_METHODS
    public void Init(int cantAnimals, GameObject animal, bool isAquatic)
    {
        currentAnimals = new List<GameObject>();

        CantAnimals = cantAnimals;
        water.SetActive(isAquatic);

        for (int i = 0; i < cantAnimals; i++)
        {
            GameObject go = Instantiate(animal, center.position, Quaternion.identity, transform);
            Animation animation = go.GetComponent<Animation>();
            for (int j = 0; j < 10; j++)
            {
                animation.PlayQueued("Fear");
            }
            currentAnimals.Add(go);
        }
    }
    #endregion
}
