using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {
    

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("ShipSelect");
    }

    public void OnManagementClicked()
    {
        SceneManager.LoadScene("Ship List");
    }

    public void OnRandomClicked()
    {
        SceneManager.LoadScene("RandomSelect");
    }
}
