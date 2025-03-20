using System;
using UnityEngine;

public class MapParent : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject[] collisionDynamicObjs;
    [SerializeField] private GameObject[] triggerObjs;

    public event Action<Collision> OnChildCollisionEnter;

    private void Awake()
    {
        
    }

    private void MakeDynamic()
    {

    }
}
