using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFade : MonoBehaviour
{

    [SerializeField] private Camera gameOverCam;
    [SerializeField] private Animator backAnimator;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject gameOverUI;


    void Activate_Fade()
    {
        backAnimator.SetBool("FadeIn",true);
        playerUI.SetActive(false);
        StartCoroutine(GameOvertext());
    }
    private IEnumerator GameOvertext()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.SetActive(true);
    }
    
    void Change_camera()
    {
        gameOverCam.cullingMask = LayerMask.GetMask("Player");
    }


}
