// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



//THEME TOGGLER=============================================
const darkTheme = document.querySelector("#darkTheme");
const lightTheme = document.querySelector("#lightTheme");

// function to toggle between light and dark theme
function toggleTheme() {
    if (localStorage.getItem('theme') === 'theme-dark') {
        setTheme('theme-light');
    } else {
        setTheme('theme-dark');
    }
}

// Immediately invoked function to set the theme on initial load
(function () {
    if (localStorage.getItem('theme') === 'theme-dark') {
        setTheme('theme-dark');
        document.getElementById('slider').checked = false;
    } else {
        setTheme('theme-light');
        document.getElementById('slider').checked = true;
    }
})();

function setTheme(theme) {
    if (theme === "theme-dark") {
        darkTheme.rel = 'stylesheet';
        lightTheme.rel = 'alternate stylesheet';
        localStorage.setItem("theme", theme);
    } else {
        darkTheme.rel = 'alternate stylesheet';
        lightTheme.rel = 'stylesheet';
        localStorage.setItem("theme", theme);
    }
}
//===============================================================