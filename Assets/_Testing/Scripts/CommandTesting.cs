using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COMMANDS;

public class CommandTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Running());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CommandManager.Instance.Execute("moveCharDemo", "left");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CommandManager.Instance.Execute("moveCharDemo", "right");
        }
    }

    IEnumerator Running()
    {
        yield return CommandManager.Instance.Execute("print"); //이전명령이 다되고 실행될 수 있게끔 yi re 붙힘
        yield return CommandManager.Instance.Execute("print_1p", "Hello world");
        yield return CommandManager.Instance.Execute("print_mp", "line1", "line2", "line3");

        yield return CommandManager.Instance.Execute("lambda");
        yield return CommandManager.Instance.Execute("lambda_1p", "Hello lambda");
        yield return CommandManager.Instance.Execute("lambda_mp", "lambda1", "lambda2", "lambda3");

        yield return CommandManager.Instance.Execute("process");
        yield return CommandManager.Instance.Execute("process_1p", "3");
        yield return CommandManager.Instance.Execute("process_mp", "process line1", "process line2", "process line3");
    }
}
