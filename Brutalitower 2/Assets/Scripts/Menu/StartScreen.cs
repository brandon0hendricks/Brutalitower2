using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;



public class StartScreen : MonoBehaviour
{
    public GameObject CanvasObject;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("X was pressed.");
        //SceneManager.LoadScene("MainMenu");
        CanvasObject.SetActive(false);
        }
    }
}
