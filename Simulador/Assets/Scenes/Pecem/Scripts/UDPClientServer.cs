using System.Net;
using System.Net.Sockets;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace SABP.UDP
{
    public class NewBehaviourScript : MonoBehaviour
    {
        public TMP_InputField Input;
        public TMP_Text Text;

        UDPServer s;
        UDPClient c;

        // Start is called before the first frame update
        void Start()
        {
            s = gameObject.AddComponent<UDPServer>();
            c = gameObject.AddComponent<UDPClient>();

            Text.text = GetLocalIPAddress();
        }

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter) && Input.text != "")
            {
                Parse(Input.text);
                Input.text = "";
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void Parse(string text)
        {
            string[] words = text.Split(' ');

            if (words.Length == 2)
            {
                if (words[0].ToLower() == "create" && words[1].ToLower() == "server")
                    s.Create(Text);
                else if (words[0].ToLower() == "start" && words[1].ToLower() == "server")
                    s.StartReceivingData();
                else if (words[0].ToLower() == "close" && words[1].ToLower() == "server")
                    ;
                else if (words[0].ToLower() == "create" && words[1].ToLower() == "client")
                    c.Create(Text, 23001, 23000);
                else if (words[0].ToLower() == "send" && words[1].ToLower() == "broadcast")
                    c.SendBraodcast("oi, isto é um teste!");

                Input.Select();
                Input.ActivateInputField();
            }
            else
                Text.text = "comando não reconhecido";
        }
    }
}