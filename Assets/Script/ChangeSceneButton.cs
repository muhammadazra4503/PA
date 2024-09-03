using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    // Nama scene yang ingin dituju
    public string sceneName;

    // Fungsi ini dipanggil ketika tombol ditekan
    public void OnButtonPress()
    {
        // Memuat scene dengan nama yang telah ditentukan
        SceneManager.LoadScene(sceneName);
    }
}
