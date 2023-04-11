using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.Networking;
using TreeEditor;

public class PlayerScript : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public Animator animator;
    public bool isRun,isJump;
    public ParticleSystem AniPlayer;
    public GameObject menu;
    private bool isMenu;
    public GameObject btnMenu;
    private int countCoin;
    public TMP_Text textCoin;
    public AudioSource audioCoin;
    public TMP_Text textLife;
    public TMP_Text textFire;
    public TMP_InputField oldpass, newpass;
    public GameObject menuChangepass;
    private bool isRight;
    private Vector2 originalPosion;
    private float countFire;
    // Start is called before the first frame update
    void Start()
    {
        originalPosion = transform.localPosition;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isJump = false;
        isRun = false;
        isMenu= false;
        isRight = true;
        countFire = 0;
        if(LoginScript.responeUserModel.score >=0){ 
            countCoin= LoginScript.responeUserModel.score;
            textCoin.text = countCoin + " x";
        }
        if(LoginScript.responeUserModel.positionX != ""){
            float x = float.Parse(LoginScript.responeUserModel.positionX);
            float y = float.Parse(LoginScript.responeUserModel.positionY);
            float z = float.Parse(LoginScript.responeUserModel.positionZ);



            transform.position= new Vector3(x, y, z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        textFire.text = countFire + "";
        Quaternion rotation =  AniPlayer.transform.localRotation;
        animator.SetBool("IsJump", isJump);
        animator.SetBool("IsRun", isRun);
        Vector2 scale = transform.localScale;
        if (Input.GetKey(KeyCode.RightArrow))
            {
                isRight = true;
                rotation.y = 180;
                AniPlayer.transform.localRotation = rotation;
                AniPlayer.Play();
                isRun =true;
                scale.x *= scale.x  < 0 ? -1 : 1;
                transform.localScale = scale;
                transform.Translate(Vector3.right * 5f * Time.deltaTime);
        }else if (Input.GetKey(KeyCode.LeftArrow))
        {
            isRight = false;    
            rotation.y = 0; 
            AniPlayer.transform.localRotation = rotation;
            AniPlayer.Play();
            isRun =true;
            scale.x *= scale.x  < 0 ? 1 : -1;
            transform.localScale = scale;
            transform.Translate(Vector3.left * 5f * Time.deltaTime);
        } else{
            isRun = false;
         }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AniPlayer.Stop();
            if (isJump == false)
            {
                isJump = true;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 300));
            }

        }
        if (Input.GetKeyDown(KeyCode.P)){
            if(isMenu== false) {
                StartMenu();
            }
            else
            {
                Resume();
            }
         }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if(countFire > 0)
            {
                var x = transform.position.x + (isRight ? 0.9f : -0.9f);
                var y = transform.position.y;
                var z = transform.position.z;
                GameObject game = (GameObject)Instantiate(
                     Resources.Load("Prefabs/bullet"),
                     new Vector3(x, y, z),
                     Quaternion.identity
                     );
                game.GetComponent<BulletScript>().setIsRight(isRight);
                countFire--;
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("dat")){
            isJump = false;
        }else 
        if(other.gameObject.CompareTag("fire")){
            transform.localPosition = originalPosion;
        }
        else
        if (other.gameObject.CompareTag("Snail"))
        {
            Vector2 direction = other.GetContact(0).normal;
                if(Mathf.Round(direction.x) == -1 || Mathf.Round(direction.x) == 1){
                transform.localPosition = originalPosion;
            }else if(Mathf.Round(direction.y) == 1)
            {
                Destroy(other.gameObject);
            }

        }
        
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            Destroy(other.gameObject);
            countCoin += 10;
            textCoin.text = countCoin + " X ";
            audioCoin.Play();
        }else if (other.gameObject.CompareTag("flag"))
        {
            Debug.Log("Save Thanh Cong =>>>>>>>>>>>>>>>"); 
            saveScore();
        }
        else if (other.gameObject.CompareTag("bullet"))
        {
            countFire += 2;
            
            Destroy(other.gameObject);
        }
    }
    public void deleteScore()
    {
        var user = LoginScript.responeUserModel.username;
        SaveScoreModel saveScoreModel = new SaveScoreModel(user, 0);
        StartCoroutine(SaveScore(saveScoreModel));
        SaveScore(saveScoreModel);

        var x =  "";
        var y =  "";
        var z =  "";
        SavePosition savePosition = new SavePosition(user, x, y, z);
        StartCoroutine(SavePosition(savePosition));
        SavePosition(savePosition);
    }
    public void saveScore()
    {
        var user = LoginScript.responeUserModel.username;
        Debug.Log(countCoin);
        SaveScoreModel saveScoreModel = new SaveScoreModel(user, countCoin);
        StartCoroutine(SaveScore(saveScoreModel));
        SaveScore(saveScoreModel);

        var x = transform.position.x +"";
        var y = transform.position.y + "";
        var z = transform.position.z + "";

        SavePosition savePosition = new SavePosition(user, x, y, z);
        StartCoroutine(SavePosition(savePosition));
        SavePosition(savePosition);
    }
    public void changePass()
    {
        var user = LoginScript.responeUserModel.username;
        var oldpassword = oldpass.text;
        var newpassword = newpass.text;
        ChangePassWordModel changePassWordModel = new ChangePassWordModel(user, oldpassword, newpassword);
        StartCoroutine(ChangePassWord(changePassWordModel));
        ChangePassWord(changePassWordModel);
    }
    public void StartMenu()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
        isMenu = true;
        btnMenu.SetActive(false);
    }
    public void Resume(){
        isMenu = false;
        menu.SetActive(false);
        if(menuChangepass != null)
        {
            menuChangepass.SetActive(false);
        }
        Time.timeScale = 1;
        btnMenu.SetActive(true);
    }
    public void NextLevel(){
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
    public void BackLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    IEnumerator SaveScore(SaveScoreModel saveScoreModel)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(saveScoreModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/save-score", "POST");
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
                Resume();
            }
            else
            {
            }
        }
        request.Dispose();
    }
        IEnumerator SavePosition(SavePosition savePosition)
        {
            //…
            string jsonStringRequest = JsonConvert.SerializeObject(savePosition);

            var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/save-position", "POST");
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
                    
                }
                else
                {
                Debug.Log(response.notification);
                }
            }
            request.Dispose();
        }
    IEnumerator ChangePassWord(ChangePassWordModel changePassWordModel)
    {
        //…
        string jsonStringRequest = JsonConvert.SerializeObject(changePassWordModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/change-password", "POST");
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
                Resume();

            }
            else
            {
                Debug.Log(response.notification);
            }
        }
        request.Dispose();
    }
}

