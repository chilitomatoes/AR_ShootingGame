using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparePool : MonoBehaviour
{
    public List<GameObject> prefabs;

    // Start is called before the first frame update
    void Start()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if(pool!=null && this.prefabs != null)
        {
            foreach (GameObject prefab in this.prefabs)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }
    }

}
