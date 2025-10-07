using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace SABP.UDP
{
    public class UDPServer : MonoBehaviour
    {
        private TMPro.TMP_Text Text;

        Socket _socket;
        IPEndPoint _IPEP;
        private int retryCount;
        List<EndPoint> _endpoints;

        public void Create(TMPro.TMP_Text text)
        {
            Text = text;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _IPEP = new IPEndPoint(IPAddress.Parse("192.168.15.8"), 23000);
            _socket.EnableBroadcast = true;
            _endpoints = new List<EndPoint>();
            Text.text = "Server criado";
        }

        public void StartReceivingData()
        {
            try
            {
                SocketAsyncEventArgs saea = new SocketAsyncEventArgs();
                saea.SetBuffer(new byte[1024], 0, 1024);
                saea.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                if (!_socket.IsBound)
                    _socket.Bind(_IPEP);

                saea.Completed += Saea_Completed;

                if (!_socket.ReceiveFromAsync(saea))
                {
                    Text.text = "Falha ao receber dados - erro de socket: " + saea.SocketError;
                    if (retryCount++ >= 10)
                    {

                    }
                    else
                    {
                        StartReceivingData();
                    }
                }
                Text.text = "Server pronto para receber";
            }
            catch (System.Exception ex)
            {
                Text.text = ex.Message;
            }
        }

        private void Saea_Completed(object sender, SocketAsyncEventArgs e)
        {
            string textReceived = Encoding.ASCII.GetString(e.Buffer, 0, e.BytesTransferred);
            Text.text = "Texto recebido: " + textReceived + ". #B: " + e.BytesTransferred.ToString() + ". EP: " + e.RemoteEndPoint.ToString();

            if (textReceived.Equals("<DISCOVER>"))
            {
                if (!_endpoints.Contains(e.RemoteEndPoint))
                {
                    _endpoints.Add(e.RemoteEndPoint);
                    Text.text = "New client " + e.RemoteEndPoint + " added.";

                    
                }
            }

            StartReceivingData();
        }
    }
}