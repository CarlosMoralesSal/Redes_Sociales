using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Plus.v1;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Plus.v1.Data;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Facebook;
using System.Data;
using System.Numerics;
using System.Xml;

public partial class Paneles : System.Web.UI.Page
{
    
    String app_id = "882181981814604";
    String scope = "email";
    String _client_id = "68518039682-tgtsdd01imreud0sasat0471faajh0ih.apps.googleusercontent.com";
    String _client_secret = "CWAuxjwp7CyxMYMuU4O7J3iU";
    System.Numerics.BigInteger ipnum;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["Facebook"] != null)
            {


                if (Request["code"] == null)
                {

                    Response.Redirect(string.Format("https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}", app_id, Request.Url.AbsoluteUri, scope));
                }
                else
                {
                    CheckAuthorization();
                }

            }

           
            if (Session["Google"] != null)
            {
                GetUserInfoGoogle();
            }

        }
    }

    private void GetUserInfoGoogle()
    {
        String url = Request.Url.Query;
        

        String strIp;

        if (url != null)
        {

            Char[] delimiterChars = { '=' };
            String[] words = url.Split(delimiterChars);
            String code = Request.QueryString["code"];
            String redirect_uri = "http://localhost:59573/Paneles.aspx";
            String mail=String.Empty;
            String name=String.Empty;
            Controlador.Registro registro = new Controlador.Registro();

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
            webRequest.Method = "POST";
            String Parameters = "code=" + code;
            Parameters = Parameters + "&client_id=" + _client_id;
            Parameters = Parameters + "&client_secret=" + _client_secret;
            Parameters = Parameters + "&redirect_uri=" + redirect_uri;
            Parameters = Parameters + "&grant_type=authorization_code";
            Byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream postStream = webRequest.GetRequestStream();
            //Add the post data to the web request
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            WebResponse response = webRequest.GetResponse();
            postStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(postStream);
            String responseFromServer = reader.ReadToEnd();

            Controlador.GooglePlusAccessToken serStatus = JsonConvert.DeserializeObject<Controlador.GooglePlusAccessToken>(responseFromServer);

            if (serStatus != null)
            {
                String accessToken = String.Empty;
                accessToken = serStatus.access_token;
                var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + accessToken;
                if (!String.IsNullOrEmpty(accessToken))
                {
                    HttpClient client = new HttpClient();
                    client.CancelPendingRequests();
                    HttpResponseMessage output = client.GetAsync(urlProfile).Result;


                    if (output.IsSuccessStatusCode)
                    {
                        String outputData = output.Content.ReadAsStringAsync().Result;
                        Controlador.GoogleUserOutputData serStatus2 = JsonConvert.DeserializeObject<Controlador.GoogleUserOutputData>(outputData);

                        if (serStatus2 != null)
                        {
                            mail = serStatus2.email;
                            name = serStatus2.name;
                            
                            String geoposicion = "";
                            strIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                            if (strIp == null)
                            {
                                strIp = Request.ServerVariables["REMOTE_ADDR"];
                            }

                                                                              
                            String usrIP = Request.UserHostAddress;


                            String apiUrl = "http://api.ipinfodb.com/v3/ip-city/?key=04e88385a9c161c8c3dbfbe8ae5ac070873e6e0a1a27014dd4aabfcfc1655aa4&ip=212.128.152.144&format=xml";

                            XmlDocument respon = GetXmlResponse(apiUrl);
                            // Display each entity's info.
                            geoposicion = ProcessEntityElements(respon);

                            Int64 idPersona=registro.Insertar(name, mail,geoposicion);
                            posOculto.Value = idPersona.ToString();
                            

                            entrar.Visible = false;
                           

                        }
                    }

                }

            }

        }

        entrar.Visible = false;
        salir.Visible = true;

    }

    

    private void CheckAuthorization()
    {

        String app_id = "882181981814604";
        String app_secret = "2a9755755903a88f8d6f159f93602281";
        String scope = "email";
        Controlador.Registro registro = new Controlador.Registro();
        String mail = String.Empty;
        String name = String.Empty;
        String user_location = String.Empty;
        
        String geoposicion = "";
        
        String strIp;
        

        if (Request["code"] == null)
        {
            Response.Redirect(String.Format("https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}", app_id, Request.Url.AbsoluteUri, scope));
        }
        else
        {

            Dictionary<String, String> tokens = new Dictionary<String, string>();
            String url = String.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&scope={2}&code={3}&client_secret={4}", app_id, Request.Url.AbsoluteUri, scope, Request["code"].ToString(), app_secret);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {

                StreamReader reader = new StreamReader(response.GetResponseStream());
                String vals = reader.ReadToEnd();

                foreach (String token in vals.Split('&'))
                {
                    tokens.Add(token.Substring(0, token.IndexOf("=")), token.Substring(token.IndexOf("=") + 1, token.Length - token.IndexOf("=") - 1));
                }

            }

            String access_token = tokens["access_token"];
            String usrIP=Request.UserHostAddress;


            String apiUrl = "http://api.ipinfodb.com/v3/ip-city/?key=04e88385a9c161c8c3dbfbe8ae5ac070873e6e0a1a27014dd4aabfcfc1655aa4&ip=212.128.152.144&format=xml";

            
            // Send the request and get back an XML response.
            XmlDocument respon = GetXmlResponse(apiUrl);
            // Display each entity's info.
            geoposicion=ProcessEntityElements(respon);


            
            strIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIp == null)
            {
                strIp = Request.ServerVariables["REMOTE_ADDR"];
            }
           
                       
            var client = new FacebookClient(access_token);
            dynamic result = client.Get("/me");
            mail = Convert.ToString(result.email);
            name = Convert.ToString(result.name);
            user_location = Convert.ToString(result.user_location);
                       

            Int64 idPersona=registro.Insertar(name,mail,geoposicion);
            
                      
            
            entrar.Visible = false;
            
            salir.Visible = true;
            Session.Remove("Facebook");
           
        }
    }

    public static XmlDocument GetXmlResponse(string requestUrl)
    {
        try
        {
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(response.GetResponseStream());
            return (xmlDoc);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            Console.Read();
            return null;
        }
    }

    private String ProcessEntityElements(XmlDocument response)
    {
        String geoposicion = "";
        XmlNodeList entryElements = response.GetElementsByTagName("Response");
        for (int i = 0; i <= entryElements.Count - 1; i++)
        {
            XmlElement element = (XmlElement)entryElements[i];
            XmlElement latitudeElement = (XmlElement)element.GetElementsByTagName(
              "latitude")[0];
            XmlElement longElement = (XmlElement)element.GetElementsByTagName(
              "longitude")[0];
           
            if (longElement == null)
                throw new Exception("Longitude not found");
            geoposicion = latitudeElement.InnerText;
            geoposicion += "," + longElement.InnerText;
            

            
        }
        return geoposicion;
    }

    protected void LoginToFacebookClicked(object sender, ImageClickEventArgs e)
    {
        Session["Facebook"] = true;
        CheckAuthorization();
    }
        

    protected void LoginToGooglePlus_Click(object sender, EventArgs e)
    {
        Session["Google"] = true;
        Response.Redirect("https://accounts.google.com/o/oauth2/auth?client_id=68518039682-tgtsdd01imreud0sasat0471faajh0ih.apps.googleusercontent.com&redirect_uri=http://localhost:59573/Paneles.aspx&response_type=code&scope=email&state=DCEEFWF45453sdffef424&access_type=online&approval_prompt=auto");
    }




    protected void LoginToFacebookClicked(object sender, EventArgs e)
    {
        Session["Facebook"] = true;
        CheckAuthorization();
    }
}