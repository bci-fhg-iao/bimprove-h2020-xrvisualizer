using UnityEngine;
using System;
using System.Text;
using System.Diagnostics;
using System.Collections;

public class windowManager : MonoBehaviour
{
    IntPtr m_handle_browser = IntPtr.Zero;
    IntPtr m_handle_unity_app = IntPtr.Zero;
    // bool m_handles_init = false;
    bool m_searchHandles = true;

    public string m_unity_app_title = "WuM-Campus"; //Productname in Player-Settings
    public string m_browser_title = "Scene-Client"; //Title in Browser-App

    void Awake()
    {

    }



    IEnumerator startChrome_Coroutine()
    {
        print("startChrome_Coroutine: " + Time.time);
        yield return new WaitForSeconds(5);
        string url = Application.streamingAssetsPath + "\\_WebClient\\" + "test_cmd_01.html";
        string app_url = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";
        startChrome(app_url, url);
        print("startChrome_Coroutine: " + Time.time);
    }

    void OnApplicationQuit()
    {
       // startExtApp("C:\\Windows\\System32\\taskkill.exe", "/f /im chrome.exe");
    }

    // Use this for initialization
    void Start()
    {
        //kill chrome
         //startExtApp("C:\\Windows\\System32\\taskkill.exe", "/f /im chrome.exe");

        //StartCoroutine(startChrome_Coroutine());
        //StartCoroutine(maximizeWindow());

    }


    // Update is called once per frame
    void Update()
    {
        //search handle of browser
        if (m_searchHandles && m_handle_browser == IntPtr.Zero)
        {
            helper_Win_API.EnumWindows(enumWindowTitle, IntPtr.Zero);
        }

        //search handle of unity-app
        if (m_searchHandles && m_handle_unity_app == IntPtr.Zero)
        {
            helper_Win_API.EnumWindows(enumWindowTitle, IntPtr.Zero);
        }

    }


    public void startChrome(string app_url, string url)
    {
        startExtApp(app_url, "--kiosk --incognito " + url);
    }

    public void showBrowser()
    {
        if (m_handle_browser != IntPtr.Zero)
        {
            helper_Win_API.ShowWindow(m_handle_browser, SW.SW_MAXIMIZE);
            helper_Win_API.SetActiveWindow(m_handle_browser);
            helper_Win_API.SetForegroundWindow(m_handle_browser);
        }
        else
        {
            UnityEngine.Debug.Log("ERROR [windowManager]showBrowser: No handle for browser");
        }
    }

    public void showApp()
    {
        if (m_handle_unity_app != IntPtr.Zero)
        {
            helper_Win_API.ShowWindow(m_handle_unity_app, SW.SW_MAXIMIZE);
            helper_Win_API.SetActiveWindow(m_handle_unity_app);
            helper_Win_API.SetForegroundWindow(m_handle_unity_app);
            UnityEngine.Debug.Log("[windowManager]showApp:maximize OK");
        }
        else
        {
            UnityEngine.Debug.Log("ERROR [windowManager]showApp: No handle for unity app");
        }
    }

    public bool enumWindowTitle(IntPtr hWnd, IntPtr lParam)
    {
        // UnityEngine.Debug.Log("Fenster: <" + GetWindowText(hWnd) + ">");
        string find_title = GetWindowText(hWnd);

        if (m_handle_browser == IntPtr.Zero && find_title.Contains(m_browser_title))
        {
            m_handle_browser = hWnd;
            UnityEngine.Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!Browser-Client found");

            //helper_Win_API.SetWindowText(browserApp, "Appsist ist TOOOLLLL");
        }

        // if (find_title.Contains("Appsist_App"))
        if (m_handle_unity_app == IntPtr.Zero && find_title.Contains(m_unity_app_title))
        {
            m_handle_unity_app = hWnd;
            UnityEngine.Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!Unity-App found");
            //helper_Win_API.SetWindowText(browserApp, "Appsist ist TOOOLLLL");
        }

        if (m_handle_unity_app != IntPtr.Zero && m_handle_browser != IntPtr.Zero)
        {
            m_searchHandles = false;
        }
        return true;
    }


    public static string GetWindowText(IntPtr hWnd)
    {
        int size = helper_Win_API.GetWindowTextLength(hWnd);
        if (size > 0)
        {
            var builder = new StringBuilder(size + 1);
            helper_Win_API.GetWindowText(hWnd, builder, builder.Capacity);
            return builder.ToString();
        }

        return String.Empty;
    }


    public bool enumParentWindow(IntPtr hWnd, IntPtr lParam)
    {
        bool enumeratingWindowsSucceeded = helper_Win_API.EnumChildWindows(hWnd, enumWindowTitle, IntPtr.Zero);

        return true;
    }


    void startExtApp(string app_url, string file_url)
    {
        if (!System.IO.File.Exists(app_url))
        {
            UnityEngine.Debug.Log("[windowManager.startExtApp()] ERROR: Application path of external program is incorrect: " + app_url);
        }
        else
        {
            System.Diagnostics.Process app = new System.Diagnostics.Process();
            app.StartInfo.FileName = app_url;
            app.StartInfo.Arguments = file_url;
            app.Start();
            app.WaitForInputIdle();
            UnityEngine.Debug.Log("windowManager.startExtApp: OK");
        }
    }


    void printProcesses()
    {
        Process[] running = Process.GetProcesses();
        foreach (Process process in running)
        {
            try
            {
                if (!process.HasExited)
                {
                    UnityEngine.Debug.Log("Process-ID: " + process.Id + " " + process.ProcessName);
                    UnityEngine.Debug.Log("MainWindowHandle: " + process.MainWindowHandle);
                    UnityEngine.Debug.Log("Handle: " + process.Handle + "\n\n");
                }
            }
            catch (System.InvalidOperationException)
            {
                //do nothing
                //UnityEngine.Debug.Log("***** InvalidOperationException was caught!");
            }

        }
    }

    private IEnumerator maximizeWindow()
    {
        Screen.fullScreen = false; // so Screen.currentResolution.height is height of desktop-resolution -> see unity-doc
        yield return null;
        yield return null;

        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true); //unity-application set to resolution of desktop and in full-screen mode    
        yield return null;
    }
}
