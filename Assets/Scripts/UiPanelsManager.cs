using UnityEngine;

public class UiPanelsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPanel;

    [SerializeField]
    private GameObject gameOverPanel;

    private void OnEnable()
    {
        Player.OnBirdStateChange += Player_OnBirdStateChange;        
    }

    private void OnDisable()
    {
        Player.OnBirdStateChange -= Player_OnBirdStateChange;
    }

    private void Player_OnBirdStateChange(BirdState state)
    {
        ToggleTutorial(state == BirdState.Idle);
        ToggleGameOver(state == BirdState.Dead);
    }

    private void ToggleTutorial(bool toggleVisible)
    {
        tutorialPanel.SetActive(toggleVisible);
    }

    private void ToggleGameOver(bool toggleVisible)
    {
        gameOverPanel.SetActive(toggleVisible);
    }
}
