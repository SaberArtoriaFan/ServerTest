using UnityEngine;
using System.Collections.Generic;

public class DebugOutputToGUI : MonoBehaviour
{
    private Queue<string> logQueue = new Queue<string>();
    private string logContent = "";
    bool isOpen = true;
    void OnEnable()

    {
#if !UNITY_Editor
        isOpen = false;
#endif
            Application.logMessageReceived += HandleLog;
        Debug.Log("开始使用");
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    // 当有新的日志时，将其添加到队列中
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logQueue.Enqueue(logString);

        // 限制日志条数
        if (logQueue.Count > 20)
        {
            logQueue.Dequeue();
        }

        // 重新构建显示内容
        logContent = "";
        foreach (string log in logQueue)
        {
            logContent += log + "\n";
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = !isOpen;
        }
    }
    // 使用 Unity 的 OnGUI 方法显示日志内容
    void OnGUI()
    {
        if(isOpen) 
            GUI.Label(new Rect(10, 10, 500, 500), logContent);
    }
}

