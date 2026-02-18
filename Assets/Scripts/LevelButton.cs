using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelNumber;

    [Header("Level Button")]
    [SerializeField] private GameObject badgeImage;
    [SerializeField] private Sprite lockImage;
    [SerializeField] private TMP_Text levelText;

    [Header("LevelData Config")]
    [SerializeField] private LevelDataConfig allLevels;

    void Start()
    {
        if (allLevels != null)
        {
            if (!allLevels.levels[levelNumber].levelUnLock)
            {
                this.GetComponent<Image>().sprite = lockImage;
                this.GetComponent<Button>().interactable = false;
            }
            else
            {
                levelText.text = allLevels.levels[levelNumber].levelNumber.ToString();
            }

            if (allLevels.levels[levelNumber].levelComplete == true)
            {
                badgeImage.SetActive(true);
            }
        }
    }
}
