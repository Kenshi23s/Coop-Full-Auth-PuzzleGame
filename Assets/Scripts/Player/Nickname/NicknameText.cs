using UnityEngine;
using UnityEngine.UI;

public class NicknameText : MonoBehaviour
{
    Transform _owner;

    const float head_offset = 2.5f;

    Text _myText;

    public NicknameText SetOwner(NetworkPlayer owner)
    {
        _owner = owner.transform;
        _myText = GetComponent<Text>();
        return this;
    }

    public void UpdateText(string str)
    {
        _myText.text = str;
    }

    public void UpdatePosition()
    {
        transform.position = _owner.position + Vector3.up * head_offset;
    }

}
