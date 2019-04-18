/***************************************************************************
 * Object Pool Manager
 * 物件池管理器
 * Last Updated: 2018/11/01
 * Description:
 * 1. 當前使用方案2 - OPD
 * 2. 方案1（EVENT）：透過事件進行回收
 * 3. 方案2（OPD）：透過OPD傳值回收
 ***************************************************************************/
#define OPD
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }
    void Awake() { if (Instance == null) Instance = this; }

    private Dictionary<string, ObjectPoolData> dictionaryOPD = new Dictionary<string, ObjectPoolData>(); // 物件池索引

    // Resource Manager 讀取階段生成
    public ObjectPoolData CreateObjectPool(GameObject prefab, int countBatch, int countMax)
    {
        ObjectPoolData OPD = new ObjectPoolData
        {
            pool = new Queue<GameObject>(),
            container = new GameObject().transform,
            prefab = prefab,
            countBatch = countBatch,
        };
        OPD.container.SetParent(transform);
        OPD.container.name = prefab.name;
        dictionaryOPD.Add(prefab.name, OPD);
        return OPD;
    }


    // 建立物件池（舊版）
    public ObjectPoolData CreatObjectPool(GameObject prefab, int countBatch, int countMax)
    {
        // 查詢物件池索引有無相同物件
        if (!dictionaryOPD.ContainsKey(prefab.name))
        {
            // 創建新物件池資料
            ObjectPoolData OPD = new ObjectPoolData
            {
                pool = new Queue<GameObject>(),
                container = new GameObject().transform,
                prefab = prefab,
                countBatch = countBatch,
            };
            OPD.container.transform.SetParent(transform);
            OPD.container.name = prefab.name;
            Clone(OPD);
            // 將資料加入物件池總管
            dictionaryOPD.Add(prefab.name, OPD);
            return OPD;
        }
        else 
        {
            ObjectPoolData OPD = dictionaryOPD[prefab.name];
            if (OPD.index < countMax)
                Clone(OPD);
            return OPD;
        }
    }
    public void Clone(ObjectPoolData OPD)
    {
        for (int i = 0; i < OPD.countBatch; i++)
        {
            GameObject item = Instantiate(OPD.prefab, OPD.container) as GameObject;
            item.name += OPD.index;
#if EVENT
            item.GetComponent<ObjectPoolRecoverySystem>().RecoverCallee += OPD.Recovery; // 方案1
#elif OPD
            item.GetComponent<ObjectRecycleSystem>().OPD = OPD; // 方案2
#endif
            item.SetActive(false);
            OPD.pool.Enqueue(item);
            OPD.index++;
        }
    }
}

public class ObjectPoolData
{
    internal Queue<GameObject> pool;
    internal Transform container;
    internal GameObject prefab;
    internal int countBatch;
    internal int index;
    internal GameObject Reuse(Vector3 position, Quaternion rotation)
    {
        if(pool.Count <= 0)
            ObjectPoolManager.Instance.Clone(this);
        GameObject item = pool.Dequeue();
        item.transform.SetPositionAndRotation(position, rotation);
        item.SetActive(true);
        return item;
    }
    internal GameObject ReuseAmmo(Vector3 position, Quaternion rotation)
    {
        if (pool.Count <= 0)
            ObjectPoolManager.Instance.Clone(this);
        GameObject item = pool.Dequeue();
        item.transform.SetPositionAndRotation(position, rotation);


        //item.SetActive(true);
        return item;
    }
#if EVENT
    public void Recycle(object sender, RecoverEventArgs e)
    {
        GameObject gameObject = e.objRecovery;
        gameObject.transform.SetParent(container);
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }
#elif OPD
    // 方案2
    public void Recycle(GameObject item)
    {
        item.transform.SetParent(container);
        item.SetActive(false);
        pool.Enqueue(item);
    }
#endif
}


// 需要回收的物件需繼承此類
public class ObjectRecycleSystem : MonoBehaviour
{
    protected float timeRecovery;
#if EVENT
    public event RecoverEventHandler RecoverCallee;
#elif OPD
    public ObjectPoolData OPD;
#endif

    protected virtual void Recycle(GameObject obj)
    {
#if EVENT
        RecoverCallee(this, new RecoverEventArgs(obj));
#elif OPD
        OPD.Recycle(obj);
#endif
    }
}

#if EVENT
/// <summary>
/// 回收事件
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public delegate void RecoverEventHandler(object sender, RecoverEventArgs e);
public delegate void RecoverEventHandler<TEventArgs>(object sender, TEventArgs e) where TEventArgs : EventArgs;
public class RecoverEventArgs : EventArgs
{
    public readonly GameObject objRecovery;
    public RecoverEventArgs(GameObject obj)
    {
        objRecovery = obj;
    }
}
#endif