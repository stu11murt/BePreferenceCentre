var answers = JSON.parse($('#InkeyJsonAnswers').val());
//var answers = JSON.parse(GetAnswers());
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
var questionId = 0;

$(document).ready(function () {
    $('#userEmailSubmit').hide();
});

var options = {
    shouldSort: true,
    tokenise: false,
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
        line += "<tr><td></td></tr>";
        line += "<tr>";
        line += "<td><img src='http://www.deepbluedevelopment.co.uk/images/ClientImages/inkey/INKEY_LacticAcid.jpg' height='300' '/></td>";
        line += "<td class='inkey-result'><h1>" + item.ProductName + "<br /><span class='inkey-phonetic'>/\ lak tic /\ a-sed</span></h1><br />" + item.Answer + "</td>";
        line += "<td style='height: 290px; width: 1px; border-right: 1px dotted black;'></td><td class='inkey-related'><span><strong>RELATED ARTICLES</strong></span><br /><br /><strong><i>Using lactic acid?</i></strong><br /><hr><br /><strong><i>My daily routine?</i></strong><hr><br /><strong><i>What is lactic acid made from?</i></strong><hr><br /><strong><i>When should i use Lactic Acid?</i></strong><hr></td>";
        line += "</tr>";
        //line += "<tr><td>" + item.InstructionsForUse + "</td></tr>";
        $('#inkey-results').html(line).addClass('fadeIn');
        i++;
    });

    if (i === 0) {
        var InkeyQuestion = {
            UserQuestion: $('#searchTextbox').val()
        };
        console.log("Whoops no answer");
        $.ajax({
            url: '../api/InkeyUserQuestions',
            data: InkeyQuestion,
            type: "POST",
        })
            .done(function (result) {
                console.log(result);
                questionId = result.InkeyUserQuestionsId;
                console.log(questionId);
                $('#inkey-results').html('<div><p>Hi, we are really sorry but we have not got an answer to this question at the moment, however your question has been logged and we will answer it a.s.a.p</p><p>Enter your email address below to get a personal response to this question.</p></div>').addClass('fadeIn');
                $('#userEmailSubmit').show();
            });

        return false;
    }
};


function GetAnswers() {
    $.ajax({
        url: '../api/InkeyUserQuestions',
        type: "GET",
    })
        .done(function (result) {
            console.log("lookeme ma : " + result);
            //$('#inkey-results').html('<div><p>Hi, we are really sorry but we have not got an answer to this question at the moment, however your question has been logged and we will answer it a.s.a.p</p></div>').addClass('fadeIn');
        });

    return false;
}

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
    if (e.keyCode === 13) {
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
            

    }
}


$('#submitUserEmail').click(function () {
   
    var InkeyQuestion = {
        InkeyUserQuestionsId: questionId,
        userEmail: $('#userEmail').val()
    };
    
    $.ajax({
        url: '../api/InkeyUserQuestionsEmail',
        data: InkeyQuestion,
        type: "POST",
        success: function (msg) {
            $('#inkey-results').html("");
            $('#inkey-results').html('<div><p>Thank you, we will be in touch as soon as possible.</p></div>');
            $('#userEmailSubmit').hide();
        },
        error: function (errormessage) {
            $('#inkey-results').html("");
            $('#inkey-results').html('<div><p>Sorry this email address does not seem valid. Please try again.</p></div>');
           
        }
    })
        

    return false;
});


/*.done(function (result) {
            console.log(result);
           
        });*/