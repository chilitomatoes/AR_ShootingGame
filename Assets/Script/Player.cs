using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{

    [Header("Bullet Pool Setting")]
    public List<GameObject> pool;
    public int poolSize = 5;
    public GameObject bulletSpawnPoint;

    [Header("Shoot Setting")]
    public bool isShooting; 
    public float bulletSpeed = 0.2f;
    public float firingRate = 2f;

    [Header("Score")]
    public bool victory;
    public bool lose;

    private float lastShootTime;
    private PhotonView PV;
    private string bullet = "Bullet";
    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            CreateBulletPool();
        }   
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isShooting)
        {
            if(PV.IsMine)
            {
                PV.RPC("Shoot",RpcTarget.All);
            }

        }
    }

    [PunRPC]
    void Shoot()
    {
        GameObject bullet = GetPooledObject();
        if (bullet != null && Time.time >= lastShootTime + (1 / firingRate))
        {
            bullet.transform.position = bulletSpawnPoint.transform.position;
            bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
            lastShootTime = Time.time;
            bullet.SetActive(true);
        }
    }

    public void CreateBulletPool()
    {
        GameObject tmp;
        for (int i = 0; i < poolSize; i++)
        {
            tmp = PhotonNetwork.Instantiate(bullet, Vector3.zero, Quaternion.identity);
            PV.RPC("assignPool", RpcTarget.All, tmp.GetComponent<PhotonView>().ViewID);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        return null;
    }

    [PunRPC]
    void assignPool(int viewID)
    {
        GameObject tmp = PhotonNetwork.GetPhotonView(viewID).gameObject;
        pool.Add(tmp);
        tmp.SetActive(false);
    }
}
