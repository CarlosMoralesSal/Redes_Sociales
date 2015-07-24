<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Paneles.aspx.cs" Inherits="Paneles" %>

<!DOCTYPE html>

<script>


    // This is called with the results from from FB.getLoginStatus().
  //  var x = document.getElementById("entrar");
  //  var latlondata = "";
  //  function geoLocation()
  //  {
        
  //      if (navigator.geolocation) {
            
  //          navigator.geolocation.getCurrentPosition(showPosition, showError);
  //          //alert('Después');
  //          //loadJsonData(latlondata);
  //          //latlondata = mostrarPosicion(position)
  //          //loadJsonData(latlondata);
  //      }
  //      else { x.innerHTML = "Geolocation is not supported by this browser."; }
  //  }
  //  function showPosition(position) {
        
  //      alert('En showPosition');
  //      latlondata = position.coords.latitude + "," + position.coords.longitude;
  //      //alert(latlondata);
  //      //document.getElementById("posOculto").setAttribute("Value", position.coords.latitude.toString());
  //      var latlon = "Your Latitude Position is:=" + position.coords.latitude + "," + "Your Longitude Position is:=" + position.coords.longitude;
  //      //alert(latlon);
  //      //alert('Voy a ir a loadJsonData');
  //      var myVar = setTimeout(loadJsonData(latlondata), 8000);
  //      //loadJsonData(latlondata);
  //      //loadJsonData();
        
  ////      var img_url = "http://maps.googleapis.com/maps/api/staticmap?center="
  ////+ latlondata + "&zoom=14&size=400x300&sensor=false";
  ////      document.getElementById("mapholder").innerHTML = "<img src='" + img_url + "' />";
  //      //setCookie("geoposicion", position.coords.latitude + "," + position.coords.longitude, 1, '', '', '');
  //  }


  //  function mostrarPosicion(position) {

  //      var latlondata = position.coords.latitude + "," + position.coords.longitude;
        
  //      return latlondata;
  //  }



  //  function showError(error) {
  //      //alert('Estoy en showPosition');
  //      if (error.code == 1) {
  //          x.innerHTML = "User denied the request for Geolocation."
  //      }
  //      else if (err.code == 2) {
  //          x.innerHTML = "Location information is unavailable."
  //      }
  //      else if (err.code == 3) {
  //          x.innerHTML = "The request to get user location timed out."
  //      }
  //      else {
  //          x.innerHTML = "An unknown error occurred."
  //      }
  //  }


    
    function statusChangeCallback(response) {

        document.getElementById("entrar").style.display = 'none';
        document.getElementById("salir").style.display = 'block';
        console.log('statusChangeCallback');
        console.log(response);
        // The response object is returned with a status field that lets the
        // app know the current login status of the person.
        // Full docs on the response object can be found in the documentation
        // for FB.getLoginStatus().
        if (response.status === 'connected') {
            // Logged into your app and Facebook.
            testAPI();
            FB.api('/me', function (response) {
                alert('Tu nombre es: ' + response.name);
                alert('Tu email es:' + response.email);
            });
        } else if (response.status === 'not_authorized') {
            // The person is logged into Facebook, but not your app.
            document.getElementById('status').innerHTML = 'Please log ' +
        'into this app.';
        } else {
            // The person is not logged into Facebook, so we're not sure if
            // they are logged into this app or not.
            document.getElementById('status').innerHTML = 'Please log ' +
        'into Facebook.';
        }
    }

    // This function is called when someone finishes with the Login
    // Button.  See the onlogin handler attached to it in the sample
    // code below.
    function checkLoginState() {
        FB.getLoginStatus(function (response) {
            statusChangeCallback(response);

        });
        alert("Hola");
        statusChangeCallback(response);


    }

    window.fbAsyncInit = function () {
        FB.init({
            appId: '882181981814604',
            cookie: true,  // enable cookies to allow the server to access 
            // the session
            xfbml: true,  // parse social plugins on this page
            version: 'v2.1' // use version 2.1
        });

        // Now that we've initialized the JavaScript SDK, we call 
        // FB.getLoginStatus().  This function gets the state of the
        // person visiting this page and can return one of three states to
        // the callback you provide.  They can be:
        //
        // 1. Logged into your app ('connected')
        // 2. Logged into Facebook, but not your app ('not_authorized')
        // 3. Not logged into Facebook and can't tell if they are logged into
        //    your app or not.
        //
        // These three cases are handled in the callback function.

        FB.getLoginStatus(function (response) {

            if (response.status === 'connected') {
                // the user is logged in and has authenticated your
                // app, and response.authResponse supplies
                // the user's ID, a valid access token, a signed
                // request, and the time the access token 
                // and signed request each expire
                var uid = response.authResponse.userID;
                var accessToken = response.authResponse.accessToken;


            } else if (response.status === 'not_authorized') {
                // the user is logged in to Facebook, 
                // but has not authenticated your app
            } else {
                // the user isn't logged in to Facebook.
            }
        });


    };

    // Load the SDK asynchronously
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    // Here we run a very simple test of the Graph API after login is
    // successful.  See statusChangeCallback() for when this call is made.
    function testAPI() {
        console.log('Welcome!  Fetching your information.... ');
        FB.api('/me', function (response) {
            console.log('Successful login for: ' + response.name);
            document.getElementById('status').innerHTML =
        'Thanks for logging in, ' + response.name + '!';
        });
    }
    function salir() {
        FB.logout(function (response) {
            // user is now logged out

        });
        //window.location.href = "http://www.w3schools.com";
        document.getElementById("entrar").style.display = 'block';
        document.getElementById("salir").style.display = 'none';

    }
</script>

<!--
  Below we include the Login Button social plugin. This button uses
  the JavaScript SDK to present a graphical Login button that triggers
  the FB.login() function when clicked.
-->



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Descargas</title>
    <script type="text/javascript" src="http://platform.linkedin.com/in.js">
    api_key: 77t6kbj0c3q4fs
    authorize: true
    lang:  es_ES 
</script>
    <link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
    <link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap-theme.min.css" />
    <link rel="Stylesheet" href="Style/social-buttons-3.css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
   
</head>
<body>


    <form id="form1" runat="server">
    <div id="status">
</div>
        <div>
            <asp:ScriptManager ID="ScriptManager1" 
                               runat="server" />
            
            <br />
        </div>
        <div id="linkedin" style="display:block">
       
          </div>
         <div id="entrar" visible="true" runat="server" class="form-group">
            <div class="row">
              <div class="col-xs-12 col-sm-6 col-md-12 col-lg-12">
               <asp:LinkButton ID="btnLoginToFacebook" CssClass="btn btn-facebook" runat="server" OnClick="LoginToFacebookClicked"><span aria-hidden="true" class="fa fa-facebook"></span> | <strong>Entrar vía Facebook</strong></asp:LinkButton>
               <asp:HiddenField ID="posOculto" Value="" runat="server" />
                 
               <asp:LinkButton ID="LoginToGooglePlus" CssClass="btn btn-google-plus" runat="server"  OnClick="LoginToGooglePlus_Click"><span aria-hidden="true" class="fa fa-google-plus"> | <strong>Entrar vía G+</strong></span></asp:LinkButton>
                   
            </div>
           </div>
         </div>
         <div id="salir" visible="false" runat="server">
          <a href="Default.aspx" onclick="salir();">Log Outut</a>
          <a href="images/suria_escalona.pdf" onclick="geoLocation()" runat="server">Descarga</a>
         </div>


    </form>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    
</body>
</html>


