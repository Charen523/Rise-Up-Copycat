using System;
using System.Collections;
using UnityEngine;

public class MapPattern : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private ObjDynamicOnCollision[] colDynamicObjs;
    [SerializeField] private GameObject[] triggerObjs;

    public event Action<Collision2D> OnChildCollisionEvent;

    [SerializeField] private float moveSpeed = 1f;

    private readonly string floorTag = "Floor";

    #region Unity Life Cycles
    private void Awake()
    {
        OnChildCollisionEvent += OnChildCollisionEnter;
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.down * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnChildCollisionEnter(Collision2D collision)
    {
        if (colDynamicObjs.Length != 0)
        {
            MakeDynamic();
        }
        else if (collision.gameObject.CompareTag(floorTag))
        {
            Destroy(gameObject);
        }
    }
    #endregion


    #region MainMethod
    public void InvokeChildCollision(Collision2D col)
    {
        OnChildCollisionEvent?.Invoke(col);
    }
    #endregion

    #region Sub Method
    private void MakeDynamic()
    {
        foreach (ObjDynamicOnCollision obj in colDynamicObjs)
        {
            obj.rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    #endregion
}
