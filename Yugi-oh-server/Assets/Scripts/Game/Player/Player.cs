using Riptide;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ushort Id { get; private set; }
    public string Username { get; private set; }

    public void Init(ushort id, string username)
    {
        Id = id;
        Username = username;
    }

    private void OnDestroy()
    {
        PlayerManager.RemovePlayer(Id);
    }

    #region Messages

    /* sending message */
    public void ApproveLogin(bool approve)
    {
        Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.ApproveLogin);
        msg.AddBool(approve);
        NetworkManager.Instance.Server.Send(msg, Id);
    }    

    #endregion
}
