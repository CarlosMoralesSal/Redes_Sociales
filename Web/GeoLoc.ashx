<%@ WebHandler Language="C#" Class="GeoLoc" %>

using System;
using System.Web;

public class GeoLoc : IHttpHandler {
    
    
    
    
    public void ProcessRequest (HttpContext context) 
    {
        
        String jsonString = String.Empty;
        HttpContext.Current.Request.InputStream.Position = 0;
        String cook = String.Empty;
       
        Controlador.dsRegistroTableAdapters.registro_redessocialesTableAdapter q;
        
        q = new Controlador.dsRegistroTableAdapters.registro_redessocialesTableAdapter();
        
        using (System.IO.StreamReader inputStream =
        new System.IO.StreamReader(HttpContext.Current.Request.InputStream))
        {
            jsonString = inputStream.ReadToEnd();
            System.Web.Script.Serialization.JavaScriptSerializer jSerialize =
                new System.Web.Script.Serialization.JavaScriptSerializer();
            var geo = jSerialize.Deserialize<GeoCoords>(jsonString);
            String cookie = context.Request.Cookies["persona"].ToString();
            if (cookie != null)
            {
                HttpCookie strcookie = context.Request.Cookies["persona"];    
                cook = strcookie.Value.ToString();
                
            }
            if (geo != null)
            {
                
                String posicion = geo.Posicion;
                
                q.ActualizarRegistro(posicion, Convert.ToInt64(cook));
             
                
                context.Response.Write(jSerialize.Serialize(
                     new
                     {
                         Response = "Message Has been sent successfully"
                     }));
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    

}