 using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;

public class LoginScript : MonoBehaviour
{
    public TMP_InputField edtUser, edtPassword;
    public TMP_InputField edtNewUser, edtNewPassword,PassAgain;
    public Selectable first;
    private EventSystem eventSystem;
    public TMP_Text txtError;
    public Button btnLogin;
    public GameObject menuRegister, menuLogin;
    public static ResponeUserModel responeUserModel;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        first.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return)){
            btnLogin.onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            Selectable next = eventSystem
            .currentSelectedGameObject
            .GetComponent<Selectable>().FindSelectableOnDown();
            if(next != null) next.Select(); 
        }
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            Selectable next = eventSystem
            .currentSelectedGameObject
            .GetComponent<Selectable>().FindSelectableOnUp();
            if(next != null) next.Select(); 
        }
    }
    public void checkLogin(){
        var user = edtUser.text;
        var pass = edtPassword.text;
        UserModel userModel = new UserModel(user, pass);
        StartCoroutine(Login(userModel));
        Login(userModel);
       /* if(user.Equals("hau") && pass.Equals("123")){
            SceneManager.LoadScene(1);
        }else
        {
            txtError.text = "Please enter your Username and Password";
        }*/
    }
    public void checkRegister()
    {
        var user = edtNewUser.text;
        var pass = edtNewPassword.text;
        var passagain = PassAgain.text;
        UserModel userModel = new UserModel(user, pass);
        if (pass.Equals(passagain))
        {
            StartCoroutine(Register(userModel));
            Register(userModel);
        }
        else
        {
            txtError.text = "Xác nhận pass sai r cu";
        }
    }
    IEnumerator Login(UserModel userModel)
    {
//…
        string jsonStringRequest = JsonConvert.SerializeObject(userModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/login", "POST");
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
            responeUserModel = JsonConvert.DeserializeObject<ResponeUserModel>(jsonString);
            if(responeUserModel.status == 1)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                txtError.text = responeUserModel.notification;
            }

        }
        request.Dispose();
    }

    IEnumerator Register(UserModel userModel)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(userModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/register", "POST");
        Debug.Log("Check request");
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
            responeUserModel = JsonConvert.DeserializeObject<ResponeUserModel>(jsonString);
            if (responeUserModel.status == 1)
            {
                setActive();
                txtError.text = responeUserModel.notification;
            }
            else
            {
                txtError.text = responeUserModel.notification;
            }
        }
        request.Dispose();
    }
    public void setActive()
    {
        menuLogin.SetActive(true);
        menuRegister.SetActive(false);
    }


}
