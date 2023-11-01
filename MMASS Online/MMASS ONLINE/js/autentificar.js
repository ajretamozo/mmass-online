

const backendDir = "http://localhost:5000/"


//Variable para mantener el token de logueo, se le pasa siempre al llamado a CallWS
//var globalBearer = "1";//Lo Inicializo asi no da error al llamar metodos que no necesitan autentificacion
function callWS(ws, op, data, onDone) {
    if (sessionStorage.getItem('token') === null && op !== "authenticate") {
        location.href = '/login.html';
        return null;
    }
    beginWait();
    return $.ajax({        
        method: "POST",
        url: backendDir + ws + "/" + op,
        data: data,
        headers: { 'Authorization': sessionStorage.getItem('token') },
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (res) {
        if ($("#divMessage").hasClass("alert-danger"))
            simpleHide("divMessage");
        hidePleaseWait();
        onDone(res);


        $(".app-icon").css("animation", "");
    }).fail(function (jqXHR, textStatus, errorThrown) {//alert(jqXHR.responseText+" - "+textStatus+" - "+errorThrown);
        $(".app-icon").css("animation", "");
        hidePleaseWait();
        if (errorThrown === "Unauthorized") {
            cerrarSesion();
        }
        if (errorThrown != "SyntaxError: Unexpected end of JSON input" && errorThrown != "SyntaxError: JSON.parse: unexpected end of data at line 1 column 3 of the JSON data") {
            showMessage("La acción solicitada no se puede realizar en este momento, intente de nuevo más tarde", "error");
            reportError(jqXHR.responseText + " - " + textStatus + " - " + errorThrown, "WS: " + ws + " - op:" + op, "");
            console.log(jqXHR.responseText + " - " + textStatus + " - " + errorThrown);
        }
    });
}

function setUserName() {

    $("#NombreUsuario").text(getUserName());
    $("#NombreUsuario").attr("data-id", getUserId());
}

function isUserLogged() {
    return localStorage.getItem('token') !== null;
}

function cerrarSesion() {
    if (window.location.href.indexOf("/orden.html") > -1) {
        desbloquearOp();
        setTimeout(function () {
            sessionStorage.setItem('token', null);
            location.href = '/login.html';
        }, 5);
    }
    else {
        sessionStorage.setItem('token', null);
        location.href = '/login.html';
    }
} 

function autentificar(user, pass, onDone,onError) {
    $.when(
        callWS("users", "authenticate", '{"username": "' + user + '","password": "' + pass + '"}', function (data) { _onAutentificar(data);  })
        
    ).then(function (data) { onDone(data); }, function (data1) {
        onError();
    });  
    
}    

function _onAutentificar(data) {
    //globalBearer = 'Bearer ' + data.token;
    //$window.sessionStorage.accessToken = data.token;
    sessionStorage.setItem('token', "Bearer " + data.token);
    sessionStorage.setItem('userId',  data.id);
    sessionStorage.setItem('userName',data.firstName);
    sessionStorage.setItem('userRol', data.usrrol);

    //inicializar();
}
function getUserName() {
    return sessionStorage.getItem('userName');
}
function getUserId() {
    return sessionStorage.getItem('userId');
}
///FUNCIONES UI
function beginWait() {
    $(".app-icon").css("animation", "spin 0.5s linear infinite");
    onPostback = true;
    $("#divBlock").show();
    setTimeout(showPleaseWait, 500);
}

function showPleaseWait() {
    if (onPostback) $("#pleaseWaitDialog").modal();
}

function hidePleaseWait() {
    onPostback = false;
    $("#divBlock").hide();
    $("#pleaseWaitDialog").modal('hide');
}
function reportError(msg, url, line) {
    console.log(msg + " " + url + " " + line);
}
function showMessage(message, type = "success") {
    $("#divMessage").removeClass("alert alert-danger").removeClass("alert alert-success").removeClass("alert alert-info");
    if (type == "success") {
        $("#divMessage").addClass("alert alert-success").html(message);
        setTimeout(function () { $("#divMessage").show(); }, 5);//si no lo llamo asi no muestra el mensaje
        setTimeout(function () { $("#divMessage").hide("slideDown"); }, 3000);

    } else if (type == "info") {
        $("#divMessage").addClass("alert alert-info").show().html(message);
        setTimeout(function () { $("#divMessage").hide("slideDown"); }, 3000);
    } else {
        $("#divMessage").addClass("alert alert-danger").show().html(message);
    }
}
function simpleHide(e) {
    document.getElementById(e).style.display = "none";
}
function showLargeModal(message, title = "") {
    $("#largeModalBody").html(message);
    $("#largeModalTitle").html(title);
    $("#largeModal").modal('show');
}

function showModal(message, options) {
            $("#modalCenterBody").html(message);
            strHTML = '';
            options.forEach(function (option) {
                strHTML += "<button type='button' class='btn btn-secondary' data-dismiss='modal' onClick=" + option.optionFunc + ">" + option.optionDesc + "</button>"
            });
            $("#modalCenterFooter").html(strHTML);
            $("#modalCenter").modal('show');
}

function getUserRol() {
    return sessionStorage.getItem('userRol');
}

function setUserRol() {
    if (getUserRol() == 1) {
        $("#usuarios").removeClass("disabled");
    }
}
