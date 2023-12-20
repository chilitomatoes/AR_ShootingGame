using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Bullet : MonoBehaviour
{
    private string env = "Environment";
    private string player1 = "Player1";
    private string player2 = "Player2";

    private PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == env)
        {
            if (PV.IsMine)
            {
                PV.RPC("DeactivateBullet", RpcTarget.All);
            }
        }
        else if(other.tag == player1)
        {
            if (PV.IsMine)
            {
                int P2Score = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["P2SCORE"].ToString());
                P2Score++;
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "P2SCORE", P2Score } });
                PV.RPC("DeactivateBullet", RpcTarget.All);
            }
        }
        else if (other.tag == player2)
        {        
            if (PV.IsMine)
            {
                int P1Score = int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["P1SCORE"].ToString());
                P1Score++;
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "P1SCORE", P1Score } });
                PV.RPC("DeactivateBullet", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void DeactivateBullet()
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}
