using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkView : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private CageView cagePrefab = null;
    [SerializeField] private Transform[] cagePositions = null;
    #endregion

    #region PRIVATE_FIELDS
    private List<CageView> currCages = null;
    #endregion

    #region PUBLIC_METHODS
    public void Init(int cantCages, int minAnimalsPerCage, int maxAnimalsPerCage, GameObject animal, bool isAquatic)
    {
        currCages = new List<CageView>();
        for (int i = 0; i < cantCages; i++)
        {
            Vector3 spawnPosition = Vector3.zero;
            bool containsPosition = false;
            while (!containsPosition)
            {
                spawnPosition = cagePositions[Random.Range(0, cagePositions.Length)].position;
                bool founded = false;
                for (int j = 0; j < currCages.Count; j++)
                {
                    if (currCages[j].transform.position == spawnPosition)
                    {
                        founded = true;
                        containsPosition = true;
                    }
                }
                containsPosition = !founded;
            }
            GameObject go = Instantiate(cagePrefab.gameObject, spawnPosition, Quaternion.identity, transform);
            CageView cageView = go.GetComponent<CageView>();
            cageView.Init(Random.Range(minAnimalsPerCage, maxAnimalsPerCage), animal, isAquatic);
        }
    }

    public void DeInit()
    {
        for (int i = 0; i < currCages.Count; i++)
        {
            Destroy(currCages[i].gameObject);
        }
    }
    #endregion
}
