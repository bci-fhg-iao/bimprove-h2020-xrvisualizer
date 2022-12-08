using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using SimpleJSON;
using System.IO;

public class HTTPServerScript : MonoBehaviour
{
    modelManager m_model_manager = null;
    string m_scene_ID = "";

    public int m_X_pos = 0;
    public int m_Y_pos = 0;

    //Server root path
    public string serverRootPath;

    public delegate void szenegraphDelegateAction(SzenegraphCommand sgc);

    public class SzenegraphCommand
    {
        public szenegraphDelegateAction mCommand;
        public string mDataIn;
        public string mDataOut;
        public EventWaitHandle mWaitHandle;

        public SzenegraphCommand()
        {
            mCommand = null;
            mDataIn = null;
            mDataOut = null;
            mWaitHandle = null;
        }

        public void waitForAnswer()
        {
            if (mWaitHandle == null)
            {
                mWaitHandle = new AutoResetEvent(false);
                mWaitHandle.WaitOne();
            }
        }

        public void done()
        {
            if (mWaitHandle != null)
            {
                mWaitHandle.Set();
                mWaitHandle = null;
            }
        }

    }
    private Queue<SzenegraphCommand> sgCommandQueue;

    Thread m_serverThread;

    void awake()
    {

    }

    // Use this for initialization
    void Start()
    {


        if (string.IsNullOrEmpty(serverRootPath))
        {
            serverRootPath = Application.dataPath + "/../webclient";
            Debug.Log("HTTPServerScript: Rootpath="+serverRootPath);
        }

#if UNITY_WEBPLAYER
      Debug.Log("Webserver is not initialized due to security policies!");
#else
        Debug.Log("HTTPServerScript Started");
        Debug.Log("HTTPServerScript Thread: " + Thread.CurrentThread.ManagedThreadId);

        sgCommandQueue = new Queue<SzenegraphCommand>();

        CCVRHTTPServer.WebServer httpServer = new CCVRHTTPServer.WebServer(serverRootPath);
        //httpServer.debug = true;

        httpServer.setLogCallback(logger);

        //httpServer.addGetCallback("showScene/{scene-ID}", wsdm_showSceneGetCallback);
        httpServer.addGetCallback("showScene/{scene-ID}", wsdm_Callback_ShowScene);

        ////httpServer.addGetCallback("RoutisVRAPI/{version}/status", wsdmStatusGetCallback);
        httpServer.addGetCallback("status", wsdmStatusGetCallback);

        //httpServer.addGetCallback("RoutisVRAPI/{version}/test", wsdmTestGetCallback);
        //httpServer.addPostCallback("RoutisVRAPI/{version}/test", wsdmTestPostCallback);

        //org:
        //Thread thread = new Thread(new ThreadStart(httpServer.start_server));
        //thread.Start();
        m_serverThread = new Thread(new ThreadStart(httpServer.start_server));
        m_serverThread.Start();
        Debug.Log("HTTPServerScript Thread Started");
#endif

    }
    void OnApplicationQuit()
    {
        m_serverThread.Abort();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_model_manager == null)
        {
            GameObject scriptHolder = GameObject.Find("_Scripts");

            if (scriptHolder != null)
            {
                m_model_manager = scriptHolder.GetComponent<modelManager>();
            }

            if (m_model_manager != null)
            {
                Debug.Log("Model_Manager: found");
            }
        }

#if !UNITY_WEBPLAYER

        while (sgCommandQueue.Count > 0)
        {
            lock (sgCommandQueue)
            {
                SzenegraphCommand sgc = sgCommandQueue.Dequeue();
                Debug.Log("HTTPServerScript Unity Dequeue command");
                sgc.mCommand(sgc);
                Debug.Log("HTTPServerScript Unity Dequeue command ... done");
                sgc.done();
            }
        }
#endif
    }

#if !UNITY_WEBPLAYER

    //public bool wsdm_showSceneGetCallback(ref HttpListenerRequest handler_request, ref byte[] responseBodyBytes, ref int statusCode, ref string contentType, Dictionary<string, string> parsedParameters)
    //{
    //    Debug.Log("UnityWebServer: Show-scene callback activated - threaded!");

    //    m_scene_ID = "";

    //    if (parsedParameters.ContainsKey("SCENE-ID"))
    //    {
    //        m_scene_ID = parsedParameters["SCENE-ID"];
    //    }

    //    Debug.Log("UnityWebServer: showSceneGetCallback! SCENE-ID is: " + m_scene_ID);

    //    if (m_model_manager != null)
    //    {
    //        m_model_manager.showScene(m_scene_ID);
    //    }

    //    SzenegraphCommand sgc = new SzenegraphCommand();

    //    lock (sgCommandQueue)
    //    {
    //        sgc.mCommand = szdmShowAR;
    //        sgCommandQueue.Enqueue(sgc);
    //    }

    //    string textbody = "Request of AR-Scene " + m_scene_ID;
    //    responseBodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
    //    statusCode = 200;
    //    contentType = "text/plain; charset=utf-8";

    //    return true;
    //}

    public void logger(string msg)
    {
        //if (InvokeRequired)
        //{
        //  this.BeginInvoke(new Action<string>(logger), new Object[] { msg });
        //  return;
        //}
        //tbLogMessages.AppendText(msg);
        Debug.Log("WS: " + msg);
    }



    //Web Server Delegate Methods
    public bool wsdmStatusGetCallback(ref HttpListenerRequest handler_request, ref byte[] responseBodyBytes, ref int statusCode, ref string contentType, Dictionary<string, string> parsedParameters)
    {
        Debug.Log("UnityWebServer Status callback activated!");

        double version = 0.0;

        if (parsedParameters.ContainsKey("VERSION"))
        {
            version = double.Parse(parsedParameters["VERSION"]);
        }


        //todo JSON support!!!


        string textbody = "OK this is version " + version;
        responseBodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
        statusCode = 200;
        contentType = "text/plain; charset=utf-8";

        return true;
    }

    public bool wsdm_Callback_ShowScene(ref HttpListenerRequest handler_request, ref byte[] responseBodyBytes, ref int statusCode, ref string contentType, Dictionary<string, string> parsedParameters)
    {
        Debug.Log("UnityWebServer testGet callback activated!");

        Debug.Log("UnityWebServer: wsdm_Callback_ShowScene callback activated - send to main thread!");

        lock (m_scene_ID)
        {
            m_scene_ID = "";

            if (parsedParameters.ContainsKey("SCENE-ID"))
            {
                m_scene_ID = parsedParameters["SCENE-ID"];
            }
            Debug.Log("UnityWebServer: wsdm_Callback_ShowScene SCENE-ID is: " + m_scene_ID);
        }
       

        SzenegraphCommand sgc = new SzenegraphCommand();

        lock (sgCommandQueue)
        {
            sgc.mCommand = szdm_showScene;
            sgCommandQueue.Enqueue(sgc);
        }
        //todo JSON support!!!

        Debug.Log("UnityWebServer waiting for answer!");
        sgc.waitForAnswer();
        Debug.Log("UnityWebServer answer: " + sgc.mDataOut);


        ////string textbody = "OK this is version " + version;
        //responseBodyBytes = System.Text.Encoding.UTF8.GetBytes(sgc.mDataOut);
        //statusCode = 200;
        //contentType = "text/plain; charset=utf-8";


        string textbody = "{}";
        responseBodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
        statusCode = 200;
        contentType = "application/json";

        return true;
    }



    public bool wsdmTestGetCallback(ref HttpListenerRequest handler_request, ref byte[] responseBodyBytes, ref int statusCode, ref string contentType, Dictionary<string, string> parsedParameters)
    {
        Debug.Log("UnityWebServer testGet callback activated!");

        //double version = 0.0;

        //if (parsedParameters.ContainsKey("VERSION"))
        //{
        //  version = double.Parse(parsedParameters["VERSION"]);
        //}

        SzenegraphCommand sgc = new SzenegraphCommand();

        lock (sgCommandQueue)
        {
            sgc.mCommand = szdmTestGet;
            sgCommandQueue.Enqueue(sgc);
        }
        //todo JSON support!!!

        Debug.Log("UnityWebServer waiting for answer!");
        sgc.waitForAnswer();
        Debug.Log("UnityWebServer answer: " + sgc.mDataOut);


        //string textbody = "OK this is version " + version;
        responseBodyBytes = System.Text.Encoding.UTF8.GetBytes(sgc.mDataOut);
        statusCode = 200;
        contentType = "text/plain; charset=utf-8";

        return true;
    }

    public bool wsdmTestPostCallback(ref HttpListenerRequest handler_request, ref byte[] responseBodyBytes, ref int statusCode, ref string contentType, Dictionary<string, string> parsedParameters)
    {
        Debug.Log("UnityWebServer testPost callback activated!");

        int version = 0;

        if (parsedParameters.ContainsKey("VERSION"))
        {
            version = int.Parse(parsedParameters["VERSION"]);
        }

        SzenegraphCommand sgc = new SzenegraphCommand();
        sgc.mCommand = szdmTestPost;

        Debug.Log("UnityWebServer testPost callback prelock");
        lock (sgCommandQueue)
        {
            sgCommandQueue.Enqueue(sgc);
        }
        Debug.Log("UnityWebServer testPost callback postlock");
        //todo JSON support!!!

        string incomingData = GetRequestPostData(handler_request);
        JSONNode jsonObj = new JSONClass();
        JSONClass objectNode = new JSONClass();

        JSONArray posArr = new JSONArray();
        JSONArray oriArr = new JSONArray();

        //JSONData dta = new JSONData(false);

        try
        {
            jsonObj.Add("object", objectNode);
            objectNode.Add("pos", posArr);
            objectNode.Add("ori", oriArr);
            //objectNode.Add("visible", dta);

            posArr[0].AsDouble = 10.0;
            posArr[-1].AsDouble = 100.0;

            jsonObj["object"]["visible"].AsBool = true;


            //jsonObj["object"]["pos"][-1] = new JSONData(0.0);
            //jsonObj["object"]["pos"][-1] = new JSONData(1.0);
            //jsonObj["object"]["pos"][-1] = new JSONData(2.0);

            //jsonObj["object"]["ori"][-1] = new JSONData(3.0);
            //jsonObj["object"]["ori"][-1] = new JSONData(4.0);
            //jsonObj["object"]["ori"][-1] = new JSONData(5.0);

            //jsonObj["object"] = "blaaaa";
            Debug.Log("json: " + jsonObj.ToJSON(0));
        }
        catch (System.Exception ex)
        {
            Debug.Log("exception: " + ex.ToString());
        }
        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter bw = new BinaryWriter(memoryStream);
        jsonObj.Serialize(bw);

        //memoryStream.Position = 0;
        //var sr = new StreamReader(memoryStream);
        //string myString = sr.ReadToEnd();


        string textbody = "OK this is version " + version + " we have data: " + incomingData;// + " --- " + jsonObj.ToJSON(0);
        responseBodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
        statusCode = 200;
        contentType = "text/plain; charset=utf-8";

        Debug.Log("UnityWebServer testPost callback done!");

        return true;
    }

    public static string GetRequestPostData(HttpListenerRequest request)
    {
        if (!request.HasEntityBody)
        {
            return null;
        }
        using (System.IO.Stream body = request.InputStream) // here we have data
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
            {
                return reader.ReadToEnd();
            }
        }
    }

    //void webserverDelegateMethod(HttpListenerRequest handler_request, ref byte[] bodyBytes, ref int statusCode, ref string contentType)
    //{
    //  //is called in webserver accept thread
    //  Debug.Log("webserverDelegateMethod Thread: " + Thread.CurrentThread.ManagedThreadId);

    //  lock (sgCommandQueue)
    //  {
    //    sgCommandQueue.Enqueue(szenegraphDelegateMethod);
    //  }

    //  string textbody = "OK";
    //  bodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
    //  statusCode = 200;
    //  contentType = "text/plain; charset=utf-8";
    //}

    //void getSliderValueDelegate(HttpListenerRequest handler_request, ref byte[] bodyBytes, ref int statusCode, ref string contentType)
    //{
    //  //is called in webserver accept thread
    //  Debug.Log("webserverDelegateMethod Thread: " + Thread.CurrentThread.ManagedThreadId);

    //  //do i need info from sg ?
    //  //lock (sgCommandQueue)
    //  //{
    //  //  sgCommandQueue.Enqueue(szenegraphDelegateMethod);
    //  //}

    //  //return value as simple text in body
    //  string textbody = "0";
    //  bodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
    //  statusCode = 200;
    //  contentType = "text/plain; charset=utf-8";
    //}


    //void wsdmStart(HttpListenerRequest handler_request, ref byte[] bodyBytes, ref int statusCode, ref string contentType)
    //{
    //  //is called in webserver accept thread
    //  Debug.Log("wsdmStart Thread: " + Thread.CurrentThread.ManagedThreadId);

    //  lock (sgCommandQueue)
    //  {
    //    SzenegraphCommand sgc = new SzenegraphCommand();
    //    sgc.mCommand = szdmStart;
    //    sgCommandQueue.Enqueue(sgc);
    //  }

    //  string textbody = "{'status':'OK'}";
    //  bodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
    //  statusCode = 200;
    //  contentType = "application/json";
    //}

    //void wsdmSzenario1(HttpListenerRequest handler_request, ref byte[] bodyBytes, ref int statusCode, ref string contentType)
    //{
    //  //is called in webserver accept thread
    //  Debug.Log("webserverDelegateMethod Thread: " + Thread.CurrentThread.ManagedThreadId);

    //  SzenegraphCommand sgc = new SzenegraphCommand();

    //  lock (sgCommandQueue)
    //  {
    //    sgc.mCommand = szdmSzenario1;
    //    sgCommandQueue.Enqueue(sgc);
    //  }

    //  //

    //  string textbody = "{'status':'OK'}";
    //  bodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
    //  statusCode = 200;
    //  contentType = "application/json";
    //}


    //void wsdmSlider(HttpListenerRequest request, ref byte[] bodyBytes, ref int statusCode, ref string contentType)
    //{
    //  //is called in webserver accept thread
    //  Debug.Log("webserverDelegateMethod Slider with content type " + request.ContentType +  " Thread: " + Thread.CurrentThread.ManagedThreadId);


    //  if (request.HasEntityBody)
    //  {
    //    System.IO.Stream body = request.InputStream;
    //    System.Text.Encoding encoding = request.ContentEncoding;
    //    System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
    //    if (request.ContentType != null)
    //    {
    //      Debug.Log("Client data content type" + request.ContentType);
    //    }
    //    Debug.Log("Client data content length " + request.ContentLength64);

    //    string s = reader.ReadToEnd();
    //    Debug.Log("Body Received: " + s);
    //    body.Close();
    //    reader.Close();
    //  }
    //  foreach ( string key in request.QueryString.Keys )
    //  {
    //    string res;
    //    string[] values = request.QueryString.GetValues(key);
    //    res = key + "=";
    //    foreach (string value in values)
    //    {
    //      res += value + " ";
    //    }
    //    Debug.Log("Query String: " + res);
    //  }
    //  //Debug.Log("Query String: ");


    //  lock (sgCommandQueue)
    //  {
    //    //sgCommandQueue.Enqueue(szdmFreewalk);

    //    string[] values = request.QueryString.GetValues("value");

    //    if ( values.Length > 0 )
    //    {
    //      Debug.Log("Has Slider Value: " + values[0]);

    //      sgCommandQueue.Enqueue(() => szdmSlider( int.Parse(values[0]) ) );
    //    }

    //  }

    //  string textbody = "{}";
    //  bodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
    //  statusCode = 200;
    //  contentType = "application/json";
    //}

    //void wsdmPostTestData(HttpListenerRequest request, ref byte[] bodyBytes, ref int statusCode, ref string contentType)
    //{
    //  //is called in webserver accept thread
    //  Debug.Log("wsdmPostTestData with content type " + request.ContentType + " Thread: " + Thread.CurrentThread.ManagedThreadId);

    //  lock (sgCommandQueue)
    //  {
    //    string[] values = request.QueryString.GetValues("value");

    //    if (values.Length > 0)
    //    {
    //      //Debug.Log("Has Slider Value: " + values[0]);

    //      mMechanicalSliderValue = int.Parse(values[0]);

    //      sgCommandQueue.Enqueue(() => szdmMechanicalSlider(mMechanicalSliderValue));
    //    }
    //  }

    //  string textbody = "{}";
    //  bodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
    //  statusCode = 200;
    //  contentType = "application/json";
    //}

    //void wsdmGetTestData(HttpListenerRequest handler_request, ref byte[] bodyBytes, ref int statusCode, ref string contentType)
    //{
    //  //is called in webserver accept thread
    //  Debug.Log("wsdmGetTestData Thread: " + Thread.CurrentThread.ManagedThreadId + " val: " + mMechanicalSliderValue);


    //  SzenegraphCommand sgc = new SzenegraphCommand();


    //  //do i need info from sg ?
    //  lock (sgCommandQueue)
    //  {
    //    sgCommandQueue.Enqueue(szenegraphDelegateMethod);
    //  }




    //  //return value as simple text in body
    //  string textbody = sgc.mData;
    //  bodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
    //  statusCode = 200;
    //  contentType = "text/plain; charset=utf-8";
    //}

    //void wsdmPostMechanical2Slider(HttpListenerRequest request, ref byte[] bodyBytes, ref int statusCode, ref string contentType)
    //{
    //  //is called in webserver accept thread
    //  Debug.Log("wsdmPostMechanical2Slider Slider with content type " + request.ContentType + " Thread: " + Thread.CurrentThread.ManagedThreadId);

    //  lock (sgCommandQueue)
    //  {
    //    string[] values = request.QueryString.GetValues("value");

    //    if (values.Length > 0)
    //    {
    //      //Debug.Log("Has Slider Value: " + values[0]);

    //      mMechanical2SliderValue = int.Parse(values[0]);

    //      sgCommandQueue.Enqueue(() => szdmMechanical2Slider(mMechanical2SliderValue));
    //    }
    //  }

    //  string textbody = "{}";
    //  bodyBytes = System.Text.Encoding.UTF8.GetBytes(textbody);
    //  statusCode = 200;
    //  contentType = "application/json";
    //}

#endif

    //Szenegraph Delegate Method
    //void szenegraphDelegateMethod()
    //{
    //  //will be called in main thread
    //  Debug.Log("szenegraphDelegateMethod Thread: " + Thread.CurrentThread.ManagedThreadId);

    //  GameObject bla = GameObject.Find("Sphere");

    //  Renderer flupp = bla.GetComponent<Renderer>();

    //  string res;

    //  res = "Sphere was: " + flupp.enabled;

    //  if (!flupp.enabled)
    //  {
    //    flupp.enabled = true;
    //  }
    //  else
    //  {
    //    flupp.enabled = false;
    //  }

    //  res += " and is now " + flupp.enabled;
    //}

    //void szdmStart()
    //{
    //  //will be called in main thread
    //  Debug.Log("szdmStart Thread: " + Thread.CurrentThread.ManagedThreadId);

    //  GameObject bla = GameObject.Find("TestText");
    //  TextMesh tm = bla.GetComponent<TextMesh>();

    //  if (tm)
    //  {
    //    tm.text = "Start Pressed!";
    //  }
    //}

    //void szdmSzenario1()
    //{
    //  //will be called in main thread
    //  Debug.Log("szdmSzenario1 Thread: " + Thread.CurrentThread.ManagedThreadId);

    //  GameObject bla = GameObject.Find("TestText");
    //  TextMesh tm = bla.GetComponent<TextMesh>();

    //  if (tm)
    //  {
    //    tm.text = "Szenario1 Pressed!";
    //  }
    //}

    void szdmTestPost(SzenegraphCommand sgc)
    {
        //will be called in main thread
        Debug.Log("szdmTestPost Thread: " + Thread.CurrentThread.ManagedThreadId);

        GameObject bla = GameObject.Find("TestText");
        TextMesh tm = bla.GetComponent<TextMesh>();

        if (tm)
        {
            tm.text = "TestPost";
        }
    }

    void szdm_showScene(SzenegraphCommand sgc)
    {
        //will be called in main thread
        Debug.Log("szdmTestPost Thread: " + Thread.CurrentThread.ManagedThreadId);
        lock (m_scene_ID)
        {
            m_model_manager.showScene(m_scene_ID);
        }
        helper_Win_API.SetCursorPos(Screen.width/2, Screen.height- Screen.height/3);
    }

    void szdmTestGet(SzenegraphCommand sgc)
    {
        //will be called in main thread
        Debug.Log("szdmTestGet Thread: " + Thread.CurrentThread.ManagedThreadId);

        GameObject bla = GameObject.Find("TestText");
        TextMesh tm = bla.GetComponent<TextMesh>();

        if (tm)
        {
            tm.text = "TestGet";
        }


        sgc.mDataOut = @"{ 'test' : 'json' }";
    }

    //void szdmShowAR(SzenegraphCommand sgc)
    //{
    //    //GameObject appsist_obj = GameObject.Find("appsist_manager");
    //    //m_model_manager = appsist_obj.GetComponent<appsist_GUI>();

    //    if (m_model_manager != null)
    //    {
    //        //m_model_manager.showARscene(appsist_AR_ID);
    //        Debug.Log("[HTTPserver]szdmShowAR: showARscene: OBJ " + m_scene_ID);
    //    }
    //    else
    //    {
    //        Debug.Log("ERROR: [HTTPserver]szdmShowAR:  OBJ no reference to gui manager");
    //    }
    //}

}
