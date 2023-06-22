using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameHandler : MonoBehaviour
{
    public static NicknameHandler instance { get; private set; }

    [SerializeField] NicknameText _nicknamePrefab;

    List<NicknameText> _allNicknames;

    public NicknameText AddNickname( NetworkPlayer owner)
    {
        return default;
        var newNickname = Instantiate(_nicknamePrefab, transform).SetOwner(owner);

        _allNicknames.Add(newNickname);

        owner.OnLeft += () =>
        {
            _allNicknames.Remove(newNickname);
            Destroy(newNickname.gameObject);
        };

        return newNickname;
    }


    private void LateUpdate()
    {
        return;
        foreach (var nickname in _allNicknames)
        {
            nickname.UpdatePosition();
        }
    }
}
