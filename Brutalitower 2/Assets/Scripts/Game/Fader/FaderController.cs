using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderController : MonoBehaviour
{
    public void RemoveFader()
    {
        gameObject.SetActive(false);
    }
}
