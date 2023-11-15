using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using Assets;
namespace Assets
{
    public class SocketIOConfig : MonoBehaviour
    {
        [SerializeField]
        private AndroidNotificationController androidNotificationController;
        public SocketIOUnity socket;
        public string serverUrlLink = "http://192.168.1.21:9090/";

        public TextMeshProUGUI orderId;
        public TextMeshProUGUI userEmail;
        public TextMeshProUGUI productName;
        public TextMeshProUGUI status;
        public TextMeshProUGUI price;
        public Button cardTemp;
        public Button processingNotif;
        public Button shippedNotif;
        public Button deliveredNotif;
        void Start()
        {
            StartCoroutine(Get(serverUrlLink));
            UnityThread.executeInUpdate(() => {
                socket.Emit("notification", "Your order is Processing");
                processingNotif.onClick.AddListener(sendProccessing);
                shippedNotif.onClick.AddListener(sendShipped);
                deliveredNotif.onClick.AddListener(sendDelivered);
            });

            StartCoroutine(GetDataFromAPI("http://192.168.1.52:9090/order/getOrders"));


        }
        public IEnumerator Get(string url)
        {
            var uri = new Uri(url);
            socket = new SocketIOUnity(uri);
            socket.Connect();

            socket.On("connection", (response) =>
            {
                Debug.Log("Before connection");
                Debug.Log("socket connected");
            });

            socket.On("notification", response => {
                UnityThread.executeInUpdate(() => {
                    //vibreur
                    SceneManager.LoadScene("delivery");
                    SendNotificationOnClick(response.GetValue<string>());
                });
            });
            /* socket.On("Notification", response =>
             {
                 Debug.Log("Event" + response.ToString());
                 UnityThread.executeInUpdate(() => {
                 SendNotificationOnClick(response.ToString());

         });
             });*/

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(www.error);
                }
                else if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Connected to API");
                    // Handle the data received from the API
                    string data = www.downloadHandler.text;
                }
                else
                {
                    Debug.Log("Error! Data couldn't be retrieved.");
                }
            }

        }

        private void sendProccessing()
        {
            socket.Emit("notification", "Your order is Processing");
            StartCoroutine(SendUpdateRequest("http://192.168.1.52:9090/order/", orderId.text, "Processing"));
        }

        private void sendShipped()
        {
            socket.Emit("notification", "Your order is Shipped");
            StartCoroutine(SendUpdateRequest("http://192.168.1.52:9090/order/", orderId.text, "Shipped"));
        }

        private void sendDelivered()
        {
            socket.Emit("notification", "Your order is Delivered");
            StartCoroutine(SendUpdateRequest("http://192.168.1.52:9090/order/", orderId.text, "Delivered"));
        }
        private void SendNotificationOnClick(string content)
        {
            string title = "Jiaan notification";
            int fireTimeInSeconds = 3;

            androidNotificationController.SendNotification(title, content, fireTimeInSeconds);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public IEnumerator GetDataFromAPI(string url)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError("Network error: " + www.error);
                }
                else if (www.result == UnityWebRequest.Result.Success)
                {
                    string jsonData = www.downloadHandler.text;
                    Debug.Log(jsonData);
                    ParseJsonData(www.downloadHandler.text);
                }
                else
                {
                    Debug.Log("Error! Order couldn't be retrieved !!!!!!.");
                }
            }
        }

        IEnumerator SendUpdateRequest(string url, string orderId, string newStatus)
        {
            // Create JSON payload
            string jsonPayload = "{\"status\"" + newStatus + "\"}";

            // Create UnityWebRequest with PUT method
            UnityWebRequest request = UnityWebRequest.Put(url + orderId, jsonPayload);
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Order status updated successfully");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Debug.LogError("Failed to update order status. Error: " + request.error);
            }
        }

        void ParseJsonData(string jsonData)
        {
            OrdersData ordersData = JsonConvert.DeserializeObject<OrdersData>(jsonData);


            if (ordersData != null && ordersData.orders.Count > 0)
            {
                Order firstOrder = ordersData.orders[0];
                orderId.text = firstOrder._id;
                status.text = firstOrder.status;
                userEmail.text = firstOrder.user.email;

                if (firstOrder.products != null && firstOrder.products.Count > 0)
                {
                    Product2 product = firstOrder.products[1].product;
                    productName.text = product.name;
                    price.text = product.price.ToString();

                    Debug.Log("Order ID: " + firstOrder._id);
                    Debug.Log("Product Name: " + product.name);
                    Debug.Log("Status: " + firstOrder.status);
                    Debug.Log("Price: " + product.price);
                    Debug.Log("User Email: " + firstOrder.user.email);
                }
            }
        }
    }

}