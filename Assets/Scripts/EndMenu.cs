using UnityEngine.SceneManagement;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public void GoToSamosTown()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SamosTown");
    }
}
