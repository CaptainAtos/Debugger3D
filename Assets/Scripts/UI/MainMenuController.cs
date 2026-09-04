using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Szene")]
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Panels")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Optionen")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (optionsPanel != null) { optionsPanel.SetActive(false); }
        if (creditsPanel != null) { creditsPanel.SetActive(false); }

        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume);
        }
    }

    public void OnStartPressed()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnOptionsPressed()
    {
        optionsPanel.SetActive(true);
    }

    public void OnCloseOptionsPressed()
    {
        optionsPanel.SetActive(false);
    }

    public void OnCreditsPressed()
    {
        creditsPanel.SetActive(true);
    }

    public void OnCloseCreditsPressed()
    {
        creditsPanel.SetActive(false);
    }

    public void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetVolume(float linearValue)
    {
        float clamped = Mathf.Clamp(linearValue, 0.0001f, 1f);
        float dB = Mathf.Log10(clamped) * 20f;
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("MasterVolume", linearValue);
    }

    //Just a Comment cause Git Push Problems resolving
    // Next Comment cause fetiching Problems, uuugh I hate GitHub ... but I need it.

}