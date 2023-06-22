using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] LobbyHandler _networkHandler;

    [Header("Panels")]
    [SerializeField] GameObject _initialPanel;
    [SerializeField] GameObject _statusPanel;
    [SerializeField] GameObject _browserPanel;
    [SerializeField] GameObject _hostGamePanel;

    [Header("Buttons")]
    [SerializeField] Button _joinLobbyButton;
    [SerializeField] Button _openHostButton;
    [SerializeField] Button _hostGameButton;

    [Header("Inputfields")]
    [SerializeField] InputField _hostSessionName;
    [SerializeField] InputField _nicknameInputField;

    [Header("Texts")]
    [SerializeField] Text _statusText;
    [SerializeField] Text _failedConnectionText;

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerNickname"))
            _nicknameInputField.text = PlayerPrefs.GetString("PlayerNickname");


        _joinLobbyButton.onClick.AddListener(BTN_JoinLobby);
        _openHostButton.onClick.AddListener(BTN_ShowHostPanel);
        _hostGameButton.onClick.AddListener(BTN_CreateGameSession);

        //cuando el NR se termine de conectar al Lobby        
        _networkHandler.OnJoinedLobby += () =>
        {
            _statusPanel.SetActive(false);
            _browserPanel.SetActive(true);
        };

        //cuando falla el unirse al lobby
        _networkHandler.OnFailedJoinLobby += () =>
         {
             _statusPanel.SetActive(false);
             _initialPanel.SetActive(true);
             _failedConnectionText.gameObject.SetActive(true);
         };
    }

    #region Button Methods

    void BTN_JoinLobby()
    {
        string x = VocabularyManager.ValidNickname(_nicknameInputField.text);
        PlayerPrefs.SetString("PlayerNickname", x);
        PlayerPrefs.Save();

        _networkHandler.JoinLobby();

        _initialPanel.SetActive(false);

        _statusPanel.SetActive(true);

        _statusText.text = "Joining Lobby...";
    }

    void BTN_ShowHostPanel()
    {
        _browserPanel.SetActive(false);

        _hostGamePanel.SetActive(true);
    }

    void BTN_CreateGameSession()
    {
        _networkHandler.CreateSession(_hostSessionName.text, "Level");
    }

    #endregion
}
