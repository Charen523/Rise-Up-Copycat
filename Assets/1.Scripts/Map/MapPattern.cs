using System;
using UnityEngine;

public class MapPattern : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private ObjDynamicOnCollision[] colDynamicObjs;
    [SerializeField] private GameObject[] triggerObjs;

    public event Action<Collision2D> OnChildCollisionEvent;

    [SerializeField] private float moveSpeed = 1.5f;

    #region Unity Life Cycles
    private void Awake()
    {
        OnChildCollisionEvent += OnChildCollisionEnter;
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.down * moveSpeed * Time.fixedDeltaTime;

        if (transform.position.y == -15)
        {
            Destroy(gameObject);
        }
    }

    private void OnChildCollisionEnter(Collision2D collision)
    {
        if (colDynamicObjs.Length != 0)
        {
            MakeDynamic();
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
            if (obj == null || obj.rb == null)
                continue;

            obj.rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    #endregion
}
