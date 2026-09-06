using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectPoolBase<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected T prefab;
    [SerializeField] protected int initialCapacity = 10;
    [SerializeField] protected int maxSize = 50;

    protected IObjectPool<T> pool;

    protected virtual void Awake()
    {
        pool = new ObjectPool<T>(
            CreateItem,
            OnGetItem,
            OnReleaseItem,
            OnDestroyItem,
            true,
            initialCapacity,
            maxSize
        );
    }

    protected virtual T CreateItem()
    {
        return Instantiate(prefab, transform);
    }

    protected virtual void OnGetItem(T item)
    {
        item.gameObject.SetActive(true);
    }

    protected virtual void OnReleaseItem(T item)
    {
        item.gameObject.SetActive(false);
    }

    protected virtual void OnDestroyItem(T item)
    {
        if (item != null)
        {
            Destroy(item.gameObject);
        }
    }

    public T Get() => pool.Get();

    public void Release(T item) => pool.Release(item);
}
