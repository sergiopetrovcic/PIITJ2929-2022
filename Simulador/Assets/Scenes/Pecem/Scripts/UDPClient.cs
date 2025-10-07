using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace SABP.UDP
{
    public class UDPClient : MonoBehaviour
    {
        private TMPro.TMP_Text Text;

        private Socket _socket;
        private IPEndPoint _IPEPBroadcast, _IPEPLocal;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Create(TMPro.TMP_Text text, int localPort, int remotePort)
        {
            Text = text;
            _IPEPBroadcast = new IPEndPoint(IPAddress.Broadcast, remotePort);
            _IPEPLocal = new IPEndPoint(IPAddress.Any, localPort);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Text.text = "Cliente criado";
        }

        public void SendBraodcast(string data)
        {
            if (string.IsNullOrEmpty(data))
                return;

            try
            {
                if (!_socket.IsBound)
                    _socket.Bind(_IPEPLocal);

                byte[] dataBytes = Encoding.ASCII.GetBytes(data);

                SocketAsyncEventArgs saea = new SocketAsyncEventArgs();
                saea.SetBuffer(dataBytes, 0, dataBytes.Length);
                saea.RemoteEndPoint = _IPEPBroadcast;
                saea.Completed += Saea_Completed;
                _socket.SendToAsync(saea);
            }
            catch (System.Exception ex)
            {
                Text.text = ex.Message;
            }
        }

        private void Saea_Completed(object sender, SocketAsyncEventArgs e)
        {
            Text.text = "Dados enviados com sucesso para: " + e.RemoteEndPoint.ToString();
        }
    }
}