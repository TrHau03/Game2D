using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;

public class ForGotPass : MonoBehaviour
{
    public TMP_InputField txtUser,txtOTP,txtNewPass,txtRenewPass;
    public GameObject resetPass,sendOTP,login;
    // Update is called once per frame
    public void checkSendOTP()
    {
        var user = txtUser.text;
        SendOTPModel sendOTPModel = new SendOTPModel(user);
        StartCoroutine(SendOTP(sendOTPModel));
        SendOTP(sendOTPModel);
    }
    public void resetPasword ()
    {
        var newPass = txtNewPass.text;
        var reNew = txtRenewPass.text;
        if (newPass.Equals(reNew)) {
            var user = txtUser.text;
            var txtotp = int.Parse(txtOTP.text);
            ResetPassModel resetPassModel = new ResetPassModel(user, txtotp, newPass);
            StartCoroutine(ResetPassAPI(resetPassModel));
            ResetPassAPI(resetPassModel);
        }
    }
    IEnumerator SendOTP(SendOTPModel sendOTPModel)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(sendOTPModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/send-otp", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ResponseModel response = JsonConvert.DeserializeObject<ResponseModel>(jsonString);
            Debug.Log(response.notification);
            Debug.Log(response.status);
            if (response.status == 1)
            {
                sendOTP.SetActive(false);
                resetPass.SetActive(true);
                // thanh cong
            }
            else
            {
                // that bai
            }
        }
        request.Dispose();
    }
    IEnumerator ResetPassAPI(ResetPassModel resetPassModel)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(resetPassModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/reset-password", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ResponseModel response = JsonConvert.DeserializeObject<ResponseModel>(jsonString);
            Debug.Log(response.notification);
            Debug.Log(response.status);
            if (response.status == 1)
            {
                resetPass.SetActive(false);
                login.SetActive(true);
                // thanh cong
            }
            else
            {
                // that bai
            }
        }
        request.Dispose();
    }

}
