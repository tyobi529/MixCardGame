using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// MonoBehaviourではなくMonoBehaviourPunCallbacksを継承して、Photonのコールバックを受け取れるようにする
public class LoginController : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // PhotonServerSettingsに設定した内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }

    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            var v = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));


            PhotonNetwork.Instantiate("GameManager", v, Quaternion.identity);
        }

        else
        {
            //var v = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            ////テスト
            //PhotonNetwork.Instantiate("TestCube", v, Quaternion.identity);
        }



    }
}