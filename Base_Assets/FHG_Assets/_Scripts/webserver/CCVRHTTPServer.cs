using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using MonoWorkaroundSystem;

namespace CCVRHTTPServer
{
    //[EnableCors("*", "*", "PUT, POST")]
    public class WebServer
    {
        public delegate bool webserverDelegateAction(ref HttpListenerRequest request, ref byte[] responseBodyBytes, ref int statusCode, ref string contentType, Dictionary<string, string> parsedParameters);
        public delegate void wsLogDelegateAction(string message);

        // öffentliche Felder
        private string mHttpHost = "localhost";
        public string pHost
        {
            get { return mHttpHost; }
            set
            {
                //stop if running
                mHttpHost = value;
            }
        }

        private string mHttpPort = "8082";
        public string pPort
        {
            get { return mHttpPort; }
            set
            {
                //stop if running
                mHttpPort = value;
            }
        }

        public Boolean mDebugFlag = false;
        // private Felder
        private HttpListener mListener = null;
        private String mRootPath = @"C:\appsist_data";

        //private Dictionary<string, webserverDelegateAction> callbackdict = new Dictionary<string, webserverDelegateAction>();
        private Dictionary<UriTemplate, webserverDelegateAction> mGetCallbackdict = new Dictionary<UriTemplate, webserverDelegateAction>();
        private Dictionary<UriTemplate, webserverDelegateAction> mPutCallbackdict = new Dictionary<UriTemplate, webserverDelegateAction>();
        private Dictionary<UriTemplate, webserverDelegateAction> mPostCallbackdict = new Dictionary<UriTemplate, webserverDelegateAction>();
        private Dictionary<UriTemplate, webserverDelegateAction> mDeleteCallbackdict = new Dictionary<UriTemplate, webserverDelegateAction>();

        private wsLogDelegateAction mLog = null;

        // öffentliche Methoden
        public WebServer(string rootPath)
        {
            mRootPath = rootPath;
            Console.WriteLine("root_path: " + mRootPath);
            Console.WriteLine("Server Constructor Thread: " + Thread.CurrentThread.ManagedThreadId);
            mListener = new HttpListener();
        }

        public void setPrefix()
        {

            //if (listener.Prefixes.Count != 0)
            //{
            //  listener.Prefixes.Clear();
            //}

            //string temp_prefix = "http://" + mHttpHost + ":" + mHttpPort + "/";
            //listener.Prefixes.Add(temp_prefix);

            string temp_prefix = "http://+:" + mHttpPort + "/";
            mListener.Prefixes.Add(temp_prefix);
        }

        public void setLogCallback(wsLogDelegateAction callback)
        {
            mLog = callback;
        }

        public void logMessage(string msg)
        {
            if (mLog != null)
            {
                mLog(msg + "\n");
            }
        }

        public void addGetCallback(string uripath, webserverDelegateAction callback)
        {
            UriTemplate ut = new UriTemplate(uripath);
            mGetCallbackdict.Add(ut, callback);
        }

        public void addPutCallback(string uripath, webserverDelegateAction callback)
        {
            UriTemplate ut = new UriTemplate(uripath);
            mPutCallbackdict.Add(ut, callback);
        }

        public void addPostCallback(string uripath, webserverDelegateAction callback)
        {
            UriTemplate ut = new UriTemplate(uripath);
            mPostCallbackdict.Add(ut, callback);
        }

        public void addDeleteCallback(string uripath, webserverDelegateAction callback)
        {
            UriTemplate ut = new UriTemplate(uripath);
            mDeleteCallbackdict.Add(ut, callback);
        }


        public void start_server()
        {
            if (!mListener.IsListening)
            {
                logMessage("Starting server");
                logMessage("Serving: " + mRootPath);
                Console.WriteLine("Starting server");
                Console.WriteLine("Server Start Thread: " + Thread.CurrentThread.ManagedThreadId);

                setPrefix();

                try
                {
                    mListener.Start();
                }
                catch (HttpListenerException ex)
                {
                    //UInt32 x = 0x80004005;
                    logMessage("Listener exception: " + ex.ToString());
                    //logMessage( ex.HResult.ToString("X") );
                    //logMessage( x.ToString("X") );

                    //if (ex.HResult == unchecked((int)0x80004005))
                    //{
                    //  logMessage("Port locked ... unlock http port!");
                    //}

                    return;
                }


                while (mListener.IsListening)
                {
                    wait_request();
                }
            }
        }

        public void stop_server()
        {
            if (mListener.IsListening)
            {
                logMessage("Stopping server");
                mListener.Stop();
            }
        }

        // private Methoden
        private void wait_request()
        {
            Console.WriteLine("wait request");
            IAsyncResult result = mListener.BeginGetContext(callback, mListener);
            result.AsyncWaitHandle.WaitOne();
        }

        private void callback(IAsyncResult result)
        {
            if (mListener.IsListening)
            {
                HttpListenerContext context = mListener.EndGetContext(result);
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                //Debug
                ShowRequestProperties2(request);
                //My Own Server!
                response.Headers.Add(HttpResponseHeader.Server, "CCVR Unity .NET Server v1.1");
                request_handler(response, request);
            }
            else
            {
                return;
            }
        }

        public static void ShowRequestProperties2(HttpListenerRequest request)
        {
            Console.WriteLine("Server.ShowRequestProperties2 -->");
            Console.WriteLine("KeepAlive:" + request.KeepAlive);
            Console.WriteLine("Local end point: " + request.LocalEndPoint.ToString());
            Console.WriteLine("Remote end point: " + request.RemoteEndPoint.ToString());
            Console.WriteLine("Is local? " + request.IsLocal);
            Console.WriteLine("HTTP method: " + request.HttpMethod);
            Console.WriteLine("Protocol version: " + request.ProtocolVersion);
            Console.WriteLine("Is authenticated: " + request.IsAuthenticated);
            Console.WriteLine("Is secure: " + request.IsSecureConnection);
            Console.WriteLine("Encoding: " + request.ContentEncoding);

            System.Collections.Specialized.NameValueCollection qstring = request.QueryString;
            // Get each header and display each value.
            foreach (string key in qstring.AllKeys)
            {
                string[] values = qstring.GetValues(key);
                string vals = "";
                if (values.Length > 0)
                {
                    foreach (string value in values)
                    {
                        vals += value + ";";
                    }
                }
                else
                {
                    vals = "no value";
                }
                Console.WriteLine("The values of the (" + key + ") query are: " + vals);
            }

            System.Collections.Specialized.NameValueCollection headers = request.Headers;
            // Get each header and display each value.
            foreach (string key in headers.AllKeys)
            {
                string[] values = headers.GetValues(key);
                string vals = "";
                if (values.Length > 0)
                {
                    foreach (string value in values)
                    {
                        vals += value + ";";
                    }
                }
                else
                {
                    vals = "no value";
                }
                Console.WriteLine("The values of the (" + key + ") header are: " + vals);
            }

            string[] types = request.AcceptTypes;
            if (types != null)
            {
                string mtyps = "";
                foreach (string s in types)
                {
                    mtyps += s + ";";
                }
                Console.WriteLine("Acceptable MIME types: " + mtyps);
            }

            Console.WriteLine("<-- Server.ShowRequestProperties2");
        }

        public byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }

        private void request_handler(HttpListenerResponse handler_response, HttpListenerRequest handler_request)
        {
            Console.WriteLine("request_handler");
            string document = handler_request.Url.AbsolutePath;
            byte[] responsebyte = null;
            //string queryParams = "";

            Uri requestURI = handler_request.Url;

            Console.WriteLine("Document is: " + document);
            Console.WriteLine("Url is: " + requestURI);

            //      Uri prefix = new Uri("http://" + mHttpHost + ":"+ mHttpPort + "/");
            Uri prefix = new Uri("http://" + handler_request.LocalEndPoint.ToString() + "/");


            webserverDelegateAction cb = null;
            bool callbackExecuted = false;
            string contentType = "";
            int statusCode = 0;
            UriTemplateMatch results;

            Dictionary<UriTemplate, webserverDelegateAction> cbDict = null;

            switch (handler_request.HttpMethod)
            {
                case "GET":
                    cbDict = mGetCallbackdict;
                    break;
                case "PUT":
                    cbDict = mPutCallbackdict;
                    break;
                case "POST":
                    cbDict = mPostCallbackdict;
                    break;
                case "DELETE":
                    cbDict = mDeleteCallbackdict;
                    break;
                default:
                    Console.WriteLine("Unknown Http Method: " + handler_request.HttpMethod);
                    break;
            }


            if (cbDict != null)
            {
                foreach (KeyValuePair<UriTemplate, webserverDelegateAction> item in cbDict)
                {
                    results = item.Key.Match(prefix, requestURI);
                    if (results != null)
                    {
                        Console.WriteLine("Matching Callback found: " + item.Key.ToString());
                        cb = item.Value;

                        Dictionary<string, string> uriParams = new Dictionary<string, string>();

                        foreach (string variableName in results.BoundVariables.Keys)
                        {
                            uriParams.Add(variableName, results.BoundVariables[variableName]);
                        }

                        uriParams.Add("Querystring", handler_request.Url.Query);

                        bool cbRes = cb(ref handler_request, ref responsebyte, ref statusCode, ref contentType, uriParams);

                        //todo: wait here for unity response ?

                        if (cbRes == true)
                        {
                            handler_response.StatusCode = statusCode;
                            handler_response.ContentType = contentType;
                            handler_response.AppendHeader("Access-Control-Allow-Origin", "*");
                        }
                        else
                        {
                            handler_response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            handler_response.ContentType = "text/html; charset=utf-8";
                            //responsebyte = File.ReadAllBytes(root_path + "//error//error.html");
                            responsebyte = System.Text.Encoding.UTF8.GetBytes(document + " Server Error!");
                            handler_response.AppendHeader("Access-Control-Allow-Origin", "*"); //test FNS
                        }
                        callbackExecuted = true;
                        break; //cbdict lookup ... only first match is processed!
                    }
                } //cbdict foreach
            }

            if (!callbackExecuted)
            {
                string docfilepath = mRootPath + document;

                if (File.Exists(docfilepath))
                {
                    Console.WriteLine("file exists");
                    handler_response.StatusCode = (int)HttpStatusCode.OK;

                    string ext = Path.GetExtension(docfilepath);

                    Console.WriteLine("File ext " + ext);

                    string mime = MimeType.MimeTypeMap.GetMimeType(ext);

                    Console.WriteLine("mimetype " + mime);

                    handler_response.ContentType = mime;
                    // responsebyte = File.ReadAllBytes(docfilepath);

                    responsebyte = ReadAllBytes(docfilepath);

                    Console.WriteLine("file read!");
                }
                else
                {
                    if (mDebugFlag)
                    {
                        handler_response.StatusCode = (int)HttpStatusCode.NotFound;
                        string responsestring = "Server running! ";
                        responsestring += "document: " + document + " ";
                        responsestring += "documentpath: " + docfilepath + " ";
                        responsestring += "root_path " + mRootPath + " ";
                        handler_response.ContentType = "text/html; charset=utf-8";
                        responsebyte = System.Text.Encoding.UTF8.GetBytes(responsestring);
                    }
                    else
                    {

                        if (document == "/")
                        {
                            Console.WriteLine("Will redirect to: " + mRootPath + "/index.html");
                            handler_response.StatusCode = (int)HttpStatusCode.Redirect;

                            //handler_response.Redirect("http://" + mHttpHost + ":"+ mHttpPort + "/index.html");
                            handler_response.Redirect("http://" + handler_request.LocalEndPoint.ToString() + "/index.html");
                            responsebyte = null;
                            handler_response.OutputStream.Close();
                        }
                        else
                        {
                            handler_response.StatusCode = (int)HttpStatusCode.NotFound;
                            handler_response.ContentType = "text/html; charset=utf-8";
                            //responsebyte = File.ReadAllBytes(root_path + "//error//error.html");
                            responsebyte = System.Text.Encoding.UTF8.GetBytes(document + " Not Found!");
                        }
                    }
                }
            }

            if (responsebyte != null)
            {
                handler_response.ContentLength64 = responsebyte.Length;
                System.IO.Stream output = handler_response.OutputStream;
                output.Write(responsebyte, 0, responsebyte.Length);
                output.Close();
            }
            Console.WriteLine("close!");
        }
    }


}
