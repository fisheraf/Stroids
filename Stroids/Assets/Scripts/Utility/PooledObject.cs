using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [System.NonSerialized]
    ObjectPool poolInstanceForPrefab;

    public ObjectPool Pool { get; set; }
    public Rigidbody2D rb { get; private set; }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public T GetPooledInstance<T>() where T : PooledObject
    {
        if (!poolInstanceForPrefab)
        {
            poolInstanceForPrefab = ObjectPool.GetPool(this);
        }
        return (T)poolInstanceForPrefab.GetObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Kill Zone"))
        {
            ReturnToPool();
        }
    }

    /*private void OnLevelWasLoaded()
    {
        ReturnToPool();
    }*/

    public void ReturnToPool()
    {
        if (Pool)
        {
            Pool.AddObject(this);
        }
        else
        {
            Debug.Log("I die!");
            Destroy(gameObject);
        }
    }
}