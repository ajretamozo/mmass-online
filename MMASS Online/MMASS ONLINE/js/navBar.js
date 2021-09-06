
    function addNavBar(divId){
        html = '';
        html += "<nav class ='navbar navbar-expand-md navbar-dark sticky-top bg-dark'>";
        html += "<a class='navbar-brand' href='#'>MMASS Online</a>";
        html +="<button class='navbar-toggler' type='button' data-toggle='collapse' data-target='#navbarCollapse' aria-controls='navbarCollapse' aria-expanded='false' aria-label='Toggle navigation'>";
        html +="<span class='navbar-toggler-icon'></span>";
        html +="</button>";
        html +="<div class='collapse navbar-collapse' id='navbarCollapse'>";
        html +="<ul class='navbar-nav mr-auto'>";
        html +="<li class='nav-item'>";
        html +="<a class='nav-link' href='tarifaslist.html'>Tarifas</a>";
        html +="</li>";
        html +="<li class='nav-item'>";
        html +="<a class='nav-link' href='tiposavisoslist.html'>Tipos de Publicidad</a>";
        html +="</li>";
        html +="<li class='nav-item dropdown'>";
        html +="<a class='nav-link dropdown-toggle' href='#' id='navbardrop' data-toggle='dropdown'>";
        html +="Ordenes";
        html +="</a>";
        html +="<div class='dropdown-menu'>";
        html +="<a class='dropdown-item' href='orden.html'>Nueva Orden</a>";
        html +="<a class='dropdown-item' href='ordenList.html'>Buscar</a>";
        html +="</div>";
        html +="</li>";
        html +="<li class='nav-item'>";
        html +="<a class='nav-link disabled' href='#'>Reportes</a>";
        html +="</li>";
        html +="</ul>";
        html +="</div>";
        html +="</nav>";
        $("#"+divId).html(html);
        
    }