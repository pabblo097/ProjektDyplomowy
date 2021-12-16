$(window).ready(updateHeight);
$(window).resize(updateHeight);

function updateHeight() {
    var div = $('iframe');
    var width = div.width();

    div.css('height', width * 0.5225);
}