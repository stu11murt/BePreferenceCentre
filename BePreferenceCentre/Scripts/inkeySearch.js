var answers = JSON.parse($('#InkeyJsonAnswers').val());

//var jsonFile = readTextFile("/Scripts/Json/inkey.json");
//var allText = "";
//function readTextFile(file) {
//    var rawFile = new XMLHttpRequest();
//    rawFile.open("GET", file, false);
//    rawFile.onreadystatechange = function () {
//        if (rawFile.readyState === 4) {
//            if (rawFile.status === 200 || rawFile.status == 0) {
//                allText = rawFile.responseText;
//                console.log(allText);
//                answers = JSON.parse(allText);
//            }
//        }
//    }
//    rawFile.send(null);
//}


var options = {
    shouldSort: true,
    tokenise: true,
    includeAllMatches: true,
    threshold: 0.3,
    distance: 100,
    maxPatternLength: 200,
    minMatchCharLength: 3,
    keys: ["Question"]
}

var f = new Fuse(answers, options);
function DoSearch(srchValue) {
    $('#inkey-results').html("");
    var line = "";
    var i = 0;
    f.search(srchValue).forEach(function (item, index) {
        if (i > 0)
            return false;
        console.log("count : " + i);
        line += "<tr><td><h1>" + item.ProductName + "</h1></td></tr>";
        line += "<tr>";
        line += "<td>" + item.Answer + "</td>";
        line += "<td><img src='" + GetProductImage(item.ProductName) + "' height='200' '/></td>";
        line += "</tr>";
        line += item.InstructionsForUse
        $('#inkey-results').html(line).addClass('fadeIn');
        i++;
    });
};

//$('#searchTextbox').keyup(function () {
//    $('#inkey-results').html("");
//    var line = "";
//    f.search(this.value).forEach(function (item, index) {

//        line += "<tr>";
//        line += "<td>" + item.Answer + "</td>";
//        line += "<td><img src='" + item.ProductImageLink + "' height='150' '/></td>";
//        line += "</tr>";
//        $('#inkey-results').html(line);
//    });
//});

$('#ask-inkey').click(function () {
    $('#inkey-results').html("");
    DoSearch($('#searchTextbox').val());

});

$('#searchTextbox').keydown(function (e) {
    if (e.keyCode == 13) {
        $('#inkey-results').html("");
        DoSearch($('#searchTextbox').val());
    }
})

function GetProductImage(productRef) {
    console.log("Prod Ref : " + productRef);
    switch (productRef) {
        case "Lactic Acid":
            console.log("TEST");
            return "{{ 'bod400.jpg' | asset_img_url }}";
            break;

    }
}