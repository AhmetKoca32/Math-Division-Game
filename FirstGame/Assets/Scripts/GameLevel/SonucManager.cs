using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SonucManager : MonoBehaviour
{
  public void YenidenBasla()
    {
        SceneManager.LoadScene("GameLevel");
    }
  public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("MenuLevel");
    }

}
