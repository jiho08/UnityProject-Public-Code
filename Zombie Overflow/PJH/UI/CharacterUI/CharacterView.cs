using Players;
using Score.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Character
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private CharacterSO[] characters;

        [Header("<color=yellow>[ Character UI ]</color>")] [SerializeField]
        private RawImage characterImage;

        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private Button selectButton;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Button closeButton;

        private int _characterIndex;

        private void Awake()
        {
            selectButton.onClick.AddListener(HandleSelectButton);
            leftButton.onClick.AddListener(HandleLeftButton);
            rightButton.onClick.AddListener(HandleRightButton);
            closeButton.onClick.AddListener(HandleCloseButton);
            
            SetCharacter();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            selectButton.onClick.RemoveListener(HandleSelectButton);
            leftButton.onClick.RemoveListener(HandleLeftButton);
            rightButton.onClick.RemoveListener(HandleRightButton);
            closeButton.onClick.RemoveListener(HandleCloseButton);
        }

        private void HandleCloseButton()
        {
            gameObject.SetActive(false);
        }

        private void HandleRightButton()
        {
            ++_characterIndex;
            SetCharacter();

            if (_characterIndex == characters.Length - 1)
                rightButton.gameObject.SetActive(false);
            
            leftButton.gameObject.SetActive(true);
        }

        private void HandleLeftButton()
        {
            --_characterIndex;
            SetCharacter();
            
            if (_characterIndex == 0)
                leftButton.gameObject.SetActive(false);
            
            rightButton.gameObject.SetActive(true);
        }

        private void HandleSelectButton()
        {
            GameManager.Instance.SelectCharacter(characters[_characterIndex]);
            GameManager.Instance.SelectMap();
            SceneManager.LoadScene(Random.Range(1, 4));
        }

        private void SetCharacter()
        {
            characterImage.texture = characters[_characterIndex].characterRenderTexture;
            characterName.text = characters[_characterIndex].characterName;
        }
    }
}