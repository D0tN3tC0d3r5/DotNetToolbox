function scrollToBottom(element) {
    const htmlElement = document.getElementsByTagName("html")[0];
    htmlElement.scrollTop = htmlElement.scrollHeight;
}

function scrollToTop(element) {
    const htmlElement = document.getElementsByTagName("html")[0];
    htmlElement.scrollTop = 0;
}