using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class launcher : MonoBehaviourPunCallbacks
{

    public GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("photon连接成功");

        PhotonNetwork.JoinOrCreateRoom("Room", new Photon.Realtime.RoomOptions() { MaxPlayers = 4 }, default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("尝试加入房间");
        base.OnJoinedRoom();
        Debug.Log("加入房间成功");
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(-3, 1, -5), Quaternion.identity, 0);
        Debug.Log("角色生成成功");
        if (player.GetPhotonView().IsMine) {
            Debug.Log("设置自己的摄象机");
            camera.GetComponent<CameraControll>().SetTarget(player.transform);
        }
        
    }
        
        
}
