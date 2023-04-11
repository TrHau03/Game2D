using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendOTPModel 
{
    public SendOTPModel(string username)
    {
        this.username = username;
    }

    public string username { get; set; }
}
