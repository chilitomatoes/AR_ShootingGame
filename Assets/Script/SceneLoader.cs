using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    public Animator anim;
    private void Awake()
    {

    }

    private void Update()
    {

    }

    public void LoadScene(int scene)
    {
        StartCoroutine(sceneTransition(scene));
        
    }

    IEnumerator sceneTransition(int scene)
    {
        anim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel(scene);
        anim.SetTrigger("FadeIn");
    }
}
