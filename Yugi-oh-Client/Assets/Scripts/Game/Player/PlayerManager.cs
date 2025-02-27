using InexperiencedDeveloper.Core;
using Riptide;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private GameObject m_PlayerPrefab;
    private static GameObject s_PlayerPrefab;
    private static Dictionary<ushort, Player> s_Player = new Dictionary<ushort, Player>();
    public static Player GetPlayer(ushort id)
    {
        s_Player.TryGetValue(id, out Player Player);
        return Player;
    }
    public static bool RemovePlayer(ushort id)
    {
        if (s_Player.TryGetValue(id, out Player Player))
        {
            s_Player.Remove(id);
            return true;
        }
        return false;
    }

    public static Player LocalPlayer => GetPlayer(NetworkManager.Instance.Client.Id);
    public static bool IsLocalPlayer(ushort id) => id == LocalPlayer.Id;

    protected override void Awake()
    {
        base.Awake();
        s_PlayerPrefab = m_PlayerPrefab;
    }

    public void SpawnInitalPlayer(string username)
    {
        Player Player = Instantiate(s_PlayerPrefab, Vector3.zero,Quaternion.identity).GetComponent<Player>();
        Player.name = $"{username} -- LOCAL PLAYER (WAITING FOR SERVER)";
        ushort id = NetworkManager.Instance.Client.Id;
        Player.Init(id, username, true);
        s_Player.Add(id, Player);
        Player.RequestInit();
    }

    private static void InitializeLocalPlayer()
    {
        LocalPlayer.name = $"{LocalPlayer.Username} -- {LocalPlayer.Id} -- LOCAL";
    }

    #region Messages

    /* recieving message */
    [MessageHandler((ushort)ServerToClientMsg.ApproveLogin)]
    private static void RecieveApproveLogin(Message msg)
    {
        bool approve = msg.GetBool();
        if (approve)
        {
            InitializeLocalPlayer();
        }
    }

    #endregion
}
