using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("New Game")]
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text controllerSensativityTextValue;
    [SerializeField] private Slider controllerSensativitySlider;
    [SerializeField] private int defaultSensativity = 4;

    public int mainControllerSensativity = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt;
    
    [Header("Levels To Load")]
    [SerializeField] private string newGameLevel;
    [SerializeField] private GameObject noSavedGameDialog;

    private string levelToLoad;

    public void NewGameDialogYes(){
        StartCoroutine(LoadLevel(newGameLevel));
    }

    // Play the animation, wait and then load the scene.
    private IEnumerator LoadLevel(string levelToLoad){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelToLoad);
    }

    public void LoadGameDialogYes()
    {
        if(PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }else{
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);

        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSensativity(float sensativity)
    {
        mainControllerSensativity = Mathf.RoundToInt(sensativity);
        controllerSensativityTextValue.text = mainControllerSensativity.ToString("0");
    }

    public void GameplayApply(){
        if(invertYToggle.isOn){
            PlayerPrefs.SetInt("masterInvertY", 1);
        }else{
            PlayerPrefs.SetInt("masterInvertY", 0);
        }

        PlayerPrefs.SetFloat("masterSensativity", mainControllerSensativity);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType){
        if(MenuType == "Audio"){
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if(MenuType == "Gameplay"){
            mainControllerSensativity = defaultSensativity;
            controllerSensativitySlider.value = defaultSensativity;
            controllerSensativityTextValue.text = defaultSensativity.ToString("0");
            invertYToggle.isOn = false;
            GameplayApply();
        }
    }

    public IEnumerator ConfirmationBox(){
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
