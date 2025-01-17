using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TextMeshProUGUI playersCountText;

    private NetworkVariable<int> playersNum = new NetworkVariable<int> (0, NetworkVariableReadPermission.Everyone);

    private void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Hide();
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    private void Update()
    {
        playersCountText.text = ("Players " + playersNum.Value.ToString());
        if (!IsServer)
            return;
        playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }

    void Hide()
    {
        hostButton.gameObject.SetActive(false);
        clientButton.gameObject.SetActive(false);
    }
}
