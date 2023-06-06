using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class SessionListHandler : MonoBehaviour
{
    [SerializeField] LobbyHandler _networkRunner;

    [SerializeField] Text _statusText;

    [SerializeField] SessionItem _sessionItemPrefab;

    [SerializeField] VerticalLayoutGroup _verticalLayoutGroup;

    private void OnEnable()
    {
        _networkRunner.OnSessionListUpdate += ReceiveSessionList;
    }

    private void OnDisable()
    {
        _networkRunner.OnSessionListUpdate -= ReceiveSessionList;
    }
    
    void ClearSessionList()
    {
        foreach (Transform child in _verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        _statusText.gameObject.SetActive(false);
    }

    void ReceiveSessionList(List<SessionInfo> newSessionList)
    {
        //limpiamos la lista de sesiones creadas antes
        ClearSessionList();

        //si no hay
        if (newSessionList.Count == 0)
        {
            //Mostramos el texto diciendo que no se encontraron
            NoSessionsFound();
        }
        else
        {
            //Agregamos cada sesion a la lista
            foreach (var session in newSessionList)
            {
                AddSessionToList(session);
            }
        }
    }

    void NoSessionsFound()
    {
        _statusText.text = "No Sessions Found";

        _statusText.gameObject.SetActive(true);
    }

    void AddSessionToList(SessionInfo session)
    {
        var newSessionItem = Instantiate(_sessionItemPrefab, _verticalLayoutGroup.transform);
        newSessionItem.SetSessionInfo(session);

        newSessionItem.OnJoinSession += JoinSelectedSession;
    }
    
    void JoinSelectedSession(SessionInfo session)
    {
        _networkRunner.JoinSession(session);
    }
}
