// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function relation(ID) {
    var elm = document.getElementById("Relation_Id");
    elm.value = ID;
}

function Unknown() {
    var elm = document.getElementById("Tel_No");
    elm.value = "9999-99-9999";
}

function CopySammary() {
    let range = document.createRange();
    let span = document.getElementById('smy');
    range.selectNodeContents(span);
  
    //指定した範囲を選択状態にする
    let selection = document.getSelection();
    selection.removeAllRanges();
    selection.addRange(range);
  
    //コピー
    document.execCommand('copy');
    selection.empty(range);
}