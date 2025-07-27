using UnityEngine;
using System.Collections.Generic;

public class RIPlacementManager : MonoBehaviour
{
    [SerializeField] RI.PlacementState _placementState;

    [SerializeField] List<GameObject> _tSections;

    void Start()
    {

    }

    void Update()
    {

    }

    public void ChangePlacementState(RI.PlacementState newState)
    {
        _placementState = newState;

        foreach (var section in _tSections)
        {
            section.SetActive(false);
        }
        _tSections[Mathf.Clamp((int)newState - 1, 0, (int)RI.PlacementState.END - 1)].SetActive(true);
    }
}

namespace RI
{
    public enum PlacementState
    {
        NONE,
        Section1,
        Section2,
        Section3,
        END
    }
}