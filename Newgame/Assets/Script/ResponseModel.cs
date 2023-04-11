using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseModel 
{
    public ResponseModel(string notification, int status)
    {
        this.notification = notification;
        this.status = status;
    }

    public string notification { get; set; }
    public int status { get; set; }
}
