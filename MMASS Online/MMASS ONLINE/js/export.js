function exportTableToExcel(tableID, filename = '') {

    var downloadLink;
    var dataType = 'application/vnd.ms-excel;charset=UTF-8';

    //Clona la tabla y elimina las filas agrupaciones
    var exTable = $('#' + tableID).clone();
    var tableHTML = exTable.find('.tableexport-ignore').remove().end().prop('outerHTML');
    //Reemplaza caracteres mal encodeados
    tableHTML = tableHTML.replace(/ /g, '%20');
    tableHTML = tableHTML.replace(/á/g, '%E1');
    tableHTML = tableHTML.replace(/é/g, '%E9');
    tableHTML = tableHTML.replace(/í/g, '%ED');
    tableHTML = tableHTML.replace(/ó/g, '%F3');
    tableHTML = tableHTML.replace(/ú/g, '%FA');
    tableHTML = tableHTML.replace(/Á/g, '%C1');
    tableHTML = tableHTML.replace(/É/g, '%C9');
    tableHTML = tableHTML.replace(/Í/g, '%CD');
    tableHTML = tableHTML.replace(/Ó/g, '%D3');
    tableHTML = tableHTML.replace(/Ú/g, '%DA');
    tableHTML = tableHTML.replace(/ñ/g, '%F1');
    tableHTML = tableHTML.replace(/Ñ/g, '%D1');

    // Specify file name
    filename = filename ? filename + '.xls' : 'excel_data.xls';

    // Create download link element
    downloadLink = document.createElement("a");

    document.body.appendChild(downloadLink);

    if (navigator.msSaveOrOpenBlob) {
        var blob = new Blob(['\ufeff', tableHTML], {
            type: dataType
        });
        navigator.msSaveOrOpenBlob(blob, filename);
    } else {
        // Create a link to the file
        downloadLink.href = 'data:' + dataType + ', ' + tableHTML;

        // Setting the file name
        downloadLink.download = filename;

        //triggering the function
        downloadLink.click();
    }
}