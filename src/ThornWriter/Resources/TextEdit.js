function loadEditor(teaxAreaId) {
    var textArea = document.getElementById(teaxAreaId);

    return CodeMirror.fromTextArea(textArea, {
        mode: "htmlmixed"
    });
}

var codeMirrorInstance = loadEditor("textEdit");

function setEditorTheme(themeName) {
  
}

function setEditorContent(content) {
    codeMirrorInstance.setValue(content);
}

function getEditorContent() {
    return codeMirrorInstance.getValue();
}

window.onerror = function (msg, url, linenumber) {
    alert('Error message: ' + msg + '\nURL: ' + url + '\nLine Number: ' + linenumber);
    return true;
}