using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public Slider generationSlider;
    public Text generationText;

    private int generations = 10; // Default value

    private void Start()
    {
        generationSlider.onValueChanged.AddListener(UpdateGenerationValue);
        UpdateGenerationValue(generationSlider.value);
    }

    public void UpdateGenerationValue(float value)
    {
        generations = Mathf.RoundToInt(value);
        generationText.text = "Generations: " + generations;
    }

    public void OnPlayButtonPressed()
    {
        GameManager.Instance.StartGame(generations); 
    }
}
