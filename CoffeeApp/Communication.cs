using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Sockets;
using System.Threading;

namespace CoffeeApp
{
    static class Communication
    {
        static System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
        static Socket socket = client.Client;
        static int timeout = 1000; //timeout in millisecs
        const int Port = 1500;
        public static bool isConnected;

        public static bool connect(String IPAddress)
        {
            //try
            //{
            //    for (int i = 0; i < 2; i++)
            //    {
            //        client.BeginConnect(IPAddress, Port, ConnectCallback, client);
            //    }

            //}
            //catch (Exception)
            //{

            //}
            //    return isConnected;
            try
            {
                client.ConnectAsync(IPAddress, 1500).Wait(timeout);
            }
            catch (Exception e)
            {
                return false;
            }
            isConnected = true;
            return true;
        }

        public static bool Send(String message)
        {
            try
            {
                NetworkStream networkStream = client.GetStream();
                Byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                isConnected = false;
                return false;
            }
        }
        public static string receive()
        {
            byte[] buffer = new byte[1024];
            int startTickCount = System.Environment.TickCount;
            int received = 0;  // how many bytes is already received
            if (System.Environment.TickCount > startTickCount + timeout) { return null; }
            try
            {
                received += socket.Receive(buffer, 0 + received, buffer.Length - received, SocketFlags.None);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.WouldBlock ||
                    ex.SocketErrorCode == SocketError.IOPending ||
                    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                {
                    // socket buffer is probably empty, wait and try again
                    Thread.Sleep(30);
                }
                else
                    throw ex;  // any serious error occurr
            }
            string receivedString = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return receivedString;
        }
        /// <summary>
        /// receives message with corresponding code, disposes all other replies and timeouts after 5 tries.
        /// </summary>
        /// <returns></returns>
        public static string receiveMessage(string msgCode, int retry = 0)
        {
            string receivedMessage = receive();
            if (retry > 4)
            {
                return null; // timeout.
            }
            if (receivedMessage.Substring(0, 4) == msgCode) //is the correct message received?
            {
                receivedMessage = receivedMessage.Remove(0, 4); // remove code
                return receivedMessage;
            }
            else
            {
                Thread.Sleep(250); //retry
                return receiveMessage(msgCode, retry + 1);
            }
        }

        private static void ConnectCallback(IAsyncResult asyncResult)
        {
            try
            {
                TcpClient Connectionclient = (TcpClient)asyncResult.AsyncState;
                Connectionclient.EndConnect(asyncResult);
                isConnected = true;
                // labelConnectionState.Text = "Connected";
            }
            catch (SocketException socketException)
            {
                isConnected = false;
                // labelConnectionState.Text = "Server unavailable";
            }
        }
    }
}